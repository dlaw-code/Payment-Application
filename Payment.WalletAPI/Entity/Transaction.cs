namespace Payment.WalletAPI.Entity
{
    public class Transaction
    {
        public int Id { get; set; }
        public int AccountId { get; set; } // The account involved in the transaction
        public decimal Amount { get; set; } // Amount for the transaction
        public string Type { get; set; } // "Deposit" or "Withdrawal"
        public DateTime CreatedAt { get; set; } // Timestamp of the transaction

        // Navigation property
        public virtual Account Account { get; set; }
    }
}
