namespace eCommerceTicketsWebApplication.DTOS
{
    public class OrderDTO
    {
        public int Id { get; set; }

        public int Amount { get; set; }

        public float Price { get; set; }

        public string Name { get; set; }

        public string FullName { get; set; }
    }
}
