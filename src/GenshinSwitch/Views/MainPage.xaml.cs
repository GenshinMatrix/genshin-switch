using GenshinSwitch.Models;
using GenshinSwitch.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace GenshinSwitch.Views;

public sealed partial class MainPage : Page
{
    public MainViewModel ViewModel { get; }

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();

        foreach (var kv in Settings.Contacts.Get())
        {
            ViewModel.Contacts.Add(kv.Value);
        }
        InitializeComponent();
    }
}
