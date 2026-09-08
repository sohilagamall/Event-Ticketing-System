using EventTicketing.Application.Features.Authentication.Login;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventTicketing.Infrastructure.Identity.Services;
public sealed class IdentityLoginService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IAccessTokenGenerator accessTokenGenerator) : ILoginService
{
    public async Task<LoginResult> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default) 
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var user = await userManager.FindByEmailAsync(email);
        if(user is null || string.IsNullOrWhiteSpace(user.Email))
        {
            return LoginResult.Failure();
        }

        // Identity verifies the password hash and records failed attempts.
        // After the configured limit, it locks the account temporarily (15 minutes by default -> lockout).
        var signInResult = await signInManager.CheckPasswordSignInAsync(
            user,
            request.Password,
            lockoutOnFailure: true);
        if(!signInResult.Succeeded)
        {
            return LoginResult.Failure();
        }

        var roles = await userManager.GetRolesAsync(user);
        var accessToken = accessTokenGenerator.Generate(user.Id, user.Email, roles);

        return LoginResult.Success(accessToken.value, accessToken.ExpiresAt);

    }
}

