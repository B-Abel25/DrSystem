using DoctorSystem.Dtos;
using DoctorSystem.Entities;
using DoctorSystem.Entities.Contexts;
using DoctorSystem.Model.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DoctorSystem.Data
{
    public class AddressSeedModel
    {
        public string IrSzam { get; set; }
        public string City { get; set; }
        public string County { get; set; }
    }


    public class DoctorSeedModel
    {
        public string Name { get; set; }
        public string DateOfBirth { get; set; }
        public string SealNumber { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PostCode { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string Password { get; set; }
    }

    public class ClientSeedModel
    {
        public string Name { get; set; }
        public string DateOfBirth { get; set; }
        public string SealNumber { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PostCode { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string Password { get; set; }
        public string Member { get; set; }
        public string DoctorId { get; set; }
    }

    public class Seed
    {
        private static readonly Random random = new Random();

        public static async Task SeedDatas(BaseDbContext _context)
        {
            await SeedAddress(_context);
            await SeedDoctors(_context);
            await SeedClients(_context);
            await SeedOfficeHours(_context);
        }

        private static async Task SeedAddress(BaseDbContext _context)
        {
            if (await _context._county.AnyAsync()) return;

            var AddressData = await System.IO.File.ReadAllTextAsync("Data/AddressSeedData.json", Encoding.UTF8);
            var addressModels = JsonSerializer.Deserialize<List<AddressSeedModel>>(AddressData);
            
            foreach (var addressModel in addressModels)
            {
                var place = new Place();
                place.PostCode = int.Parse(addressModel.IrSzam);


                var city = await _context._city.Include(x => x.County).SingleOrDefaultAsync(x => x.Name == addressModel.City);
                if (city == null)
                {
                    city = new City();
                    city.Name = addressModel.City;
                    var county = await _context._county.SingleOrDefaultAsync(x => x.Name == addressModel.County);
                    if (county == null)
                    {
                        county = new County();
                        county.Name = addressModel.County;
                    }
                    city.County= county;
                }
                place.City = city;

                _context._places.Add(place);
                await _context.SaveChangesAsync();
            }

        }

        private static async Task SeedDoctors(BaseDbContext _context)
        {
            if (await _context._doctors.AnyAsync()) return;

            string docData = await System.IO.File.ReadAllTextAsync("Data/DoctorSeedData.json", Encoding.UTF8);
            List<DoctorSeedModel> docModels = JsonSerializer.Deserialize<List<DoctorSeedModel>>(docData);

            foreach (var doctorModel in docModels)
            {
                Doctor doc = new Doctor();

                

                doc.Name = doctorModel.Name;
                doc.BirthDate = DateTime.Parse(doctorModel.DateOfBirth);
                do
                {
                    doc.SealNumber = random.Next(10000, 100000).ToString();
                } while (await _context._doctors.SingleOrDefaultAsync(x => x.SealNumber == doc.SealNumber) != null);
                doc.Email = doctorModel.Email;
                doc.PhoneNumber = doctorModel.PhoneNumber;
                string PC = doctorModel.PostCode.Split(' ')[0];
                string CT = doctorModel.PostCode.Split(' ')[1];
                doc.Place = await _context._places.SingleOrDefaultAsync(x => x.PostCode == int.Parse(PC) && x.City.Name == CT);
                doc.Street = doctorModel.Street;
                doc.HouseNumber = doctorModel.HouseNumber;
                doc.EmailToken = Guid.NewGuid().ToString() + (char)random.Next(97, 123);
                var hmac = new HMACSHA512();
                doc.Password = hmac.ComputeHash(Encoding.UTF8.GetBytes(doctorModel.Password));
                doc.PasswordSalt = hmac.Key;
                doc.Duration = 10;

                _context._doctors.Add(doc);
                await _context.SaveChangesAsync();
            }

        }

        private static async Task SeedClients(BaseDbContext _context)
        {
            if (await _context._clients.AnyAsync()) return;

            var cliData = await System.IO.File.ReadAllTextAsync("Data/ClientSeedData.json", Encoding.UTF8);
            var cliModels = JsonSerializer.Deserialize<List<ClientSeedModel>>(cliData);

            var Barbi = await _context._doctors.SingleOrDefaultAsync(x => x.Name == "Antal Barbara");
            foreach (var clientModel in cliModels)
            {
                Client cli = new Client();



                cli.Name = clientModel.Name;
                cli.BirthDate = DateTime.Parse(clientModel.DateOfBirth);
                do
                {
                    cli.MedNumber = random.Next(100000000, 1000000000).ToString();
                } while (await _context._clients.SingleOrDefaultAsync(x => x.MedNumber == cli.MedNumber) != null);
                cli.Email = clientModel.Email;
                cli.PhoneNumber = clientModel.PhoneNumber;
                string PC = clientModel.PostCode.Split(' ')[0];
                string CT = clientModel.PostCode.Split(' ')[1];
                cli.Place = await _context._places.SingleOrDefaultAsync(x => x.PostCode == int.Parse(PC) && x.City.Name == CT);
                cli.Street = clientModel.Street;
                cli.HouseNumber = clientModel.HouseNumber;
                cli.EmailToken = Guid.NewGuid().ToString() + (char)random.Next(97, 123);
                var hmac = new HMACSHA512();
                cli.Password = hmac.ComputeHash(Encoding.UTF8.GetBytes(clientModel.Password));
                cli.PasswordSalt = hmac.Key;
                cli.Doctor = await _context._doctors.SingleOrDefaultAsync(x => x.Id == Barbi.Id);
                cli.Member = clientModel.Member == "false" ? false : true;
                cli.MotherName = cliModels[cliModels.IndexOf(clientModel)].ToString() + cliModels[cliModels.IndexOf(clientModel)];
                _context._clients.Add(cli);
                await _context.SaveChangesAsync();
            }

        }

        private static async Task SeedOfficeHours(BaseDbContext _context)
        {
            if (await _context._officehours.AnyAsync()) return;

            List<Doctor> doctors = await _context._doctors.ToListAsync();
            List<OfficeHours> ohs = new List<OfficeHours>();
            

            foreach (var doctor in doctors)
            {
                for (int i = 0; i < 5; i++)
                {
                    OfficeHours oh = new OfficeHours();
                    oh.Doctor = doctor;
                    oh.Closing = DateTime.Parse("18:00");
                    oh.Opening = DateTime.Parse("10:00");
                    oh.Day = (Days)i;
                    oh.Closed = false;
                    _context._officehours.Add(oh);
                }
            }
            await _context.SaveChangesAsync();
        }

    }
}
