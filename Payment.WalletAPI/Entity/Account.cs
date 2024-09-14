using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Payment.WalletAPI.Entity
{
    public class Account
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; } // Navigation property to ApplicationUser

        public ICollection<Transaction> Transactions { get; set; } // An account has many transactions
    }

    // Configuration for Account entity
    public class AccountConfig : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.AccountNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(a => a.Balance)
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0);

            // Relationship: An account belongs to one ApplicationUser
            builder.HasOne(a => a.ApplicationUser)
                .WithMany(u => u.Accounts)
                .HasForeignKey(a => a.ApplicationUserId);

            // Relationship: An account has many transactions
            builder.HasMany(a => a.Transactions)
                .WithOne(t => t.Account)
                .HasForeignKey(t => t.AccountId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
