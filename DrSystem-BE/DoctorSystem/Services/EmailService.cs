using Microsoft.Extensions.Logging;
using MailKit.Net.Smtp;
using MimeKit;
using System.Text;
using DoctorSystem.Entities;

namespace DoctorSystem.Services
{
    public class EmailService
    {

        private readonly ILogger<EmailService> _logger;

        public EmailService(ILogger<EmailService> logger)
        {
            _logger = logger;
        }
        public void SuccesfulRegistration(Client c)
        {
            //https://www.c-sharpcorner.com/article/send-email-using-templates-in-asp-net-core-applications/
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("DoctorSystemapp@gmail.com"));
            email.To.Add(MailboxAddress.Parse(c.Email));
            email.Subject = "Sikeres regisztráció!";

           
           
            
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) {
                Text = string.Format(
                @System.IO.File.ReadAllText("Services/EmailTemplates/SuccessfulRegistration.html", Encoding.UTF8), c.Name, c.MedNumber, c.BirthDate.ToShortDateString(), c.Token, c.Token)
                };
        

            // send email
            var smtp = new SmtpClient();
            smtp.CheckCertificateRevocation = false;

            smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTlsWhenAvailable);
            smtp.Authenticate("DoctorSystemapp@gmail.com", "Aw3Se4Dr5");
            smtp.Send(email);
            smtp.Disconnect(true);

        }
    }
}
