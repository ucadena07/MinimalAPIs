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
            

            genresEndpoints.MapGet("/", [EnableCors(policyName: "free")] async (IGenresRepository repo) =>
            {
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
            }).AddEndpointFilter<TestFilters>();


            genresEndpoints.MapPost("/", async (Genre genre, IGenresRepository repo, IOutputCacheStore cStore, IValidator<Genre> validator) =>
            {
                var validatinResults = await validator.ValidateAsync(genre);   
                if(!validatinResults.IsValid)
                {
                    return TypedResults.ValidationProblem(validatinResults.ToDictionary());
                }



                var id = await repo.Create(genre);
                await cStore.EvictByTagAsync("genre-get", default);
                return Results.Created($"/genre/{id}", genre);
            });
            genresEndpoints.MapPut("/", async (Genre genre, IGenresRepository repo, IOutputCacheStore cStore, IValidator<Genre> validator) =>
            {
                var validatinResults = await validator.ValidateAsync(genre);
                if (!validatinResults.IsValid)
                {
                    return TypedResults.ValidationProblem(validatinResults.ToDictionary());
                }
                var exist = await repo.Exists(genre.Id);
                if (!exist)
                {
                    return Results.NotFound();
                }
                await repo.Update(genre);
                return Results.NoContent();
            });
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
