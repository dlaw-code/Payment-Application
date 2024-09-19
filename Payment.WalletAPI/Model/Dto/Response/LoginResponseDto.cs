using Payment.WalletAPI.Model.Dto.Request;

namespace Payment.WalletAPI.Model.Dto.Response
{
    public class LoginResponseDto
    {
        public UserDto User { get; set; }
        public string Token { get; set; }
    }
}
