namespace Payment.WalletAPI.Model.Dto.Request
{
    public class WithdrawRequest
    {
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
    }
}
