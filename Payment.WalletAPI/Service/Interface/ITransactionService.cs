using Payment.WalletAPI.Entity;
using Payment.WalletAPI.Model.Dto.Response;

namespace Payment.WalletAPI.Service.Interface
{
    public interface ITransactionService
    {
        Task<List<TransactionDto>> GetTransactionHistoryAsync(Guid accountId);
        // Add any other methods related to transactions if needed
    }
}
