using Microsoft.AspNetCore.Mvc;
using Payment.WalletAPI.Model.Dto.Request;
using Payment.WalletAPI.Model.Dto;
using Payment.WalletAPI.Service.Interface;

[ApiController]
[Route("api/[controller]")]
public class RecurringPaymentController : ControllerBase
{
    private readonly IRecurringPaymentService _recurringPaymentService;

    public RecurringPaymentController(IRecurringPaymentService recurringPaymentService)
    {
        _recurringPaymentService = recurringPaymentService;
    }

    [HttpPost("recurring")]
    public async Task<IActionResult> CreateRecurringPayment([FromBody] RecurringPaymentRequest request)
    {
        var recurringPayment = await _recurringPaymentService.CreateRecurringPaymentAsync(request);

        return Ok(new ResponseDto<object>
        {
            Message = "Recurring payment set up successfully",
            Result = recurringPayment
        });
    }
}
