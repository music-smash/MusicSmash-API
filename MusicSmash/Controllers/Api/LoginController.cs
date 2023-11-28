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

        public LoginController()
        {
                
        }

        [Route("/callback")]
        [HttpGet]
        public async void Callback(
                            [FromRoute] string state,
                            [FromRoute] string code,
                            [FromRoute] string error)
        {
            if (state is null || error is not null)
                return;

            //Get token
            var result = await _spotifyAPI.AccessTokenAsync(new()
                { 
                    GrantType = "authorization_code",
                    Code = code,
                    RedirectUri = Request.GetEncodedUrl()
            });

            Console.WriteLine(result.AccessToken);
            // set token on local
            Redirect("/");
        }

    }
}
