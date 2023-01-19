using Dapper;
using eCommerceTicketsWebApplication.Data.Enums;
using eCommerceTicketsWebApplication.DTOS;
using eCommerceTicketsWebApplication.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.SqlTypes;

namespace eCommerceTicketsWebApplication.Data.Repositories
{
    public class TransactionsRepository : ITransactionsRepository
    {
        SqlConnection connection;
        readonly string connectionString;


        public TransactionsRepository()
        {
            connectionString = "Data Source=localhost;Initial Catalog=eCommerceTicketsDb;Integrated Security=True;Pooling=False;TrustServerCertificate=True";
            connection = new SqlConnection(connectionString);
        }


        public async Task DepositById(decimal amount, string userId)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@UserId", userId);

            using(IDbConnection db = new SqlConnection("Data Source=localhost;Initial Catalog=eCommerceTicketsDb;Integrated Security=True;Pooling=False;TrustServerCertificate=True"))
            {
                var wallet = await db.QueryFirstOrDefaultAsync<Wallet>("GetWalletByUserId", dp, commandType: CommandType.StoredProcedure);

                dp = new DynamicParameters();
                dp.Add("@UserId", userId);
                dp.Add("@Balance", wallet.Balance + amount);

                await db.ExecuteAsync("UpdateBalanceById", dp, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task PayById(decimal amount, string userId)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@UserId", userId);

            using (IDbConnection db = connection)
            {
                decimal balance = await db.QueryFirstOrDefaultAsync<decimal>("GetBalance", dp, commandType: CommandType.StoredProcedure);

                dp = new DynamicParameters();
                dp.Add("@UserId", userId);
                dp.Add("@Balance", balance - amount);

                if (balance - amount > 0)
                {
                    await db.ExecuteAsync("UpdateBalanceById", dp, commandType: CommandType.StoredProcedure);
                }
            }
        }

        public async Task WithdrawById(decimal amount, string userId)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@UserId", userId);

            using (IDbConnection db = connection)
            {
                var wallet = await db.QueryFirstOrDefaultAsync<Wallet>("GetWalletByUserId", dp, commandType: CommandType.StoredProcedure);

                dp = new DynamicParameters();
                dp.Add("@UserId", userId);
                dp.Add("@Balance", wallet.Balance - amount);

                if (wallet.Balance >= amount)
                {
                    await db.ExecuteAsync("UpdateBalanceById", dp, commandType: CommandType.StoredProcedure);
                }
            }
        }

        public async Task RecordTransaction(string userId, decimal amount, decimal currentBalance, TransactionType transactionType, TransactionStatus transactionStatus)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@UserId", userId);
            dp.Add("@Amount", amount);
            dp.Add("@CurrentBalance", currentBalance);
            dp.Add("@TransactionType", transactionType);
            dp.Add("@TransactionStatus", transactionStatus);

            using(IDbConnection db = new SqlConnection("Data Source=localhost;Initial Catalog=eCommerceTicketsDb;Integrated Security=True;Pooling=False;TrustServerCertificate=True"))
            {
                await db.ExecuteAsync("SaveTransaction", dp, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<TransactionHistoryDTO>> TransactionHistory(string userId)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@UserId", userId);

            using(IDbConnection db = new SqlConnection("Data Source=localhost;Initial Catalog=eCommerceTicketsDb;Integrated Security=True;Pooling=False;TrustServerCertificate=True"))
            {
                var transactionHistory =  await db.QueryAsync<TransactionHistoryDTO>("TransactionHistory", dp, commandType: CommandType.StoredProcedure);
                return transactionHistory;
            }
        }

        public async Task<int> GetLastTransactionId()
        {
            using (IDbConnection db = new SqlConnection("Data Source=localhost;Initial Catalog=eCommerceTicketsDb;Integrated Security=True;Pooling=False;TrustServerCertificate=True"))
            {
                int id = await db.ExecuteScalarAsync<int>("SelectLastRecord", commandType: CommandType.StoredProcedure);
                return id;
            }
        }

        public async Task<string> GetUserId()
        {
            int id = await GetLastTransactionId();
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@Id", id);
            using (IDbConnection db = new SqlConnection("Data Source=localhost;Initial Catalog=eCommerceTicketsDb;Integrated Security=True;Pooling=False;TrustServerCertificate=True"))
            {
                string userId = await db.ExecuteScalarAsync<string>("GetUserIdFromTransaction", dp, commandType: CommandType.StoredProcedure);
                return userId;
            }
        }

        public async Task RecordCasinoTransaction(string userId, decimal amount, decimal currentBalance, BetType betType,TransactionStatus transactionStatus, string currency)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@UserId", userId);
            dp.Add("@Amount", amount);
            dp.Add("@CurrentBalance", currentBalance);
            dp.Add("@TransactionStatus", transactionStatus);
            dp.Add("@BetType", betType);
            dp.Add("@Currency", currency);

            using(IDbConnection db = new SqlConnection(connectionString))
            {
                await db.ExecuteAsync("RecordCasinoTransaction", dp, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task UpdateBalance(string userId, decimal currentBalance)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@UserId", userId);
            dp.Add("@Balance", currentBalance);

            using(IDbConnection db = new SqlConnection(connectionString))
            {
                await db.ExecuteAsync("UpdateBalanceById", dp, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task RecordWinTransaction(string userId, decimal amount, decimal currentBalance, WinType winType, TransactionStatus transactionStatus, string currency)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@UserId", userId);
            dp.Add("@Amount", amount);
            dp.Add("@CurrentBalance", currentBalance);
            dp.Add("@WinType", winType);
            dp.Add("@TransactionStatus", transactionStatus);
            dp.Add("@Currency", currency);

            using(IDbConnection db = new SqlConnection(connectionString))
            {
                await db.ExecuteAsync("RecordWinTransaction", dp, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
