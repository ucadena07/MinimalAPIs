using Microsoft.AspNetCore.Cors;
using MinimalAPIsMovies.Entities;

var builder = WebApplication.CreateBuilder(args);

//Services zone begins

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

//Services zone ends
var app = builder.Build();


//Middleware zone begin
app.UseCors();
app.UseOutputCache();

app.MapGet("/", () => "Hello World!");

app.MapGet("/genres",[EnableCors(policyName:"free")] () =>
{
    return new List<Genre>() { new Genre() {Id = 1, Name = "Drama" }, new Genre() { Id = 2, Name = "Action" }, new Genre() { Id = 3, Name = "Comedy" } };   
}).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(15)));
//Middleware zone ends
app.Run();
