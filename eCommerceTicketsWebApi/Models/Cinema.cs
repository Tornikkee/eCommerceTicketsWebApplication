using eCommerceTicketsWebApplication.Data.Base;
using System.ComponentModel.DataAnnotations;

namespace eCommerceTicketsWebApi.Models
{
    public class Cinema : IEntityBase
    {
        public Cinema()
        {
            Movies = new List<Movie>();
        }

        [Key]
        public int Id { get; set; }

        [Display(Name = "Cinema Logo")]
        [Required(ErrorMessage = "Cinema logo is required")]
        public string Logo { get; set; }

        [Display(Name = "Cinema Name")]
        [Required(ErrorMessage = "Cinema name is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "Cinema description is required")]
        public string Description { get; set; }

        //RelationProps
        public List<Movie> Movies { get; set; }
    }
}
