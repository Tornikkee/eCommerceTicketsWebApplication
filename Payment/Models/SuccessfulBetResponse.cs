namespace Payment.Models
{
    public class SuccessfulBetResponse
    {
        public int StatusCode { get; set; }

        public BetInfo Data { get; set; }
    }
}
