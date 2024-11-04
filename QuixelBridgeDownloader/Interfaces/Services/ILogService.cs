using System.Drawing;
using QuixelBridgeDownloader.Entities;

namespace QuixelBridgeDownloader.Interfaces.Services
{
    public interface ILogService
    {
        public IQueryable<Log>? Logs { get; }

        public Log AddLog(Log log, bool printLog = true);
        public Log AddLog(Log log, Color color, bool printLog = true);
        public Task<Log> AddLogAsync(Log log, bool printLog = true, CancellationToken cancellationToken = default);
        public Task<Log> AddLogAsync(Log log, Color color, bool printLog = true, CancellationToken cancellationToken = default);
    }
}
