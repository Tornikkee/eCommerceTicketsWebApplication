using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;

namespace eCommerceTicketsWebApplication.Models
{
    public class Wallet
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        public decimal Balance { get; set; }
    }
}
