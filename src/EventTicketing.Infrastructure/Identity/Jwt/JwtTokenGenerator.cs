using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EventTicketing.Application.Features.Authentication.Login;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
namespace EventTicketing.Infrastructure.Identity.Jwt;
public sealed class JwtTokenGenerator (IOptions<JwtOptions> jwtOptions, TimeProvider timeProvider) : IAccessTokenGenerator
{
    public GeneratedAccessToken Generate(Guid userId, string email, IEnumerable<string> roles)
    {
        var options = jwtOptions.Value;
        var now = timeProvider.GetUtcNow();
        var expiresAt = now.AddMinutes(options.AccessTokenLifetimeMinutes);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new(JwtRegisteredClaimNames.Email, email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach(var role in roles)
        {
            // This makes [Authorize(Roles = "...")] work later.
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        var signingKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(options.Key));

        var signingCredentials = new SigningCredentials(
            signingKey,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: options.Issuer,
            audience: options.Audience,
            claims: claims,
            notBefore: now.UtcDateTime,
            expires: expiresAt.UtcDateTime,
            signingCredentials: signingCredentials);

        var value = new JwtSecurityTokenHandler()
            .WriteToken(token);

        return new GeneratedAccessToken(value, expiresAt);

    }

}