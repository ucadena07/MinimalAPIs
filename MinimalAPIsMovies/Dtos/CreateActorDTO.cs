namespace MinimalAPIsMovies.Dtos
{
    public class CreateActorDTO
    {
        public string Name { get; set; }
        public DateTime? DoB { get; set; }
        public IFormFile Picture { get; set; }
    }
}
