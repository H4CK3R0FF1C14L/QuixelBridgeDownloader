using QuixelBridgeDownloader.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuixelBridgeDownloader.Entities
{
    public class Item : Entity
    {
        public string? QuixelId { get; set; }
        public string? Name { get; set; }
        public string? Tags { get; set; }
        public string? PreviewImage { get; set; }
        public string? Categories { get; set; }
        public bool IsSendedFirstMessage { get; set; }
        public bool IsSendedSecondMessage { get; set; }

        [NotMapped]
        public string[] TagsArray
        {
            get => Tags?.Split(',') ?? Array.Empty<string>();
            set => Tags = string.Join(',', value);
        }

        [NotMapped]
        public string[] CategoriesArray
        {
            get => Categories?.Split(',') ?? Array.Empty<string>();
            set => Categories = string.Join(',', value);
        }
    }
}
