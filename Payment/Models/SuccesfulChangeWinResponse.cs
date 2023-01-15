namespace Payment.Models
{
    public class SuccesfulChangeWinResponse
    {
        public int StatusCode { get; set; }

        public ChangeWinInfo? Data { get; set; }
    }
}
