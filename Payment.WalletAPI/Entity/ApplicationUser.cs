using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Payment.WalletAPI.Entity
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<Account> Accounts { get; set; } // A user can have multiple accounts
    }

    // Configuration for ApplicationUser
    public class ApplicationUserConfig : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("ApplicationUsers");

            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(100);

            // Relationship: A user has many accounts
            builder.HasMany(u => u.Accounts)
                .WithOne(a => a.ApplicationUser)
                .HasForeignKey(a => a.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
