using Microsoft.Extensions.Logging;
using MimeKit;
using System.Text;
using DoctorSystem.Entities;
using Microsoft.Extensions.Configuration;
using System.IO;
using System;

namespace DoctorSystem.Services
{
    public class EmailService
    {

        private readonly ILogger<EmailService> _logger;
        private readonly IConfiguration _config;

        public EmailService(ILogger<EmailService> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }
        public void SuccessfulRegistration(Client c)
        {
            //https://www.c-sharpcorner.com/article/send-email-using-templates-in-asp-net-core-applications/
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("DoctorSystemapp@gmail.com"));
            email.To.Add(MailboxAddress.Parse(c.Email));
            email.Subject = "Sikeres regisztráció!";

        //TODO kép csatolás https://stackoverflow.com/questions/19910871/adding-image-to-system-net-mail-message
        //TODO kép beillesztése https://stackoverflow.com/questions/18358534/send-inline-image-in-email
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) {
                Text = string.Format(
                File.ReadAllText("Services/EmailTemplates/SuccessfulRegistration.html", Encoding.UTF8), c.Name, c.MedNumber, c.BirthDate.ToShortDateString(), c.EmailToken, c.EmailToken)
            };


            Send(email);
        }

        private void Send(MimeMessage MM)
        {
            var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.CheckCertificateRevocation = false;

            smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTlsWhenAvailable);
            smtp.Authenticate("DoctorSystemapp@gmail.com", "Aw3Se4Dr5");
            smtp.Send(MM);
            smtp.Disconnect(true);
        }

        public void NewPassword(User c)
        {
            //https://www.c-sharpcorner.com/article/send-email-using-templates-in-asp-net-core-applications/
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("DoctorSystemapp@gmail.com"));
            email.To.Add(MailboxAddress.Parse(c.Email));
            email.Subject = "Új jelszó kérelem";
            if (c.GetType() == typeof(Doctor))
            {
                email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = "<a href=\"" + _config["Root"] + "/admin/new-password/" + c.EmailToken + "\">Új jelszó</a>" };
            }
            else
            {
                email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = "<a href=\"" + _config["Root"] + "/new-password/" + c.EmailToken + "\">Új jelszó</a>" };
            }

            Send(email);
        }

     
    }
}
