using GenshinSwitch.Contracts.Services;
using GenshinSwitch.Core.Settings;
using GenshinSwitch.Helpers;
using GenshinSwitch.Models;
using Microsoft.UI.Xaml;

namespace GenshinSwitch.Services;

public class ThemeSelectorService : IThemeSelectorService
{
#if LEGACY
    private const string SettingsKey = "AppBackgroundRequestedTheme";
#endif

    public ElementTheme Theme { get; set; } = ElementTheme.Default;

#if LEGACY
    private readonly ILocalSettingsService localSettingsService;

    public ThemeSelectorService(ILocalSettingsService localSettingsService)
    {
        this.localSettingsService = localSettingsService;
    }
#endif

    public ThemeSelectorService()
    {
    }

    public async Task InitializeAsync()
    {
        Theme = await LoadThemeFromSettingsAsync();
        await Task.CompletedTask;
    }

    public async Task SetThemeAsync(ElementTheme theme)
    {
        Theme = theme;

        await SetRequestedThemeAsync();
        await SaveThemeInSettingsAsync(Theme);
    }

    public async Task SetRequestedThemeAsync()
    {
        if (App.MainWindow.Content is FrameworkElement rootElement)
        {
            rootElement.RequestedTheme = Theme;

            TitleBarHelper.UpdateTitleBar(Theme);
        }

        await Task.CompletedTask;
    }

    private async Task<ElementTheme> LoadThemeFromSettingsAsync()
    {
#if LEGACY
        string? themeName = await localSettingsService.ReadSettingAsync<string>(SettingsKey);

        if (Enum.TryParse(themeName, out ElementTheme cacheTheme))
        {
            return cacheTheme;
        }
        return ElementTheme.Default;
#endif
        await Task.CompletedTask;
        return Settings.Theme.Get();
    }

    private async Task SaveThemeInSettingsAsync(ElementTheme theme)
    {
#if LEGACY
        await localSettingsService.SaveSettingAsync(SettingsKey, theme.ToString());
#endif
        await Task.CompletedTask;
        Settings.Theme.Set(theme);
        SettingsManager.Save();
    }
}
