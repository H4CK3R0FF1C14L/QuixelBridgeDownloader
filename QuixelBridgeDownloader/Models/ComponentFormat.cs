using System.Text.Json.Serialization;

namespace QuixelBridgeDownloader.Models
{
    public class ComponentFormat
    {
        [JsonPropertyName("mimeType")]
        public string? MimeType { get; set; }

        [JsonPropertyName("contentLength")]
        public int? ContentLength { get; set; }

        [JsonPropertyName("lodType")]
        public string? LodType { get; set; }

        [JsonPropertyName("tris")]
        public int? Tris { get; set; }

        [JsonPropertyName("uri")]
        public string? Uri { get; set; }
    }
}
