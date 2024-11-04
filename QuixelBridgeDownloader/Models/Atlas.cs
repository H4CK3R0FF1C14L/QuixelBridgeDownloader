using System.Text.Json.Serialization;

namespace QuixelBridgeDownloader.Models
{
    public class Atlas : Asset
    {
        [JsonPropertyName("maps")]
        public List<Map>? Maps { get; set; }

        [JsonPropertyName("components")]
        public List<Component>? Components { get; set; }

        [JsonPropertyName("uasset")]
        public List<UAsset>? UAssets { get; set; }
    }
}
