using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Velocity.Backend.Configurations;
using Velocity.Shared.Requests;
using Velocity.Shared.Responses;
using Velocity.Shared.Wrapper;
using HttpResult = Microsoft.AspNetCore.Http.IResult;

namespace Velocity.Backend.Endpoints;

internal static class LoginEndpoints
{
    public static HttpResult Login(LoginRequest loginRequest, TokenConfiguration tokenConfiguration)
    {
        if(loginRequest.UserName == "admin" && loginRequest.Password == "admin")
        {
            var claims = new List<Claim>
            {
                new ("Name", loginRequest.UserName),
                new ("FullName", "Administrator"),
            };
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(5),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfiguration.Secret)),
                    SecurityAlgorithms.HmacSha256
                )
            );
            var jwtHandler = new JwtSecurityTokenHandler();
            var tokenString = jwtHandler.WriteToken(token);
            return Results.Ok(Result<LoginResponse>.Success(new LoginResponse(tokenString)));
        }
        return Results.Unauthorized();
    }
}