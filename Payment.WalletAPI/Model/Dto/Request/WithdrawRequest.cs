namespace Payment.WalletAPI.Model.Dto.Request
{
    public class WithdrawRequest
    {
        public string AccountNumber { get; set; } // Change this from AccountId to AccountNumber
        public decimal Amount { get; set; }
    }

}
