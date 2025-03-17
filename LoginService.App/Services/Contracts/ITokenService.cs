using LoginService.App.Models;

namespace LoginService.App.Services.Contracts
{
    public interface ITokenService
    {
        Task<string> GenerateToken(Login login);
    }

}
