using System.Text.Json.Serialization;

namespace QuixelBridgeDownloader.Models
{
    public class Image
    {
        [JsonPropertyName("contentLength")]
        public int? ContentLength { get; set; }

        [JsonPropertyName("resolution")]
        public string? Resolution { get; set; }

        [JsonPropertyName("uri")]
        public string? Uri { get; set; }

        [JsonPropertyName("tags")]
        public List<string>? Tags { get; set; }
    }
}
