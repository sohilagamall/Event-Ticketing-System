using EventTicketing.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace EventTicketing.Infrastructure.Persistence.Configurations;

public sealed class ApplicationUserConfig : IEntityTypeConfiguration<ApplicationUser>
{ 
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(user => user.FirstName)
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(user => user.LastName)
            .HasMaxLength(100)
            .IsRequired();
        builder.Property(user => user.CreatedAt)
            .IsRequired();
        builder.Property(user => user.UpdatedAt);
            
    }
}

