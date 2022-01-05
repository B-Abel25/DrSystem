using Microsoft.Extensions.Logging;
using MailKit.Net.Smtp;
using MimeKit;

namespace DoctorSystem.Services
{
    public class EmailService
    {

        private readonly ILogger<EmailService> _logger;

        public EmailService(ILogger<EmailService> logger)
        {
            _logger = logger;
        }
        public void sendEmail(string to, string content, string _subject)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("popsimon00@outlook.hu"));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = _subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = content };

            // send email
            var smtp = new SmtpClient();
            smtp.CheckCertificateRevocation = false;

            smtp.Connect("smtp.office365.com", 587, MailKit.Security.SecureSocketOptions.StartTlsWhenAvailable);
            smtp.Authenticate("popsimon00@outlook.hu", "Popsitapsi");
            smtp.Send(email);
            smtp.Disconnect(true);

        }
    }
}
