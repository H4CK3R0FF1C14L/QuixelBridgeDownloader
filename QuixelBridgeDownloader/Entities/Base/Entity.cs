using QuixelBridgeDownloader.Interfaces.Base;

namespace QuixelBridgeDownloader.Entities.Base
{
    public abstract class Entity : IEntity
    {
        public int Id { get; set; }
    }
}
