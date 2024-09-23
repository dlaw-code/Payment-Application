namespace Payment.WalletAPI.Model.Dto.Response
{
    public class TransactionDto
    {
        public decimal Amount { get; set; }
        public string TransactionType { get; set; } // Credit, Debit, Transfer
        public string TransactionNumber { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Recipient { get; set; } // Optional (for transfers)
    }


}
