using Microsoft.EntityFrameworkCore;
using Payment.WalletAPI.Entity;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<ShortCode> ShortCodes { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>()
            .Property(a => a.Balance)
            .HasColumnType("decimal(18,2)"); // Specify precision and scale

        // Configure the relationship between ShortCode and Account
        modelBuilder.Entity<ShortCode>()
            .HasOne(sc => sc.FromAccount) // ShortCode has one Account
            .WithMany() // Account can have many ShortCodes
            .HasForeignKey(sc => sc.FromAccountId) // Foreign key in ShortCode
            .OnDelete(DeleteBehavior.Cascade); // Optional: delete short codes if the account is deleted

        modelBuilder.Entity<Transaction>()
        .Property(t => t.Amount)
        .HasColumnType("decimal(18,2)"); // Specify precision and scale

        // Define relationships
        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.Account)
            .WithMany() // Optionally configure this if you have navigation property in Account
            .HasForeignKey(t => t.AccountId)
            .OnDelete(DeleteBehavior.Cascade); // Optional: delete transactions if the account is deleted
    }

}
