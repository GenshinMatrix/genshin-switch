using Microsoft.UI.Xaml;

namespace GenshinSwitch.Contracts.Services;

public interface IThemeSelectorService
{
    public ElementTheme Theme { get; }

    public Task InitializeAsync();

    public Task SetThemeAsync(ElementTheme theme);

    public Task SetRequestedThemeAsync();
}
