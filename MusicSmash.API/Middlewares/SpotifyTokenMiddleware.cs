using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MusicSmash.API.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace MusicSmash.API.Middlewares
{
    public class SpotifyTokenMiddleware
    {
        private readonly RequestDelegate _next;

        public SpotifyTokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IUserService userService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                await attachUserToContext(context, userService, token);

            await _next(context);
        }

        private async Task attachUserToContext(HttpContext context, IUserService userService, string token)
        {
            try
            {
                context.Items["User"] = await userService.GetMeAsync(token);
            }
            catch
            {
                //Do nothing if JWT validation fails
                // user is not attached to context so the request won't have access to secure routes
            }
        }
    }
}
