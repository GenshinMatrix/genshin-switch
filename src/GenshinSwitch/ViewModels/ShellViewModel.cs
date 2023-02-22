using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using GenshinSwitch.Contracts.Services;
using GenshinSwitch.Core;
using GenshinSwitch.Core.Settings;
using GenshinSwitch.Fetch.Muter;
using GenshinSwitch.Models;
using GenshinSwitch.Models.Messages;
using GenshinSwitch.Views;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Windows.System;

namespace GenshinSwitch.ViewModels;

public partial class ShellViewModel : ObservableRecipient
{
    public INavigationService NavigationService { get; }

    public INavigationViewService NavigationViewService { get; }

    [ObservableProperty]
    private bool isBackEnabled;

    [ObservableProperty]
    private object? selected;

    [ObservableProperty]
    private bool autoMute = Settings.AutoMute;
    partial void OnAutoMuteChanged(bool value)
    {
        MuteManager.AutoMute = value;
        Settings.AutoMute.Set(value);
        SettingsManager.Save();
        WeakReferenceMessenger.Default.Send(new AutoMuteChangedMessage());
    }
    private void OnAutoMuteChangedReceived()
    {
        autoMute = Settings.AutoMute;
        OnPropertyChanged(nameof(AutoMute));
    }

    public ShellViewModel(INavigationService navigationService, INavigationViewService navigationViewService)
    {
        NavigationService = navigationService;
        NavigationService.Navigated += OnNavigated;
        NavigationViewService = navigationViewService;
        WeakReferenceMessenger.Default.Register<AutoMuteChangedMessage>(this, (_, _) => OnAutoMuteChangedReceived());
    }

    private void OnNavigated(object sender, NavigationEventArgs e)
    {
        IsBackEnabled = NavigationService.CanGoBack;

        if (e.SourcePageType == typeof(SettingsPage))
        {
            Selected = NavigationViewService.SettingsItem;
            return;
        }

        NavigationViewItem? selectedItem = NavigationViewService.GetSelectedItem(e.SourcePageType);
        if (selectedItem != null)
        {
            Selected = selectedItem;
        }
    }

    [RelayCommand]
    public static void ActiveApp()
    {
        App.MainWindow.Activate();
        App.MainWindow.Show();
    }

    [RelayCommand]
    public static void HideApp()
    {
        App.MainWindow.Hide();
    }

    [RelayCommand]
    public static void ActiveOrHideApp()
    {
        if (App.MainWindow.Visible)
        {
            App.MainWindow.Hide();
        }
        else
        {
            App.MainWindow.Activate();
            App.MainWindow.Show();
        }
    }

    [RelayCommand]
    private void SetAutoMute()
    {
        AutoMute = !AutoMute;
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

    [RelayCommand]
    public static void ExitApp()
    {
        (App.Current as App)?.ExitForce();
    }
}
