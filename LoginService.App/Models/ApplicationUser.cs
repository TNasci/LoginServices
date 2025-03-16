using Microsoft.AspNetCore.Identity;

namespace LoginService.App.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Role { get; set; }
    }
}
