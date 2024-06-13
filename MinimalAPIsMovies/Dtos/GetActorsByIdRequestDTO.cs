using MinimalAPIsMovies.Repositories;

namespace MinimalAPIsMovies.Dtos
{
    public class GetActorsByIdRequestDTO
    {
        public IActorsRepository Repository { get; set; }
        public int Id { get; set; }
    }
}
