using Console = Colorful.Console;
using QuixelBridgeDownloader.Entities;
using QuixelBridgeDownloader.Interfaces.Base;
using System.Drawing;
using QuixelBridgeDownloader.Interfaces.Services;

namespace QuixelBridgeDownloader.Services.Database
{
    public class LogService : ILogService
    {
        #region Properies

        #region Private Properties
        
        #region DI Properties

        private readonly IRepository<Log> _logRepository;

        #endregion

        #endregion

        #region Public Properties

        public IQueryable<Log>? Logs => _logRepository.Items;

        #endregion

        #endregion

        #region Class Constructor

        public LogService(IRepository<Log> logRepository)
        {
            _logRepository = logRepository;
        }

        #endregion

        #region Public Methods

        #region Add

        public Log AddLog(Log log, bool printLog = true)
        {
            if (printLog)
                Console.WriteLine($"[{log.CreationTime}][{log.Type}]{Environment.NewLine}[{log.Message}]{Environment.NewLine}");

            return _logRepository.Add(log);
        }

        public Log AddLog(Log log, Color color, bool printLog = true)
        {
            if (printLog)
                Console.WriteLine($"[{log.CreationTime}][{log.Type}]{Environment.NewLine}[{log.Message}]{Environment.NewLine}", color);

            return _logRepository.Add(log);
        }

        public async Task<Log> AddLogAsync(Log log, bool printLog = true, CancellationToken cancellationToken = default)
        {
            if (printLog)
                Console.WriteLine($"[{log.CreationTime}][{log.Type}]{Environment.NewLine}[{log.Message}]{Environment.NewLine}");

            return await _logRepository.AddAsync(log, cancellationToken);
        }

        public async Task<Log> AddLogAsync(Log log, Color color, bool printLog = true, CancellationToken cancellationToken = default)
        {
            if (printLog)
                Console.WriteLine($"[{log.CreationTime}][{log.Type}]{Environment.NewLine}[{log.Message}]{Environment.NewLine}", color);

            return await _logRepository.AddAsync(log, cancellationToken);
        }

        #endregion

        #endregion
    }
}
