using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Payment.WalletAPI.Entity;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<ShortCode> ShortCodes { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<RecurringPayment> RecurringPayments { get; set; }

    public DbSet<ApplicationUser> ApplicationUsers { get; set; }

    // In your AppDbContext class
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Account>()
            .HasIndex(a => a.AccountNumber)
            .IsUnique();

        // RecurringPayment relationships
        modelBuilder.Entity<RecurringPayment>()
            .HasOne(rp => rp.FromAccount)
            .WithMany() // No navigation property back to RecurringPayments
            .HasForeignKey(rp => rp.FromAccountNumber)
            .HasPrincipalKey(a => a.AccountNumber)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete

        modelBuilder.Entity<RecurringPayment>()
            .HasOne(rp => rp.ToAccount)
            .WithMany() // No navigation property back to RecurringPayments
            .HasForeignKey(rp => rp.ToAccountNumber)
            .HasPrincipalKey(a => a.AccountNumber)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete

        // ShortCode relationship
        modelBuilder.Entity<ShortCode>()
        .HasOne(sc => sc.FromAccount)
        .WithMany() // Adjust if there's a navigation property back
        .HasForeignKey(sc => sc.FromAccountId) // Ensure this matches the updated property
        .OnDelete(DeleteBehavior.Restrict); // Choose behavior as needed
    }





}
