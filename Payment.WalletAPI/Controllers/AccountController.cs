using Microsoft.AspNetCore.Mvc;
using Payment.WalletAPI.Model.Dto;
using Payment.WalletAPI.Model.Dto.Request;
using Payment.WalletAPI.Model.Dto.Response;
using Payment.WalletAPI.Service.Interface;


[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountsController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    // POST: api/accounts
    [HttpPost]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountRequest request)
    {
        var accountId = await _accountService.CreateAccountAsync(request.UserId, request.InitialBalance);
        if (accountId == null)
        {
            return BadRequest(new ResponseDto<object>
            {
                IsSuccess = false,
                Message = "Account creation failed",
                Errors = new List<string> { "Unable to create account" }
            });
        }

        return CreatedAtAction(nameof(GetBalance), new { id = accountId }, new ResponseDto<object>
        {
            Result = null,
            Message = "Account created successfully"
        });
    }

    // POST: api/accounts/deposit
    [HttpPost("deposit")]
    public async Task<IActionResult> DepositFunds([FromBody] DepositRequest request)
    {
        var result = await _accountService.DepositFundsAsync(request.AccountId, request.Amount);
        if (!result)
        {
            return BadRequest(new ResponseDto<object>
            {
                IsSuccess = false,
                Message = "Deposit failed",
                Errors = new List<string> { "Invalid account or amount." }
            });
        }

        return Ok(new ResponseDto<object>
        {
            Result = null,
            Message = "Deposit successful"
        });
    }

    // GET: api/accounts/{id}/balance
    [HttpGet("{id}/balance")]
    public async Task<IActionResult> GetBalance(int id)
    {
        var balance = await _accountService.GetAccountBalanceAsync(id);
        if (balance == null)
        {
            return NotFound(new ResponseDto<object>
            {
                IsSuccess = false,
                Message = "Account not found",
                Errors = new List<string> { "The specified account does not exist." }
            });
        }

        return Ok(new ResponseDto<AccountBalanceDto>
        {
            Result = new AccountBalanceDto { Balance = balance.Value },
            Message = "Balance retrieved successfully"
        });
    }
}
