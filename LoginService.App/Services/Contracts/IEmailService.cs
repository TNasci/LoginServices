using LoginService.App.Models;

namespace LoginService.App.Services.Contracts
{
    public interface IEmailService
    {
        Task SendEmailAsync(ApplicationUser user, string subject, string message);
    }
}
