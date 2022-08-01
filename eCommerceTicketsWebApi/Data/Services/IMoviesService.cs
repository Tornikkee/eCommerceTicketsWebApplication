using eCommerceTicketsWebApi.Models;
using eCommerceTicketsWebApplication.Data.Base;
using eCommerceTicketsWebApplication.Data.ViewModel;

namespace eCommerceTicketsWebApplication.Data.Services
{
    public interface IMoviesService : IEntityBaseRepository<Movie>
    {
        Task<Movie> GetMovieByIdAsync(int id);
        Task<NewMovieDropdownsVM> GetNewMovieDropdownsValues();
        Task AddNewMovieAsync(NewMovieVM data);
        Task UpdateMovieAsync(NewMovieVM data);
    }
}
