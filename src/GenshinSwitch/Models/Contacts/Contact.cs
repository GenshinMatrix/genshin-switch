using CommunityToolkit.Mvvm.ComponentModel;
using System.Reflection;
using Xunkong.Hoyolab.Account;
using Xunkong.Hoyolab.SpiralAbyss;
using Xunkong.Hoyolab.TravelNotes;
using YamlDotNet.Serialization;

namespace GenshinSwitch.Models;

[Obfuscation]
public partial class Contact : ObservableRecipient
{
    public string Guid { get; set; } = System.Guid.NewGuid().ToString("N");

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

    private string? prod = null!;
    public string? Prod
    {
        get => prod;
        set => SetProperty(ref prod, value);
    }

    private string? server = null!;
    public string? Server
    {
        get => server;
        set => SetProperty(ref server, value);
    }

    private string? region = null!;
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

    private int? uid = null!;
    public int? Uid
    {
        get => uid;
        set => SetProperty(ref uid, value);
    }

    private int? level = null!;
    public int? Level
    {
        get => level;
        set => SetProperty(ref level, value);
    }
}
