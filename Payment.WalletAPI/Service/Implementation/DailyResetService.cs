using Payment.WalletAPI.Service.Interface;

namespace Payment.WalletAPI.Service.Implementation
{
    public class DailyResetService : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly IServiceScopeFactory _serviceScopeFactory; // Use IServiceScopeFactory
        private readonly ILogger<DailyResetService> _logger;

        public DailyResetService(IServiceScopeFactory serviceScopeFactory, ILogger<DailyResetService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(ResetSpending, null, TimeSpan.Zero, TimeSpan.FromDays(1));
            return Task.CompletedTask;
        }

        private void ResetSpending(object state)
        {
            using (_logger.BeginScope("Resetting daily spending limits"))
            {
                using (var scope = _serviceScopeFactory.CreateScope()) // Create a scope
                {
                    var accountService = scope.ServiceProvider.GetRequiredService<IAccountService>();
                    accountService.ResetDailySpendingAsync();
                    _logger.LogInformation("Daily spending limits reset successfully.");
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }


}
