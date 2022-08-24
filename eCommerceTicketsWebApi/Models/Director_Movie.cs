using eCommerceTicketsWebApi.Models;

namespace eCommerceTicketsWebApplication.Models
{
    public class Director_Movie
    {
        public int MovieId { get; set; }

        public Movie Movie { get; set; }

        public int DirectorId { get; set; }

        public Director Actor { get; set; }
    }
}
