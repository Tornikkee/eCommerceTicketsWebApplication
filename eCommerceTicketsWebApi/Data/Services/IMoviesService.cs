using eCommerceTicketsWebApi.Models;
using eCommerceTicketsWebApplication.Data.Base;

namespace eCommerceTicketsWebApplication.Data.Services
{
    public interface IMoviesService : IEntityBaseRepository<Movie>
    {
        Task<Movie> GetMovieByIdAsync(int id);
    }
}
