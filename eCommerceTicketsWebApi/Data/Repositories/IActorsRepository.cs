using eCommerceTicketsWebApi.Models;
using eCommerceTicketsWebApplication.Models;

namespace eCommerceTicketsWebApplication.Data.Repositories
{
    public interface IActorsRepository
    {
        Task<IEnumerable<Actor>> GetAllAsync();
        Task<Actor> GetByIdAsync(int id);
        Task AddAsync(Actor entity);
        Task UpdateAsync(int id, Actor entity);
        Task DeleteAsync(int id);
    }
}
