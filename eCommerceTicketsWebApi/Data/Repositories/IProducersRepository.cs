using eCommerceTicketsWebApi.Models;

namespace eCommerceTicketsWebApplication.Data.Repositories
{
    public interface IProducersRepository
    {
        Task<IEnumerable<Producer>> GetAllAsync();
        Task<Producer> GetByIdAsync(int id);
        Task AddAsync(Producer entity);
        Task UpdateAsync(int id, Producer entity);
        Task DeleteAsync(int id);
    }
}
