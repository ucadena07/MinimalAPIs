using MinimalAPIsMovies.Entities;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(conf =>
    {
        conf.WithOrigins(builder.Configuration["allowedOrigins"]).AllowAnyMethod().AllowAnyHeader();
    });
    opt.AddPolicy("free", conf =>
    {
        conf.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/genres", () =>
{
    return new List<Genre>() { new Genre() {Id = 1, Name = "Drama" }, new Genre() { Id = 2, Name = "Action" }, new Genre() { Id = 3, Name = "Comedy" } };   
});

app.Run();
