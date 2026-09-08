using System;
using System.Collections.Generic;
using System.Text;

namespace EventTicketing.Application.Features.Authentication.Register
{
    public sealed record RegisterCustomerResult( bool Succeeded, Guid? UserId, IReadOnlyList<string> Errors)
    {
        public static RegisterCustomerResult Success(Guid userId) =>
            new(true, userId, []);

        public static RegisterCustomerResult Failure(IReadOnlyList<string> errors) =>
            new(false, null, errors);
    }
}
