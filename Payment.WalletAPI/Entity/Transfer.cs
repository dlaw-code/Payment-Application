using Payment.WalletAPI.Entity.Enums;

namespace Payment.WalletAPI.Entity
{
    public class Transfer 
    {
        public int FromAccountId { get; set; }
        public int ToAccountId { get; set; }
        public decimal Amount { get; set; }
    }
}
