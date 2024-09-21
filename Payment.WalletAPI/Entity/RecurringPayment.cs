using Payment.WalletAPI.Entity.Enums;

namespace Payment.WalletAPI.Entity
{
    public class RecurringPayment
    {
        public int Id { get; set; }
        public string FromAccountNumber { get; set; }  // Account number of the payer
        public string ToAccountNumber { get; set; }    // Account number of the recipient
        public decimal Amount { get; set; }            // Payment amount
        public RecurrenceFrequency Frequency { get; set; }  // Frequency of the payment
        public DateTime StartDate { get; set; }        // Start date for the payment
        public DateTime? EndDate { get; set; }         // Optional: End date for the payment
        public DateTime CreatedAt { get; set; }        // Time the recurring payment was created
        public DateTime NextPaymentDate { get; set; }  // The next date when the payment will be processed
        

        // Navigation properties
        public virtual Account FromAccount { get; set; }
        public virtual Account ToAccount { get; set; }
    }

}
