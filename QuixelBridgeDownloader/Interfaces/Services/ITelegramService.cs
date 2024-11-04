using TL;

namespace QuixelBridgeDownloader.Interfaces.Services
{
    public interface ITelegramService
    {
        public Task<int> UploadFileAsync(string filePath, string caption = "", int replyToMsgId = 0, DateTime schedule_date = default, MessageEntity[]? entities = null);
        public Task<Message[]> UploadFilesAsync(string folderPath, string caption = "", int replyToMsgId = 0, DateTime schedule_date = default, MessageEntity[]? entities = null);
    }
}
