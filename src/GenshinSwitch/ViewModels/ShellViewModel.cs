using CommunityToolkit.Mvvm.ComponentModel;

using GenshinSwitch.Contracts.Services;
using GenshinSwitch.Views;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace GenshinSwitch.ViewModels;

public class ShellViewModel : ObservableRecipient
{
    public INavigationService NavigationService { get; }

    public INavigationViewService NavigationViewService { get; }

    private bool isBackEnabled;
    public bool IsBackEnabled
    {
        get => isBackEnabled;
        set => SetProperty(ref isBackEnabled, value);
    }

    private object? selected;
    public object? Selected
    {
        get => selected;
        set => SetProperty(ref selected, value);
    }

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
}
