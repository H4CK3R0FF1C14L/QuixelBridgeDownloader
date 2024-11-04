namespace QuixelBridgeDownloader.Interfaces.Services
{
    public interface IArchiveService
    {
        public void CreateSplitZip(string sourceDirectory, long partSize);
    }
}
