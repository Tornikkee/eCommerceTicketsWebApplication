using Dapper;
using eCommerceTicketsWebApi.Models;
using eCommerceTicketsWebApplication.Models;
using System.Data;
using System.Data.SqlClient;

namespace eCommerceTicketsWebApplication.Data.Repositories
{
    public class ProducersRepository : IProducersRepository
    {
        SqlConnection connection;
        private readonly string connectionString;

        public ProducersRepository()
        {
            connectionString = "Data Source=localhost;Initial Catalog=eCommerceTicketsDb;Integrated Security=True;Pooling=False;TrustServerCertificate=True";
            connection = new SqlConnection(connectionString);
        }

        public async Task AddAsync(Producer entity)
        {
            using (IDbConnection db = connection)
            {
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@ProfilePictureURL", entity.ProfilePictureURL);
                dynamicParameters.Add("@FullName", entity.FullName);
                dynamicParameters.Add("@Bio", entity.Bio);
                await db.ExecuteAsync("AddProducer", dynamicParameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task DeleteAsync(int id)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@Id", id);
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                await db.ExecuteAsync("DeleteProducerById", dynamicParameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<Producer>> GetAllAsync()
        {
            using (IDbConnection db = connection)
            {
                IEnumerable<Producer> producers = await db.QueryAsync<Producer>("GetAllProducers", commandType: CommandType.StoredProcedure);
                return producers;
            }
        }

        public async Task<Producer> GetByIdAsync(int id)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@Id", id);
            using (IDbConnection db = connection)
            {
                var producer = await db.QueryFirstOrDefaultAsync<Producer>("GetProducerById", dynamicParameters, commandType: CommandType.StoredProcedure);
                return producer;
            }
        }

        public async Task UpdateAsync(int id, Producer entity)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@Id", id);
            dynamicParameters.Add("@ProfilePictureURL", entity.ProfilePictureURL);
            dynamicParameters.Add("@FullName", entity.FullName);
            dynamicParameters.Add("@Bio", entity.Bio);

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                await db.ExecuteAsync("UpdateProducer", dynamicParameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
