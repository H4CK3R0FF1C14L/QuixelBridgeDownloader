using Microsoft.Extensions.Configuration;
using MimeMapping;
using QuixelBridgeDownloader.Interfaces.Services;
using TL;
using WTelegram;

namespace QuixelBridgeDownloader.Services
{
    public class TelegramService : ITelegramService
    {
        #region Properties

        #region Private Properties

        #region DI Properties
        
        private readonly IConfiguration? _configuration;
        private readonly HttpClient _httpClient;

        #endregion

        #region Other Properties

        private readonly string? _apiId;
        private readonly string? _apiHash;
        private readonly string? _phoneNumber;
        private readonly long _chatId;

        #endregion

        #endregion

        #endregion

        #region Class Constructor

        public TelegramService(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;

            _apiId = _configuration["Telegram:api_id"];
            _apiHash = _configuration["Telegram:api_hash"];
            _phoneNumber = _configuration["Telegram:phone_number"];
            _chatId = Convert.ToInt64(_configuration["Telegram:chat_id"]);
        }

        #endregion

        #region Public Methods

        public async Task<int> UploadFileAsync(string filePath, string caption = "", int replyToMessageId = 0, DateTime scheduleDate = default, MessageEntity[]? entities = null)
        {
            Helpers.Log = (lvl, str) => { };
            using Client client = new Client(Config);
            User user = await client.LoginUserIfNeeded();

            Messages_Chats chats = await client.Messages_GetAllChats();
            InputPeer peer = chats.chats[_chatId];
            InputFileBase inputFile = await client.UploadFileAsync(filePath);

            Message telegramMessage = await client.SendMediaAsync(peer, caption, inputFile, reply_to_msg_id: replyToMessageId, entities: entities, schedule_date: scheduleDate);
            return telegramMessage.ID;
        }

        public async Task<Message[]> UploadFilesAsync(string folderPath, string caption = "", int replyToMessageId = 0, DateTime scheduleDate = default, MessageEntity[]? entities = null)
        {
            Helpers.Log = (lvl, str) => { };
            using Client client = new Client(Config);
            User user = await client.LoginUserIfNeeded();

            Messages_Chats chats = await client.Messages_GetAllChats();
            InputPeer peer = chats.chats[_chatId];

            List<string> files = Directory.GetFiles(folderPath).ToList();
            List<InputMedia> mediaList = new List<InputMedia>();

            foreach (string file in files)
            {
                InputFileBase uploadedFile = await client.UploadFileAsync(file);

                string mimeType = MimeUtility.GetMimeMapping(file);

                InputMediaUploadedDocument inputMediaUploadedDocument = new InputMediaUploadedDocument
                {
                    file = uploadedFile,
                    mime_type = mimeType,
                    attributes = new[] { new DocumentAttributeFilename { file_name = Path.GetFileName(file) } }
                };

                mediaList.Add(inputMediaUploadedDocument);
            }
            return await client.SendAlbumAsync(peer, mediaList, caption, replyToMessageId, entities, scheduleDate);
        }

        #endregion

        #region Private Methods

        private string? Config(string what)
        {
            switch (what)
            {
                case "session_pathname": return Environment.CurrentDirectory.Replace("\\", "/") + "/WTelegram.session";
                case "api_id": return _apiId!;
                case "api_hash": return _apiHash!;
                case "phone_number": return _phoneNumber!;
                case "verification_code": Console.Write("Enter Telegram Code: "); return Console.ReadLine()!;
                default: return null;
            }
        }

        #endregion
    }
}
