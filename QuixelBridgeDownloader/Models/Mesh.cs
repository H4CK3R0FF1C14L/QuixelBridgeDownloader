using System.Text.Json.Serialization;

namespace QuixelBridgeDownloader.Models
{
    public class Mesh
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("uris")]
        public List<MeshUri>? MeshUris { get; set; }

        [JsonPropertyName("tris")]
        public int? Tris { get; set; }
    }
}
