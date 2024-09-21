using Payment.WalletAPI.Entity.Enums;

namespace Payment.WalletAPI.Model.Dto.Response
{
    public class RecurringPaymentResponseDto
    {
        public int Id { get; set; }
        public string FromAccountNumber { get; set; }
        public string ToAccountNumber { get; set; }
        public decimal Amount { get; set; }
        public RecurrenceFrequency Frequency { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime NextPaymentDate { get; set; }
        public bool IsActive { get; set; }
    }

}
