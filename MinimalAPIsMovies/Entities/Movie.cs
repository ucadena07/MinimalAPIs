namespace MinimalAPIsMovies.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool InTheathers { get; set; }
        public DateTime? RealeaseDate { get; set; }
        public string Poster { get; set; }
    }
}
