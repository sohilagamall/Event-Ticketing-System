using EventTicketing.Application.Features.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace EventTicketing.Infrastructure.Identity;

// Creates the system roles if they are missing.
// It is safe to run repeatedly.
public static class IdentityRoleSeeder
{
    public static async Task SeedRolesAsync(this IServiceProvider serviceProvider)
    {
        // RoleManager is a scoped service, so create a scope for this startup work.
        // This is a common pattern in ASP.NET Core for doing work at startup that requires scoped services, means it works just once when the application starts, and it can use services that are registered with a scoped lifetime.
        using var scope = serviceProvider.CreateScope();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        foreach(var roleName in ApplicationRoles.AllRoles)
        {
            if (await roleManager.RoleExistsAsync(roleName))
            {
                continue; // Skip if the role already exists
            }

            var result = await roleManager.CreateAsync(
                new IdentityRole<Guid>(roleName));
            if(!result.Succeeded)
            {
                var errors = string.Join(
                    ", ", 
                    result.Errors.Select(e => e.Description));

                throw new Exception($"Failed to create role '{roleName}': {errors}");
            }
        }
         

    }
        
}

