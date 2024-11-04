# QuixelBridgeDownloader
A piece of shit asset download software from the Quixel Megascans website.
To use it, you need to populate appsettings.json with data from the [Quixel Dev API](https://quixel.com/account) and [Telegram API](https://my.telegram.org/)
Also the project uses 7-Zip to create Split Archives and you need to specify the path to 7z.exe in appsettings.json

# Build

To build the project you need the [.NET SDK](https://dotnet.microsoft.com/en-us/download) and execute the following commands.

```bash
dotnet restore
dotnet build
```
