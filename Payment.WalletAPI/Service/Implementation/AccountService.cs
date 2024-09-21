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

    public async Task<Guid?> CreateAccountAsync(string userId, decimal initialBalance)
    {
        // Ensure initial balance is non-negative
        if (initialBalance < 0) return null;

        // Check if an account already exists for the given user ID
        var existingAccount = await _context.Accounts
            .FirstOrDefaultAsync(a => a.UserId == userId);

        if (existingAccount != null)
        {
            // Optionally, return the existing account ID or handle as needed
            return existingAccount.Id;
        }

        // Generate a unique account number
        var accountNumber = GenerateUniqueAccountNumber();

        var account = new Account
        {
            UserId = userId,
            Balance = initialBalance,
            AccountNumber = accountNumber // Assign the generated account number
        };

        _context.Accounts.Add(account);
        await _context.SaveChangesAsync();

        return account.Id; // Return the newly created account GUID
    }

    // Function to generate a unique account number
    private string GenerateUniqueAccountNumber()
    {
        Random random = new Random();
        string accountNumber;

        do
        {
            // Generate two parts: a 5-digit and a 5-digit number to form a 10-digit account number
            string part1 = random.Next(10000, 99999).ToString(); // 5 digits
            string part2 = random.Next(10000, 99999).ToString(); // 5 digits

            accountNumber = part1 + part2; // Combine to form a 10-digit number

        } while (_context.Accounts.Any(a => a.AccountNumber == accountNumber)); // Ensure the account number is unique

        return accountNumber;
    }



    public async Task<bool> DepositFundsAsync(string accountNumber, decimal amount)
    {
        // Validate the account number format
        if (!IsValidAccountNumber(accountNumber))
        {
            return false; // Invalid account number format
        }

        // Fetch the account using the account number
        var account = await _context.Accounts
            .FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);

        if (account == null || amount <= 0) return false; // Invalid account or amount

        // Deposit the funds
        account.Balance += amount;

        await _context.SaveChangesAsync();
        return true;
    }

    private bool IsValidAccountNumber(string accountNumber)
    {
        // Implement your validation logic here
        return accountNumber.Length == 10 && long.TryParse(accountNumber, out _); // Example: 10 digits long
    }

    public async Task<decimal?> GetAccountBalanceAsync(Guid accountId)
    {
        var account = await _context.Accounts.FindAsync(accountId);
        return account?.Balance; // Return balance or null if account not found
    }

    public async Task<decimal?> GetAccountBalanceByUserIdAsync(string userId)
    {
        var account = await _context.Accounts
            .FirstOrDefaultAsync(a => a.UserId == userId);

        return account?.Balance; // Return balance or null if account not found
    }


    public async Task<bool> TransferFundsAsync(TransferRequest request)
    {
        if (request.Amount <= 0) return false; // Validate transfer amount

        // Fetch the accounts using the account numbers
        var fromAccount = await _context.Accounts
            .FirstOrDefaultAsync(a => a.AccountNumber == request.FromAccountNumber);
        var toAccount = await _context.Accounts
            .FirstOrDefaultAsync(a => a.AccountNumber == request.ToAccountNumber);

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
        var account = await _context.Accounts
            .FirstOrDefaultAsync(a => a.AccountNumber == request.AccountNumber);

        if (account == null || request.Amount <= 0)
        {
            return false; // Invalid account or amount
        }

        // Check daily spending limit
        if (account.DailySpent + request.Amount > account.DailyLimit)
        {
            return false; // Exceeds daily limit
        }

        // Check if sufficient balance
        if (account.Balance < request.Amount)
        {
            return false; // Insufficient funds
        }

        // Perform withdrawal
        account.Balance -= request.Amount;
        account.DailySpent += request.Amount;

        await _context.SaveChangesAsync();
        return true;
    }




    public async Task<string> GenerateShortCodeAsync(ShortCodeRequest request)
    {
        // Fetch the account using the account number
        var fromAccount = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == request.FromAccountNumber);
        if (fromAccount == null)
        {
            return null; // Invalid account number
        }

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
            FromAccountNumber = fromAccount.AccountNumber, // Save the account number
            Amount = request.Amount,
            CreatedAt = DateTime.UtcNow
        };

        _context.ShortCodes.Add(shortCodeEntry);
        await _context.SaveChangesAsync();

        return shortCode;
    }




    public void ResetDailySpending()
    {
        var accounts = _context.Accounts.ToList();
        foreach (var account in accounts)
        {
            account.DailySpent = 0; // Reset daily spending
        }
        _context.SaveChanges(); // Save changes to the database
    }

    public async Task ResetDailySpendingAsync()
    {
        // Logic to reset daily spending limits for all accounts
        var accounts = await _context.Accounts.ToListAsync(); // Assuming you're using EF Core
        foreach (var account in accounts)
        {
            account.DailySpent = 0; // Reset to 0 or however you want to handle it
        }
        await _context.SaveChangesAsync();
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

        // Check if the short code has expired
        if ((DateTime.UtcNow - shortCodeEntry.CreatedAt).TotalMinutes > 5)
        {
            _context.ShortCodes.Remove(shortCodeEntry);
            await _context.SaveChangesAsync();
            return false; // Code has expired
        }

        // Fetch the recipient account using the account number
        var recipientAccount = await _context.Accounts
            .FirstOrDefaultAsync(a => a.AccountNumber == confirmation.ToAccountNumber);

        if (recipientAccount == null)
        {
            return false; // Recipient account not found
        }

        // Check if the sender has sufficient balance
        if (shortCodeEntry.Amount > shortCodeEntry.FromAccount.Balance)
        {
            return false; // Insufficient funds
        }

        // Perform the transfer
        shortCodeEntry.FromAccount.Balance -= shortCodeEntry.Amount;
        recipientAccount.Balance += shortCodeEntry.Amount;

        // Record transactions
        var withdrawalTransaction = new Transaction
        {
            AccountId = shortCodeEntry.FromAccount.Id, // Use the navigation property for the sender
            Amount = shortCodeEntry.Amount,
            Type = "Withdrawal",
            CreatedAt = DateTime.UtcNow
        };
        var depositTransaction = new Transaction
        {
            AccountId = recipientAccount.Id, // Use the recipient account ID
            Amount = shortCodeEntry.Amount,
            Type = "Deposit",
            CreatedAt = DateTime.UtcNow
        };

        _context.Transactions.Add(withdrawalTransaction);
        _context.Transactions.Add(depositTransaction);

        // Save changes to the accounts and transactions
        await _context.SaveChangesAsync();

        // Remove the short code after successful transfer
        _context.ShortCodes.Remove(shortCodeEntry);
        await _context.SaveChangesAsync();

        return true; // Transfer successful
    }





    public async Task<List<Transaction>> GetTransactionHistoryAsync(Guid accountId)
    {
        return await _context.Transactions
            .Where(t => t.AccountId == accountId) // Now comparing Guid with Guid
            .OrderByDescending(t => t.CreatedAt) // Order by most recent
            .ToListAsync();
    }

    public async Task<bool> DeleteAccountsByUserIdAsync(string userId)
    {
        var accounts = await _context.Accounts
            .Where(a => a.UserId == userId)
            .ToListAsync();

        if (!accounts.Any())
        {
            return false; // No accounts found for the user
        }

        _context.Accounts.RemoveRange(accounts);
        await _context.SaveChangesAsync();

        return true; // Accounts deleted successfully
    }




}
