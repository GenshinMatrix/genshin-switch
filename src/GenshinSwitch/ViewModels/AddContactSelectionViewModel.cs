using CommunityToolkit.Mvvm.ComponentModel;

namespace GenshinSwitch.ViewModels;

public class AddContactSelectionViewModel : ObservableRecipient
{
    public AddContactViewModel? Parent { get; set; }

    private string? localIconUri = null!;
    public string? LocalIconUri
    {
        get => localIconUri;
        set => SetProperty(ref localIconUri, value);
    }
}
