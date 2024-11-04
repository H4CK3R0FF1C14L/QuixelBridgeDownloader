namespace QuixelBridgeDownloader.Interfaces.Base
{
    public interface IRepository<T> where T : class, IEntity, new()
    {
        public IQueryable<T>? Items { get; }
        public bool AutoSaveChanges { get; set; }

        public T? Get(int id);
        public Task<T?> GetAsync(int id, CancellationToken token = default);

        public T Add(T item);
        public Task<T> AddAsync(T item, CancellationToken token = default);

        public void Update(T item);
        public Task UpdateAsync(T item, CancellationToken token = default);

        public void Remove(int id);
        public Task RemoveAsync(int id, CancellationToken token = default);
    }
}
