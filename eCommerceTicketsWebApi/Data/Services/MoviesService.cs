using eCommerceTicketsWebApi.Data;
using eCommerceTicketsWebApi.Models;
using eCommerceTicketsWebApplication.Data.Base;
using Microsoft.EntityFrameworkCore;

namespace eCommerceTicketsWebApplication.Data.Services
{
    public class MoviesService : EntityBaseRepository<Movie>, IMoviesService
    {
        private readonly AppDbContext _context;

        public MoviesService(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Movie> GetMovieByIdAsync(int id)
        {
            var movieDetails = await _context.Movies.Include(c => c.Cinema).Include(p => p.Producer).Include(am => am.Actors_Movies).ThenInclude(a => a.Actor).FirstOrDefaultAsync(n => n.Id == id);

            return movieDetails;
        }
    }
}
