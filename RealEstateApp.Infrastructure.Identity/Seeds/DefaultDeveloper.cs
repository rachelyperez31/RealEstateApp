using Microsoft.AspNetCore.Identity;
using RealEstateApp.Core.Application.Enums;
using RealEstateApp.Infrastructure.Identity.Entities;

namespace RealEstateApp.Infrastructure.Identity.Seeds
{
    public static class DefaultDeveloper
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            ApplicationUser defaultDeveloper = new();
            defaultDeveloper.UserName = "basicDeveloper";
            defaultDeveloper.Email = "basicdeveloper@email.com";
            defaultDeveloper.FirstName = "David";
            defaultDeveloper.LastName = "Brown";
            defaultDeveloper.PhoneNumber = "829-222-2222";
            defaultDeveloper.IdentificationCard = "402-98765432-1";
            defaultDeveloper.EmailConfirmed = true;
            defaultDeveloper.PhoneNumberConfirmed = true;
            defaultDeveloper.IsActive = true;

            if (userManager.Users.All(u => u.Id != defaultDeveloper.Id))
            {
                var user = await userManager.FindByNameAsync(defaultDeveloper.UserName);

                if (user == null)
                {
                    await userManager.CreateAsync(defaultDeveloper, "456Pa$$wOrD++");
                    await userManager.AddToRoleAsync(defaultDeveloper, Roles.Developer.ToString());
                }
            }
        }
    }
}
