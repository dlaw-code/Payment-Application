using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Payment.WalletAPI.Model.Dto.Request;
using Payment.WalletAPI.Service.Interface;
using System;
using System.Threading.Tasks;

namespace Payment.WalletAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IAccountService _accountService; // Injecting AccountService

        public TransactionController(ITransactionService transactionService, IAccountService accountService)
        {
            _transactionService = transactionService;
            _accountService = accountService;
        }

        [HttpGet("{accountId}/history")]
        public async Task<IActionResult> GetTransactionHistory(Guid accountId)
        {
            var transactions = await _transactionService.GetTransactionHistoryAsync(accountId);
            return Ok(transactions);
        }

    }
}
