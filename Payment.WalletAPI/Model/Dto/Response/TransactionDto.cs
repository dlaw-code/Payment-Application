namespace Payment.WalletAPI.Model.Dto.Response
{
    public class TransactionDto
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; } // e.g., "Deposit", "Withdrawal", "Transfer"
        public DateTime CreatedAt { get; set; }
    }
}
