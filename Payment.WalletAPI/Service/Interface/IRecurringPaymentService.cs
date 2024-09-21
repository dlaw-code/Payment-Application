using Payment.WalletAPI.Entity;
using Payment.WalletAPI.Model.Dto.Request;

namespace Payment.WalletAPI.Service.Interface
{
    public interface IRecurringPaymentService
    {
        Task<RecurringPayment> CreateRecurringPaymentAsync(RecurringPaymentRequest request);
        Task ProcessRecurringPaymentsAsync(); // Ensure this is correct
    }

}
