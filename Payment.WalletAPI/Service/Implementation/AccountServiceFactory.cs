using Payment.WalletAPI.Service.Interface;

namespace Payment.WalletAPI.Service.Implementation
{
    public class AccountServiceFactory : IAccountServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public AccountServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public AccountService Create()
        {
            return _serviceProvider.GetRequiredService<AccountService>();
        }

        public async Task ResetDailySpendingAsync()
        {
            var accountService = Create();
            await accountService.ResetDailySpendingAsync(); // Call the service method
        }
    }
}
