using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using GenshinSwitch.Contracts.Services;
using GenshinSwitch.Core;
using GenshinSwitch.Core.Services;
using GenshinSwitch.Core.Settings;
using GenshinSwitch.Helpers;
using GenshinSwitch.Models;
using GenshinSwitch.Models.Messages;
using Microsoft.UI.Xaml;
using Microsoft.VisualStudio.Threading;
using System.Reflection;
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

    [ObservableProperty]
    private string versionDescription;

    [ObservableProperty]
    private int selectedThemeIndex = (int)Settings.AppBackgroundRequestedTheme.Get();
    partial void OnSelectedThemeIndexChanged(int value)
    {
        async Task OnSelectedThemeIndexChangedAsync()
        {
            await SwitchThemeAsync((ElementTheme)value);
        }
        _ = OnSelectedThemeIndexChangedAsync();
        Settings.AppBackgroundRequestedTheme.Set((ElementTheme)value);
        SettingsManager.Save();
        WeakReferenceMessenger.Default.Send(new ThemeChangedMessage() { Theme = (ElementTheme)value });
    }

    [ObservableProperty]
    private int windowBackdropIndex = Settings.Backdrop.Get() switch
    {
        "None" => 0,
        "Acrylic" => 1,
        "Mica" or _ => 2,
    };
    partial void OnWindowBackdropIndexChanged(int value)
    {
        Settings.Backdrop.Set(value switch
        {
            0 => "None",
            1 => "Acrylic",
            2 or _=> "Mica",
        });
        SettingsManager.Save();
        WeakReferenceMessenger.Default.Send(new ThemeChangedMessage() { Backdrop = Settings.Backdrop.Get() });
    }

    [ObservableProperty]
    private bool alwaysActiveBackdrop = false;
    partial void OnAlwaysActiveBackdropChanged(bool value)
    {
        throw new NotImplementedException();
    }

    [ObservableProperty]
    private bool apiResinOnOff = false;
    partial void OnApiResinOnOffChanged(bool value)
    {

    }

    public SettingsViewModel(IThemeSelectorService themeSelectorService)
    {
        this.themeSelectorService = themeSelectorService;
        elementTheme = this.themeSelectorService.Theme;
        versionDescription = GetVersionDescription();
    }

    [RelayCommand]
    private async Task SwitchThemeAsync(ElementTheme param)
    {
        if (ElementTheme != param)
        {
            ElementTheme = param;
            await themeSelectorService.SetThemeAsync(param);
        }
    }

    [RelayCommand]
    private async Task CheckUpdateAsync()
    {
        try
        {
            await Launcher.LaunchUriAsync(new Uri("https://github.com/genshin-matrix/genshin-switch/releases"));
        }
        catch (Exception e)
        {
            Logger.Error(e);
        }
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
}
