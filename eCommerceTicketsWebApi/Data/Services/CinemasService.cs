using eCommerceTicketsWebApi.Data;
using eCommerceTicketsWebApi.Models;
using eCommerceTicketsWebApplication.Data.Base;

namespace eCommerceTicketsWebApplication.Data.Services
{
    public class CinemasService : EntityBaseRepository<Cinema>, ICinemasService
    {
        public CinemasService(AppDbContext context) : base(context)
        {

        }
    }
}
