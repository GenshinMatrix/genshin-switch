using System.Diagnostics.CodeAnalysis;

using GenshinSwitch.Contracts.Services;

using Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;

namespace GenshinSwitch.Services;

public class WebViewService : IWebViewService
{
    private WebView2? webView;

    public Uri? Source => webView?.Source;

    [MemberNotNullWhen(true, nameof(webView))]
    public bool CanGoBack => webView != null && webView.CanGoBack;

    [MemberNotNullWhen(true, nameof(webView))]
    public bool CanGoForward => webView != null && webView.CanGoForward;

    public event EventHandler<CoreWebView2WebErrorStatus>? NavigationCompleted;

    public WebViewService()
    {
    }

    [MemberNotNull(nameof(WebViewService.webView))]
    public void Initialize(WebView2 webView)
    {
        this.webView = webView;
        this.webView.NavigationCompleted += OnWebViewNavigationCompleted;
    }

    public void GoBack() => webView?.GoBack();

    public void GoForward() => webView?.GoForward();

    public void Reload() => webView?.Reload();

    public void UnregisterEvents()
    {
        if (webView != null)
        {
            webView.NavigationCompleted -= OnWebViewNavigationCompleted;
        }
    }

    private void OnWebViewNavigationCompleted(WebView2 sender, CoreWebView2NavigationCompletedEventArgs args) => NavigationCompleted?.Invoke(this, args.WebErrorStatus);
}
