using QuixelBridgeDownloader.Entities;
using QuixelBridgeDownloader.Interfaces.Base;
using QuixelBridgeDownloader.Interfaces.Services;

namespace QuixelBridgeDownloader.Services.Database
{
    public class ItemService : IItemService
    {
        #region Properies

        private readonly IRepository<Item> _itemRepository;
        public IQueryable<Item>? Items => _itemRepository.Items;

        #endregion

        #region Class Constructor

        public ItemService(IRepository<Item> itemRepository)
        {
            _itemRepository = itemRepository;
        }

        #endregion

        #region Public Methods

        #region GET

        public Item? GetItemById(int id)
        {
            return Items!.SingleOrDefault(item => item.Id == id);
        }

        public Item? GetItemByQuixelId(string quixelId)
        {
            return Items!.SingleOrDefault(item => item.QuixelId == quixelId);
        }

        #endregion

        #region Add

        public Item AddItem(Item item)
        {
            return _itemRepository.Add(item);
        }

        public async Task<Item> AddItemAsync(Item item)
        {
            return await _itemRepository.AddAsync(item);
        }

        #endregion

        #region Update

        public void UpdateItem(Item item)
        {
            _itemRepository.Update(item);
        }

        public async Task UpdateItemAsync(Item item)
        {
            await _itemRepository.UpdateAsync(item);
        }

        #endregion

        #endregion
    }
}
