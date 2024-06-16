using BeautySaloon.DAL;
using BeautySaloon.DAL.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BeautySaloon.Helpers;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        using var context = new BeautySaloonDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<BeautySaloonDbContext>>());
        
        var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        string[] roleNames = { "Admin", "User", "Worker" };
        IdentityResult roleResult;

        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                roleResult = await roleManager.CreateAsync(new ApplicationRole(roleName));
            }
        }

        var adminUser = new ApplicationUser
        {
            FirstName = "Алеся",
            SecondName = "Ковыряева",
            LastName = "Алексеевна",
            PhoneNumber = "+79501032393",
            UserName = "Alesya@gmail.com",
            Email = "Alesya.2002@gmail.com",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true
        };

        var userPassword = "123Qwe";
        var user = await userManager.FindByEmailAsync(adminUser.Email);

        if (user == null)
        {
            var createPowerUser = await userManager.CreateAsync(adminUser, userPassword);
            if (createPowerUser.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}