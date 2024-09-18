namespace Payment.WalletAPI.Model.Dto.Request
{
    public class CreateAccountRequest
    {
        public string UserId { get; set; }
        public decimal InitialBalance { get; set; }
    }
}
