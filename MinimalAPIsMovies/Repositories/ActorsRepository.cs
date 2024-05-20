using Microsoft.EntityFrameworkCore;
using MinimalAPIsMovies.Data;
using MinimalAPIsMovies.Dtos;
using MinimalAPIsMovies.Entities;

namespace MinimalAPIsMovies.Repositories;

public interface IActorsRepository
{
    Task<int> Create(Actor actor);
    Task Delete(int id);
    Task<bool> Exists(int id);
    Task<List<Actor>> GetAll(PaginationDTO pagination);
    Task<Actor> GetById(int id);
    Task<List<Actor>> GetByName(string name);
    Task Update(Actor actor);
}
public class ActorsRepository(ApplicationDbContext context) : IActorsRepository
{
    public async Task<List<Actor>> GetAll(PaginationDTO pagination)
    {
        return await context.Actors.OrderBy(a => a.Name).ToListAsync();
    }
    public async Task<Actor> GetById(int id)
    {
        return await context.Actors.AsNoTracking().SingleOrDefaultAsync(a => a.Id == id);  
    }
    public async Task<List<Actor>> GetByName(string name)
    {
        return await context.Actors.Where(a => a.Name.Contains(name)).OrderBy(a => a.Name).ToListAsync();   
    }
    public async Task<int> Create(Actor actor)
    {
        context.Add(actor); 
        await context.SaveChangesAsync();   
        return actor.Id;    
    }
    public async Task<bool> Exists(int id)
    {
        return await context.Actors.AnyAsync(it => it.Id == id);
    }
    public async Task Update(Actor actor)
    {
        context.Update(actor);  
        await context.SaveChangesAsync();   
    }
    public async Task Delete(int id)
    {
        await context.Actors.Where(it => it.Id == id).ExecuteDeleteAsync();
    }
}
