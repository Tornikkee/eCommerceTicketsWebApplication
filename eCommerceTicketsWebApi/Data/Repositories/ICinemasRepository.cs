using eCommerceTicketsWebApi.Models;
using eCommerceTicketsWebApplication.Models;

namespace eCommerceTicketsWebApplication.Data.Repositories
{
    public interface ICinemasRepository
    {
        Task<IEnumerable<Cinema>> GetAllAsync();
        Task<Cinema> GetByIdAsync(int id);
        Task AddAsync(Cinema entity);
        Task UpdateAsync(int id, Cinema entity);
        Task DeleteAsync(int id);
    }
}
