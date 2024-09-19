namespace Payment.WalletAPI.Model.Dto.Request
{
    public class ShortCodeRequest
    {
        public int FromAccountId { get; set; }
        public decimal Amount { get; set; }
    }
}
