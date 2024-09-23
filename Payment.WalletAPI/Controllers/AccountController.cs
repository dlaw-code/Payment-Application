using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Payment.WalletAPI.Model.Dto;
using Payment.WalletAPI.Model.Dto.Request;
using Payment.WalletAPI.Model.Dto.Response;
using Payment.WalletAPI.Service.Interface;


[ApiController]
[Route("api/accounts")]
[Authorize]
public class AccountsController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly ITransactionService _transactionService;

    public AccountsController(IAccountService accountService, ITransactionService transactionService)
    {
        _accountService = accountService;
        _transactionService = transactionService;
    }

    // POST: api/accounts
    [HttpPost("create")]
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

        return CreatedAtAction(nameof(GetBalance), new { userId = request.UserId }, new ResponseDto<object>
        {
            Result = null,
            Message = "Account created successfully"
        });

    }

    // POST: api/accounts/deposit
    [HttpPost("deposit")]
    public async Task<IActionResult> DepositFunds([FromBody] DepositRequest request)
    {
        // Call the DepositFundsAsync method with account number and amount
        var result = await _accountService.DepositFundsAsync(request.AccountNumber, request.Amount);

        if (!result)
        {
            return BadRequest(new ResponseDto<object>
            {
                IsSuccess = false,
                Message = "Deposit failed",
                Errors = new List<string> { "Invalid account number or amount." }
            });
        }

        return Ok(new ResponseDto<object>
        {
            Result = null,
            Message = "Deposit successful"
        });
    }



    // GET: api/accounts/{id}/balance
    [HttpGet("{userId}/balance")]
    public async Task<IActionResult> GetBalance(string userId)
    {
        var balance = await _accountService.GetAccountBalanceByUserIdAsync(userId);
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





    // POST: api/accounts/transfer
    [HttpPost("transfer")]
    public async Task<IActionResult> TransferFunds([FromBody] TransferRequest request)
    {
        var result = await _accountService.TransferFundsAsync(request);
        if (!result)
        {
            return BadRequest(new ResponseDto<object>
            {
                IsSuccess = false,
                Message = "Transfer failed",
                Errors = new List<string> { "Invalid accounts or insufficient funds." }
            });
        }

        return Ok(new ResponseDto<object>
        {
            Result = null,
            Message = "Transfer successful"
        });
    }



    // POST: api/accounts/withdraw
    [HttpPost("withdraw")]
    public async Task<IActionResult> WithdrawFunds([FromBody] WithdrawRequest request)
    {
        var result = await _accountService.WithdrawFundsAsync(request);
        if (!result)
        {
            return BadRequest(new ResponseDto<object>
            {
                IsSuccess = false,
                Message = "Withdrawal failed",
                Errors = new List<string> { "Invalid account or insufficient funds." }
            });
        }

        return Ok(new ResponseDto<object>
        {
            Result = null,
            Message = "Withdrawal successful"
        });
    }

    // POST: api/accounts/generate-short-code
    [HttpPost("generate-short-code")]
    public async Task<IActionResult> GenerateShortCode([FromBody] ShortCodeRequest request)
    {
        var shortCode = await _accountService.GenerateShortCodeAsync(request);
        return Ok(new ResponseDto<string>
        {
            Result = shortCode,
            Message = "Short code generated successfully."
        });
    }

    // POST: api/accounts/confirm-short-code
    [HttpPost("confirm-short-code")]
    public async Task<IActionResult> ConfirmShortCode([FromBody] ShortCodeConfirmation confirmation)
    {
        var result = await _accountService.ConfirmTransferWithShortCodeAsync(confirmation);
        if (!result)
        {
            return BadRequest(new ResponseDto<object>
            {
                IsSuccess = false,
                Message = "Transfer failed",
                Errors = new List<string> { "Invalid short code or insufficient funds." }
            });
        }

        return Ok(new ResponseDto<object>
        {
            Result = null,
            Message = "Transfer confirmed successfully."
        });
    }

    

    [HttpDelete("user/{userId}")]
    public async Task<IActionResult> DeleteAccountsByUserId(string userId)
    {
        var result = await _accountService.DeleteAccountsByUserIdAsync(userId);
        if (!result)
        {
            return NotFound(new ResponseDto<object>
            {
                IsSuccess = false,
                Message = "No accounts found for the specified user",
                Errors = new List<string> { "No accounts to delete." }
            });
        }

        return Ok(new ResponseDto<object>
        {
            Result = null,
            Message = "Accounts deleted successfully"
        });
    }




}
