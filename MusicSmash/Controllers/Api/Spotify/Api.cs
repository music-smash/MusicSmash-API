using Microsoft.AspNetCore.Authentication.BearerToken;
using MusicSmash.Controllers.Api.Spotify.Contracts;
using System.Net.Http.Json;

namespace MusicSmash.Controllers.Api.Spotify
{
    public class Api
    {

        public async Task<Contracts.AccessTokenResponse> AccessTokenAsync(AccessTokenRequest request)
        {
            var endpoint = "https://accounts.spotify.com/api/token";

            using(var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new ("Basic", "");
                client.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");
                var response = await client.PostAsJsonAsync(endpoint, request);

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<Contracts.AccessTokenResponse>();
            }
        }
    }
}
