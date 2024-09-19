using Microsoft.AspNetCore.Identity;

namespace Payment.WalletAPI.Entity
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
