using Dapper;
using eCommerceTicketsWebApi.Data;
using eCommerceTicketsWebApi.Models;
using eCommerceTicketsWebApplication.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Data;
using System.Runtime.Intrinsics.Arm;

namespace eCommerceTicketsWebApplication.Data.Cart
{
    public class ShoppingCart
    {
        //public AppDbContext _context { get; set; }

        public string ShoppingCartId { get; set; }

        SqlConnection connection;
        private readonly string connectionString;

        public List<ShoppingCartItem> ShoppingCartItems { get; set; }

        public ShoppingCart(/*AppDbContext context*/)
        {
            connectionString = "Data Source=localhost;Initial Catalog=eCommerceTicketsDb;Integrated Security=True;Pooling=False;TrustServerCertificate=True";
            connection = new SqlConnection(connectionString);
            //_context = context;
        }

        public static ShoppingCart GetShoppingCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            var context = services.GetService<SqlConnection>();

            string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();
            session.SetString("CartId", cartId);

            return new ShoppingCart(/*context*/) { ShoppingCartId = cartId };
        }

        public void AddItemToCart(Movie movie)
        {
            //var shoppingCartItem = _context.ShoppingCartItems.FirstOrDefault(n => n.Movie.Id == movie.Id && n.ShoppingCartId == ShoppingCartId);

            DynamicParameters dp = new DynamicParameters();
            dp.Add("@ShoppingCartId", ShoppingCartId);
            dp.Add("@MovieId", movie.Id);

            using(IDbConnection db = connection)
            {
                int amount = 1;

                ShoppingCartItem shoppingCartItem = db.QueryFirstOrDefault<ShoppingCartItem>("GetCartItemById", dp, commandType: CommandType.StoredProcedure);

                if(shoppingCartItem == null)
                {
                    DynamicParameters dp1 = new DynamicParameters();
                    dp1.Add("@ShoppingCartId", ShoppingCartId);
                    dp1.Add("@MovieId", movie.Id);
                    dp1.Add("@Amount", amount);

                    db.Execute("AddItemToCart", dp1, commandType: CommandType.StoredProcedure);
                }
                else
                {
                    DynamicParameters dp4 = new DynamicParameters();
                    dp4.Add("@ShoppingCartId", ShoppingCartId);

                    amount = db.QueryFirstOrDefault<int>("GetCartItemAmountById", dp4, commandType: CommandType.StoredProcedure);

                    DynamicParameters dp2 = new DynamicParameters();
                    dp2.Add("@ShoppingCartId", ShoppingCartId);
                    dp2.Add("@MovieId", movie.Id);

                    db.Execute("DeleteCartItemsById", dp2, commandType: CommandType.StoredProcedure);

                    amount++;
                    DynamicParameters dp3 = new DynamicParameters();
                    dp3.Add("@ShoppingCartId", ShoppingCartId);
                    dp3.Add("@MovieId", movie.Id);
                    dp3.Add("@Amount", amount);

                    db.Execute("AddItemToCart", dp3, commandType: CommandType.StoredProcedure);
                }
            }

            //if(shoppingCartItem == null)
            //{
            //    shoppingCartItem = new ShoppingCartItem()
            //    {
            //        ShoppingCartId = ShoppingCartId,
            //        Movie = movie,
            //        Amount = 1
            //    };

            //    _context.ShoppingCartItems.Add(shoppingCartItem);
            //}
            //else
            //{
            //    shoppingCartItem.Amount++;
            //}
            //_context.SaveChanges();
        }

        public void RemoveItemFromCart(Movie movie)
        {
            //var shoppingCartItem = _context.ShoppingCartItems.FirstOrDefault(n => n.Movie.Id == movie.Id && n.ShoppingCartId == ShoppingCartId);
            

            //if (shoppingCartItem != null)
            //{
            //    if(shoppingCartItem.Amount > 1)
            //    {
            //        shoppingCartItem.Amount--;
            //    }
            //    else
            //    {
            //        _context.ShoppingCartItems.Remove(shoppingCartItem);
            //    }
            //}
            //_context.SaveChanges();

            DynamicParameters dp = new DynamicParameters();
            dp.Add("@ShoppingCartId", ShoppingCartId);
            dp.Add("@MovieId", movie.Id);

            using (IDbConnection db = connection)
            {
                ShoppingCartItem shoppingCartItem = db.QueryFirstOrDefault<ShoppingCartItem>("GetCartItemById", dp, commandType: CommandType.StoredProcedure);

                DynamicParameters dp1 = new DynamicParameters();
                dp1.Add("@ShoppingCartId", ShoppingCartId);

                int amount = db.QueryFirstOrDefault<int>("GetCartItemAmountById", dp1, commandType: CommandType.StoredProcedure);

                if (shoppingCartItem != null)
                {
                    if(shoppingCartItem.Amount > 1)
                    {
                        DynamicParameters dp3 = new DynamicParameters();
                        dp3.Add("@ShoppingCartId", ShoppingCartId);
                        dp3.Add("@MovieId", movie.Id);

                        db.Execute("DeleteCartItemsById", dp3, commandType: CommandType.StoredProcedure);

                        amount--;
                        DynamicParameters dp2 = new DynamicParameters();
                        dp2.Add("@ShoppingCartId", ShoppingCartId);
                        dp2.Add("@MovieId", movie.Id);
                        dp2.Add("@Amount", amount);

                        db.Execute("AddItemToCart", dp2, commandType: CommandType.StoredProcedure);
                    }
                    else
                    {
                        DynamicParameters dp3 = new DynamicParameters();
                        dp3.Add("@ShoppingCartId", ShoppingCartId);
                        dp3.Add("@MovieId", movie.Id);

                        db.Execute("DeleteCartItemsById", dp3, commandType: CommandType.StoredProcedure);
                    }
                }
            }
        }

        public List<ShoppingCartItem> GetShoppingCartItems()
        {

            //DynamicParameters dp = new DynamicParameters();
            //dp.Add("@ShoppingCartId", ShoppingCartId);

            var sql = "SELECT s.Amount, s.ShoppingCartId, m.Name, m.Price, m.Id FROM ShoppingCartItems AS s\r\nINNER JOIN Movies AS m ON s.MovieId = m.Id";

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var items = db.Query<ShoppingCartItem, Movie, ShoppingCartItem>(sql, (item, movie) => { item.Movie = movie; return item; }, splitOn: "Name" );

                var listedItems = items.ToList();
                var list = new List<ShoppingCartItem>();

                foreach (var item in listedItems)
                {
                    if(item.ShoppingCartId == ShoppingCartId)
                    {
                        list.Add(item);
                    }
                }

                return list;
            }
            //return ShoppingCartItems ?? (ShoppingCartItems = _context.ShoppingCartItems.Where(n => n.ShoppingCartId == ShoppingCartId).Include(n => n.Movie).ToList());

        }

        public double GetShoppingCartTotal()
        {

            //var total = _context.ShoppingCartItems.Where(n => n.ShoppingCartId == ShoppingCartId).Select(n => n.Movie.Price * n.Amount).Sum();
            //return total;

            var sql = "SELECT s.Amount, s.ShoppingCartId, m.Name, m.Price, m.Id FROM ShoppingCartItems AS s\r\nINNER JOIN Movies AS m ON s.MovieId = m.Id";

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var items = db.Query<ShoppingCartItem, Movie, ShoppingCartItem>(sql, (item, movie) => { item.Movie = movie; return item; }, splitOn: "Name");

                var listedItems = items.ToList();
                var list = new List<ShoppingCartItem>();

                foreach (var item in listedItems)
                {
                    if (item.ShoppingCartId == ShoppingCartId)
                    {
                        list.Add(item);
                    }
                }

                double total = 0;
                foreach (var item in list)
                {
                    total += item.Movie.Price * item.Amount;
                }
                return total;
            }

        }

        public async Task ClearShoppingCartAsync()
        {
            //var items = await _context.ShoppingCartItems.Where(n => n.ShoppingCartId == ShoppingCartId).ToListAsync();
            //_context.ShoppingCartItems.RemoveRange(items);
            //await _context.SaveChangesAsync();

            DynamicParameters dp = new DynamicParameters();
            dp.Add("@ShoppingCartId", @ShoppingCartId);

            using (IDbConnection db = connection)
            {
                await db.ExecuteAsync("DeleteCartItemsById", dp, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
