using EventTicketing.Application.Features.Authentication.Register;
using EventTicketing.Application.Features.Authorization;
using EventTicketing.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;


namespace EventTicketing.Infrastructure.Identity
{
    public sealed class IdentityRegistrationService(UserManager<ApplicationUser> userManager, AppDbContext dbContext) : IRegistrationService
    {
        public async Task<RegisterCustomerResult> RegisterCustomerAsync(RegisterCustomerRequest request, CancellationToken cancellationToken = default)
        {
            // A user and their Customer role must be created together.
            await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

            var user = new ApplicationUser
            {
                FirstName = request.FirstName.Trim(),
                LastName = request.LastName.Trim(),
                Email = request.Email.Trim(),
                UserName = request.Email.Trim(),
                CreatedAt = DateTimeOffset.UtcNow
            };

            // Identity hashes Password before saving it.
            var createResult = await userManager.CreateAsync(user, request.Password);
            if (!createResult.Succeeded)
            {
                await transaction.RollbackAsync(cancellationToken);
                return RegisterCustomerResult.Failure(
                    createResult.Errors
                    .Select(e => e.Description)
                    .ToList());
            }
            var roleResult = await userManager.AddToRoleAsync(user, ApplicationRoles.Customer);
            if (!roleResult.Succeeded)
            {
                await transaction.RollbackAsync(cancellationToken);
                return RegisterCustomerResult.Failure(
                    roleResult.Errors
                    .Select(e => e.Description)
                    .ToList());
            }
            await transaction.CommitAsync(cancellationToken);
            return RegisterCustomerResult.Success(user.Id);
        }
    }
}
