using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GenshinSwitch.Fetch.Launch;
using GenshinSwitch.Fetch.Regedit;
using GenshinSwitch.Helpers;
using GenshinSwitch.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace GenshinSwitch.ViewModels;

public partial class AddContactViewModel : ObservableRecipient
{
    [ObservableProperty]
    private string? aliasName = null!;

    [ObservableProperty]
    private string? localIconUri = null!;

    public ObservableCollection<AddContactSelectionViewModel> LocalIconSelectionUris { get; } = new();

    private string? prod = null!;
    public string? Prod
    {
        get => prod;
        set => SetProperty(ref prod, value?.Replace("\n", string.Empty) ?? string.Empty);
    }

    [ObservableProperty]
    private string? server = null!;

    [ObservableProperty]
    private int selectedServerIndex = (int)AddContactServer.Auto;
    partial void OnSelectedServerIndexChanged(int value)
    {
        RegetProd();
    }

    [ObservableProperty]
    private string? cookie = null!;

    public ICommand ChangeIconButtonCommand { get; }

    public ICommand GenerateAliasNameCommand { get; }

    public AddContactViewModel()
    {
        foreach (var kv in LocalAvatars.Stocks)
        {
            LocalIconSelectionUris.Add(new()
            {
                Parent = this,
                LocalIconUri = kv.Value,
            });
        }

        ChangeIconButtonCommand = new RelayCommand<string>(uriString =>
        {
            LocalIconUri = uriString;
        });

        GenerateAliasNameCommand = new RelayCommand(() =>
        {
            AliasName = GenerateChineseWords.Generate();
        });
    }

    public void Reload(Contact contact = null!)
    {
        if (contact is null)
        {
            RegetProd();
            LocalIconUri = LocalAvatars.Default;
        }
        else
        {
            AliasName = contact.AliasName;
            LocalIconUri = contact.LocalIconUri;
            Server = contact.Server;
            Prod = contact.Prod;
            Cookie = contact.Cookie;
        }
    }

    [RelayCommand]
    private void RegetProd()
    {
        switch ((AddContactServer)selectedServerIndex)
        {
            case AddContactServer.Auto:
                if (!string.IsNullOrEmpty(Prod = GenshinRegedit.ProdCN))
                {
                    Server = LaunchCtrl.RegionCN;
                }
                else
                {
                    Prod = GenshinRegedit.ProdOVERSEA;
                    Server = LaunchCtrl.RegionOVERSEA;
                }
                break;
            case AddContactServer.Ch:
                Prod = GenshinRegedit.ProdCN;
                Server = LaunchCtrl.RegionCN;
                break;
            case AddContactServer.Oversea:
                Prod = GenshinRegedit.ProdOVERSEA;
                Server = LaunchCtrl.RegionOVERSEA;
                break;
        }
    }
}

public enum AddContactServer
{
    Auto,
    Ch,
    Oversea,
}
