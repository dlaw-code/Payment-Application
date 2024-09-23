using Microsoft.EntityFrameworkCore;
using Payment.WalletAPI.Entity;
using Payment.WalletAPI.Entity.Enums;
using Payment.WalletAPI.Model.Dto.Request;
using Payment.WalletAPI.Service.Interface;
using System;
using System.Linq;
using System.Threading.Tasks;

public class RecurringPaymentService : IRecurringPaymentService
{
    private readonly AppDbContext _context;

    public RecurringPaymentService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<RecurringPayment> CreateRecurringPaymentAsync(RecurringPaymentRequest request)
    {
        var recurringPayment = new RecurringPayment
        {
            FromAccountNumber = request.FromAccountNumber,
            ToAccountNumber = request.ToAccountNumber,
            Amount = request.Amount,
            Frequency = request.Frequency,
            StartDate = DateTime.UtcNow,
            NextPaymentDate = GetNextPaymentDate(request.Frequency) // First payment will be processed immediately
        };

        // Process the payment immediately
        await ProcessPayment(recurringPayment);

        // Add the recurring payment to the context
        _context.RecurringPayments.Add(recurringPayment);
        await _context.SaveChangesAsync();

        return recurringPayment;
    }

    public async Task ProcessRecurringPaymentsAsync()
    {
        // Logic to process all due payments
        var duePayments = await _context.RecurringPayments
            .Where(p => p.NextPaymentDate <= DateTime.UtcNow)
            .ToListAsync();

        foreach (var payment in duePayments)
        {
            await ProcessPayment(payment);
            payment.NextPaymentDate = GetNextPaymentDate(payment.Frequency);
        }

        await _context.SaveChangesAsync(); // Save all changes after processing
    }

    private async Task ProcessPayment(RecurringPayment payment)
    {
        var fromAccount = await _context.Accounts
            .FirstOrDefaultAsync(a => a.AccountNumber == payment.FromAccountNumber);

        var toAccount = await _context.Accounts
            .FirstOrDefaultAsync(a => a.AccountNumber == payment.ToAccountNumber);

        if (fromAccount != null && toAccount != null && fromAccount.Balance >= payment.Amount)
        {
            fromAccount.Balance -= payment.Amount;
            toAccount.Balance += payment.Amount;

            await _context.SaveChangesAsync(); // Save changes to accounts
        }
        else
        {
            // Handle insufficient funds or account not found (optional)
            throw new InvalidOperationException("Insufficient funds or account not found.");
        }
    }

    private DateTime GetNextPaymentDate(RecurrenceFrequency frequency)
    {
        return frequency switch
        {
            RecurrenceFrequency.Daily => DateTime.UtcNow.AddDays(1),
            RecurrenceFrequency.Weekly => DateTime.UtcNow.AddDays(7),
            RecurrenceFrequency.Monthly => DateTime.UtcNow.AddMonths(1),
            _ => DateTime.UtcNow
        };
    }

    public async Task DeleteAccountWithRecurringPaymentsAsync(string accountNumber)
    {
        // Find the account by AccountNumber
        var account = await _context.Accounts
            .FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);

        if (account != null)
        {
            // Remove any recurring payments associated with the account
            var recurringPayments = await _context.RecurringPayments
                .Where(rp => rp.FromAccountNumber == accountNumber || rp.ToAccountNumber == accountNumber)
                .ToListAsync();

            _context.RecurringPayments.RemoveRange(recurringPayments); // Remove all associated recurring payments

            // Remove the account itself
            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync(); // Commit the changes to the database
        }
        else
        {
            throw new Exception("Account not found."); // Handle the case where the account does not exist
        }
    }
}
