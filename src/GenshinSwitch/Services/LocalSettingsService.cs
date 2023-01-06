using GenshinSwitch.Contracts.Services;
using GenshinSwitch.Core.Contracts.Services;
using GenshinSwitch.Core.Helpers;
using GenshinSwitch.Helpers;
using GenshinSwitch.Models;

using Microsoft.Extensions.Options;

using Windows.Storage;

namespace GenshinSwitch.Services;

public class LocalSettingsService : ILocalSettingsService
{
    private const string DefaultLocalSettingsFile = "config.json";

    private readonly IFileService fileService;
    private readonly ISpecialPathService specialPathService;
    private readonly LocalSettingsOptions options;

    private readonly string applicationDataFolder;
    private readonly string localsettingsFile;

    private IDictionary<string, object> settings;

    private bool isInitialized;

    public LocalSettingsService(IFileService fileService, ISpecialPathService specialPathService, IOptions<LocalSettingsOptions> options)
    {
        this.fileService = fileService;
        this.specialPathService = specialPathService;
        this.options = options.Value;

        applicationDataFolder = this.specialPathService.GetFolder(this.options.ApplicationDataFolder!);
        localsettingsFile = this.options.LocalSettingsFile ?? DefaultLocalSettingsFile;

        settings = new Dictionary<string, object>();
    }

    private async Task InitializeAsync()
    {
        if (!isInitialized)
        {
            settings = await Task.Run(() => fileService.Read<IDictionary<string, object>>(applicationDataFolder, localsettingsFile)) ?? new Dictionary<string, object>();

            isInitialized = true;
        }
    }

    public async Task<T?> ReadSettingAsync<T>(string key)
    {
        if (RuntimeHelper.IsMSIX)
        {
            if (ApplicationData.Current.LocalSettings.Values.TryGetValue(key, out object? obj))
            {
                return await Json.ToObjectAsync<T>((string)obj);
            }
        }
        else
        {
            await InitializeAsync();

            if (settings != null && settings.TryGetValue(key, out object? obj))
            {
                return await Json.ToObjectAsync<T>((string)obj);
            }
        }

        return default;
    }

    public async Task SaveSettingAsync<T>(string key, T value)
    {
        if (RuntimeHelper.IsMSIX)
        {
            ApplicationData.Current.LocalSettings.Values[key] = await Json.StringifyAsync(value!);
        }
        else
        {
            await InitializeAsync();

            settings[key] = await Json.StringifyAsync(value!);

            await Task.Run(() => fileService.Save(applicationDataFolder, localsettingsFile, settings));
        }
    }
}
