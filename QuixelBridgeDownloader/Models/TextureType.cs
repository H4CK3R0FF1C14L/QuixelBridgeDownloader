using System.ComponentModel;

namespace QuixelBridgeDownloader.Models
{
    public enum TextureType
    {
        [Description("Albedo")]
        Albedo,

        [Description("Metalness")]
        Metalness,

        [Description("Roughness")]
        Roughness,

        [Description("Specular")]
        Specular,

        [Description("Diffuse")]
        Diffuse,

        [Description("Gloss")]
        Gloss,

        [Description("Displacement")]
        Displacement,

        [Description("Normal")]
        Normal,

        [Description("Normal Bump")]
        NormalBump,

        [Description("Normal Object")]
        NormalObject,

        [Description("Bump")]
        Bump,

        [Description("Curvature")]
        Curvature,

        [Description("Cavity")]
        Cavity,

        [Description("AO")]
        AO,

        [Description("Opacity")]
        Opacity,

        [Description("Brush")]
        Brush,

        [Description("Fuzz")]
        Fuzz,

        [Description("Mask")]
        Mask,

        [Description("Thickness")]
        Thickness,

        [Description("Translucency")]
        Translucency,

        [Description("Transmission")]
        Transmission
    }
}
