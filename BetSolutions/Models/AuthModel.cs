namespace BetSolutions.Models
{
    public class AuthModel
    {
        public string? Token { get; set; }
        public int MerchantId { get; set; }
        public string Lang { get; set; }
        public int GameId { get; set; }
        public int ProductId { get; set; }
        public int IsFreePlay { get;set; }
        public string Platform { get; set; }
    }
}
