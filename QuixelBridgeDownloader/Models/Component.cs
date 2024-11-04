using System.Text.Json.Serialization;

namespace QuixelBridgeDownloader.Models
{
    public class Component
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("colorSpace")]
        public string? ColorSpace { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("uris")]
        public List<ComponentUri>? Uris { get; set; }
    }
}
