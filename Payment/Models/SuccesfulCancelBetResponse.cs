namespace Payment.Models
{
    public class SuccesfulCancelBetResponse
    {
        public int StatusCode { get; set; }

        public CancelBetInfo? Data { get; set; }
    }
}
