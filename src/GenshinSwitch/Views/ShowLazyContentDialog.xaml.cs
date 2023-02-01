using GenshinSwitch.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace GenshinSwitch.Views;

public sealed partial class ShowLazyContentDialog : ContentDialog
{
    public ShowLazyViewModel ViewModel { get; }

    public ShowLazyContentDialog(string file)
    {
        ViewModel = App.GetService<ShowLazyViewModel>();
        ViewModel.Reload(file);
        InitializeComponent();
    }
}
