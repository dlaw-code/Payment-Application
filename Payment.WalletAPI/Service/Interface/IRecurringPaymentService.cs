using Payment.WalletAPI.Entity;
using Payment.WalletAPI.Model.Dto.Request;

namespace Payment.WalletAPI.Service.Interface
{
    public interface IRecurringPaymentService
    {
        Task<RecurringPayment> CreateRecurringPaymentAsync(RecurringPaymentRequest request);
        Task ProcessRecurringPaymentsAsync();
        Task DeleteAccountWithRecurringPaymentsAsync(string accountNumber);
    }

}
