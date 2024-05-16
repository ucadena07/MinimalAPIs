using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using MinimalAPIsMovies.Data;
using MinimalAPIsMovies.Endpoints;
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
app.MapGroup("/genres").MapGenres();

//Middleware zone ends
app.Run();
