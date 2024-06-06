namespace MinimalAPIsMovies.Dtos
{
    public class AuthResponse
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
