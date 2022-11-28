using System.ComponentModel.DataAnnotations;

namespace eCommerceTicketsWebApplication.DTOS
{
    public class PayDTO
    {
        [Display(Name = "Email address")]
        [Required(ErrorMessage = "Email address is required")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
