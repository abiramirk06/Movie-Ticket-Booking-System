using Microsoft.AspNetCore.Identity;
using MovieTicketBookingSystem.API.Models;

namespace MovieTicketBookingSystem.API.Data.Seed;

public static class AdminSeeder
{
    public static async Task SeedAdminAsync(
        IServiceProvider serviceProvider,
        IConfiguration configuration)
    {
        var userManager = serviceProvider
            .GetRequiredService<UserManager<ApplicationUser>>();

        var roleManager = serviceProvider
            .GetRequiredService<RoleManager<IdentityRole>>();

        var fullName = configuration["AdminAccount:FullName"];
        var email = configuration["AdminAccount:Email"];
        var password = configuration["AdminAccount:Password"];

        // Do not create an Admin account without configured credentials.
        if (string.IsNullOrWhiteSpace(fullName) ||
            string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password))
        {
            return;
        }

        const string adminRole = "Admin";

        // Ensure Admin role exists.
        if (!await roleManager.RoleExistsAsync(adminRole))
        {
            var roleResult = await roleManager.CreateAsync(
                new IdentityRole(adminRole));

            if (!roleResult.Succeeded)
            {
                throw new InvalidOperationException(
                    "Unable to create Admin role.");
            }
        }

        // Check whether the Admin account already exists.
        var adminUser = await userManager.FindByEmailAsync(email);

        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                FullName = fullName,
                Email = email,
                UserName = email,
                EmailConfirmed = true
            };

            var createResult = await userManager.CreateAsync(
                adminUser,
                password);

            if (!createResult.Succeeded)
            {
                var errors = string.Join(
                    "; ",
                    createResult.Errors.Select(e => e.Description));

                throw new InvalidOperationException(
                    $"Unable to create Admin account: {errors}");
            }
        }

        // Assign Admin role if it is not already assigned.
        if (!await userManager.IsInRoleAsync(adminUser, adminRole))
        {
            var assignmentResult = await userManager.AddToRoleAsync(
                adminUser,
                adminRole);

            if (!assignmentResult.Succeeded)
            {
                throw new InvalidOperationException(
                    "Unable to assign Admin role.");
            }
        }
    }
}