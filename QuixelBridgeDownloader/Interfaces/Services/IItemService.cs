using QuixelBridgeDownloader.Entities;

namespace QuixelBridgeDownloader.Interfaces.Services
{
    public interface IItemService
    {
        public IQueryable<Item>? Items { get; }

        public Item? GetItemById(int id);
        public Item? GetItemByQuixelId(string name);
        public Item AddItem(Item item);
        public Task<Item> AddItemAsync(Item item);
        public void UpdateItem(Item item);
        public Task UpdateItemAsync(Item item);
    }
}
