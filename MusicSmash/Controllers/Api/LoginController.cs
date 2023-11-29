using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using MusicSmash.Controllers.Api.Spotify;

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
        public async Task<RedirectResult> Callback(
                            [FromQuery] string state = null,
                            [FromQuery] string code = null,
                            [FromQuery] string error = null)
        {
            if (code is null || error is not null)
                return Redirect("/home");

            //Get token
            var result = await _spotifyAPI.AccessTokenAsync(new()
                { 
                    GrantType = "authorization_code",
                    Code = code,
                    RedirectUri = Request.GetEncodedUrl().Split('?')[0]
            });

            Console.WriteLine(result.AccessToken);
            // set token on local
            return Redirect("/");
        }

    }
}
