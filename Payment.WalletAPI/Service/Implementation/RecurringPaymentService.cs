using Microsoft.EntityFrameworkCore;
using Payment.WalletAPI.Entity;
using Payment.WalletAPI.Entity.Enums;
using Payment.WalletAPI.Model.Dto.Request;
using Payment.WalletAPI.Service.Interface;



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
            NextPaymentDate = DateTime.UtcNow, // First payment is processed immediately
        };

        _context.RecurringPayments.Add(recurringPayment);
        await _context.SaveChangesAsync();

        return recurringPayment;
    }

    public async Task ProcessRecurringPaymentsAsync()
    {
        var now = DateTime.UtcNow;
        var duePayments = await _context.RecurringPayments
            .Where(rp => rp.NextPaymentDate <= now)
            .ToListAsync();

        foreach (var payment in duePayments)
        {
            // Logic to process the payment goes here

            // Convert the frequency to a TimeSpan and schedule the next payment
            payment.NextPaymentDate = GetNextPaymentDate(payment.NextPaymentDate, payment.Frequency);
        }

        await _context.SaveChangesAsync();
    }

    private DateTime GetNextPaymentDate(DateTime currentDate, RecurrenceFrequency frequency)
    {
        return frequency switch
        {
            RecurrenceFrequency.Daily => currentDate.AddDays(1),
            RecurrenceFrequency.Weekly => currentDate.AddDays(7),
            RecurrenceFrequency.Monthly => currentDate.AddMonths(1),
            //RecurrenceFrequency.Yearly => currentDate.AddYears(1),
            _ => currentDate // Default case
        };
    }

}
