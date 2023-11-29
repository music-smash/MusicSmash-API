using Microsoft.AspNetCore.Authentication.BearerToken;
using MusicSmash.Controllers.Api.Spotify.Contracts;
using System.Net.Http.Json;
using System.Text;
using System.Text.Unicode;

namespace MusicSmash.Controllers.Api.Spotify
{
    public class Api
    {
        private readonly IConfiguration _configuration;

        public Api(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Contracts.AccessTokenResponse> AccessTokenAsync(AccessTokenRequest request)
        {
            var endpoint = "https://accounts.spotify.com/api/token";

            var clientId = _configuration["spotify:client-id"];
            var clientSecret = _configuration["spotify:client-secret"];

            using(var client = new HttpClient())
            {

                var auth = UTF8Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}");
                client.DefaultRequestHeaders.Authorization = new ("Basic", Convert.ToBase64String(auth));
                
                var httpRequest = new FormUrlEncodedContent(request.ToDictionary());

                var response = await client.PostAsync(endpoint, httpRequest);

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<Contracts.AccessTokenResponse>();
            }
        }
    }
}
