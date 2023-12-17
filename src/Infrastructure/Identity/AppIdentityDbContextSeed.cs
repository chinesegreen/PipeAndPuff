using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity;

public class AppIdentityDbContextSeed
{
    public static async Task SeedAsync(AppIdentityDbContext identityDbContext, UserManager<ApplicationUser> userManager)
    {

        if (identityDbContext.Database.IsNpgsql())
        {
            identityDbContext.Database.Migrate();
        }

        string adminUserName = "admin@microsoft.com";
        var adminUser = new ApplicationUser { UserName = adminUserName, Email = adminUserName };
        await userManager.CreateAsync(adminUser, "Admin6");
        adminUser = await userManager.FindByNameAsync(adminUserName);
        if (adminUser != null)
        {
            var claim = new Claim("IsAdmin", "Y");
            await userManager.AddClaimAsync(adminUser, claim);
        }
    }
}
