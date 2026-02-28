using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using biztrack.ostad.Domain;

namespace biztrack.ostad.Application.Data;

public static class DbInitializer
{
    public const string AdminRole = "Admin";
    public const string UserRole = "User";
    public const string DefaultAdminEmail = "admin@biztrack.local";
    public const string DefaultAdminPassword = "Admin@123";

    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        var logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("DbInitializer");
        try
        {
            using var scope = serviceProvider.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            await EnsureRoleAsync(roleManager, AdminRole);
            await EnsureRoleAsync(roleManager, UserRole);
            await EnsureAdminUserAsync(userManager, logger);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initializing the database.");
            throw;
        }
    }

    private static async Task EnsureRoleAsync(RoleManager<IdentityRole> roleManager, string roleName)
    {
        if (await roleManager.RoleExistsAsync(roleName))
            return;
        await roleManager.CreateAsync(new IdentityRole(roleName));
    }

    private static async Task EnsureAdminUserAsync(UserManager<ApplicationUser> userManager, ILogger logger)
    {
        var admin = await userManager.FindByEmailAsync(DefaultAdminEmail);
        if (admin != null)
        {
            logger.LogInformation("Default admin user already exists.");
            return;
        }

        admin = new ApplicationUser
        {
            UserName = DefaultAdminEmail,
            Email = DefaultAdminEmail,
            EmailConfirmed = true,
            FirstName = "Admin",
            LastName = "User",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        var result = await userManager.CreateAsync(admin, DefaultAdminPassword);
        if (!result.Succeeded)
        {
            logger.LogError("Failed to create default admin: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
            return;
        }

        await userManager.AddToRoleAsync(admin, AdminRole);
        logger.LogInformation("Default admin user created: {Email}", DefaultAdminEmail);
    }
}
