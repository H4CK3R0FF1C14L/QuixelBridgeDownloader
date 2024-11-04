using System.Text.Json.Serialization;

namespace QuixelBridgeDownloader.Models
{
    public class DownloadInfo
    {
        [JsonPropertyName("asset")]
        public string? AssetId { get; set; }

        [JsonPropertyName("license")]
        public string? License { get; set; }

        [JsonPropertyName("config")]
        public Config? Config { get; set; }

        [JsonPropertyName("billboards")]
        public List<Map>? Billboards { get; set; }

        [JsonPropertyName("meshes")]
        public List<Model>? Meshes { get; set; }

        [JsonPropertyName("components")]
        public List<Map>? Components { get; set; }

        [JsonPropertyName("paying")]
        public bool? Paying { get; set; }

        [JsonPropertyName("previews")]
        public List<Image>? Previews { get; set; }

        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("time")]
        public string? Time { get; set; }

        [JsonPropertyName("expires")]
        public string? Expires { get; set; }
    }
}
