using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GenshinSwitch.Controls;
using GenshinSwitch.Fetch.Regedit;
using GenshinSwitch.Helpers;
using GenshinSwitch.Models;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace GenshinSwitch.ViewModels;

public partial class AddContactViewModel : ObservableRecipient
{
    private string? aliasName = null!;
    public string? AliasName
    {
        get => aliasName;
        set => SetProperty(ref aliasName, value);
    }

    private string? localIconUri = null!;
    public string? LocalIconUri
    {
        get => localIconUri;
        set => SetProperty(ref localIconUri, value);
    }

    public ObservableCollection<AddContactSelectionViewModel> LocalIconSelectionUris { get; } = new();

    private string? prod = null!;
    public string? Prod
    {
        get => prod;
        set => SetProperty(ref prod, value?.Replace("\n", string.Empty));
    }

    [Obsolete]
    private string? region = null!;
    [Obsolete]
    public string? Region
    {
        get => region;
        set => SetProperty(ref region, value);
    }

    private string? cookie = null!;
    public string? Cookie
    {
        get => cookie;
        set => SetProperty(ref cookie, value);
    }

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
            Prod = GenshinRegedit.ProdCN;
            LocalIconUri = LocalAvatars.Default;
        }
        else
        {
            AliasName = contact.AliasName;
            LocalIconUri = contact.LocalIconUri;
            Prod = contact.Prod;
            Cookie = contact.Cookie;
        }
    }

    [RelayCommand]
    private async Task RegetProdAsync()
    {
        if (await new MessageBoxX("是否确定要从注册表重新获取账号？", "重新获取账号").ShowAsync() == ContentDialogResult.Secondary)
        {
            Prod = GenshinRegedit.ProdCN;
        }
    }
}
