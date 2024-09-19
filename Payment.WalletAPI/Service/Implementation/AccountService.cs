using Microsoft.EntityFrameworkCore;
using Payment.WalletAPI.Entity;
using Payment.WalletAPI.Model.Dto.Request;
using Payment.WalletAPI.Model.Dto.Response;
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


    public async Task<string> GenerateShortCodeAsync(ShortCodeRequest request)
    {
        string shortCode;
        bool codeExists;

        do
        {
            shortCode = new Random().Next(1000, 9999).ToString();
            codeExists = await _context.ShortCodes.AnyAsync(sc => sc.Code == shortCode);
        } while (codeExists);

        var shortCodeEntry = new ShortCode
        {
            Code = shortCode,
            FromAccountId = request.FromAccountId,
            Amount = request.Amount,
            CreatedAt = DateTime.UtcNow // Capture the current time
        };

        _context.ShortCodes.Add(shortCodeEntry);
        await _context.SaveChangesAsync();

        return shortCode;
    }



    public async Task<bool> ConfirmTransferWithShortCodeAsync(ShortCodeConfirmation confirmation)
    {
        var shortCodeEntry = await _context.ShortCodes
            .Include(sc => sc.FromAccount)
            .SingleOrDefaultAsync(sc => sc.Code == confirmation.ShortCode);

        if (shortCodeEntry == null)
        {
            return false; // Short code not found
        }

        if ((DateTime.UtcNow - shortCodeEntry.CreatedAt).TotalMinutes > 5)
        {
            _context.ShortCodes.Remove(shortCodeEntry);
            await _context.SaveChangesAsync();
            return false; // Code has expired
        }

        var recipientAccount = await _context.Accounts.FindAsync(confirmation.ToAccountId);
        if (recipientAccount == null)
        {
            return false; // Recipient account not found
        }

        if (shortCodeEntry.Amount > shortCodeEntry.FromAccount.Balance)
        {
            return false; // Insufficient funds
        }

        // Withdraw from sender's account
        shortCodeEntry.FromAccount.Balance -= shortCodeEntry.Amount;

        // Deposit to recipient's account
        recipientAccount.Balance += shortCodeEntry.Amount;

        // Record transactions
        var withdrawalTransaction = new Transaction
        {
            AccountId = shortCodeEntry.FromAccountId,
            Amount = shortCodeEntry.Amount,
            Type = "Withdrawal",
            CreatedAt = DateTime.UtcNow
        };
        var depositTransaction = new Transaction
        {
            AccountId = confirmation.ToAccountId,
            Amount = shortCodeEntry.Amount,
            Type = "Deposit",
            CreatedAt = DateTime.UtcNow
        };

        _context.Transactions.Add(withdrawalTransaction);
        _context.Transactions.Add(depositTransaction);

        // Save changes to the accounts and transactions
        await _context.SaveChangesAsync();

        // After successful transfer, remove the short code
        _context.ShortCodes.Remove(shortCodeEntry);
        await _context.SaveChangesAsync();

        return true; // Transfer successful
    }


    public async Task<List<Transaction>> GetTransactionHistoryAsync(int accountId)
    {
        return await _context.Transactions
            .Where(t => t.AccountId == accountId)
            .OrderByDescending(t => t.CreatedAt) // Order by most recent
            .ToListAsync();
    }



}
