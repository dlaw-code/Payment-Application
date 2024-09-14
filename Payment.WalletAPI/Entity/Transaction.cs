using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;

namespace Payment.WalletAPI.Entity
{
    public class Transaction
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; } // Navigation property to Account

        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; } // E.g., "Deposit", "Withdrawal", "Transfer"
    }

    // Configuration for Transaction entity
    public class TransactionConfig : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Amount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(t => t.TransactionDate)
                .IsRequired();

            builder.Property(t => t.TransactionType)
                .HasMaxLength(50)
                .IsRequired();

            // Relationship: A transaction belongs to one account
            builder.HasOne(t => t.Account)
                .WithMany(a => a.Transactions)
                .HasForeignKey(t => t.AccountId);
        }
    }
}
