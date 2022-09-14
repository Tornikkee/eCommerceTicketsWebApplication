namespace eCommerceTicketsWebApplication.DTOS
{
    public class CinemaDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<MovieDTO> movie = new List<MovieDTO>();
    }
}
