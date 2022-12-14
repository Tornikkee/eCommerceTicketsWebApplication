namespace eCommerceTicketsWebApplication.Models
{
    public class AuthInfo
    {
        public string UserId { get; set; }

        public string PublicToken { get; set; }

        public override string ToString()
        {
            return $"{UserId}, {PublicToken}";
        }
    }
}
