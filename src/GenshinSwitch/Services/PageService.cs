using CommunityToolkit.Mvvm.ComponentModel;

using GenshinSwitch.Contracts.Services;
using GenshinSwitch.ViewModels;
using GenshinSwitch.Views;

using Microsoft.UI.Xaml.Controls;

namespace GenshinSwitch.Services;

public class PageService : IPageService
{
    private readonly Dictionary<string, Type> pages = new();

    public PageService()
    {
        Configure<MainViewModel, MainPage>();
        Configure<WebViewViewModel, WebViewPage>();
        Configure<SettingsViewModel, SettingsPage>();
    }

    public Type GetPageType(string key)
    {
        Type? pageType;
        lock (pages)
        {
            if (!pages.TryGetValue(key, out pageType))
            {
                throw new ArgumentException($"Page not found: {key}. Did you forget to call PageService.Configure?");
            }
        }

        return pageType;
    }

    private void Configure<TVM, TV>()
        where TVM : ObservableObject
        where TV : Page
    {
        lock (pages)
        {
            string key = typeof(TVM).FullName!;
            if (pages.ContainsKey(key))
            {
                throw new ArgumentException($"The key {key} is already configured in PageService");
            }

            Type type = typeof(TV);
            if (pages.Any(p => p.Value == type))
            {
                throw new ArgumentException($"This type is already configured with key {pages.First(p => p.Value == type).Key}");
            }

            pages.Add(key, type);
        }
    }
}
