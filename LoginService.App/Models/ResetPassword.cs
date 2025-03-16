namespace LoginService.App.Models
{
    public class ResetPassword
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public string NewPassword { get; set; }
    }
}
