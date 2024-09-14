namespace Payment.WalletAPI.Model.Dto
{
    public class TransactionDto
    {
        public int Id { get; set; }
        public int AccountId { get; set; } // ID of the account associated with the transaction
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; } // E.g., "Deposit", "Withdrawal", "Transfer"
    }
}
