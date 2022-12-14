namespace eCommerceTicketsWebApplication.Data.Repositories
{
    public interface ILaunchRepository
    {
        Task<string> GetTokenByUserId(string userId);
    }
}
