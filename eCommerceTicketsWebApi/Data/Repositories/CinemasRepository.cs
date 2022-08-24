using Dapper;
using eCommerceTicketsWebApi.Models;
using eCommerceTicketsWebApplication.Models;
using System.Data;
using System.Data.SqlClient;

namespace eCommerceTicketsWebApplication.Data.Repositories
{
    public class CinemasRepository : ICinemasRepository
    {
        SqlConnection connection;
        private readonly string connectionString;

        public CinemasRepository()
        {
            connectionString = "Data Source=localhost;Initial Catalog=eCommerceTicketsDb;Integrated Security=True;Pooling=False;TrustServerCertificate=True";
            connection = new SqlConnection(connectionString);
        }

        public async Task AddAsync(Cinema entity)
        {
            using (IDbConnection db = connection)
            {
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@Logo", entity.Logo);
                dynamicParameters.Add("@Name", entity.Name);
                dynamicParameters.Add("@Description", entity.Description);
                await db.ExecuteAsync("AddCinema", dynamicParameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task DeleteAsync(int id)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@Id", id);
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                await db.ExecuteAsync("DeleteCinemaById", dynamicParameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<Cinema>> GetAllAsync()
        {
            using (IDbConnection db = connection)
            {
                IEnumerable<Cinema> cinemas = await db.QueryAsync<Cinema>("GetAllCinemas", commandType: CommandType.StoredProcedure);
                return cinemas;
            }
        }

        public async Task<Cinema> GetByIdAsync(int id)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@Id", id);
            using (IDbConnection db = connection)
            {
                var cinema = await db.QueryFirstOrDefaultAsync<Cinema>("GetCinemaById", dynamicParameters, commandType: CommandType.StoredProcedure);
                return cinema;
            }
        }

        public async Task UpdateAsync(int id, Cinema entity)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@Id", id);
            dynamicParameters.Add("@Logo", entity.Logo);
            dynamicParameters.Add("@Name", entity.Name);
            dynamicParameters.Add("@Description", entity.Description);

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                await db.ExecuteAsync("UpdateCinema", dynamicParameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
