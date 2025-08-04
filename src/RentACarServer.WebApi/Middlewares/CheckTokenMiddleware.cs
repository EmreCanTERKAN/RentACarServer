using RentACarServer.Domain.LoginTokens;
using System.Security.Claims;

namespace RentACarServer.WebApi.Middlewares;

public sealed class CheckTokenMiddleware(
    ILoginTokenRepository loginTokenRepository) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var token = context.Request.Headers.Authorization.ToString().Replace("Bearer ", "");

        if (string.IsNullOrWhiteSpace(token))
        {
            await next(context);
            return;
        }

        var userId = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

        if(userId is null)
        {
            throw new TokenException();
        }

        var isTokenAvailable = await loginTokenRepository.AnyAsync(p => 
        p.UserId == userId && 
        p.Token.Value == token && 
        p.IsActive.Value == true);

        if (!isTokenAvailable)
        {
            throw new TokenException();
        }

        await next(context);
    }
}

public sealed class  TokenException : Exception;