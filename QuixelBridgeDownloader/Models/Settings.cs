namespace QuixelBridgeDownloader.Models
{
    public class Settings
    {
        public List<string>? TextureResolutions { get; set; }
        public List<string>? TextureTypes { get; set; }
        public List<string>? TextureMimeTypes { get; set; }
        
        public bool Highpoly { get; set; }
        public bool LowerLodMeshes { get; set; }
        public bool LowerLodNormals { get; set; }
        public bool AlbedoLods { get; set; }
        public bool ZTool { get; set; }
        public bool Brushes { get; set; }
        public string? MaxLod { get; set; }
        public string? MeshMimeType { get; set; }

        public int MaxLodToInt()
        {
            if (MaxLod != null && MaxLod.StartsWith("LOD") && int.TryParse(MaxLod.Substring(3), out int lodNumber))
                return lodNumber;
            return 0; 
        }
    }
}