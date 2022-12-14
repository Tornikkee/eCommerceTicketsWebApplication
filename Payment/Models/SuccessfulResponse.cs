namespace Payment.Models
{
    public class SuccessfulResponse
    {
        public int StatusCode { get; set; }

        public UserInfo? Data { get; set; }
    }
}
