using QuixelBridgeDownloader.Models;

namespace QuixelBridgeDownloader.Interfaces.Services
{
    public interface IQuixelService
    {
        public AuthData? AccountInfo { get; }
        public Settings? Settings { get; }

        public void ChangeSettings();
        public Task<bool> AuthAsync();
        public Task<bool> GetAllAssetsAsync();
        public Task<bool> AddAllAssetsToDatabaseAsync();
        public Task<bool> AddItemToLibraryAsync(string id);
        public Task<Asset?> GetItemAsync(string assetId);
        public Task<DownloadConfig> CreateDownloadConfigAsync(Asset asset);
        public Task<DownloadInfo?> GetDownloadInfoAsync(DownloadConfig downloadConfig);
        public Task<string?> DownloadAssetAsync(DownloadInfo downloadInfo);
    }
}
