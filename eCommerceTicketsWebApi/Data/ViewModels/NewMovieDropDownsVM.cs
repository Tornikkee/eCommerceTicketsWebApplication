using eCommerceTicketsWebApi.Models;

namespace eCommerceTicketsWebApplication.Data.ViewModels
{
    public class NewMovieDropDownsVM
    {
        public NewMovieDropDownsVM()
        {

        }
        public List<Producer> Producers { get; set; }
        public List<Cinema> Cinemas { get; set; }
        public List<Actor> Actors { get; set; }
    }
}
