using eCommerceTicketsWebApplication.DTOS;
using eCommerceTicketsWebApplication.Models;

namespace eCommerceTicketsWebApplication.Data.Repositories
{
    public interface IOrdersRepository
    {
        Task StoreOrderAsync(List<ShoppingCartItem> items, string userId, string userEmailAddress);

        Task<List<OrderDTO>> GetOrdersByUserIdAsync(string userId);
    }
}
