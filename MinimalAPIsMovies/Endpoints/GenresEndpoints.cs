using FluentValidation;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.OutputCaching;
using MinimalAPIsMovies.Entities;
using MinimalAPIsMovies.Filters;
using MinimalAPIsMovies.Repositories;

namespace MinimalAPIsMovies.Endpoints
{
    public static class GenresEndpoints
    {
        public static RouteGroupBuilder MapGenres(this RouteGroupBuilder genresEndpoints) 
        {
            

            genresEndpoints.MapGet("/", [EnableCors(policyName: "free")] async (IGenresRepository repo, ILoggerFactory loggerFactory) =>
            {
                var type = typeof(GenresEndpoints);
                var logger = loggerFactory.CreateLogger(type.FullName);
                logger.LogTrace("This is a trace message");
                logger.LogDebug("This is a debug message");
                logger.LogInformation("This is a information message");
                logger.LogWarning("This is a warning message");
                logger.LogError("This is a error message");
                logger.LogCritical("This is a critical message");
                //logger.LogInformation("getting the list of genres");


                return await repo.GetAll();
            }).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(15)).Tag("genre-get"));

            genresEndpoints.MapGet("/{id:int}", async (int id, IGenresRepository repo) =>
            {
                var genre = await repo.GetById(id);
                if (genre is null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(genre);
            });


            genresEndpoints.MapPost("/", async (Genre genre, IGenresRepository repo, IOutputCacheStore cStore) =>
            {
                var id = await repo.Create(genre);
                await cStore.EvictByTagAsync("genre-get", default);
                return Results.Created($"/genre/{id}", genre);

            }).AddEndpointFilter<GenresValidationFilter>();


            genresEndpoints.MapPut("/", async (Genre genre, IGenresRepository repo, IOutputCacheStore cStore) =>
            {
                var exist = await repo.Exists(genre.Id);
                if (!exist)
                {
                    return Results.NotFound();
                }
                await repo.Update(genre);
                return Results.NoContent();
            }).AddEndpointFilter<GenresValidationFilter>(); 
            genresEndpoints.MapDelete("/{id:int}", async (int id, IGenresRepository repo) =>
            {
                var genre = await repo.Exists(id);
                if (!genre)
                {
                    return Results.NotFound();
                }
                await repo.Delete(id);
                return Results.NoContent();
            });

            return genresEndpoints;
        }

    }
}
