using System.Text.Json.Serialization;

namespace QuixelBridgeDownloader.Models
{
    public class Model
    {
        [JsonPropertyName("mimeType")]
        public string? MimeType { get; set; }

        [JsonPropertyName("lod")]
        public int? Lod { get; set; }

        [JsonPropertyName("contentLength")]
        public int? ContentLength { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("variant")]
        public int? Variant { get; set; }

        [JsonPropertyName("tris")]
        public int? Tris { get; set; }

        [JsonPropertyName("uri")]
        public string? Uri { get; set; }
    }
}
