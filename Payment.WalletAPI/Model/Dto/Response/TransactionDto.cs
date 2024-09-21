namespace Payment.WalletAPI.Model.Dto.Response
{
    public class TransactionDto
    {
        public int Id { get; set; }
        public Guid AccountId { get; set; }
        public string AccountNumber { get; set; } // Added to show the human-readable account number
        public decimal Amount { get; set; }
        public string Type { get; set; } // e.g., "Deposit", "Withdrawal", "Transfer"
        public string TransactionDescription { get; set; } // Added for better context in transaction details
        public DateTime CreatedAt { get; set; }
    }

}
