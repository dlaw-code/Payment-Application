namespace Payment.WalletAPI.Model.Dto
{
    public class AccountDto
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public string ApplicationUserId { get; set; } // ID of the user that owns the account
        public string ApplicationUserFullName { get; set; } // Could be added for convenience to display user's name
        public List<TransactionDto> Transactions { get; set; }
    }
}
