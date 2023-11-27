using System.Text.Json.Serialization;

namespace MusicSmash.Controllers.Api.Spotify.Contracts
{
    public class AccessTokenRequest
    {
        [JsonPropertyName("grant_type")]
        public required string GrantType { get; set; }

        [JsonPropertyName("code")]
        public required string Code { get; set; }

        [JsonPropertyName("redirect_uri")]
        public required string RedirectUri { get; set; }
    }
}
