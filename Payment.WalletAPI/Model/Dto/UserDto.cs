namespace Payment.WalletAPI.Model.Dto
{
    public class UserDto
    {
        public string Id { get; set; } // This is IdentityUser's ID
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; } // From IdentityUser
        public List<AccountDto> Accounts { get; set; }
    }
}
