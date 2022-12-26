using eCommerceTicketsWebApplication.Data.Enums;

namespace eCommerceTicketsWebApplication.DTOS
{
    public class TransactionHistoryDTO
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public decimal Amount { get; set; }

        public decimal CurrentBalance { get; set; }

        public TransactionType TransactionType { get; set; }

        public TransactionStatus TransactionStatus { get; set; }

        public BetType BetType { get; set; }

        public string? Currency { get; set; }

        public WinType WinType { get; set; }
    }
}
