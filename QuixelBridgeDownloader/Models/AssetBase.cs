using System.Text.Json.Serialization;

namespace QuixelBridgeDownloader.Models
{
    public class AssetBase
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("tags")]
        public List<string>? Tags { get; set; }

        [JsonPropertyName("previews")]
        public Previews? Previews { get; set; }

        [JsonPropertyName("points")]
        public int Points { get; set; }

        [JsonPropertyName("categories")]
        public List<string>? Categories { get; set; }

        [JsonPropertyName("averageColor")]
        public string? AverageColor { get; set; }

        [JsonPropertyName("physicalSize")]
        public string? PhysicalSize { get; set; }
    }
}
