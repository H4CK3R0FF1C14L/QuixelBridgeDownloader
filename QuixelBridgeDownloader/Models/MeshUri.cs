using System.Text.Json.Serialization;

namespace QuixelBridgeDownloader.Models
{
    public class MeshUri
    {
        [JsonPropertyName("mimeType")]
        public string? MimeType { get; set; }

        [JsonPropertyName("contentLength")]
        public int? ContentLength { get; set; }

        [JsonPropertyName("uri")]
        public string? Uri { get; set; }
    }
}
