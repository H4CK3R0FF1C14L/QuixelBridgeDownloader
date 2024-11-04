using Microsoft.EntityFrameworkCore;
using QuixelBridgeDownloader.Entities.Base;
using QuixelBridgeDownloader.Interfaces.Base;

namespace QuixelBridgeDownloader.Data.Context
{
    public class DbRepository<T> : IRepository<T> where T : Entity, new()
    {
        #region Properties

        #region Private Properties

        private readonly ApplicationContext DB;
        private readonly DbSet<T>? _Set;

        #endregion

        #region Public Properties

        public bool AutoSaveChanges { get; set; } = true;
        public virtual IQueryable<T> Items => _Set!;

        #endregion

        #endregion

        #region Class Constructor

        public DbRepository(ApplicationContext db)
        {
            DB = db;
            _Set = DB.Set<T>();
        }

        #endregion

        #region Public Methods

        #region  Get Methods

        public T? Get(int id) => Items.SingleOrDefault(item => item.Id == id);

        public async Task<T?> GetAsync(int id, CancellationToken cancellationToken = default) 
            => await Items.SingleOrDefaultAsync(item => item.Id == id, cancellationToken: cancellationToken)
                          .ConfigureAwait(false);

        #endregion

        #region Add Methods

        public T Add(T item)
        {
            ArgumentNullException.ThrowIfNull(item);
            DB.Entry(item).State = EntityState.Added;

            if (AutoSaveChanges)
                DB.SaveChanges();

            return item;
        }

        public async Task<T> AddAsync(T item, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(item);
            DB.Entry(item).State = EntityState.Added;

            if (AutoSaveChanges)
                await DB.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return item;
        }

        #endregion

        #region Update Methods

        public void Update(T item)
        {
            ArgumentNullException.ThrowIfNull(item);
            DB.Entry(item).State = EntityState.Modified;

            if (AutoSaveChanges)
                DB.SaveChanges();
        }

        public async Task UpdateAsync(T item, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(item);
            DB.Entry(item).State = EntityState.Modified;

            if (AutoSaveChanges)
                await DB.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        #endregion

        #region Remove Methods

        public void Remove(int id)
        {
            var item = _Set!.Local.FirstOrDefault(i => i.Id == id) ?? new T { Id = id };
            DB.Remove(item);

            if (AutoSaveChanges)
                DB.SaveChanges();
        }

        public async Task RemoveAsync(int id, CancellationToken cancellationToken = default)
        {
            var item = _Set!.Local.FirstOrDefault(i => i.Id == id) ?? new T { Id = id };
            DB.Remove(item);

            if (AutoSaveChanges)
                await DB.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        #endregion

        #endregion
    }
}
