using Payment.WalletAPI.Model.Dto;
using Payment.WalletAPI.Model.Dto.Request;
using Payment.WalletAPI.Model.Dto.Response;

namespace Payment.WalletAPI.Service.Interface
{
    public interface IAccountService
    {
        Task<Guid?> CreateAccountAsync(string userId, decimal initialBalance);
        Task<bool> DepositFundsAsync(string accountNumber, decimal amount);
        Task<decimal?> GetAccountBalanceAsync(Guid accountId);
        Task<decimal?> GetAccountBalanceByUserIdAsync(string userId); // Add this line
        Task<bool> TransferFundsAsync(TransferRequest request);
        Task<bool> WithdrawFundsAsync(WithdrawRequest request);
        Task<string> GenerateShortCodeAsync(ShortCodeRequest request);
        Task<bool> ConfirmTransferWithShortCodeAsync(ShortCodeConfirmation confirmation);
        Task ResetDailySpendingAsync();
        Task<bool> DeleteAccountsByUserIdAsync(string userId);
    }




}
