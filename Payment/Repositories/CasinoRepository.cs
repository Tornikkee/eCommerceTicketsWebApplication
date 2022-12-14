using Dapper;
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
