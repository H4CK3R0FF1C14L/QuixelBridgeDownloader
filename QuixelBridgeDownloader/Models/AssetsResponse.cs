using System.Text.Json.Serialization;

namespace QuixelBridgeDownloader.Models
{
    public class AssetsResponse
    {
        [JsonPropertyName("assets")]
        public List<AssetBase>? Assets { get; set; }

        [JsonPropertyName("page")]
        public int Page { get; set; }

        [JsonPropertyName("pages")]
        public int Pages { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("count")]
        public int Count { get; set; }
    }
}
