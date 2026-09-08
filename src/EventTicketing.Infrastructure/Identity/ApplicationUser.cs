
using Microsoft.AspNetCore.Identity;
namespace EventTicketing.Infrastructure.Identity;

// Inherits secure user behavior from ASP.NET Core Identity.
public sealed class ApplicationUser : IdentityUser<Guid>
{
    // Public profile data. Password-related fields remain owned by Identity.
    public string FirstName { get; set; } = string.Empty;   
    public string LastName { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

}

