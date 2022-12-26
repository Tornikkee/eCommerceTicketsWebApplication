using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace eCommerceTicketsWebApplication.Data.Repositories
{
    public class LaunchRepository : ILaunchRepository
    {
        private readonly string connectionString;

        public LaunchRepository()
        {
            connectionString = "Data Source=localhost;Initial Catalog=eCommerceTicketsDb;Integrated Security=True;Pooling=False;TrustServerCertificate=True";
        }

        public async Task FillUsersAndTokens(string token, string userId, string privateToken)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@Token", token);
            dp.Add("@UserId", userId);
            dp.Add("@PrivateToken", privateToken);

            using(IDbConnection db = new SqlConnection(connectionString))
            {
                await db.ExecuteAsync("FillUsersAndTokens", dp, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<string> GetTokenByUserId(string userId)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@UserId", userId);

            using(IDbConnection db = new SqlConnection(connectionString))
            {
                string token = await db.ExecuteScalarAsync<string>("GetToken", dp, commandType: CommandType.StoredProcedure);
                return token;
            }
        }
    }
}
