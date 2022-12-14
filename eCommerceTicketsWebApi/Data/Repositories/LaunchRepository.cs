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
