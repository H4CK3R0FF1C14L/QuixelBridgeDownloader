using System.Drawing;
using QuixelBridgeDownloader.Entities;

namespace QuixelBridgeDownloader.Interfaces.Services
{
    public interface ILogService
    {
        IQueryable<Log>? Logs { get; }

        Log AddLog(Log log, bool printLog = true);
        Log AddLog(Log log, Color color, bool printLog = true);
        Task<Log> AddLogAsync(Log log, bool printLog = true, CancellationToken cancellationToken = default);
        Task<Log> AddLogAsync(Log log, Color color, bool printLog = true, CancellationToken cancellationToken = default);
    }
}
