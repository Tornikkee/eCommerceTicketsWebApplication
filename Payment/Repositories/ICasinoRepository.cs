using eCommerceTicketsWebApplication.Data.Enums;
using Payment.Models;

namespace Payment.Repositories
{
    public interface ICasinoRepository
    {
        Task<UserInfo> GetUser(string userId);
        Task<string> GetUserIdWithToken(string token);
        Task<string> GetPrivateToken(string userId);
        int Bet(decimal amount, string transactionId, BetType betType, string currency, string userId, out decimal currentBalance, out int internalTransactionId);
        int Win(decimal amount, string transactionId, WinType winType, string currency, string userId, out decimal currentBalance, out int internalTransactionId);
        int CancelBet(decimal amount, string transactionId, BetType betType, string currency, string userId, string betTransactionId, out decimal currentBalance, out int internalTransactionId);
    }
}
