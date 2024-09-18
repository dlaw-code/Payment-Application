namespace Payment.WalletAPI.Model.Dto.Request
{
    public class DepositRequest
    {
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
    }
}
