using System;
using System.Collections.Generic;
using System.Text;

namespace EventTicketing.Application.Features.Authentication.Login;

public sealed record LoginResult(bool Succeeded, string? AccessToken, DateTimeOffset? ExpiresAt, IReadOnlyList<string> Errors)
{
    public static LoginResult Success( string accessToken, DateTimeOffset expiresAt) =>
        new(true, accessToken, expiresAt, []);

    public static LoginResult Failure() =>
        new(false, null, null, ["invalid email or password."]);
}

