using System.Threading.Tasks;
using CurlCode.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace CurlCode.Infrastructure.Identity;

public class AdminSeeder
{
    public static async Task SeedAdminAsync(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var adminUser = await userManager.FindByNameAsync("mohaned178");
        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = "mohaned178",
                Email = "mohanedhesham644@gmail.com",
                IsAdmin = true
            };
            await userManager.CreateAsync(adminUser, "Mohaned123*");
        }
        else
        {
            adminUser.Email = "mohanedhesham644@gmail.com";
            adminUser.IsAdmin = true;
            await userManager.UpdateAsync(adminUser);
        }
    }
}
