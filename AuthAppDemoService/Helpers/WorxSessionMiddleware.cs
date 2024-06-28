// WorxSessionMiddleware.cs
using AuthAppDemoService.Helpers;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

public class WorxSessionMiddleware
{
    private readonly RequestDelegate _next;

    public WorxSessionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, WorxSession worxSession)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token != null)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

            if (jwtToken != null)
            {
                // get user id claim value and convert it from string to long
                long.TryParse(jwtToken.Claims.FirstOrDefault(claim => claim.Type == "nameid")?.Value, out long userId);
                // set user id value to Session user id Property
                worxSession.UserId = userId;
            }
        }

        await _next(context);
    }
}

