using Payment.WalletAPI.Entity.Enums;

namespace Payment.WalletAPI.Entity
{
    public class Payment 
    {
        public int Id { get; set; }
        public string PaymentMethod { get; set; } // e.g., "CreditCard", "BankTransfer", "Crypto"
        public decimal Amount { get; set; }
        public string Currency { get; set; } // e.g., "USD", "EUR"
        public string TransactionId { get; set; } // Unique identifier from the payment gateway
        public DateTime PaymentDate { get; set; } // Date and time of the payment
        public PaymentStatus Status { get; set; } // Enum for tracking payment status
        public string CustomerId { get; set; } // Reference to the customer
        public string Description { get; set; } // Description of what the payment was for
    }
}
