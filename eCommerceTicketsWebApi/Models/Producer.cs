using eCommerceTicketsWebApplication.Data.Base;
using System.ComponentModel.DataAnnotations;

namespace eCommerceTicketsWebApi.Models
{
    public class Producer : IEntityBase
    {
        public Producer()
        {
            Movies = new List<Movie>();
        }
        [Key]
        public int Id { get; set; }

        [Display(Name = "Profile Picture")]
        [Required(ErrorMessage = "Profile Picture is required")]
        public string ProfilePictureURL { get; set; }

        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "Full Name is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Full name must be between 3 and 50 characters")]
        public string FullName { get; set; }

        [Display(Name = "Biography")]
        [Required(ErrorMessage = "Biography is required")]
        public string Bio { get; set; }

        //RelationProps
        public List<Movie> Movies { get; set; }
    }
}
