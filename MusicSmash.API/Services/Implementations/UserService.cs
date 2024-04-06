using MusicSmash.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace MusicSmash.API.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<User> GetMeAsync(string jwtToken)
        {
            // GEt with bearer token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var response = await _httpClient.GetAsync("https://api.spotify.com/v1/me");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<User>(content);
        }
    }
}
