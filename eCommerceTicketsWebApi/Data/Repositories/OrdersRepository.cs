using Dapper;
using eCommerceTicketsWebApi.Models;
using eCommerceTicketsWebApplication.DTOS;
using eCommerceTicketsWebApplication.Models;
using System.Data;
using System.Data.SqlClient;

namespace eCommerceTicketsWebApplication.Data.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        SqlConnection connection;
        private readonly string connectionString;

        public OrdersRepository()
        {
            connectionString = "Data Source=localhost;Initial Catalog=eCommerceTicketsDb;Integrated Security=True;Pooling=False;TrustServerCertificate=True";
            connection = new SqlConnection(connectionString);
        }

        public async Task<List<OrderDTO>> GetOrdersByUserIdAsync(string userId)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@UserId", userId);

            using(IDbConnection db = connection)
            {
                var orders = await db.QueryAsync<OrderDTO>("GetOrderByUserId", dp, commandType: CommandType.StoredProcedure);
                return orders.ToList();
            }
        }

        public async Task StoreOrderAsync(List<ShoppingCartItem> items, string userId, string userEmailAddress)
        {
            using(IDbConnection db = connection)
            {
                var order = new Order()
                {
                    UserId = userId,
                    Email = userEmailAddress
                };
                DynamicParameters dp = new DynamicParameters();
                dp.Add("@UserId", userId);
                dp.Add("@Email", userEmailAddress);

                await db.ExecuteAsync("StoreOrder", dp, commandType: CommandType.StoredProcedure);

                foreach (var item in items)
                {
                    dp = new DynamicParameters();
                    dp.Add("@Amount", item.Amount);
                    dp.Add("@MovieId", item.Movie.Id);
                    dp.Add("@OrderId", order.Id);
                    dp.Add("@Price", item.Movie.Price);

                    await db.ExecuteAsync("StoreOrderItem", dp, commandType: CommandType.StoredProcedure);
                }
            }
            
        }
    }
}
