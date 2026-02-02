using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TiktokGame2Server.Others;

public class TokenAuthMiddleware
{
    private readonly RequestDelegate _next;

    public TokenAuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ITokenService tokenService)
    {
        // 只验证API接口（可根据实际情况调整路径过滤）
        if (context.Request.Path.StartsWithSegments("/api") || context.Request.Path.StartsWithSegments("/Game"))
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault();

            if (string.IsNullOrEmpty(token) || !tokenService.ValidateToken(token))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Token无效或未提供");
                return;
            }
        }

        await _next(context);
    }
}