using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;

namespace GenshinSwitch.Controls;

/// <summary>
/// https://github.com/xunkong/xunkong/blob/main/App/Controls/SettingCard.xaml.cs
/// </summary>
[INotifyPropertyChanged]
public sealed partial class SettingCard : UserControl
{
    [ObservableProperty]
    private object icon = null!;

    [ObservableProperty]
    private object content = null!;

    [ObservableProperty]
    private object selector = null!;

    public SettingCard()
    {
        InitializeComponent();
    }
}
