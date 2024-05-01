using System.Text.Json.Serialization;

namespace MusicSmash.Models
{
    public class User
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}