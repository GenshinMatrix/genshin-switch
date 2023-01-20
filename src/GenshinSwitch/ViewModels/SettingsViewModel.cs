using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GenshinSwitch.Contracts.Services;
using GenshinSwitch.Core;
using GenshinSwitch.Core.Services;
using GenshinSwitch.Helpers;
using Microsoft.UI.Xaml;
using System.Reflection;
using System.Windows.Input;
using Windows.ApplicationModel;
using Windows.System;

namespace GenshinSwitch.ViewModels;

public partial class SettingsViewModel : ObservableRecipient
{
    private readonly IThemeSelectorService themeSelectorService;

    public string AppName => Pack.AppName;
    public string AppVersion => Pack.AppVersion;
    public string UserDataPath => SpecialPathService.Provider.GetPath(string.Empty);

    private ElementTheme elementTheme;
    public ElementTheme ElementTheme
    {
        get => elementTheme;
        set => SetProperty(ref elementTheme, value);
    }

    private string versionDescription;
    public string VersionDescription
    {
        get => versionDescription;
        set => SetProperty(ref versionDescription, value);
    }

    public ICommand SwitchThemeCommand { get; }

    public SettingsViewModel(IThemeSelectorService themeSelectorService)
    {
        this.themeSelectorService = themeSelectorService;
        elementTheme = this.themeSelectorService.Theme;
        versionDescription = GetVersionDescription();

        SwitchThemeCommand = new RelayCommand<ElementTheme>(
            async (param) =>
            {
                if (ElementTheme != param)
                {
                    ElementTheme = param;
                    await this.themeSelectorService.SetThemeAsync(param);
                }
            });
    }

    private static string GetVersionDescription()
    {
        Version version;

        if (RuntimeHelper.IsMSIX)
        {
            PackageVersion packageVersion = Package.Current.Id.Version;

            version = new(packageVersion.Major, packageVersion.Minor, packageVersion.Build, packageVersion.Revision);
        }
        else
        {
            version = Assembly.GetExecutingAssembly().GetName().Version!;
        }

        return $"{"AppDisplayName".GetLocalized()} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
    }

    [RelayCommand]
    private async Task CheckUpdateAsync()
    {
        try
        {
            Uri uri = new("https://github.com/genshin-matrix/genshin-switch/releases");
            await Launcher.LaunchUriAsync(uri);
        }
        catch (Exception e)
        {
            Logger.Error(e);
        }
    }
}
