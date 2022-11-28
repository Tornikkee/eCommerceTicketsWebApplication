using eCommerceTicketsWebApplication.DTOS;
using eCommerceTicketsWebApplication.Models;
using System.Data.SqlTypes;

namespace eCommerceTicketsWebApplication.Data.Repositories
{
    public interface IWalletsRepository
    {
        Task<decimal> GetBalanceAsync(string userId);
        Task CreateWallet(string userId);
        Task<Wallet> GetWalletByUserId(string userId);
    }
}
