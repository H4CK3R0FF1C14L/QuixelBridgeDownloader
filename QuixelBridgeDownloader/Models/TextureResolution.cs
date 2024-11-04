using System.ComponentModel;

namespace QuixelBridgeDownloader.Models
{
    public enum TextureResolution
    {
        [Description("8192x8192")]
        _8k,

        [Description("4096x4096")]
        _4k,

        [Description("2048x2048")]
        _2k,

        [Description("1024x1024")]
        _1k
    }
}
