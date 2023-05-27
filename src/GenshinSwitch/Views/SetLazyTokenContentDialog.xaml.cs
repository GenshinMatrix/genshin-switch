using GenshinSwitch.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace GenshinSwitch.Views;

public sealed partial class SetLazyTokenContentDialog : ContentDialog
{
    public SetLazyTokenViewModel ViewModel { get; }

    public SetLazyTokenContentDialog()
    {
        ViewModel = App.GetService<SetLazyTokenViewModel>();
        InitializeComponent();
    }

    public async Task<(ContentDialogResult, string)> ShowAndGetTokenAsync()
    {
        ContentDialogResult result = await ShowAsync();
        string tokenInput = ViewModel.TokenInput;
        return (result, tokenInput);
    }
}
