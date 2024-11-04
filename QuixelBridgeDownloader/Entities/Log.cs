using QuixelBridgeDownloader.Entities.Base;
using QuixelBridgeDownloader.Models;

namespace QuixelBridgeDownloader.Entities
{
    public class Log : Entity
    {
        public LogType? Type { get; set; }
        public string? Message { get; set; }
        public DateTime? CreationTime { get; set; }
    }
}
