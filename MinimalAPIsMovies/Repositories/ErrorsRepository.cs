using MinimalAPIsMovies.Data;
using MinimalAPIsMovies.Entities;

namespace MinimalAPIsMovies.Repositories;
public interface IErrorsRepository
{
    Task Create(Error err);
}

public class ErrorsRepository(ApplicationDbContext context) : IErrorsRepository 
{
    public async Task Create(Error err)
    {
        context.Add(err);
        await context.SaveChangesAsync();
    }
}
