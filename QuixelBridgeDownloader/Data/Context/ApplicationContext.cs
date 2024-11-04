using Microsoft.EntityFrameworkCore;
using QuixelBridgeDownloader.Entities;

namespace QuixelBridgeDownloader.Data.Context
{
    public class ApplicationContext : DbContext
    {
        #region Properties
        
        #region Public Properties

        public DbSet<Item>? Items { get; set; }
        public DbSet<Log>? Logs { get; set; }

        #endregion

        #endregion

        #region Class Constructor
        
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }

        #endregion
    }
}
