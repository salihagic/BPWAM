using BPWA.Common.Configuration;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BPWA.Common.Services
{
    public class EmailService : IEmailService
    {
        private EmailSettings _settings;

        public EmailService(EmailSettings settings)
        {
            _settings = settings;
        }

        public async Task Send(string to, string subject, string body)
        {
            MailMessage mail = new MailMessage();
            SmtpClient smtpServer = new SmtpClient(_settings.MailServer);
            mail.From = new MailAddress(_settings.SenderEmail);
            mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = body;
            smtpServer.Port = _settings.MailPort;
            smtpServer.Credentials = new System.Net.NetworkCredential(_settings.SenderEmail, _settings.SenderEmailPassword);
            smtpServer.EnableSsl = true;
            await smtpServer.SendMailAsync(mail);
        }
    }
}
