using System;
using System.Collections.Generic;
using System.Text;

namespace EventTicketing.Application.Features.Authentication.Login;
public interface ILoginService
{
    Task<LoginResult> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
}

