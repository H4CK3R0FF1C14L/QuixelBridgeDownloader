using QuixelBridgeDownloader.Data.Context;

namespace QuixelBridgeDownloader.Data
{
    public class DbInitializer
    {
        #region Properties

        #region Private Properties

        private readonly ApplicationContext? DB;

        #endregion

        #endregion

        #region Class Constructor

        public DbInitializer(ApplicationContext? db)
        {
            DB = db;
        }

        #endregion

        #region Public Methods

        public async Task InitializeAsync()
        {
            //await DB!.Database.EnsureDeletedAsync().ConfigureAwait(false);
            await DB!.Database.EnsureCreatedAsync().ConfigureAwait(false);
        }

        #endregion
    }
}
