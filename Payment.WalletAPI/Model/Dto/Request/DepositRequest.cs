namespace Payment.WalletAPI.Model.Dto.Request
{
    public class DepositRequest
    {
        public string AccountNumber { get; set; } // Change to string for account number
        public decimal Amount { get; set; }
    }

}
