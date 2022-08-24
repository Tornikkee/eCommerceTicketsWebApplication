using Dapper;
using eCommerceTicketsWebApplication.Models;
using System.Data;
using System.Data.SqlClient;

namespace eCommerceTicketsWebApplication.Data.Repositories
{
    public class DirectorsRepository : IDirectorsRepository
    {
        SqlConnection connection;
        IConfiguration? configuration;
        private readonly string connectionString;

        public DirectorsRepository()
        {
            connectionString = "Data Source=localhost;Initial Catalog=eCommerceTicketsDb;Integrated Security=True;Pooling=False;TrustServerCertificate=True";
            connection = new SqlConnection(connectionString);
        }

        public async Task AddAsync(Director entity)
        {
            using(IDbConnection db = connection)
            {
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@ProfilePictureURL", entity.ProfilePictureURL);
                dynamicParameters.Add("@FullName", entity.FullName);
                dynamicParameters.Add("@Bio", entity.Bio);
                await db.ExecuteAsync("AddDirector", dynamicParameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task DeleteAsync(int id)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@Id", id);
            using(IDbConnection db = new SqlConnection(connectionString))
            {
                await db.ExecuteAsync("DeleteDirectorById", dynamicParameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<Director>> GetAllAsync()
        {
            using(IDbConnection db = connection)
            {
                IEnumerable<Director> directors = await db.QueryAsync<Director>("GetAllDirectors", commandType: CommandType.StoredProcedure);
                return directors;
            }
        }

        public async Task<Director> GetByIdAsync(int id)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@Id", id);
            using(IDbConnection db = connection)
            {
                var director = await db.QueryFirstOrDefaultAsync<Director>("GetDirectorById", dynamicParameters, commandType: CommandType.StoredProcedure);
                return director;
            }
        }

        public async Task UpdateAsync(int id, Director entity)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@Id", id);
            dynamicParameters.Add("@ProfilePictureURL", entity.ProfilePictureURL);
            dynamicParameters.Add("@FullName", entity.FullName);
            dynamicParameters.Add("@Bio", entity.Bio);
            
            using (IDbConnection db = new SqlConnection(connectionString))
            { 
                await db.ExecuteAsync("UpdateDirector", dynamicParameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
