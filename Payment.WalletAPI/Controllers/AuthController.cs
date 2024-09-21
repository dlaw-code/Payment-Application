using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Payment.WalletAPI.Model.Dto.Request;
using Payment.WalletAPI.Model.Dto;
using Payment.WalletAPI.Service.Interface;
using Payment.WalletAPI.Model.Dto.Response;

namespace Payment.WalletAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        protected ResponseDto<object> _response; // Assuming it's a generic response

        public AuthController(IAuthService authService)
        {
            _authService = authService;
            _response = new ResponseDto<object>(); // Default to object if generic type is uncertain
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto model)
        {
            var errorMessage = await _authService.Register(model);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                _response.IsSuccess = false;
                _response.Message = errorMessage;
                return BadRequest(_response);
            }
            return Ok(_response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            var loginResponse = await _authService.Login(model);
            var response = new ResponseDto<LoginResponseDto>(); // Assuming loginResponse is of type LoginResponseDto

            if (loginResponse.User == null)
            {
                response.IsSuccess = false;
                response.Message = "Username or password is incorrect";
                response.Errors.Add("Invalid credentials");
                return BadRequest(response);
            }

            response.Result = loginResponse;
            return Ok(response);
        }

        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDto model)
        {
            var assignRoleSuccessful = await _authService.AssignRole(model.Email, model.Role.ToUpper());
            var response = new ResponseDto<bool>(); // Assuming boolean result

            if (!assignRoleSuccessful)
            {
                response.IsSuccess = false;
                response.Message = "Error encountered";
                response.Errors.Add("Invalid credentials");
                return BadRequest(response);
            }

            response.Result = assignRoleSuccessful;
            return Ok(response);
        }
    }

}

