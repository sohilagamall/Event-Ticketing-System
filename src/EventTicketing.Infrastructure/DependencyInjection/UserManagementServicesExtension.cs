using EventTicketing.Application.Features.UserManagement.ListUsers;
using EventTicketing.Infrastructure.Identity.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EventTicketing.Infrastructure.DependencyInjection;

public static class UserManagementServiceCollectionExtensions
{
    public static IServiceCollection AddUserManagementInfrastructure(
        this IServiceCollection services)
    {
        services.AddScoped<IUserQueryService, IdentityUserQueryService>();

        return services;
    }
}