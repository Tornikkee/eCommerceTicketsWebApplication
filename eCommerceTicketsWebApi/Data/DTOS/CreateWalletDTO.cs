using System.ComponentModel.DataAnnotations;

namespace eCommerceTicketsWebApplication.DTOS
{
    public class CreateWalletDTO
    {
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
