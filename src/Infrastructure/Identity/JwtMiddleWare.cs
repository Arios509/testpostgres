using Infrastructure.Identity.Helpers;
using Microsoft.Extensions.Primitives;

namespace Infrastructure.Identity;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IJwtUtils jwtUtils)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token != null)
            AttachMerchantToContext(context, token, jwtUtils);

        await _next(context);
    }

    public void AttachMerchantToContext(HttpContext context, string token, IJwtUtils jwtUtils)
    {
        try
        {
            if (StringValues.IsNullOrEmpty(token))
            {
                //context.Result = new JsonResult(new { message = "Missing Authorization Header" }) { StatusCode = StatusCodes.Status401Unauthorized };
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            var validatedToken = jwtUtils.ValidateToken(token);

            if (validatedToken != null)
            {
                context.Items["UserId"] = validatedToken.Claims.First(d => d.Type == "userId").Value;
                context.Items["Name"] = validatedToken.Claims.First(d => d.Type == "name").Value;
                context.Items["Permissions"] = validatedToken.Claims.First(d => d.Type == "permissions").Value;
            };
        }
        catch (Exception ex)
        {
        }
    }
}