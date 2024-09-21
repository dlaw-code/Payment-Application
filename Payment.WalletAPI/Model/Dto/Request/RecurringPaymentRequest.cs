using Payment.WalletAPI.Entity.Enums;

namespace Payment.WalletAPI.Model.Dto.Request
{
    public class RecurringPaymentRequest
    {
        public string FromAccountNumber { get; set; }
        public string ToAccountNumber { get; set; }
        public decimal Amount { get; set; }
        public RecurrenceFrequency Frequency { get; set; }
    }

}
