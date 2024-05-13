using System.ComponentModel.DataAnnotations;

namespace MinimalAPIsMovies.Entities
{
    public class Genre
    {
        public int Id { get; set; }
        [StringLength(150)]
        public string Name { get; set; }
    }
}
