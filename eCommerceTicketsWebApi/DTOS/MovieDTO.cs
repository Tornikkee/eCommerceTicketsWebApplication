using eCommerceTicketsWebApi.Models;
using eCommerceTicketsWebApplication.Data.Enums;

namespace eCommerceTicketsWebApplication.DTOS
{
    public class MovieDTO
    {
        public int Id { get; set; }

        public int CinemaId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public string ImageURL { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public MovieCategory MovieCategory { get; set; }

        public Cinema Cinema { get; set; }

        public Producer Producer { get; set; }

       // public  List<CinemaDTO>  cinemas { get; set; }
    }
}
