using DoctorSystem.Controllers;
using DoctorSystem.Entities;
using DoctorSystem.Interfaces;
using DoctorSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace DoctorSystemTest
{
    [TestFixture]
    public class AccountControllerTest
    {
        private AccountController _controller;
        private Mock<IClientRepository> _clientRepo;
        private Mock<IDoctorRepository> _doctorRepo;
        private Mock<IPlaceRepository> _placeRepo;
        private Mock<ITokenService> _tokenService;
        private Mock<EmailService> _emailService;
        private Mock<RouterService> _routerService;


        [SetUp]
        public void Setup()
        {

            _clientRepo = new Mock<IClientRepository>();
            _doctorRepo = new Mock<IDoctorRepository>();
            _placeRepo = new Mock<IPlaceRepository>();
            _tokenService = new Mock<ITokenService>();
            _emailService = new Mock<EmailService>();
            _routerService = new Mock<RouterService>();

            Client c = new Client()
            {
                Name = "Béla",
                MedNumber = "",
                Email = "",
                PhoneNumber = "+36",
                Password = new byte[] { Byte.Parse("123") },
                PasswordSalt = new byte[] { Byte.Parse("124") },
                EmailToken = "",
                Place = new Place(),
                Street = "",
                HouseNumber = "",
                BirthDate = DateTime.Parse("1234.12.12"),
                Doctor = new Doctor(),
                Member = false,
                MotherName = "",
                BirthPlace = new City()
            };

            _clientRepo.Setup(x => x.GetClientByEmailTokenAsync(It.IsAny<string>())).ReturnsAsync(c);
            _routerService.Setup(x => x.Route(It.IsAny<string>())).Returns("http://localhost:4200/login");

            _controller = new AccountController(_tokenService.Object, _emailService.Object, _routerService.Object, _clientRepo.Object, _doctorRepo.Object, _placeRepo.Object);
        }
        [TestCase(Category = "Controller tests", TestName = "DeleteClientEndpoint_Redirect", Description = "")]
        [Test]
        public void DeleteClientEndpointTest_Redirect()
        {
            //arrange

            //act
            Task<ActionResult> result = _controller.DeleteClient("123");
            //assert
            _clientRepo.Verify(x => x.GetClientByEmailTokenAsync(It.IsAny<string>()), Times.Once());
          
            Assert.AreEqual(typeof(RedirectResult), result.Result.GetType());
        }

        [TestCase(Category = "Controller tests", TestName = "DeleteClientEndpoint_Unauthorized", Description = "")]
        [Test]
        public void DeleteClientEndpointTest_Unauthorized()
        {
            //arrange

            //act
            Task<ActionResult> result = _controller.DeleteClient("true");
            //assert
            _clientRepo.Verify(x => x.GetClientByEmailTokenAsync(It.IsAny<string>()), Times.Once());

            Assert.AreEqual(typeof(UnauthorizedObjectResult), result.Result.GetType());
        }
    }
}