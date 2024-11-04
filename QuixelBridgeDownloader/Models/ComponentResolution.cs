using System.Text.Json.Serialization;

namespace QuixelBridgeDownloader.Models
{
    public class ComponentResolution
    {
        [JsonPropertyName("resolution")]
        public string? Resolution { get; set; }

        [JsonPropertyName("formats")]
        public List<ComponentFormat>? Formats { get; set; }
    }
}
