using eCommerceTicketsWebApplication.Data.Enums;
using Microsoft.Extensions.Configuration.UserSecrets;
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
        int ChangeWin(decimal amount, decimal previousAmount, string transactionId, string previousTransactionId, string userId, string currency, out decimal currentBalance, out int internalTransactionId);
    }
}
