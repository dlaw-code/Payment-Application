namespace Payment.WalletAPI.Entity
{
    public class ShortCode
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public Guid FromAccountId { get; set; } // Change this to Guid
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation property
        public virtual Account FromAccount { get; set; }
    }


}
