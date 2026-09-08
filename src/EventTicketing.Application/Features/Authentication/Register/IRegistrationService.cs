using System;
using System.Collections.Generic;
using System.Text;

namespace EventTicketing.Application.Features.Authentication.Register;

    public interface IRegistrationService
    {
    //outcome returned after trying registration
   Task<RegisterCustomerResult> RegisterCustomerAsync(
        RegisterCustomerRequest request, CancellationToken cancellationToken = default);
}

