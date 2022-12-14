namespace Payment.Models
{
    public class SuccessfulBalanceResponse
    {
        public int StatusCode { get; set; }

        public Balance? Data { get; set; }
    }
}
