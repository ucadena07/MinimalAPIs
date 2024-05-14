using Microsoft.EntityFrameworkCore;
using MinimalAPIsMovies.Data;
using MinimalAPIsMovies.Entities;

namespace MinimalAPIsMovies.Repositories;

public interface IGenresRepository
{
    Task<int> Create(Genre genre);
    Task<Genre> GetById(int id);    
    Task<List<Genre>> GetAll();    
    Task<bool> Exists(int id);
    Task Update(Genre genre);
    Task Delete(int id);    
    
}

public class GenresRepository : IGenresRepository
{
    private readonly ApplicationDbContext context;

    public GenresRepository(ApplicationDbContext context)
    {
        this.context = context;
    }
    public async Task<int> Create(Genre genre)
    {
        context.Add(genre); 
        return await context.SaveChangesAsync();      
    }

    public async Task Delete(int id)
    {
        await context.Genres.Where(it => it.Id == id).ExecuteDeleteAsync();
    }

    public async Task<bool> Exists(int id)
    {
        return await context.Genres.AnyAsync(it => it.Id == id);   
    }

    public async Task<List<Genre>> GetAll()
    {
        return await context.Genres?.ToListAsync();    
    }

    public async Task<Genre> GetById(int id)
    {
        return await context.Genres?.FirstOrDefaultAsync(it => it.Id == id);
    }

    public async Task Update(Genre genre)
    {
        context.Update(genre);
        await context.SaveChangesAsync();   
    }
}