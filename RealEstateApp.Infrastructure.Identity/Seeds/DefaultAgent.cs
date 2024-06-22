using Microsoft.AspNetCore.Identity;
using RealEstateApp.Core.Application.Enums;
using RealEstateApp.Infrastructure.Identity.Entities;

namespace RealEstateApp.Infrastructure.Identity.Seeds
{
    public static class DefaultAgent
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            ApplicationUser defaultAgent = new();
            defaultAgent.UserName = "basicAgent";
            defaultAgent.Email = "basicagent@email.com";
            defaultAgent.FirstName = "Emily";
            defaultAgent.LastName = "Johnson";
            defaultAgent.PhoneNumber = "829-333-3333";
            defaultAgent.IdentificationCard = "402-258963-8";
            defaultAgent.EmailConfirmed = true;
            defaultAgent.PhoneNumberConfirmed = true;
            defaultAgent.IsActive = true;

            if (userManager.Users.All(u => u.Id != defaultAgent.Id))
            {
                var user = await userManager.FindByNameAsync(defaultAgent.UserName);

                if (user == null)
                {
                    await userManager.CreateAsync(defaultAgent, "789Pa$$wOrD++");
                    await userManager.AddToRoleAsync(defaultAgent, Roles.Agent.ToString());
                }
            }
        }
    }
}
