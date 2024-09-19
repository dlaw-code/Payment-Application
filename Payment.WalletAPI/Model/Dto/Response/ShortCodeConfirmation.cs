namespace Payment.WalletAPI.Model.Dto.Response
{
    public class ShortCodeConfirmation
    {
        public string ShortCode { get; set; }
        public int ToAccountId { get; set; }
        public decimal Amount { get; set; }
    }
}
