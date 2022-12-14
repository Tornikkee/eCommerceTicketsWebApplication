using Payment.Models;

namespace Payment.Repositories
{
    public interface ICasinoRepository
    {
        Task<UserInfo> GetUser(string userId);
        Task<string> GetUserIdWithToken(string token);
    }
}
