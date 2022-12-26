namespace Payment.Models
{
    public class SuccessfulWinResponse
    {
        public int StatusCode { get; set; }

        public WinInfo Data { get; set; }
    }
}
