using Microsoft.AspNetCore.Identity;
using RealEstateApp.Core.Application.Enums;
using RealEstateApp.Infrastructure.Identity.Entities;

namespace RealEstateApp.Infrastructure.Identity.Seeds
{
    public static class DefaultAdmin
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            ApplicationUser defaultAdmin = new();
            defaultAdmin.UserName = "basicAdmin";
            defaultAdmin.Email = "basicadmin@email.com";
            defaultAdmin.FirstName = "Jane";
            defaultAdmin.LastName = "Smith";
            defaultAdmin.PhoneNumber = "829-111-1111";
            defaultAdmin.IdentificationCard = "402-1234567-8";
            defaultAdmin.EmailConfirmed = true;
            defaultAdmin.PhoneNumberConfirmed = true;
            defaultAdmin.IsActive = true;

            if (userManager.Users.All(u => u.Id != defaultAdmin.Id))
            {
                var user = await userManager.FindByNameAsync(defaultAdmin.UserName);

                if (user == null)
                {
                    await userManager.CreateAsync(defaultAdmin, "123Pa$$wOrD++");
                    await userManager.AddToRoleAsync(defaultAdmin, Roles.Admin.ToString());
                }
            }
        }
    }
}
