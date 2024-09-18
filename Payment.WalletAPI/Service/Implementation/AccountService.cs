using Payment.WalletAPI.Entity;
using Payment.WalletAPI.Service.Interface;
using System;

public class AccountService : IAccountService
{
    private readonly AppDbContext _context;

    public AccountService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int?> CreateAccountAsync(string userId, decimal initialBalance)
    {
        if (initialBalance < 0) return null; // Ensure initial balance is non-negative

        var account = new Account
        {
            UserId = userId,
            Balance = initialBalance
        };

        _context.Accounts.Add(account);
        await _context.SaveChangesAsync();

        return account.Id; // Return the newly created account ID
    }

    public async Task<bool> DepositFundsAsync(int accountId, decimal amount)
    {
        if (amount <= 0) return false; // Validate deposit amount

        var account = await _context.Accounts.FindAsync(accountId);
        if (account == null) return false; // Check if account exists

        account.Balance += amount; // Update balance
        _context.Accounts.Update(account);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<decimal?> GetAccountBalanceAsync(int accountId)
    {
        var account = await _context.Accounts.FindAsync(accountId);
        return account?.Balance; // Return balance or null if account not found
    }
}
