using Dapper;
using eCommerceTicketsWebApi.Models;
using eCommerceTicketsWebApplication.Models;
using System.Data;
using System.Data.SqlClient;

namespace eCommerceTicketsWebApplication.Data.Repositories
{
    public class ActorsRepository : IActorsRepository
    {
        SqlConnection connection;
        private readonly string connectionString;

        public ActorsRepository()
        {
            connectionString = "Data Source=localhost;Initial Catalog=eCommerceTicketsDb;Integrated Security=True;Pooling=False;TrustServerCertificate=True";
            connection = new SqlConnection(connectionString);
        }

        public async Task AddAsync(Actor entity)
        {
            using (IDbConnection db = connection)
            {
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@ProfilePictureURL", entity.ProfilePictureURL);
                dynamicParameters.Add("@FullName", entity.FullName);
                dynamicParameters.Add("@Bio", entity.Bio);
                await db.ExecuteAsync("AddActor", dynamicParameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task DeleteAsync(int id)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@Id", id);
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                await db.ExecuteAsync("DeleteActorById", dynamicParameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<Actor>> GetAllAsync()
        {
            using (IDbConnection db = connection)
            {
                IEnumerable<Actor> actors = await db.QueryAsync<Actor>("GetAllActors", commandType: CommandType.StoredProcedure);
                return actors;
            }
        }

        public async Task<Actor> GetByIdAsync(int id)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@Id", id);
            using (IDbConnection db = connection)
            {
                var actor = await db.QueryFirstOrDefaultAsync<Actor>("GetActorById", dynamicParameters, commandType: CommandType.StoredProcedure);
                return actor;
            }
        }

        public async Task UpdateAsync(int id, Actor entity)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@Id", id);
            dynamicParameters.Add("@ProfilePictureURL", entity.ProfilePictureURL);
            dynamicParameters.Add("@FullName", entity.FullName);
            dynamicParameters.Add("@Bio", entity.Bio);

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                await db.ExecuteAsync("UpdateActor", dynamicParameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
