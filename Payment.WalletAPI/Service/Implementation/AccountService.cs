using Payment.WalletAPI.Entity;
using Payment.WalletAPI.Model.Dto.Request;
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

    public async Task<bool> TransferFundsAsync(TransferRequest request)
    {
        if (request.Amount <= 0) return false; // Validate transfer amount

        var fromAccount = await _context.Accounts.FindAsync(request.FromAccountId);
        var toAccount = await _context.Accounts.FindAsync(request.ToAccountId);

        if (fromAccount == null || toAccount == null) return false; // Check if both accounts exist
        if (fromAccount.Balance < request.Amount) return false; // Check for sufficient funds

        // Perform the transfer
        fromAccount.Balance -= request.Amount;
        toAccount.Balance += request.Amount;

        _context.Accounts.Update(fromAccount);
        _context.Accounts.Update(toAccount);
        await _context.SaveChangesAsync();

        return true;
    }


    public async Task<bool> WithdrawFundsAsync(WithdrawRequest request)
    {
        if (request.Amount <= 0) return false; // Validate withdrawal amount

        var account = await _context.Accounts.FindAsync(request.AccountId);
        if (account == null) return false; // Check if account exists
        if (account.Balance < request.Amount) return false; // Check for sufficient funds

        // Perform the withdrawal
        account.Balance -= request.Amount;

        _context.Accounts.Update(account);
        await _context.SaveChangesAsync();

        return true;
    }

}
