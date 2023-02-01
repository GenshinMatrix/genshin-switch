using GenshinSwitch.Models;
using GenshinSwitch.ViewModels;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Controls;
using System.Diagnostics.CodeAnalysis;

namespace GenshinSwitch.Views;

[SuppressMessage("Usage", "VSTHRD101:Avoid unsupported async delegates", Justification = "<Pending>")]
public sealed partial class MainPage : Page
{
    public MainViewModel ViewModel { get; }

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();

        foreach (var kv in Settings.Contacts.Get())
        {
            ViewModel.Contacts.Add(kv.Value);

            _ = App.MainWindow.DispatcherQueue.TryEnqueue(DispatcherQueuePriority.Low, async () =>
            {
                await kv.Value.ViewModel.FetchAllAsync();
            });
        }
        InitializeComponent();
    }
}
