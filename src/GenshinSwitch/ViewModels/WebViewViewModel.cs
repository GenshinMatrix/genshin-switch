using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using GenshinSwitch.Contracts.Services;
using GenshinSwitch.Contracts.ViewModels;

using Microsoft.Web.WebView2.Core;

namespace GenshinSwitch.ViewModels;

// TODO: Review best practices and distribution guidelines for WebView2.
// https://docs.microsoft.com/microsoft-edge/webview2/get-started/winui
// https://docs.microsoft.com/microsoft-edge/webview2/concepts/developer-guide
// https://docs.microsoft.com/microsoft-edge/webview2/concepts/distribution
public class WebViewViewModel : ObservableRecipient, INavigationAware
{
    // TODO: Set the default URL to display.
    private Uri source = new("https://webstatic.mihoyo.com/ys/app/interactive-map/");
    private bool isLoading = true;
    private bool hasFailures;

    public IWebViewService WebViewService { get; }

    public Uri Source
    {
        get => source;
        set => SetProperty(ref source, value);
    }

    public bool IsLoading
    {
        get => isLoading;
        set => SetProperty(ref isLoading, value);
    }

    public bool HasFailures
    {
        get => hasFailures;
        set => SetProperty(ref hasFailures, value);
    }

    public ICommand BrowserBackCommand { get; }

    public ICommand BrowserForwardCommand { get; }

    public ICommand ReloadCommand { get; }

    public ICommand RetryCommand { get; }

    public ICommand OpenInBrowserCommand { get; }

    public WebViewViewModel(IWebViewService webViewService)
    {
        WebViewService = webViewService;

        BrowserBackCommand = new RelayCommand(() => WebViewService.GoBack(), () => WebViewService.CanGoBack);
        BrowserForwardCommand = new RelayCommand(() => WebViewService.GoForward(), () => WebViewService.CanGoForward);
        ReloadCommand = new RelayCommand(() => WebViewService.Reload());
        RetryCommand = new RelayCommand(OnRetry);
        OpenInBrowserCommand = new RelayCommand(async () => await Windows.System.Launcher.LaunchUriAsync(WebViewService.Source), () => WebViewService.Source != null);
    }

    public void OnNavigatedTo(object parameter)
    {
        WebViewService.NavigationCompleted += OnNavigationCompleted;
    }

    public void OnNavigatedFrom()
    {
        WebViewService.UnregisterEvents();
        WebViewService.NavigationCompleted -= OnNavigationCompleted;
    }

    private void OnNavigationCompleted(object? sender, CoreWebView2WebErrorStatus webErrorStatus)
    {
        IsLoading = false;
        OnPropertyChanged(nameof(BrowserBackCommand));
        OnPropertyChanged(nameof(BrowserForwardCommand));
        if (webErrorStatus != default)
        {
            HasFailures = true;
        }
    }

    private void OnRetry()
    {
        HasFailures = false;
        IsLoading = true;
        WebViewService?.Reload();
    }
}
