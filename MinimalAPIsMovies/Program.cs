using FluentValidation;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using MinimalAPIsMovies.Data;
using MinimalAPIsMovies.Endpoints;
using MinimalAPIsMovies.Entities;
using MinimalAPIsMovies.Repositories;
using MinimalAPIsMovies.Services;
using MinimalAPIsMovies.Utils;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Error = MinimalAPIsMovies.Entities.Error;


var builder = WebApplication.CreateBuilder(args);

//Services zone begins
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer("name=DefaultConnection")
);

builder.Services.AddIdentityCore<IdentityUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<UserManager<IdentityUser>>();
builder.Services.AddScoped<SignInManager<IdentityUser>>();  

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
builder.Services.AddScoped<IErrorsRepository, ErrorsRepository>();  
builder.Services.AddScoped<IFileStorage, LocalFileStorage>();  
builder.Services.AddHttpContextAccessor();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddProblemDetails();

builder.Services.AddAuthentication().AddJwtBearer(opts =>
{
    opts.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = false,  
        ValidateAudience = false,   
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,  
        //IssuerSigningKey = KeysHandler.GetKey(builder.Configuration).First(),   
        IssuerSigningKeys = KeysHandler.GetAllKeys(builder.Configuration)
    };
});
builder.Services.AddAuthorization();    

//Services zone ends
var app = builder.Build();


//Middleware zone begin
app.UseSwagger();
app.UseSwaggerUI();
app.UseExceptionHandler(exHandler =>
{
    exHandler.Run(async context =>
    {
        var exHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
        var ex = exHandlerFeature?.Error;

        var error = new Error();
        error.Date = DateTime.UtcNow;
        error.StackTrace = ex.StackTrace;
        error.ErrorMessage = ex.Message;    


        var repo = context.RequestServices.GetRequiredService<IErrorsRepository>();
        await repo.Create(error);
        await Results.BadRequest(new { type = "error", message="an unexpected exception has occured", status = 500}).ExecuteAsync(context);
    });
});
app.UseStatusCodePages();   
app.UseStaticFiles();
app.UseCors();
app.UseOutputCache();

app.UseAuthorization();

app.MapGet("/", () => "Hello World!");
app.MapGet("/error", () => { throw new InvalidOperationException("example error"); });
app.MapGroup("/genres").MapGenres();
app.MapGroup("/actors").MapActors();

//Middleware zone ends
app.Run();
