using System.ComponentModel.DataAnnotations;

namespace Payment.Models
{
    public class UserInfo
    {
        [Key]
        public string Id { get; set; }

        public string UserName { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string CountryCode { get; set; }

        public string CountryName { get; set; }

        public int Gender { get; set; }

        public string Currency { get; set; }
    }
}
