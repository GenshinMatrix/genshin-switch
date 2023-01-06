using GenshinSwitch.Contracts.Services;
using GenshinSwitch.ViewModels;
using Microsoft.UI.Xaml;

namespace GenshinSwitch.Activation;

public class DefaultActivationHandler : ActivationHandler<LaunchActivatedEventArgs>
{
    private readonly INavigationService navigationService;

    public DefaultActivationHandler(INavigationService navigationService)
    {
        this.navigationService = navigationService;
    }

    protected override bool CanHandleInternal(LaunchActivatedEventArgs args)
    {
        // None of the ActivationHandlers has handled the activation.
        return navigationService.Frame?.Content == null;
    }

    protected async override Task HandleInternalAsync(LaunchActivatedEventArgs args)
    {
        navigationService.NavigateTo(typeof(MainViewModel).FullName!, args.Arguments);

        await Task.CompletedTask;
    }
}
