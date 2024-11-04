using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using QuixelBridgeDownloader.Entities;
using QuixelBridgeDownloader.Extensions;
using QuixelBridgeDownloader.Interfaces.Services;
using QuixelBridgeDownloader.Models;
using Spectre.Console;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace QuixelBridgeDownloader.Services
{
    public class QuixelService : IQuixelService
    {
        #region Properties

        #region Private Properties

        #region DI Properties

        private readonly HttpClient _httpClient;
        private readonly IConfiguration? _configuration;
        private readonly IItemService _itemService;
        private readonly ILogService _logService;

        #endregion

        #region Quixel Auth Data

        private readonly string _appId;
        private readonly string _appKey;
        private readonly string _login;
        private readonly string _password;

        #endregion

        #region Other Shit

        private readonly string _downloadPath = $"{Environment.CurrentDirectory}\\Temp";

        private readonly string _megascansAssets = "https://megascans.se/v1/assets";
        private readonly string _megascansAssetsDownload = "https://megascans.se/v1/downloads";

        // private readonly string _megascansAssetsDownloadPaid = "http://downloadp.megascans.se/download/";
        // private readonly string _megascansAssetsDownloadFree = "http://downloadf.megascans.se/download/";
        // private readonly string _megascansAssetsDownloadHttps = "?url=https%3A%2F%2Fmegascans.se%2Fv1%2Fdownloads";

        private List<AssetBase> Assets = new List<AssetBase>() { };

        #endregion

        #endregion

        #region Public Properties

        public AuthData? AccountInfo { get; private set; }
        public Settings? Settings { get; private set; }

        #endregion

        #endregion

        #region Class Constructor

        public QuixelService(HttpClient httpClient, IConfiguration configuration, IItemService itemService, ILogService logService)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _itemService = itemService;
            _logService = logService;

            _appId = _configuration["Quixel:app_id"]!;
            _appKey = _configuration["Quixel:app_key"]!;
            _login = _configuration["Quixel:login"]!;
            _password = _configuration["Quixel:password"]!;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Change Download Settings
        /// </summary>
        public void ChangeSettings()
        {
            Console.Clear();

            #region Select Texture Resolutions

            string[] textureResolutions = new string[]
            {
                TextureResolution._8k.GetDescription(),
                TextureResolution._4k.GetDescription(),
                TextureResolution._2k.GetDescription(),
                TextureResolution._1k.GetDescription()
            };

            List<string> selectedResolutions = AnsiConsole.Prompt(
                new MultiSelectionPrompt<string>()
                    .Title("Select the desired Texture Resolutions (use [yellow]<spacebar>[/] to select):")
                    .PageSize(10)
                    .AddChoices(textureResolutions)
                    .InstructionsText("[grey](Use the ↑ and ↓ keys to navigate and <spacebar> to select)[/]"));

            #endregion

            #region Select Texture Types

            Console.Clear();

            string[] textureTypes = new string[]
            {
                TextureType.Albedo.GetDescription(),
                TextureType.Metalness.GetDescription(),
                TextureType.Roughness.GetDescription(),
                TextureType.Specular.GetDescription(),
                TextureType.Diffuse.GetDescription(),
                TextureType.Gloss.GetDescription(),
                TextureType.Displacement.GetDescription(),
                TextureType.Normal.GetDescription(),
                TextureType.NormalBump.GetDescription(),
                TextureType.NormalObject.GetDescription(),
                TextureType.Bump.GetDescription(),
                TextureType.Curvature.GetDescription(),
                TextureType.Cavity.GetDescription(),
                TextureType.AO.GetDescription(),
                TextureType.Opacity.GetDescription(),
                TextureType.Brush.GetDescription(),
                TextureType.Fuzz.GetDescription(),
                TextureType.Mask.GetDescription(),
                TextureType.Thickness.GetDescription(),
                TextureType.Translucency.GetDescription(),
                TextureType.Transmission.GetDescription(),
            };

            List<string> selectedTextureTypes = AnsiConsole.Prompt(
                new MultiSelectionPrompt<string>()
                    .Title("Select the desired Texture Types (use [yellow]<spacebar>[/] to select):")
                    .PageSize(20)
                    .AddChoices(textureTypes)
                    .InstructionsText("[grey](Use the ↑ and ↓ keys to navigate and <spacebar> to select)[/]"));

            #endregion

            #region Select Texture MimeTypes

            Console.Clear();

            string[] textureMimeTypes = new string[]
            {
                TextureMimeType.JPEG.GetDescription(),
                TextureMimeType.EXR.GetDescription()
            };

            List<string> selectedTextureMimeTypes = AnsiConsole.Prompt(
                new MultiSelectionPrompt<string>()
                    .Title("Select the desired Texture MimeTypes (use [yellow]<spacebar>[/] to select):")
                    .PageSize(20)
                    .AddChoices(textureMimeTypes)
                    .InstructionsText("[grey](Use the ↑ and ↓ keys to navigate and <spacebar> to select)[/]"));

            #endregion

            #region Highpoly

            Console.Clear();

            bool highpoly = AnsiConsole.Confirm($"Do you want to save Highpoly Source?{Environment.NewLine}Whether or not to include the highpoly. (Optional and valid for 3D objects only)");

            #endregion

            #region LowerLodMeshes

            Console.Clear();

            bool lowerLodMeshes = AnsiConsole.Confirm($"Do you want to save Lower Lod Meshes?{Environment.NewLine}Whether or not include the meshes lower than that of maxlod. (Optional and valid for 3D objects only)");

            #endregion

            #region LowerLodNormals

            Console.Clear();

            bool lowerLodNormals = AnsiConsole.Confirm($"Do you want to save Lower Lod Normals?{Environment.NewLine}Whether or not include the normals corresponding to LODs lower than that of maxlod. (Optional and valid for 3D objects only)");

            #endregion

            #region AlbedoLods

            Console.Clear();

            bool albedoLods = AnsiConsole.Confirm($"Do you want to save Albedo Lods?{Environment.NewLine}Whether or not include the albedo corresponding to LODs lower than that of maxlod. (Optional and valid for 3D objects only)");

            #endregion

            #region ZTool

            Console.Clear();

            bool ZTool = AnsiConsole.Confirm($"Do you want to save ZTool?{Environment.NewLine}Whether or not include ZTool mesh file. (Optional and valid for 3D objects only)");

            #endregion

            #region Brushes

            Console.Clear();

            bool brushes = AnsiConsole.Confirm($"Do you want to save Brushes?{Environment.NewLine}Whether or not to include the brush files. (Optional and valid for 3D objects only)");

            #endregion

            #region Select Mesh MimeTypes

            Console.Clear();

            string[] meshMimeTypes = new string[]
            {
                ModelType.FBX.GetDescription(),
                ModelType.ABC.GetDescription()
            };

            string meshMimeType = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select the Mesh MimeType:")
                    .PageSize(10)
                    .AddChoices(meshMimeTypes));

            #endregion

            #region MaxLod

            Console.Clear();

            string[] lodLevels = new string[]
            {
                LodLevel.LOD0.ToString(),
                LodLevel.LOD1.ToString(),
                LodLevel.LOD2.ToString(),
                LodLevel.LOD3.ToString(),
                LodLevel.LOD4.ToString(),
                LodLevel.LOD5.ToString(),
                LodLevel.LOD6.ToString(),
                LodLevel.LOD7.ToString(),
                LodLevel.LOD8.ToString()
            };

            string maxLod = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select the minimum level of detail:")
                    .PageSize(10)
                    .AddChoices(lodLevels));

            #endregion

            Settings = new Settings()
            {
                TextureResolutions = selectedResolutions,
                TextureTypes = selectedTextureTypes,
                TextureMimeTypes = selectedTextureMimeTypes,
                //ModelTypes = selectedModelTypes,
                Highpoly = highpoly,
                LowerLodMeshes = lowerLodMeshes,
                LowerLodNormals = lowerLodNormals,
                AlbedoLods = albedoLods,
                ZTool = ZTool,
                Brushes = brushes,
                MaxLod = maxLod,
                MeshMimeType = meshMimeType,
            };
        }

        /// <summary>
        /// Auth to Quixel account 
        /// </summary>
        /// <returns>Bool Operation Result</returns>
        public async Task<bool> AuthAsync()
        {
            string requestUri = $"https://accounts.quixel.se/api/v1/applications/{_appId}/tokens";

            StringContent content = new StringContent("{\"secret\": \"" + _appKey + "\"}", Encoding.UTF8, "application/json");

            byte[] byteArray = Encoding.ASCII.GetBytes($"{_login}:{_password}");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            HttpResponseMessage response = await _httpClient.PostAsync(requestUri, content);

            if (!response.IsSuccessStatusCode)
                return response.IsSuccessStatusCode;
                
            string result = await response.Content.ReadAsStringAsync();
            AccountInfo = JsonSerializer.Deserialize<AuthData>(result)!;

            await _logService.AddLogAsync(new Log
                {
                    Type = LogType.Info,
                    Message = $"Auth data: {result}",
                    CreationTime = DateTime.Now,
                },
                false
            );

            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Get all Assets from Quixel
        /// </summary>
        /// <returns>Bool Operation Result</returns>
        public async Task<bool> GetAllAssetsAsync()
        {
            int currentPage = 1;
            int pages = 0;

            await SetAuthToken();

            do
            {
                string url = $"{_megascansAssets}?page={currentPage}";

                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    await _logService.AddLogAsync(new Log
                        {
                            Type = LogType.Error,
                            Message = $"An error occurred while parsing all the assets. Current page: {currentPage}",
                            CreationTime = DateTime.Now,
                        },
                        System.Drawing.Color.DarkRed,
                        true
                    );
                    return false;
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();
                AssetsResponse assetsResponse = JsonSerializer.Deserialize<AssetsResponse>(jsonResponse)!;

                pages = assetsResponse.Pages;
                Assets?.AddRange(assetsResponse.Assets!);

                await _logService.AddLogAsync(new Log
                    {
                        Type = LogType.Info,
                        Message = $"Assets: {Assets?.Count} Page: {currentPage} Pages: {assetsResponse.Pages} Total: {assetsResponse.Total}",
                        CreationTime = DateTime.Now,
                    },
                    System.Drawing.Color.Aqua,
                    true
                );

                currentPage++;
            } while (currentPage <= pages);

            await _logService.AddLogAsync(new Log
                {
                    Type = LogType.Info,
                    Message = $"Total assets: {Assets?.Count}",
                    CreationTime = DateTime.Now,
                },
                System.Drawing.Color.DarkGreen,
                true
            );

            return true;
        }

        /// <summary>
        /// Add all Assets to Database
        /// </summary>
        /// <returns>Bool Operation Result</returns>
        public async Task<bool> AddAllAssetsToDatabaseAsync()
        {
            if (Assets is null)
                return false;

            foreach (AssetBase asset in Assets)
            {
                await _itemService.AddItemAsync(
                    new Item
                    {
                        QuixelId = asset.Id,
                        Name = asset.Name,
                        TagsArray = asset.Tags!.ToArray(),
                        PreviewImage = asset.Previews!.Images![0].Uri,
                        CategoriesArray = asset.Categories!.ToArray(),
                        IsSendedFirstMessage = false,
                        IsSendedSecondMessage = false,
                    }
                );
            }

            return true;
        }

        /// <summary>
        /// Purchase an asset
        /// </summary>
        /// <param name="id">Asset Id</param>
        /// <returns>Bool Operation Result</returns>
        public Task<bool> AddItemToLibraryAsync(string id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get more Asset Information
        /// </summary>
        /// <param name="assetId">Asset Id</param>
        /// <returns>Asset</returns>
        public async Task<Asset?> GetItemAsync(string assetId)
        {
            await SetAuthToken();

            string url = $"{_megascansAssets}/{assetId}";

            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                await _logService.AddLogAsync(new Log
                    {
                        Type = LogType.Error,
                        Message = $"Unable to retrieve information about the asset: {assetId}",
                        CreationTime = DateTime.Now,
                    },
                    System.Drawing.Color.DarkRed,
                    true
                );
            }

            string jsonResponse = await response.Content.ReadAsStringAsync();

            return DeserializeAsset(jsonResponse);
        }

        /// <summary>
        /// Create Download Config
        /// </summary>
        /// <param name="asset">Asset</param>
        /// <returns>Download Config</returns>
        public async Task<DownloadConfig> CreateDownloadConfigAsync(Asset asset)
        {
            DownloadConfig downloadConfig = new DownloadConfig()
            {
                AssetId = asset.Id,
                Config = new Config()
                {
                    Highpoly = Settings!.Highpoly,
                    LowerLodMeshes = Settings!.LowerLodMeshes,
                    LowerLodNormals = Settings!.LowerLodNormals,
                    AlbedoLods = Settings!.AlbedoLods,
                    ZTool = Settings!.ZTool,
                    Brushes = Settings!.Brushes,
                    MeshMimeType = Settings!.MeshMimeType
                }
            };

            switch (asset)
            {
                case _3DAsset _3dAsset:

                    #region Change Lod Level

                    if (_3dAsset.Meshes is not null)
                        downloadConfig.Config.Lods = GetLodsArray(_3dAsset.Meshes);
                    else
                    {
                        downloadConfig.Config = null;
                        downloadConfig.Meshes = GetDownloadMeshList(_3dAsset.Models!);
                    }

                    #endregion

                    #region Change Textures

                    List<DownloadComponent> components = new List<DownloadComponent>();

                    if (_3dAsset.Components is not null)
                        components = GetDownloadComponents(_3dAsset.Components);
                    else
                        components = GetDownloadMaps(_3dAsset.Maps);

                    downloadConfig.Components = components;

                    #endregion

                    break;
                case _3DPlant _3dPlant:

                    #region Change Plant Lod Level

                    if (_3dPlant.Meshes is not null)
                        downloadConfig.Config.Lods = GetLodsArray(_3dPlant.Meshes);
                    else
                    {
                        downloadConfig.Config = null;
                        downloadConfig.Meshes = GetDownloadMeshList(_3dPlant.Models!);
                    }

                    #endregion

                    #region Change Textures

                    List<DownloadComponent> plantComponents = new List<DownloadComponent>();

                    if (_3dPlant.Components is not null)
                        plantComponents = GetDownloadComponents(_3dPlant.Components);
                    else
                        plantComponents = GetDownloadMaps(_3dPlant.Maps);

                    downloadConfig.Components = plantComponents;
                    //downloadConfig.Billboards = plantComponents;

                    #endregion

                    break;
                case Surface surface:

                    #region Change Textures

                    List<DownloadComponent> surfaceComponents = new List<DownloadComponent>();

                    if (surface.Components is not null)
                        surfaceComponents = GetDownloadComponents(surface.Components);
                    else
                        surfaceComponents = GetDownloadMaps(surface.Maps);

                    downloadConfig.Components = surfaceComponents;

                    #endregion

                    break;
                case Atlas atlas:

                    #region Change Textures

                    List<DownloadComponent> atlasComponents = new List<DownloadComponent>();

                    if (atlas.Components is not null)
                        atlasComponents = GetDownloadComponents(atlas.Components);
                    else
                        atlasComponents = GetDownloadMaps(atlas.Maps);

                    downloadConfig.Components = atlasComponents;

                    #endregion

                    break;
                case Brush brush:

                    #region Change Textures

                    List<DownloadComponent> brushComponents = new List<DownloadComponent>();

                    if (brush.Components is not null)
                        brushComponents = GetDownloadComponents(brush.Components);
                    else
                        brushComponents = GetDownloadMaps(brush.Maps);

                    downloadConfig.Components = brushComponents;

                    #endregion

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

            return downloadConfig;
        }

        /// <summary>
        /// Create Download Information
        /// </summary>
        /// <param name="downloadConfig">Download Config</param>
        /// <returns>DownloadInfo</returns>
        public async Task<DownloadInfo?> GetDownloadInfoAsync(DownloadConfig downloadConfig)
        {
            await SetAuthToken();

            JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                WriteIndented = true
            };
            string json = JsonSerializer.Serialize(downloadConfig, jsonSerializerOptions);

            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(_megascansAssetsDownload, content);

            if (!response.IsSuccessStatusCode)
            {
                await _logService.AddLogAsync(new Log
                    {
                        Type = LogType.Error,
                        Message = $"Unable to complete a request to download an asset:  {downloadConfig.AssetId}",
                        CreationTime = DateTime.Now,
                    },
                    System.Drawing.Color.DarkRed,
                    true
                );

                return null;
            }

            string jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<DownloadInfo>(jsonResponse)!;
        }

        /// <summary>
        /// Download Asset
        /// </summary>
        /// <param name="downloadInfo">Download Information</param>
        /// <returns>Download Path To Folder</returns>
        public async Task<string?> DownloadAssetAsync(DownloadInfo downloadInfo)
        {
            // Trash official download method
            //string url = $"{(downloadInfo.Paying is true ? _megascansAssetsDownloadPaid : _megascansAssetsDownloadFree)}{downloadInfo.Id}{_megascansAssetsDownloadHttps}";
            //await Download(url);

            if (downloadInfo is null)
                return null;
            
            string path = $"{_downloadPath}\\{downloadInfo.AssetId}";

            int retryTimes = 5;

            if (downloadInfo.Components is not null && downloadInfo.Components.Count > 0) 
            {
                foreach (Map map in downloadInfo.Components)
                {
                    for (int i = 0; i <= retryTimes; i++) 
                    {
                        if (map.Uri is null) break;

                        Uri uri = new Uri(map.Uri);
                        string resolution = Path.GetFileName(uri.LocalPath).ToLower() switch
                        {
                            string name when name.Contains("8k") => "8K",
                            string name when name.Contains("4k") => "4K",
                            string name when name.Contains("2k") => "2K",
                            string name when name.Contains("1k") => "1K",
                            _ => "Unknown resolution"
                        };

                        string texturesPath = $"{path}\\Textures\\{resolution}";
                        Directory.CreateDirectory(texturesPath);

                        bool result = await Download(map.Uri, texturesPath);
                        if (result)
                            break;
                    }
                    
                }
            }

            if (downloadInfo.Meshes is not null && downloadInfo.Meshes.Count > 0)
            {
                int currentLod = 1;

                foreach (Model model in downloadInfo.Meshes)
                {
                    for (int i = 0; i <= retryTimes; i++)
                    {
                        if (model.Uri is null) 
                            break;

                        if (model.Uri.ToLower().Contains("lod"))
                        {
                            Match match = Regex.Match(model.Uri!, @"LOD(\d+)");

                            if (match.Success)
                            {
                                int lodValue = int.Parse(match.Groups[1].Value);

                                if (downloadInfo.Config is not null && lodValue >= downloadInfo.Config.Lods!.Length)
                                    break;
                            }
                        }

                        string modelsPath = $"{path}\\Models";
                        Directory.CreateDirectory(modelsPath);

                        bool result = await Download(model.Uri!, modelsPath);
                        if (result)
                        {
                            currentLod++;
                            break;
                        }
                    }
                }
            }

            if (downloadInfo.Previews is not null && downloadInfo.Previews.Count > 0)
            {
                foreach (Image previewImage in downloadInfo.Previews) 
                {
                    if (previewImage.Uri.IsNullOrEmpty())
                        break;

                    string previewPath = $"{path}\\Previews";
                    Directory.CreateDirectory(previewPath);

                    bool result = await Download(previewImage.Uri!, previewPath);
                    if (!result)
                        await _logService.AddLogAsync(new Log
                        {
                            Type = LogType.Error,
                            Message = $"Can't download Preview Image: {previewImage.Uri}",
                            CreationTime = DateTime.Now,
                        },
                        System.Drawing.Color.DarkRed,
                        true
                    );
                }
            }

            return path;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Set Auth Token if exist or auth to Quixel Account
        /// </summary>
        /// <returns>Nothing</returns>
        private async Task SetAuthToken()
        {
            if (AccountInfo is null || AccountInfo.Token is null)
                await AuthAsync();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccountInfo!.Token);
        }

        /// <summary>
        /// Deserialize Asset
        /// </summary>
        /// <param name="json">JSON string</param>
        /// <returns>Asset</returns>
        private static Asset DeserializeAsset(string json)
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.Converters.Add(new AssetConverter());
            return JsonSerializer.Deserialize<Asset>(json, options)!;
        }

        /// <summary>
        /// Get Lods Array
        /// </summary>
        /// <param name="meshes">List Meshes</param>
        /// <returns>Int Array with Assets Lods</returns>
        private int[] GetLodsArray(List<Mesh> meshes)
        {
            List<Lod> lods = new List<Lod>();
            foreach (Mesh mesh in meshes)
            {
                if (mesh.Type is not null && mesh.Type.ToLower().Contains("lod"))
                {
                    foreach (MeshUri meshUri in mesh.MeshUris!)
                    {
                        Match match = Regex.Match(meshUri.Uri!, @"LOD(\d+)");

                        if (match.Success)
                        {
                            int lodValue = int.Parse(match.Groups[1].Value);

                            lods.Add(new Lod()
                            {
                                LodLevel = lodValue,
                                Tris = mesh.Tris is not null ? mesh.Tris.Value : 0
                            });
                        }
                        break; //Skip other format models
                    }
                }
            }

            int[]? lodsArray = new int[lods.Count < Settings!.MaxLodToInt() + 1 ? lods.Count : Settings.MaxLodToInt() + 1];


            for (int i = 0; i < lods.Count; i++)
            {
                if (i <= Settings.MaxLodToInt())
                    lodsArray[i] = lods[i].Tris is not null ? (int)lods[i].Tris! : 0;
            }

            return lodsArray;
        }

        /// <summary>
        /// Get Download Mesh List
        /// </summary>
        /// <param name="models">Models List</param>
        /// <returns>Download Mesh List</returns>
        private List<DownloadMesh> GetDownloadMeshList(List<Model> models)
        {
            List<Lod> lods = new List<Lod>();
            foreach (Model model in models)
            {
                if (model.Type is not null && model.Type.ToLower().Contains("lod"))
                {
                    lods.Add(new Lod()
                    {
                        LodLevel = model.Lod,
                        Tris = model.Tris is not null ? model.Tris.Value : 0
                    });
                }
            }

            int?[] lodsArray = new int?[lods.Count];
            for (int i = 0; i < lods.Count; i++)
            {
                if (i <= Settings!.MaxLodToInt())
                    lodsArray[i] = lods[i].LodLevel;
            }

            // Check if _settings.MaxLodToInt() less than plantLodsArray max value and change downloadConfig.Meshes

            List<DownloadMesh> downloadMeshes = new List<DownloadMesh>();
            if (lodsArray.Max() <= Settings!.MaxLodToInt())
            {
                for (int i = 0; i <= lodsArray.Length; i++)
                {
                    downloadMeshes.Add(new DownloadMesh()
                    {
                        Lod = i,
                        MimeType = Settings.MeshMimeType,
                    });
                }
            }

            return downloadMeshes;
        }

        /// <summary>
        /// Get Download Components
        /// </summary>
        /// <param name="assetComponents">Components List</param>
        /// <returns>Download Component List</returns>
        private List<DownloadComponent> GetDownloadComponents(List<Component>? assetComponents)
        {
            List<DownloadComponent> components = new List<DownloadComponent>();

            //assetComponents[0].Uris[0].Resolutions[0].Formats[0]

            foreach (Component component in assetComponents!)
            {
                foreach (ComponentUri componentUri in component.Uris!)
                {
                    foreach (ComponentResolution componentResolution in componentUri.Resolutions!)
                    {
                        foreach (ComponentFormat componentFormat in componentResolution.Formats!)
                        {
                            if (!Settings!.TextureMimeTypes!.Any(tms => tms.ToLower().Contains(componentFormat.MimeType!.ToLower())))
                                continue;

                            /*if (!_settings!.TextureResolutions!.Any(tr => tr.ToLower().Contains(componentResolution.Resolution!.ToLower())))
                            {
                                if (!_settings!.TextureResolutions!.Any(tr => tr.ToLower().Contains(componentResolution.Resolution!.Substring(componentResolution.Resolution.Length - 4, 4).ToLower())))
                                    continue;
                            }*/

                            if (componentResolution.Resolution!.Contains("16384"))
                                continue;

                            if (!Settings!.TextureResolutions!.Any(tr => tr.ToLower().Contains(componentResolution.Resolution!.Substring(componentResolution.Resolution.Length - 4, 4)!.ToLower())))
                            {
                                if (!Settings!.TextureResolutions!.Any(tr => tr.ToLower().Contains(componentResolution.Resolution!.Substring(0, 4).ToLower())))
                                    continue;
                            }

                            if (!Settings!.TextureTypes!.Any(tt => tt.ToLower().Contains(component.Type!.ToLower())))
                                continue;

                            components.Add(new DownloadComponent()
                            {
                                Type = component.Type!.ToLower(),
                                Resolution = componentResolution.Resolution,
                                MimeType = componentFormat.MimeType,
                            });
                        }
                    }
                }
            }

            return components;
        }

        /// <summary>
        /// Get Download Maps
        /// </summary>
        /// <param name="assetMaps">Maps List</param>
        /// <returns>Download Component List</returns>
        private List<DownloadComponent> GetDownloadMaps(List<Map>? assetMaps)
        {
            List<DownloadComponent> components = new List<DownloadComponent>();

            foreach (Map map in assetMaps!)
            {
                if (!Settings!.TextureMimeTypes!.Any(tms => tms.ToLower().Contains(map.MimeType!.ToLower())))
                    continue;

                if (map.Resolution!.Contains("16384"))
                    continue;

                if (!Settings!.TextureResolutions!.Any(tr => tr.ToLower().Contains(map.Resolution.Substring(map.Resolution.Length - 4, 4)!.ToLower())))
                {
                    if (!Settings!.TextureResolutions!.Any(tr => tr.ToLower().Contains(map.Resolution!.Substring(0, 4).ToLower())))
                        continue;
                }

                if (!Settings!.TextureTypes!.Any(tt => tt.ToLower().Contains(map.Type!.ToLower())))
                    continue;

                components.Add(new DownloadComponent()
                {
                    Type = map.Type!.ToLower(),
                    Resolution = map.Resolution,
                    MimeType = map.MimeType,
                });
            }

            return components;
        }

        /// <summary>
        /// Download File
        /// </summary>
        /// <param name="url">URL</param>
        /// <param name="path">Download path</param>
        /// <returns>Bool Operation Result</returns>
        private async Task<bool> Download(string url, string path)
        {
            try
            {
                Uri uri = new Uri(url);

                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                await using Stream contentStream = await response.Content.ReadAsStreamAsync(),
                fileStream = new FileStream($"{path}\\{Path.GetFileName(uri.LocalPath)}", FileMode.Create, FileAccess.Write, FileShare.None);

                await contentStream.CopyToAsync(fileStream);

                await _logService.AddLogAsync(new Log
                    {
                        Type = LogType.Info,
                        Message = $"File downloaded successfully: {url}",
                        CreationTime = DateTime.Now,
                    },
                    System.Drawing.Color.Azure,
                    true
                );

                return true;
            }
            catch (Exception exception)
            {
                await _logService.AddLogAsync(new Log
                    {
                        Type = LogType.Error,
                        Message = $"Error downloading file: {exception.Message}",
                        CreationTime = DateTime.Now,
                    },
                    System.Drawing.Color.DarkRed,
                    true
                );

                return false;
            }
        }

        #endregion
    }
}
