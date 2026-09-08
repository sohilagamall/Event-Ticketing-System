using System;
using System.Collections.Generic;
using System.Text;

namespace EventTicketing.Application.Features.Authentication.Login;
public interface IAccessTokenGenerator
{
    GeneratedAccessToken Generate(Guid userId, string email, IEnumerable<string> roles);
}
public sealed record GeneratedAccessToken(string value, DateTimeOffset ExpiresAt);

