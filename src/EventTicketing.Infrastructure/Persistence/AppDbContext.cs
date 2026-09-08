
using EventTicketing.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EventTicketing.Infrastructure.Persistence;

// EF Core uses this class to understand and communicate with our database.
public sealed class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        //it creates Identity’s tables and mappings first
        base.OnModelCreating(builder);

        // it discovers our configuration class automatically in the configuration folder ->AppUserConfig
        builder.ApplyConfigurationsFromAssembly(
            typeof(AppDbContext).Assembly);
    }
}

