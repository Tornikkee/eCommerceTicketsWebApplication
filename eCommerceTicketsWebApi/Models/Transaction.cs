using System.Data.SqlTypes;

namespace eCommerceTicketsWebApplication.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public decimal Amount { get; set; }

        public decimal CurrentBalance { get; set; }

        public int TransactionType { get; set; }

        public int TransactionStatus { get; set; }
    }
}
