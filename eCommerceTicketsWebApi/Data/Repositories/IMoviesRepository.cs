using eCommerceTicketsWebApi.Models;
using eCommerceTicketsWebApplication.Data.ViewModel;
using eCommerceTicketsWebApplication.DTOS;
using System.Linq.Expressions;

namespace eCommerceTicketsWebApplication.Data.Repositories
{
    public interface IMoviesRepository
    {
        Task<IEnumerable<Movie>> GetAllAsync();//Procedure created
        Task<Movie> GetMovieByIdAsync(int id);//Procedure created
        Task AddAsync(NewMovieVM entity);//Procedure created
        Task UpdateAsync(int id, NewMovieVM entity);//Procedure created
        Task DeleteAsync(int id);//Procedure created
        //Task<Movie> GetMovieByIdAsync(int id);
        Task<NewMovieDropdownsVM> GetNewMovieDropdownsValues();
        //Task<IEnumerable<Movie>> GetAllAsync(params Expression<Func<MovieDTO, object>>[] includeProperties);
        //Task AddNewMovieAsync(NewMovieVM data);
        //Task UpdateMovieAsync(NewMovieVM data);
    }
}
