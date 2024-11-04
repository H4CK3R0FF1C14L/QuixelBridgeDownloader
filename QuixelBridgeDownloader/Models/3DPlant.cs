using System.Text.Json.Serialization;

namespace QuixelBridgeDownloader.Models
{
    public class _3DPlant : Asset
    {
        [JsonPropertyName("billboards")]
        public List<Map>? Billboards { get; set; }

        [JsonPropertyName("maps")]
        public List<Map>? Maps { get; set; }

        [JsonPropertyName("components")]
        public List<Component>? Components { get; set; }

        [JsonPropertyName("models")]
        public List<Model>? Models { get; set; }

        [JsonPropertyName("meshes")]
        public List<Mesh>? Meshes { get; set; }

        [JsonPropertyName("uasset")]
        public List<UAsset>? UAssets { get; set; }
    }
}
