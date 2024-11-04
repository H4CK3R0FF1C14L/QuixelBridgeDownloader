using System.Text.Json.Serialization;

namespace QuixelBridgeDownloader.Models
{
    public class Previews
    {
        [JsonPropertyName("images")]
        public List<Image>? Images { get; set; }
    }
}
