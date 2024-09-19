using Payment.WalletAPI.Entity;

namespace Payment.WalletAPI.Service.Interface
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(ApplicationUser applicationUser, IEnumerable<string> roles);
    }
}
