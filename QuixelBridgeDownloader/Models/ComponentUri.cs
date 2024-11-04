using System.Text.Json.Serialization;

namespace QuixelBridgeDownloader.Models
{
    public class ComponentUri
    {
        [JsonPropertyName("resolutions")]
        public List<ComponentResolution>? Resolutions { get; set; }
    }
}
