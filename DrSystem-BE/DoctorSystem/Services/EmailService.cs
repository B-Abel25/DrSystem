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
        public EmailService()
        {

        }

        public EmailService(ILogger<EmailService> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }
        public void SuccessfulRegistration(Client c)
        {
            MimeMessage email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("DoctorSystemapp@gmail.com"));
            email.To.Add(MailboxAddress.Parse(c.Email));
            email.Subject = "Sikeres regisztráció!";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) {
                Text = string.Format(
                File.ReadAllText("Services/EmailTemplates/SuccessfulRegistration.html", Encoding.UTF8), c.Name, c.MedNumber, c.BirthDate.ToShortDateString(), c.EmailToken, c.EmailToken)
            };


            Send(email);
        }

        private void Send(MimeMessage MM)
        {
            MailKit.Net.Smtp.SmtpClient smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.CheckCertificateRevocation = false;
            smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTlsWhenAvailable);
            smtp.Authenticate("DoctorSystemapp@gmail.com", "Aw3Se4Dr5");
            smtp.Send(MM);
            smtp.Disconnect(true);
        }

        public void NewPassword(User u)
        {
            MimeMessage email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("DoctorSystemapp@gmail.com"));
            email.To.Add(MailboxAddress.Parse(u.Email));
            email.Subject = "Új jelszó kérelem";

           
            if (u.GetType() == typeof(Doctor))
            {
                email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                 Text = string.Format(
                 File.ReadAllText("Services/EmailTemplates/NewPassword.html", Encoding.UTF8), u.Name ,_config["Root"] + "/admin/new-password/" + u.EmailToken)
                };
            }
            else
            {
                email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = string.Format(
                File.ReadAllText("Services/EmailTemplates/NewPassword.html", Encoding.UTF8), u.Name, _config["Root"] + "/new-password/" + u.EmailToken)
                };
            }

            Send(email);
        }

        public void SendEmailToEverybody(Doctor d, string subject, string content)
        {
            MimeMessage email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(d.Email));
            foreach (var client in d.Clients)
            {
                email.Bcc.Add(MailboxAddress.Parse(client.Email));
            }
            email.Subject = subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = content,
            };

            Send(email);
        }
     
        public void AcceptClient(Client c)
        {
            MimeMessage email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("DoctorSystemapp@gmail.com"));
            email.To.Add(MailboxAddress.Parse(c.Email));
            email.Subject = "Kérelme elfogadásra került!";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = string.Format(
                File.ReadAllText("Services/EmailTemplates/AcceptClient.html", Encoding.UTF8), c.Name, c.Doctor.Name)
            };


            Send(email);
        }

        public void RefuseClient(Client c)
        {
            MimeMessage email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("DoctorSystemapp@gmail.com"));
            email.To.Add(MailboxAddress.Parse(c.Email));
            email.Subject = "Kérelme elutasításra került!";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = string.Format(
                File.ReadAllText("Services/EmailTemplates/RefuseClient.html", Encoding.UTF8), c.Name, c.Doctor.Name, c.Doctor.Email, c.Doctor.PhoneNumber)
            };


            Send(email);
        }

        public void NewAppointment(Client c)
        {
            MimeMessage email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("DoctorSystemapp@gmail.com"));
            email.To.Add(MailboxAddress.Parse(c.Email));
            email.Subject ="Foglalás rögzítve";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = string.Format(
                    //TODO paraméterek és template
                File.ReadAllText("Services/EmailTemplates/NewAppointment.html", Encoding.UTF8), c.Name, c.Doctor.Name, c.Doctor.Email, c.Doctor.PhoneNumber)
            };


            Send(email);
        }

        public void DeleteAppointment(Client c)
        {
            MimeMessage email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("DoctorSystemapp@gmail.com"));
            email.To.Add(MailboxAddress.Parse(c.Email));
            email.Subject = "Foglalás törölve!";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = string.Format(
                    //TODO paraméterek és template
                File.ReadAllText("Services/EmailTemplates/DeleteAppointment.html", Encoding.UTF8), c.Name, c.Doctor.Name, c.Doctor.Email, c.Doctor.PhoneNumber)
            };


            Send(email);
        }
    }
}
