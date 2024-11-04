using System.Text.Json.Serialization;

namespace QuixelBridgeDownloader.Models
{
    public class AuthData
    {
        [JsonPropertyName("token")]
        public string? Token { get; set; }

        [JsonPropertyName("refreshToken")]
        public string? RefreshToken { get; set; }
    }
}
