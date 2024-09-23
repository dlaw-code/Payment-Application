using Payment.WalletAPI.Entity.Enums;

namespace Payment.WalletAPI.Entity
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public Guid FromAccountId { get; set; } // Account initiating the transaction
        public Guid? ToAccountId { get; set; } // Optional (for transfers)
        public decimal Amount { get; set; }
        public TransactionType TransactionType { get; set; } // Credit, Debit, Transfer
        public string TransactionNumber { get; set; } // Unique transaction reference
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual Account FromAccount { get; set; }
        public virtual Account ToAccount { get; set; } // Optional (only for transfers)
    }

}
