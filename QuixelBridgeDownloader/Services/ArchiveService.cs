using Microsoft.Extensions.Configuration;
using QuixelBridgeDownloader.Entities;
using QuixelBridgeDownloader.Interfaces.Services;
using QuixelBridgeDownloader.Models;
using System.Diagnostics;
using System.Drawing;

namespace QuixelBridgeDownloader.Services
{
    public class ArchiveService : IArchiveService
    {
        private readonly string _sevenZipPath;
        private readonly string _tempFolder = $"{Environment.CurrentDirectory}\\Temp";

        private readonly IConfiguration _configuration;
        private readonly ILogService _logService;

        public ArchiveService(IConfiguration configuration, ILogService logService)
        {
            _configuration = configuration;
            _logService = logService;

            _sevenZipPath = _configuration["7ZIP"]!;
        }

        public void CreateSplitZip(string sourceDirectory, long partSize)
        {
            string archiveName = $"{_tempFolder}\\{Path.GetFileName(sourceDirectory.TrimEnd(Path.DirectorySeparatorChar))}.zip";
            string arguments = $"a -v{partSize}m \"{archiveName}\" \"{sourceDirectory}\\*\"";

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = _sevenZipPath,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            try
            {
                using (Process process = Process.Start(startInfo)!)
                {
                    process.WaitForExit();

                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    if (process.ExitCode == 0)
                    {
                        _logService.AddLog(new Log
                            {
                                Type = LogType.Info,
                                Message = "Archiving completed successfully.",
                                CreationTime = DateTime.Now,
                            },
                            Color.DarkGreen,
                            false
                        );
                    }
                    else
                    {
                        _logService.AddLog(new Log
                            {
                                Type = LogType.Info,
                                Message = $"Can't Create Archive: {error}",
                                CreationTime = DateTime.Now,
                            },
                            Color.DarkRed,
                            false
                        );
                    }
                }
            }
            catch (Exception exception)
            {
                _logService.AddLog(new Log
                    {
                        Type = LogType.Info,
                        Message = $"There has been a mistake: {exception.Message}",
                        CreationTime = DateTime.Now,
                    },
                    Color.DarkRed,
                    false
                );
            }
        }
    }
}
