namespace MinimalAPIsMovies.Entities
{
    public class Error
    {
        public Guid Id { get; set; }
        public string ErrorMessage { get; set; }
        public string StackTrace { get; set; }
        public DateTime Date { get; set; }
    }
}
