using eCommerceTicketsWebApplication.Models;
using System.Linq.Expressions;

namespace eCommerceTicketsWebApplication.Data.Repositories
{
    public interface IDirectorsRepository
    {
        Task<IEnumerable<Director>> GetAllAsync();
        Task<Director> GetByIdAsync(int id);
        Task AddAsync(Director entity);
        Task UpdateAsync(int id, Director entity);
        Task DeleteAsync(int id);
    }
}
