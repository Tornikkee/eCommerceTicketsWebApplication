namespace Payment.Models
{
    public class PaymentRecord
    {
        public string UserId { get; set; }

        public decimal Amount { get; set; }

        public string Status { get; set; }
    }
}
