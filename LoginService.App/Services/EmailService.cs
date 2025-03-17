using LoginService.App.Models;
using LoginService.App.Services.Contracts;
using MailKit.Net.Smtp;
using MimeKit;

namespace LoginService.App.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpServer = "SmtpServer";
        private readonly int _smtpPort = 587;
        private readonly string _smtpUser = "SmtpUser";
        private readonly string _smtpPass = "SmtpPass";
        public async Task SendEmailAsync(ApplicationUser user, string subject, string message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Admin", _smtpUser));
            emailMessage.To.Add(new MailboxAddress(user.UserName, user.Email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("html") { Text = message };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_smtpServer, _smtpPort, false);
                await client.AuthenticateAsync(_smtpUser, _smtpPass);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
