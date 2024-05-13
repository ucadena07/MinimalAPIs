using Microsoft.AspNetCore.Cors;
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

app.MapGet("/genres", [EnableCors(policyName: "free")] async (IGenresRepository repo) =>
{
    return await repo.GetAll();
}).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(15)));

app.MapGet("/genres/{id:int}", async (int id, IGenresRepository repo) =>
{
    var genre = await repo.GetById(id);
    if(genre is null)
    {
        return Results.NotFound();
    }
    return Results.Ok(genre);   
});

app.MapPost("/genres", async (Genre genre, IGenresRepository repo) =>
{
    var id = await repo.Create(genre);
    return Results.Created($"/genre/{id}", genre);
});
//Middleware zone ends
app.Run();
