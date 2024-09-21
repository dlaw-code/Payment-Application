using Payment.WalletAPI.Entity.Enums;

namespace Payment.WalletAPI.Entity
{
    public class Transfer
    {
        public Guid FromAccountId { get; set; } // Changed to Guid
        public Guid ToAccountId { get; set; } // Changed to Guid
        public decimal Amount { get; set; }
    }

}
