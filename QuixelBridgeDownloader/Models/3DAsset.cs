using System.Text.Json.Serialization;

namespace QuixelBridgeDownloader.Models
{
    public class _3DAsset : Asset
    {
        [JsonPropertyName("semanticTags:3d_mesh")]
        public string? MeshType {get; set;}

        [JsonPropertyName("brushes")]
        public List<object>? Brushes { get; set; }

        [JsonPropertyName("meshes")]
        public List<Mesh>? Meshes { get; set; }

        [JsonPropertyName("models")]
        public List<Model>? Models { get; set; }

        [JsonPropertyName("components")]
        public List<Component>? Components { get; set; }

        [JsonPropertyName("maps")]
        public List<Map>? Maps { get; set; }
    }
}
