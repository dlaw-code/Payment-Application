using Payment.WalletAPI.Model.Dto.Request;
using Payment.WalletAPI.Model.Dto.Response;

namespace Payment.WalletAPI.Service.Interface
{
    public interface IAuthService
    {
        Task<string> Register(RegistrationRequestDto registrationRequestDto);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
        Task<bool> AssignRole(string email, string roleName);
    }
}
