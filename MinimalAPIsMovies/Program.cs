using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using MinimalAPIsMovies.Data;
using MinimalAPIsMovies.Entities;
using MinimalAPIsMovies.Repositories;


var builder = WebApplication.CreateBuilder(args);

//Services zone begins
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer("name=DefaultConnection")
);

builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(conf =>
    {
        var origins = builder.Configuration["AllowedOrigins"];
        conf.WithOrigins(origins).AllowAnyMethod().AllowAnyHeader();
    });
    opt.AddPolicy("free", conf =>
    {
        conf.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

builder.Services.AddOutputCache();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IGenresRepository, GenresRepository>();  

//Services zone ends
var app = builder.Build();


//Middleware zone begin
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();
app.UseOutputCache();

app.MapGet("/", () => "Hello World!");
var genresEndpoints = app.MapGroup("/genres");

genresEndpoints.MapGet("/", [EnableCors(policyName: "free")] async (IGenresRepository repo) =>
{
    return await repo.GetAll();
}).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(15)).Tag("genre-get"));

genresEndpoints.MapGet("/{id:int}", async (int id, IGenresRepository repo) =>
{
    var genre = await repo.GetById(id);
    if(genre is null)
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
});
genresEndpoints.MapPut("/", async (Genre genre, IGenresRepository repo, IOutputCacheStore cStore) =>
{
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
//Middleware zone ends
app.Run();
