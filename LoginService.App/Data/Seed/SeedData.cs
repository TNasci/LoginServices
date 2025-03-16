using LoginService.App.Models;
using LoginService.App.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Extensions;

namespace LoginService.App.Data.Seed
{
    public class SeedData
    {
        public static async Task Inicialize(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roles = { UserType.Admin.GetDisplayName(), UserType.Doctor.GetDisplayName(), UserType.Pacient.GetDisplayName() };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var adminUser = new ApplicationUser { UserName = "Admin", Email = "admin@exemple.com", Role = "Admin" };
            await CreateUserIfNotExists(userManager, adminUser, "Password!123", "Admin");

            var doctorUser = new ApplicationUser { UserName = "Doctor", Email = "doctor@exemple.com", Role = "Doctor" };
            await CreateUserIfNotExists(userManager, doctorUser, "Password!123", "Doctor");

            var pacientUser = new ApplicationUser { UserName = "Pacient", Email = "pacient@exemple.com", Role = "Pacient" };
            await CreateUserIfNotExists(userManager, pacientUser, "Password!123", "Pacient");


        }

        #region Private Method
        private static async Task CreateUserIfNotExists(UserManager<ApplicationUser> userManager, ApplicationUser user, string password, string role)
        {
            if (await userManager.FindByEmailAsync(user.Email) == null)
            {
                await userManager.CreateAsync(user, password);
                await userManager.AddToRoleAsync(user, role);
            }
        }
        #endregion
    }
}
