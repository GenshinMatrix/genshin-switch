using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using GenshinSwitch.Contracts.Services;
using GenshinSwitch.Models.Messages;
using GenshinSwitch.Views;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace GenshinSwitch.ViewModels;

public partial class ShellViewModel : ObservableRecipient
{
    public INavigationService NavigationService { get; }

    public INavigationViewService NavigationViewService { get; }

    [ObservableProperty]
    private bool isBackEnabled;

    [ObservableProperty]
    private object? selected;

    public ShellViewModel(INavigationService navigationService, INavigationViewService navigationViewService)
    {
        NavigationService = navigationService;
        NavigationService.Navigated += OnNavigated;
        NavigationViewService = navigationViewService;
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
    public static void ExitApp()
    {
        (App.Current as App)?.ExitForce();
    }
}
