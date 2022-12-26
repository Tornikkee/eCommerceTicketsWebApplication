namespace eCommerceTicketsWebApplication.Data.Repositories
{
    public interface ILaunchRepository
    {
        Task<string> GetTokenByUserId(string userId);
        Task FillUsersAndTokens(string token, string userId, string privateToken);
    }
}
