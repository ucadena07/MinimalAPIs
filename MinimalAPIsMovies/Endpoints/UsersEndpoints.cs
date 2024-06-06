using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MinimalAPIsMovies.Dtos;
using MinimalAPIsMovies.Filters;
using MinimalAPIsMovies.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace MinimalAPIsMovies.Endpoints
{
    public static class UsersEndpoints
    {
        public static RouteGroupBuilder MapUsers(this RouteGroupBuilder builder)
        {
            builder.MapPost("/register", Register).AddEndpointFilter<ValidationFilter<UserCredentialsDTO>>();
            builder.MapPost("/login", Login).AddEndpointFilter<ValidationFilter<UserCredentialsDTO>>();

            return builder;
        }



        static async Task<Results<Ok<AuthResponse>, BadRequest<IEnumerable<IdentityError>>>> Register(UserCredentialsDTO userCredentialsDTO,
            [FromServices] UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            var user = new IdentityUser
            {
                UserName = userCredentialsDTO.Email,
                Email = userCredentialsDTO.Email,
            };

            var results = await userManager.CreateAsync(user, userCredentialsDTO.Password);
            if (results.Succeeded)
            {
                var authResp = await BuildToken(userCredentialsDTO, userManager, configuration);
                return TypedResults.Ok(authResp);
            }
            else
            {
                return TypedResults.BadRequest(results.Errors);
            }
        }

        static async Task<Results<Ok<AuthResponse>, BadRequest<string>>> Login(
                                                            UserCredentialsDTO userCredentialsDTO,
                                                            [FromServices] SignInManager<IdentityUser> signInManager,
                                                            [FromServices] UserManager<IdentityUser> userManager,
                                                            IConfiguration configuration
                                                            )
        {
            var user = await userManager.FindByEmailAsync(userCredentialsDTO.Email);    
            if(user is null)
            {
                return TypedResults.BadRequest("There was a problem with the email or password");
            }
            var results = await signInManager.CheckPasswordSignInAsync(user,userCredentialsDTO.Password,lockoutOnFailure: false);

            if (results.Succeeded)
            {
                var authResp = await BuildToken(userCredentialsDTO, userManager, configuration);
                return TypedResults.Ok(authResp);   
            }
            else
            {
                return TypedResults.BadRequest("There was a problem with the email or password");
            }
        }

        async static Task<AuthResponse> BuildToken(UserCredentialsDTO userCredentialsDTO, UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            var claims = new List<Claim>
            {
                new Claim("email", userCredentialsDTO?.Email),
            };

            var user = await userManager.FindByEmailAsync(userCredentialsDTO.Email);
            var claimsFromDb = await userManager.GetClaimsAsync(user);
            claims.AddRange(claimsFromDb);

            var key = KeysHandler.GetKey(configuration).FirstOrDefault();
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var experation = DateTime.UtcNow.AddMinutes(30);

            var securityToken = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: experation,
                signingCredentials: credentials
                );

            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return new AuthResponse
            {
                Expiration = experation,
                Token = token
            };


        }
    }
}
