using FluentValidation;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using MinimalAPIsMovies.Data;
using MinimalAPIsMovies.Endpoints;
using MinimalAPIsMovies.Entities;
using MinimalAPIsMovies.Repositories;
using MinimalAPIsMovies.Services;


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
builder.Services.AddScoped<IActorsRepository, ActorsRepository>();  
builder.Services.AddScoped<IFileStorage, LocalFileStorage>();  
builder.Services.AddHttpContextAccessor();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddProblemDetails();

//Services zone ends
var app = builder.Build();


//Middleware zone begin
app.UseSwagger();
app.UseSwaggerUI();
app.UseExceptionHandler(exHandler =>
{
    exHandler.Run(async context =>
    {
        await Results.BadRequest(new { type = "error", message="an unexpected exception has occured", status = 500}).ExecuteAsync(context);
    });
});
app.UseStatusCodePages();   
app.UseStaticFiles();
app.UseCors();
app.UseOutputCache();

app.MapGet("/", () => "Hello World!");
app.MapGet("/error", () => { throw new InvalidOperationException("example error"); });
app.MapGroup("/genres").MapGenres();
app.MapGroup("/actors").MapActors();

//Middleware zone ends
app.Run();
