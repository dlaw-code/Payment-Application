namespace Payment.WalletAPI.Service.Interface
{
    public interface IAccountServiceFactory
    {
        AccountService Create();
        Task ResetDailySpendingAsync();
    }
}
