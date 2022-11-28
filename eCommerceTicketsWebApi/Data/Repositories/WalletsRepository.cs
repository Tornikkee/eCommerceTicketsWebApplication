using Dapper;
using eCommerceTicketsWebApplication.DTOS;
using eCommerceTicketsWebApplication.Models;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace eCommerceTicketsWebApplication.Data.Repositories
{
    public class WalletsRepository : IWalletsRepository
    {
        SqlConnection connection;
        private readonly string connectionString;

        public WalletsRepository()
        {
            connectionString = "Data Source=localhost;Initial Catalog=eCommerceTicketsDb;Integrated Security=True;Pooling=False;TrustServerCertificate=True";
            connection = new SqlConnection(connectionString);
        }
        public async Task<decimal> GetBalanceAsync(string userId)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@UserId", userId);

            using(IDbConnection db = new SqlConnection("Data Source=localhost;Initial Catalog=eCommerceTicketsDb;Integrated Security=True;Pooling=False;TrustServerCertificate=True"))
            {
                var balance = await db.ExecuteScalarAsync<decimal>("GetBalance", dp, commandType: CommandType.StoredProcedure);
                return balance;
            }
        }

        public async Task CreateWallet(string userId)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add(@"UserId", userId);

            using(IDbConnection db = new SqlConnection("Data Source=localhost;Initial Catalog=eCommerceTicketsDb;Integrated Security=True;Pooling=False;TrustServerCertificate=True"))
            {
                await db.ExecuteAsync("AddWallet", dp, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<Wallet> GetWalletByUserId(string userId)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@UserId", userId);

            using(IDbConnection db = new SqlConnection("Data Source=localhost;Initial Catalog=eCommerceTicketsDb;Integrated Security=True;Pooling=False;TrustServerCertificate=True"))
            {
                var wallet = await db.QueryFirstOrDefaultAsync<Wallet>("GetWalletByUserId", dp, commandType: CommandType.StoredProcedure);
                return wallet;
            }
        }
    }
}
