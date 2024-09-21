namespace Payment.WalletAPI.Model.Dto.Response
{
    public class AccountBalanceDto
    {
        public string AccountNumber { get; set; } // Newly added field for clarity
        public decimal Balance { get; set; }
    }

}
