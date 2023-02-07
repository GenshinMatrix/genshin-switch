using GenshinSwitch.Activation;
using GenshinSwitch.Contracts.Services;
using GenshinSwitch.Views;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace GenshinSwitch.Services;

public class ActivationService : IActivationService
{
    private readonly ActivationHandler<LaunchActivatedEventArgs> defaultHandler;
    private readonly IEnumerable<IActivationHandler> activationHandlers;
    private readonly IThemeSelectorService themeSelectorService;
    private UIElement? shell = null;

    public ActivationService(ActivationHandler<LaunchActivatedEventArgs> defaultHandler, IEnumerable<IActivationHandler> activationHandlers, IThemeSelectorService themeSelectorService)
    {
        this.defaultHandler = defaultHandler;
        this.activationHandlers = activationHandlers;
        this.themeSelectorService = themeSelectorService;
    }

    public async Task ActivateAsync(object activationArgs)
    {
        // Execute tasks before activation.
        await InitializeAsync();

        // Set the MainWindow Content.
        if (App.MainWindow.Content == null)
        {
            shell = App.GetService<ShellPage>();
            App.MainWindow.Content = shell ?? new Frame();
            App.MainWindow.Closed += (_, _) => (shell as IDisposable)?.Dispose();
        }

        // Handle activation via ActivationHandlers.
        await HandleActivationAsync(activationArgs);

        // Activate the MainWindow.
        App.MainWindow.Activate();

        // Execute tasks after activation.
        await StartupAsync();
    }

    private async Task HandleActivationAsync(object activationArgs)
    {
        IActivationHandler? activationHandler = activationHandlers.FirstOrDefault(h => h.CanHandle(activationArgs));

        if (activationHandler != null)
        {
            await activationHandler.HandleAsync(activationArgs);
        }

        if (defaultHandler.CanHandle(activationArgs))
        {
            await defaultHandler.HandleAsync(activationArgs);
        }
    }

    private async Task InitializeAsync()
    {
        await themeSelectorService.InitializeAsync().ConfigureAwait(false);
        await Task.CompletedTask;
    }

    private async Task StartupAsync()
    {
        await themeSelectorService.SetRequestedThemeAsync();
        await Task.CompletedTask;
    }
}
