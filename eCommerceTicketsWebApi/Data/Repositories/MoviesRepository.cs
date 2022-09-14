using Dapper;
using eCommerceTicketsWebApi.Models;
using eCommerceTicketsWebApplication.Data.ViewModel;
using eCommerceTicketsWebApplication.DTOS;
using System.Data;
using System.Data.SqlClient;
using static Dapper.SqlMapper;

namespace eCommerceTicketsWebApplication.Data.Repositories
{
    public class MoviesRepository : IMoviesRepository
    {
        SqlConnection connection;
        private readonly string connectionString;

        public MoviesRepository()
        {
            connectionString = "Data Source=localhost;Initial Catalog=eCommerceTicketsDb;Integrated Security=True;Pooling=False;TrustServerCertificate=True";
            connection = new SqlConnection(connectionString);
        }

        public async Task AddAsync(NewMovieVM entity)
        {
            using (IDbConnection db = connection)
            {
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@Name", entity.Name);
                dynamicParameters.Add("@Description", entity.Description);
                dynamicParameters.Add("@Price", entity.Price);
                dynamicParameters.Add("@ImageURL", entity.ImageURL);
                dynamicParameters.Add("@StartDate", entity.StartDate);
                dynamicParameters.Add("@EndDate", entity.EndDate);
                dynamicParameters.Add("@MovieCategory", entity.MovieCategory);
                dynamicParameters.Add("@CinemaId", entity.CinemaId);
                dynamicParameters.Add("@ProducerId", entity.ProducerId);
                await db.ExecuteAsync("AddMovie", dynamicParameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task DeleteAsync(int id)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@Id", id);
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                await db.ExecuteAsync("DeleteMovieById", dynamicParameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<Movie>> GetAllAsync()
        {
            var query = $@"SELECT Movies.Id, Movies.Name, Movies.Description, Price, ImageURL, StartDate, EndDate, MovieCategory,               Cinemas.Id as CinemaIdd, Cinemas.Name AS CinemaName
                           FROM Movies
                           LEFT JOIN Cinemas
                           ON CinemaId = Cinemas.Id";

            var moviesDictionary = new Dictionary<int, Movie>();

            using (IDbConnection db = connection)
            {
                IEnumerable<Movie> result = await db.QueryAsync<Movie, Cinema, Movie>(query, (mov, cin) =>
                {
                    Movie movie;
                    if (!moviesDictionary.TryGetValue(mov.Id, out movie))
                    {
                        movie = mov;
                        movie.Cinema = new Cinema();
                        moviesDictionary.Add(movie.Id, movie);
                    }
                    if (cin.Id > 0)
                    {
                        movie.Cinema = cin;
                    }
                    return movie;
                }, splitOn: "CinemaIdd");

                var movies = result.Distinct().ToList();
                return movies;
            }
        }

        public async Task<Movie> GetMovieByIdAsync(int id)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@Id", id);

            //var query = "DECLARE @Id int\r\nEXEC GetMovieById @Id";

            using (IDbConnection db = connection)
            {
                var movie = await db.QueryFirstOrDefaultAsync<Movie>("GetMovieById", dynamicParameters, commandType: CommandType.StoredProcedure);

                //var movie = await db.QueryAsync<Movie, Cinema, Movie>(query, (mov, cin) =>
                //{
                //    mov.Cinema = cin;
                //    return mov;
                //}, splitOn: "Id");

                return movie;
            }
        }

        public async Task<NewMovieDropdownsVM> GetNewMovieDropdownsValues()
        {
            using (IDbConnection db = new SqlConnection("Data Source=localhost;Initial Catalog=eCommerceTicketsDb;Integrated Security=True;Pooling=False;TrustServerCertificate=True"))
            {
                NewMovieDropdownsVM movieDropdowns = new NewMovieDropdownsVM();
                IEnumerable<Actor> actors = await db.QueryAsync<Actor>("GetAllActors", commandType: CommandType.StoredProcedure);
                IEnumerable<Cinema> cinemas = await db.QueryAsync<Cinema>("GetAllCinemas", commandType: CommandType.StoredProcedure);
                IEnumerable<Producer> producers = await db.QueryAsync<Producer>("GetAllProducers", commandType: CommandType.StoredProcedure);

                movieDropdowns.Actors = actors.ToList();
                movieDropdowns.Cinemas = cinemas.ToList();
                movieDropdowns.Producers = producers.ToList();

                return movieDropdowns;
            }
        }

        public async Task UpdateAsync(int id, NewMovieVM entity)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@Id", id);
            dynamicParameters.Add("@Name", entity.Name);
            dynamicParameters.Add("@Description", entity.Description);
            dynamicParameters.Add("@Price", entity.Price);
            dynamicParameters.Add("@ImageURL", entity.ImageURL);
            dynamicParameters.Add("@StartDate", entity.StartDate);
            dynamicParameters.Add("@EndDate", entity.EndDate);
            dynamicParameters.Add("@MovieCategory", entity.MovieCategory);
            dynamicParameters.Add("@ProducerId", entity.ProducerId);
            dynamicParameters.Add("@CinemaId", entity.CinemaId);

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                await db.ExecuteAsync("UpdateMovie", dynamicParameters, commandType: CommandType.StoredProcedure);
            }
        }

        //public async Task<IEnumerable<MovieDTO>> GetAllAsync(/*params Expression<Func<MovieDTO, object>>[] includeProperties*/)
        //{
        //    using (IDbConnection db = connection)
        //    {
        //        var movies = await db.QueryAsync<MovieDTO>("select* from Movies");

        //        var cinemas = await db.QueryAsync<CinemaDTO>("select * from Cinemas");

        //        foreach (var item in movies)
        //        {

        //            cinemas.Where(x => x.Id == item.CinemaId).FirstOrDefault().movie.Add(item);
        //        }

        //        return movies;
        //    }
        //}
    }
}
