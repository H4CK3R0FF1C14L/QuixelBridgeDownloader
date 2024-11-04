using System.Text.Json.Serialization;

namespace QuixelBridgeDownloader.Models
{
    public class DownloadConfig
    {
        [JsonPropertyName("asset")]
        public string? AssetId { get; set; }

        [JsonPropertyName("config")]
        public Config? Config { get; set; }

        [JsonPropertyName("billboards")]
        public List<DownloadComponent>? Billboards { get; set; }

        [JsonPropertyName("components")]
        public List<DownloadComponent>? Components { get; set; }

        [JsonPropertyName("meshes")]
        public List<DownloadMesh>? Meshes { get; set; }
    }

    public class Config
    {
        [JsonPropertyName("highpoly")]
        public bool? Highpoly { get; set; }

        [JsonPropertyName("lowerlod_meshes")]
        public bool? LowerLodMeshes { get; set; }

        [JsonPropertyName("lowerlod_normals")]
        public bool? LowerLodNormals { get; set; }

        [JsonPropertyName("albedo_lods")]
        public bool? AlbedoLods { get; set; }

        [JsonPropertyName("ztool ")]
        public bool? ZTool { get; set; }

        [JsonPropertyName("brushes")]
        public bool? Brushes { get; set; }

        [JsonPropertyName("maxlod")]
        public int? MaxLod { get; set; }

        [JsonPropertyName("lods")]
        public int[]? Lods { get; set; }

        [JsonPropertyName("meshMimeType")]
        public string? MeshMimeType { get; set; }
    }

    public class DownloadComponent
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("resolution")]
        public string? Resolution { get; set; }

        [JsonPropertyName("mimeType")]
        public string? MimeType { get; set; }
    }

    public class DownloadMesh
    {
        [JsonPropertyName("lod")]
        public int? Lod { get; set; }

        [JsonPropertyName("mimeType")]
        public string? MimeType { get; set; }
    }
}
