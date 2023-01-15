using Dapper;
using eCommerceTicketsWebApplication.Data.Enums;
using Microsoft.Data.SqlClient;
using Payment.Models;
using System.Data;

namespace Payment.Repositories
{
    public class CasinoRepository : ICasinoRepository
    {
        private readonly string connectionString;

        public CasinoRepository()
        {
            connectionString = "Data Source=localhost;Initial Catalog=eCommerceTicketsDb;Integrated Security=True;Pooling=False;TrustServerCertificate=True";
        }

        public int Bet(decimal amount, string transactionId, BetType betType, string currency, string userId, out decimal currentBalance, out int internalTransactionId)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@Amount", amount);
            dp.Add("@TransactionId", transactionId);
            dp.Add("@BetType", betType);
            dp.Add("@Currency", currency);
            dp.Add("@UserId", userId);
            dp.Add("CurrentBalance", dbType: DbType.Decimal, direction: ParameterDirection.Output, precision: 38, scale: 5);
            dp.Add("@InternalTransactionId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            dp.Add("ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            using(IDbConnection db = new SqlConnection(connectionString))
            {
                db.Execute("Bet", dp, commandType: CommandType.StoredProcedure);
                currentBalance = dp.Get<decimal>("CurrentBalance");
                internalTransactionId = dp.Get<int>("InternalTransactionId");
                return dp.Get<int>("ReturnValue");
            }
        }

        public int Win(decimal amount, string transactionId, WinType winType, string currency, string userId, out decimal currentBalance, out int internalTransactionId)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("Amount", amount);
            dp.Add("TransactionId", transactionId);
            dp.Add("WinType", winType);
            dp.Add("Currency", currency);
            dp.Add("UserId", userId);
            dp.Add("CurrentBalance", dbType: DbType.Decimal, direction: ParameterDirection.Output, precision: 38, scale: 5);
            dp.Add("InternalTransactionId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            dp.Add("ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            using(IDbConnection db = new SqlConnection(connectionString))
            {
                db.Execute("Win", dp, commandType: CommandType.StoredProcedure);
                currentBalance = dp.Get<decimal>("CurrentBalance");
                internalTransactionId = dp.Get<int>("InternalTransactionId");
                return dp.Get<int>("ReturnValue");
            }
        }

        public int CancelBet(decimal amount, string transactionId, BetType betType, string currency, string userId, string betTransactionId, out decimal currentBalance, out int internalTransactionId)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("Amount", amount);
            dp.Add("TransactionId", transactionId);
            dp.Add("BetType", betType);
            dp.Add("Currency", currency);
            dp.Add("UserId", userId);
            dp.Add("BetTransactionId", betTransactionId);
            dp.Add("CurrentBalance", dbType: DbType.Decimal, direction: ParameterDirection.Output, precision: 38, scale: 5);
            dp.Add("InternalTransactionId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            dp.Add("ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            using(IDbConnection db = new SqlConnection(connectionString))
            {
                db.Execute("CancelBet", dp, commandType: CommandType.StoredProcedure);
                currentBalance = dp.Get<decimal>("CurrentBalance");
                internalTransactionId = dp.Get<int>("InternalTransactionId");
                return dp.Get<int>("ReturnValue");
            }
        }

        public int ChangeWin(decimal amount, decimal previousAmount, string transactionId, string previousTransactionId, string userId, string currency, out decimal currentBalance, out int internalTransactionId)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@Amount", amount);
            dp.Add("@PreviousAmount", previousAmount);
            dp.Add("@TransactionId", transactionId);
            dp.Add("@PreviousTransactionId", previousTransactionId);
            dp.Add("@UserId", userId);
            dp.Add("@Currency", currency);
            dp.Add("@CurrentBalance", dbType: DbType.Decimal, direction: ParameterDirection.Output, precision: 38, scale: 5);
            dp.Add("@InternalTransactionId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            dp.Add("ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            using(IDbConnection db = new SqlConnection(connectionString))
            {
                db.Execute("ChangeWin", dp, commandType: CommandType.StoredProcedure);
                currentBalance = dp.Get<decimal>("CurrentBalance");
                internalTransactionId = dp.Get<int>("InternalTransactionId");
                return dp.Get<int>("ReturnValue");
            }
        }

        public async Task<string> GetPrivateToken(string userId)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@UserId", userId);

            using(IDbConnection db = new SqlConnection(connectionString))
            {
                var privateToken = await db.ExecuteScalarAsync<string>("GetPrivateToken", dp, commandType: CommandType.StoredProcedure);
                return privateToken;
            }
        }

        public async Task<UserInfo> GetUser(string userId)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@UserId", userId);

            using(IDbConnection db = new SqlConnection(connectionString))
            {
                var user = await db.QueryFirstOrDefaultAsync<UserInfo>("GetUser", dp, commandType: CommandType.StoredProcedure);
                return user;
            }
        }

        public async Task<string> GetUserIdWithToken(string token)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@Token", token);

            using(IDbConnection db = new SqlConnection(connectionString))
            {
                var userId = await db.ExecuteScalarAsync<string>("GetUserIdWithToken", dp, commandType: CommandType.StoredProcedure);
                return userId;
            }
        }
    }
}
