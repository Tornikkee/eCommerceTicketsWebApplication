using eCommerceTicketsWebApplication.Data.Enums;
using eCommerceTicketsWebApplication.DTOS;
using System.Data.SqlTypes;

namespace eCommerceTicketsWebApplication.Data.Repositories
{
    public interface ITransactionsRepository
    {
        Task DepositById(decimal amount, string userId);
        Task WithdrawById(decimal amount, string userId);
        Task PayById(decimal amount, string userId);
        Task RecordTransaction(string userId, decimal amount, decimal currentBalance, TransactionType transactionType, TransactionStatus transactionStatus);
        Task<IEnumerable<TransactionHistoryDTO>> TransactionHistory(string userId);
        Task<int> GetLastTransactionId();
        Task<string> GetUserId();
        Task Bet(decimal amount, string userId);
        Task RecordCasinoTransaction(string userId, decimal amount, decimal currentBalance, TransactionStatus transactionStatus, string currency);
    }
}
