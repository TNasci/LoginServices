using System.ComponentModel.DataAnnotations;

namespace LoginService.App.Models
{
    public class CreateUser
    {
        public string Email { get; set; }
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string RePassword { get; set; }
        public string Role { get; set; }
    }
}
