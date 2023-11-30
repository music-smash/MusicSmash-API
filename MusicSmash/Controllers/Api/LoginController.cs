using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using MusicSmash.Controllers.Api.Spotify;
using System.Net;

namespace MusicSmash.Controllers.Api
{
    [ApiController]
    public class LoginController : Controller
    {
        private readonly Spotify.Api _spotifyAPI;

        public LoginController(Spotify.Api spotifyAPI)
        {
            _spotifyAPI = spotifyAPI;
        }

        [Route("/callback")]
        [HttpGet]
        public async Task<ActionResult> Callback(
                            [FromQuery] string state = "",
                            [FromQuery] string code = "",
                            [FromQuery] string error = "")
        {
            if (error is not "")
                return Redirect("/");


            //Get token
            var result = await _spotifyAPI.AccessTokenAsync(new()
                { 
                    GrantType = "authorization_code",
                    Code = code,
                    RedirectUri = Request.GetEncodedUrl().Split('?')[0]
                });

            //Console.WriteLine(result.AccessToken);
            Response.Cookies.Append("token", result.AccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.Lax
            });

            return Redirect("/vote");
        }

    }
}
