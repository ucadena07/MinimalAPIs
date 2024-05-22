using Microsoft.EntityFrameworkCore;
using MinimalAPIsMovies.Entities;

namespace MinimalAPIsMovies.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Genre>().Property(p => p.Name).HasMaxLength(150);    


            modelBuilder.Entity<Actor>().Property(p => p.Name).HasMaxLength(450);    
            modelBuilder.Entity<Actor>().Property(p => p.Picture).IsUnicode();    

            modelBuilder.Entity<Movie>().Property(p => p.Title).HasMaxLength(250);
            modelBuilder.Entity<Movie>().Property(p => p.Poster).IsUnicode();   
        }
        public DbSet<Genre> Genres { get; set; }    
        public DbSet<Actor> Actors { get; set; }    
        public DbSet<Movie> Movies { get; set; }    
    }
}
