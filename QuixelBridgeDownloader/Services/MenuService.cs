using Console = Colorful.Console;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QuixelBridgeDownloader.Entities;
using QuixelBridgeDownloader.Interfaces.Services;
using QuixelBridgeDownloader.Models;
using Spectre.Console;

namespace QuixelBridgeDownloader.Services
{
    public class MenuService : IMenuService
    {
        #region Properties

        #region Private Properties

        #region DI Properties

        private readonly IArchiveService _archiveService;
        private readonly IItemService _itemService;
        private readonly ILogService _logService;
        private readonly IQuixelService _quixelService;
        private readonly ITelegramService _telegramService;

        #endregion

        #region Other Shit

        private readonly string _tempFolder = $"{Environment.CurrentDirectory}\\Temp";
        private readonly long partSize = 1024;                                                // Part Size In Mb

        #endregion

        #endregion

        #endregion

        #region Class Constructor

        public MenuService(IArchiveService archiveService, IItemService itemService, ILogService logService, IQuixelService quixelService, ITelegramService telegramService)
        {
            _archiveService = archiveService;
            _itemService = itemService;
            _logService = logService;
            _quixelService = quixelService;
            _telegramService = telegramService;
        }

        #endregion

        #region Public Methods

        public async Task CreateMenuAsync()
        {
            Console.Title = "Quixel Bridge Downloader";
            Console.ForegroundColor = System.Drawing.Color.DarkViolet;

            await _logService.AddLogAsync(new Log
                {
                    Type = LogType.Info,
                    Message = "Menu was created.",
                    CreationTime = DateTime.Now,
                },
                false
            );

            bool printAuthData = false;

            while (true)
            {
                Console.Clear();

                PrintMenuItem("1", "Auth");
                PrintMenuItem("2", "Get All Assets");
                PrintMenuItem("3", "Add All Assets To Database");
                PrintMenuItem("4", "Get Asset Info");
                PrintMenuItem("5", "Get Download Asset Info");
                PrintMenuItem("6", "Download All Assets");
                PrintMenuItem("7", "Change Settings");
                PrintMenuItem("0", "Exit");

                if (printAuthData)
                    PrintAuthInfo();

                ConsoleKeyInfo input = Console.ReadKey();

                switch (input.Key)
                {
                    case ConsoleKey.D1:
                        printAuthData = await _quixelService.AuthAsync();
                        break;
                    case ConsoleKey.D2:
                        await _quixelService.GetAllAssetsAsync();
                        break;
                    case ConsoleKey.D3:
                        await _quixelService.AddAllAssetsToDatabaseAsync();
                        break;
                    case ConsoleKey.D4:
                        await GetAssetInfoAsync();
                        break;
                    case ConsoleKey.D5:
                        await GetDownloadInfoOrDownloadAsync();
                        break;
                    case ConsoleKey.D6:
                        await DownloadAllAssetsAsync();
                        break;
                    case ConsoleKey.D7:
                        _quixelService.ChangeSettings();
                        break;
                    case ConsoleKey.D0:
                        Environment.Exit(0);
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion

        #region Private Methods

        private async Task DownloadAllAssetsAsync()
        {
            if (_quixelService.Settings is null)
                _quixelService.ChangeSettings();

            Console.Clear();

            if (_itemService.Items is null || await _itemService.Items.CountAsync() == 0)
            {
                await _quixelService.GetAllAssetsAsync();
                await _quixelService.AddAllAssetsToDatabaseAsync();
            }
            
            foreach (Item item in _itemService.Items!)
            {
                try
                {
                    if (item.IsSendedFirstMessage && item.IsSendedSecondMessage)
                        continue;

                    Directory.CreateDirectory(_tempFolder);

                    ClearTempFolder();

                    Asset? asset = await _quixelService.GetItemAsync(item.QuixelId!);
                    DownloadConfig downloadConfig = await _quixelService.CreateDownloadConfigAsync(asset!);
                    DownloadInfo? downloadInfo = await _quixelService.GetDownloadInfoAsync(downloadConfig);
                    string? path = await _quixelService.DownloadAssetAsync(downloadInfo!);

                    if (path.IsNullOrEmpty())
                        continue;

                    _archiveService.CreateSplitZip(path!, partSize);

                    string categoriesHashtags = asset!.Categories != null && asset.Categories.Any() ? "Categories: #" + string.Join(" #", asset.Categories) : string.Empty;
                    string tagsHashtags = asset!.Tags != null && asset.Tags.Any() ? "Tags: #" + string.Join(" #", asset.Tags) : string.Empty;

                    string message = $"Asset: {asset!.Name}{Environment.NewLine}" +
                                     $"Asset ID: https://quixel.com/assets/{asset.Id}{Environment.NewLine}" +
                                     $"{categoriesHashtags.Replace("-", "_").Replace(" ", "_").Replace("_#", " #")}{Environment.NewLine}" +
                                     $"{tagsHashtags.Replace("-", "_").Replace(" ", "_").Replace("_#", " #")}";

                    if (!item.IsSendedFirstMessage)
                    {
                        List<string> previewFiles = Directory.GetFiles($"{path!}\\Previews").ToList();
                        string previewImage = previewFiles[0];

                        foreach (string previewFile in previewFiles)
                        {
                            if (previewFile.Contains("HighPoly_Retina.png"))
                            {
                                previewImage = previewFile;
                                break;
                            }
                        }

                        await _telegramService.UploadFileAsync(previewImage, message);

                        item.IsSendedFirstMessage = true;
                        await _itemService.UpdateItemAsync(item);
                    }
                    
                    Directory.Delete(path!, true);
                    await _telegramService.UploadFilesAsync(_tempFolder);

                    ClearTempFolder();

                    item.IsSendedSecondMessage = true;
                    await _itemService.UpdateItemAsync(item);

                    await _logService.AddLogAsync(new Log
                        {
                            Type = LogType.Info,
                            Message = $"Asset was sended:{Environment.NewLine}{message}",
                            CreationTime = DateTime.Now,
                        },
                        System.Drawing.Color.DarkGreen,
                        true
                    );
                }
                catch (Exception exception) 
                {
                    await _logService.AddLogAsync(new Log()
                        {
                            Type = LogType.Error,
                            Message = $"Can't download asset {item.Id}: {exception.Message}",
                            CreationTime = DateTime.Now,
                        },
                        System.Drawing.Color.DarkRed,
                        true
                    );
                }
            }
        }

        /// <summary>
        /// Get Asset Information
        /// </summary>
        /// <returns>Nothing</returns>
        private async Task GetAssetInfoAsync()
        {
            Console.Clear();

            Console.Write("Type Asset ID: ");
            string assetId = Console.ReadLine();

            Asset? asset = await _quixelService.GetItemAsync(assetId);

            Console.WriteLine($"Name: {asset!.Name}");
            Console.WriteLine($"Tags: {string.Join(", ", asset!.Tags!)}");
            Console.WriteLine($"Categories: {string.Join(", ", asset!.Categories!)}");
            Console.WriteLine($"Model ID: {asset!.Id}");

            switch (asset)
            {
                case _3DAsset _3dAsset:
                    Console.WriteLine($"Model Preview Image: {_3dAsset.Previews!.Images![0].Uri}");
                    break;

                case _3DPlant _3dPlant:
                    Console.WriteLine($"Plant Preview Image: {_3dPlant.Previews!.Images![0].Uri}");
                    break;

                case Surface surface:
                    Console.WriteLine($"Surface Preview Image: {surface.Previews!.Images![0].Uri}");
                    break;

                case Atlas atlas:
                    Console.WriteLine($"Atlas Preview Image: {atlas.Previews!.Images![0].Uri}");
                    break;

                case Brush brush:
                    Console.WriteLine($"Brush Preview Image: {brush.Previews!.Images![0].Uri}");
                    break;

                default:
                    await _logService.AddLogAsync(new Log
                        {
                            Type = LogType.Error,
                            Message = $"Unknown Asset Type: {asset.Categories![0]}",
                            CreationTime = DateTime.Now,
                        },
                        System.Drawing.Color.DarkRed,
                        true
                    );
                    break;
            }

            Console.ReadKey();
        }

        /// <summary>
        /// Get Download Asset Information
        /// </summary>
        /// <returns>Nothing</returns>
        private async Task GetDownloadInfoOrDownloadAsync()
        {
            Console.Clear();

            Console.Write("Type Asset ID: ");
            string assetId = Console.ReadLine();

            if (_quixelService.Settings is null)
                _quixelService.ChangeSettings();

            Asset? asset = await _quixelService.GetItemAsync(assetId);
            DownloadConfig downloadConfig = await _quixelService.CreateDownloadConfigAsync(asset!);
            DownloadInfo? downloadInfo = await _quixelService.GetDownloadInfoAsync(downloadConfig);

            bool needDownload = AnsiConsole.Confirm($"Do you want to download Asset?");
            
            if (needDownload)
                Console.WriteLine($"Path to asset: {await _quixelService.DownloadAssetAsync(downloadInfo!)}");

            Console.WriteLine($"Download ID: {downloadInfo!.Id}");
            Console.ReadKey();
        }

        /// <summary>
        /// Print Menu Item
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="content">Text</param>
        private void PrintMenuItem(string id, string content)
        {
            Console.Write("[", System.Drawing.Color.LightSkyBlue);
            Console.Write(id, System.Drawing.Color.MediumSeaGreen);
            Console.Write("]", System.Drawing.Color.LightSkyBlue);
            Console.WriteLine(content, System.Drawing.Color.DarkViolet);
        }

        /// <summary>
        /// Print Auth Information
        /// </summary>
        private void PrintAuthInfo()
        {
            Console.WriteLine($"{Environment.NewLine}");
            Console.WriteLine($"Token: {_quixelService.AccountInfo!.Token!}");
            Console.WriteLine($"RefreshToken: {_quixelService.AccountInfo!.RefreshToken!}");
        }

        private void ClearTempFolder()
        {
            List<string> files = Directory.GetFiles(_tempFolder).ToList();

            foreach (string file in files)
            {
                File.Delete(file);
            }
        }

        #endregion
    }
}
