using System.ComponentModel;

namespace LoginService.App.Models.Enums
{
    public enum UserType
    {
        [Description("Admin")]
        Admin,

        [Description("Doctor")]
        Doctor,

        [Description("Pacient")]
        Pacient
    }
}
