using System.Text;
using EventTicketing.Application.Features.Authentication.Register;
using EventTicketing.Infrastructure.Identity;
using EventTicketing.Infrastructure.Identity.Jwt;
using EventTicketing.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using EventTicketing.Application.Features.Authentication.Login;
using EventTicketing.Infrastructure.Identity.Services;
namespace EventTicketing.Infrastructure.DependencyInjection;

public static class AuthenticationServiceExtension
{
    public static IServiceCollection AddAuthenticationInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
       var jwtOptions = configuration
            .GetRequiredSection(JwtOptions.SectionName)
            .Get<JwtOptions>()
            ?? throw new InvalidOperationException("JWT configuration is invalid or missing.");

        services.AddDataProtection();

        services.AddIdentityCore<ApplicationUser>(options =>
        {
            options.User.RequireUniqueEmail = true;

            options.Password.RequiredLength = 8;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = false;

            //After five failed password attempts, Identity locks the account for 15 minutes.
            options.Lockout.AllowedForNewUsers = true;
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);

        })
            .AddRoles<IdentityRole<Guid>>()
            .AddSignInManager()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        services.AddOptions<JwtOptions>()
            .BindConfiguration(JwtOptions.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),

                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions.Issuer,

                    ValidateAudience = true,
                    ValidAudience = jwtOptions.Audience,

                    ValidateLifetime = true,
                    RequireExpirationTime = true,
                    ClockSkew = TimeSpan.FromMinutes(1)
                };

            });

        services.AddAuthorization();
        services.AddScoped<IRegistrationService, IdentityRegistrationService>();
        services.AddSingleton(TimeProvider.System);
        services.AddScoped<IAccessTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<ILoginService, IdentityLoginService>();

        return services;

    }
}
