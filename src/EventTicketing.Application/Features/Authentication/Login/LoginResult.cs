using System;
using System.Collections.Generic;
using System.Text;

namespace EventTicketing.Application.Features.Authentication.Login;

public sealed record LoginResult(bool Succeeded, string? AccessToken, DateTimeOffset? ExpiresAT, IReadOnlyList<string> Errors)
{
    public 
}

