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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Account>()
            .HasIndex(a => a.AccountNumber)
            .IsUnique();

        // Configure RecurringPayment -> FromAccount relationship with NO ACTION on delete
        modelBuilder.Entity<RecurringPayment>()
            .HasOne(rp => rp.FromAccount)
            .WithMany()
            .HasPrincipalKey(a => a.AccountNumber)
            .HasForeignKey(rp => rp.FromAccountNumber)
            .OnDelete(DeleteBehavior.Restrict);  // Changed to Restrict

        // Configure RecurringPayment -> ToAccount relationship with NO ACTION on delete
        modelBuilder.Entity<RecurringPayment>()
            .HasOne(rp => rp.ToAccount)
            .WithMany()
            .HasPrincipalKey(a => a.AccountNumber)
            .HasForeignKey(rp => rp.ToAccountNumber)
            .OnDelete(DeleteBehavior.Restrict);  // Changed to Restrict
    }



}
