using System.Text.Json.Serialization;

namespace QuixelBridgeDownloader.Models
{
    public class UAsset
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("ueVirsion")]
        public string? UeVersion { get; set; }

        [JsonPropertyName("tier")]
        public int? Tier { get; set; }

        [JsonPropertyName("mimeType")]
        public string? MimeType { get; set; }

        [JsonPropertyName("uri")]
        public string? Uri { get; set; }
    }
}
