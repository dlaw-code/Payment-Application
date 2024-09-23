using Microsoft.EntityFrameworkCore;
using Payment.WalletAPI.Entity;
using Payment.WalletAPI.Model.Dto.Response;
using Payment.WalletAPI.Service.Interface;

namespace Payment.WalletAPI.Service.Implementation
{
    public class TransactionService : ITransactionService
    {
        private readonly AppDbContext _context;

        public TransactionService(AppDbContext context)
        {
            _context = context;
        }

        // Fetch transaction history for a specific account
        public async Task<List<TransactionDto>> GetTransactionHistoryAsync(Guid accountId)
        {
            // Fetch the transactions for the specified account
            var transactions = await _context.Transactions
                .Where(t => t.FromAccountId == accountId || t.ToAccountId == accountId)
                .ToListAsync();

            return transactions.Select(t => new TransactionDto
            {
                Amount = t.Amount,
                TransactionType = t.TransactionType.ToString(), // Convert enum to string
                TransactionNumber = t.TransactionNumber,
                TransactionDate = t.TransactionDate,
                Recipient = t.ToAccount != null ? t.ToAccount.AccountNumber : null // For transfers
            }).ToList();
        }



    }
}
