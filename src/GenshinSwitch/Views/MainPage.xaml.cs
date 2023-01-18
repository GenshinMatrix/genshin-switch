using GenshinSwitch.Models;
using GenshinSwitch.ViewModels;
using Microsoft.UI.Dispatching;
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

#pragma warning disable VSTHRD101 // Avoid unsupported async delegates
            _ = App.MainWindow.DispatcherQueue.TryEnqueue(DispatcherQueuePriority.Low, async () =>
            {
                await kv.Value.ViewModel.FetchAllAsync();
            });
#pragma warning restore VSTHRD101 // Avoid unsupported async delegates
        }
        InitializeComponent();
    }
}
