namespace QuixelBridgeDownloader.Interfaces.Base
{
    public interface IRepository<T> where T : class, IEntity, new()
    {
        IQueryable<T>? Items { get; }
        public bool AutoSaveChanges { get; set; }

        T? Get(int id);
        Task<T?> GetAsync(int id, CancellationToken token = default);

        T Add(T item);
        Task<T> AddAsync(T item, CancellationToken token = default);

        void Update(T item);
        Task UpdateAsync(T item, CancellationToken token = default);

        void Remove(int id);
        Task RemoveAsync(int id, CancellationToken token = default);
    }
}
