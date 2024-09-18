using Payment.WalletAPI.Model.Dto;
using Payment.WalletAPI.Model.Dto.Request;

namespace Payment.WalletAPI.Service.Interface
{
    public interface IAccountService
    {
        Task<int?> CreateAccountAsync(string userId, decimal initialBalance);
        Task<bool> DepositFundsAsync(int accountId, decimal amount);
        Task<decimal?> GetAccountBalanceAsync(int accountId);
        Task<bool> TransferFundsAsync(TransferRequest request);
        Task<bool> WithdrawFundsAsync(WithdrawRequest request);


    }


}
