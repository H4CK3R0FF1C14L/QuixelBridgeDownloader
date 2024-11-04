using System.Text.Json;
using System.Text.Json.Serialization;

namespace QuixelBridgeDownloader.Models
{
    public abstract class Asset
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("tags")]
        public List<string>? Tags { get; set; }

        [JsonPropertyName("categories")]
        public List<string>? Categories { get; set; }

        [JsonPropertyName("previews")]
        public Previews? Previews { get; set; }
    }

    public class AssetConverter : JsonConverter<Asset>
    {
        public override Asset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
            {
                var root = doc.RootElement;

                string? assetType = root.GetProperty("categories")[0].GetString()!;

                return assetType switch
                {
                    "3d" => JsonSerializer.Deserialize<_3DAsset>(root!.GetRawText(), options)!,
                    "3dplant" => JsonSerializer.Deserialize<_3DPlant>(root!.GetRawText(), options)!,
                    "surface" => JsonSerializer.Deserialize<Surface>(root.GetRawText(), options)!,
                    "atlas" => JsonSerializer.Deserialize<Atlas>(root.GetRawText(), options)!,
                    "brush" => JsonSerializer.Deserialize<Brush>(root.GetRawText(), options)!,
                    _ => null!
                };
            }
        }

        public override void Write(Utf8JsonWriter writer, Asset value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}
