using System.Text.Json.Serialization;

namespace MusicSmash.Controllers.Api.Spotify.Contracts
{
    public class UserProfileResponse
    {
        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; }

        [JsonPropertyName("product")]
        public string Product { get; set; }
    }
}