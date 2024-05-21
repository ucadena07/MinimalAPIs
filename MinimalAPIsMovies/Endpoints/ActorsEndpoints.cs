using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using MinimalAPIsMovies.Dtos;
using MinimalAPIsMovies.Entities;
using MinimalAPIsMovies.Repositories;
using MinimalAPIsMovies.Services;

namespace MinimalAPIsMovies.Endpoints
{
    public static class ActorsEndpoints
    {
        public static RouteGroupBuilder MapActors(this RouteGroupBuilder group)
        {
    


            group.MapGet("/", GetAll).CacheOutput(c => c.Expire(TimeSpan.FromMinutes(1)).Tag("actors-get"));

            group.MapGet("/{id:int}", GetById);
            group.MapGet("getByName/{name}", GetByName);
            group.MapPost("/", Create).DisableAntiforgery();
            group.MapPut("/{id:int}", Update).DisableAntiforgery();
            return group;
        }

        static async Task<Created<Actor>> Create([FromForm] CreateActorDTO createActorDTO, IActorsRepository _repo, IOutputCacheStore outputCacheStore, IFileStorage fileStorage)
        {
            var actor = new Actor()
            {
                DateOfBirth = createActorDTO.DoB,
                Name = createActorDTO.Name,
            };

            var picture = await fileStorage.Store("actors",createActorDTO.Picture);
            actor.Picture = picture;    
            var id = await _repo.Create(actor);
            await outputCacheStore.EvictByTagAsync("actor-get", default);
            return TypedResults.Created($"/actors/{id}", actor);
        }

        static async Task<Ok<List<Actor>>> GetAll(IActorsRepository _repo, int page =1, int recordsPerPage = 10)
        {
            var pagination = new PaginationDTO() { Page = page, RecordsPerPage = recordsPerPage};
            var actors = await _repo.GetAll(pagination);  
            return TypedResults.Ok(actors); 
        }

        static async Task<Results<Ok<Actor>, NotFound>> GetById(int id, IActorsRepository repo)
        {
            var actor = await repo.GetById(id);
            if (actor is null)
            {
                return TypedResults.NotFound();
            }
            return TypedResults.Ok(actor);
        }
        static async Task<Results<Ok<List<Actor>>, NotFound>> GetByName(string name, IActorsRepository repo)
        {
            var actors = await repo.GetByName(name);
            if (actors is null)
            {
                return TypedResults.NotFound();
            }
            return TypedResults.Ok(actors);
        }

        static async Task<Results<NoContent,NotFound>> Update(int id,[FromForm] CreateActorDTO createActorDTO, IActorsRepository _repo, IFileStorage fileStorage, IOutputCacheStore outputCacheBufferStore)
        {
            var actorDB = await _repo.GetById(id);
            if(actorDB is null)
            {
                return TypedResults.NotFound();
            }
            var actorForUpdate = new Actor()
            {
                DateOfBirth = createActorDTO.DoB,
                Name = createActorDTO?.Name,
                Picture = actorDB.Picture,
                Id = id
            };

            if (createActorDTO.Picture is not null)
            {
                //var url = await fileStorage.Delete(actorDB.Picture);

            }
            await _repo.Update(actorForUpdate);
            await outputCacheBufferStore.EvictByTagAsync("actors-get", default);
            return TypedResults.NoContent();
        }
    }
}
