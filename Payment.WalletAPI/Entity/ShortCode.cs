namespace Payment.WalletAPI.Entity
{
    public class ShortCode
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string FromAccountNumber { get; set; } // Add AccountNumber
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation property (optional)
        public virtual Account FromAccount { get; set; }
    }

}
