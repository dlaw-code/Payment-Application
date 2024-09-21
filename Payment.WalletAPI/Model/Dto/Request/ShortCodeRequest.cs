namespace Payment.WalletAPI.Model.Dto.Request
{
    public class ShortCodeRequest
    {
        public string FromAccountNumber { get; set; } // Now using AccountNumber instead of AccountId
        public decimal Amount { get; set; }
    }
}
