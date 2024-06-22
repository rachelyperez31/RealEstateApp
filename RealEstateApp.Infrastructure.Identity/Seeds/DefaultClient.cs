using Microsoft.AspNetCore.Identity;
using RealEstateApp.Core.Application.Enums;
using RealEstateApp.Infrastructure.Identity.Entities;

namespace RealEstateApp.Infrastructure.Identity.Seeds
{
    public static class DefaultClient
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            ApplicationUser defaultClient = new();
            defaultClient.UserName = "basicClient";
            defaultClient.Email = "basicclient@email.com";
            defaultClient.FirstName = "James";
            defaultClient.LastName = "Clark";
            defaultClient.PhoneNumber = "829-444-4444";
            defaultClient.IdentificationCard = "402-963741-5";
            defaultClient.EmailConfirmed = true;
            defaultClient.PhoneNumberConfirmed = true;
            defaultClient.IsActive = true;

            if (userManager.Users.All(u => u.Id != defaultClient.Id))
            {
                var user = await userManager.FindByNameAsync(defaultClient.UserName);

                if (user == null)
                {
                    await userManager.CreateAsync(defaultClient, "246Pa$$wOrD++");
                    await userManager.AddToRoleAsync(defaultClient, Roles.Client.ToString());
                }
            }
        }
    }
}
