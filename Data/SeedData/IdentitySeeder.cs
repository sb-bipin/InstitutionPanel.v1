using InstitutionPanel.Models.Entities;
using InstitutionPanel.Models.ValueObject;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace InstitutionPanel.Data.SeedData;

public static class IdentitySeeder
{
    public static async Task SeedAdminAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var userManager = scope.ServiceProvider
            .GetRequiredService<UserManager<ApplicationUser>>();

        const string adminEmail = "admin@institutionpanel.com.np";
        const string adminPassword = "Admin@123"; 

        var user = await userManager.FindByEmailAsync(adminEmail);

        if (user is not null)
            return;

        user = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            FirstName = "System",
            LastName = "Admin",
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "Seed",
            UserType = UserType.SuperAdmin
        };

        var result = await userManager.CreateAsync(user, adminPassword);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Admin seed failed: {errors}");
        }
    }
}
