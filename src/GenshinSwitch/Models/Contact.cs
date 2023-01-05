using CommunityToolkit.Mvvm.ComponentModel;
using System.Reflection;
using Xunkong.Hoyolab.Account;
using Xunkong.Hoyolab.SpiralAbyss;
using Xunkong.Hoyolab.TravelNotes;
using YamlDotNet.Serialization;

namespace GenshinSwitch.Models;

[Obfuscation]
public class Contact : ObservableRecipient
{
    public string Guid { get; set; } = System.Guid.NewGuid().ToString("N");

    protected string? aliasName = null!;
    public string? AliasName
    {
        get => aliasName;
        set => SetProperty(ref aliasName, value);
    }

    protected string? localIconUri = null!;
    public string? LocalIconUri
    {
        get => localIconUri;
        set => SetProperty(ref localIconUri, value);
    }

    protected string? prod = null!;
    public string? Prod
    {
        get => prod;
        set => SetProperty(ref prod, value);
    }

    protected string? server = null!;
    public string? Server
    {
        get => server;
        set => SetProperty(ref server, value);
    }

    protected string? region = null!;
    public string? Region
    {
        get => region;
        set => SetProperty(ref region, value);
    }

    protected string? cookie = null!;
    public string? Cookie
    {
        get => cookie;
        set => SetProperty(ref cookie, value);
    }

    protected int? uid = null!;
    public int? Uid
    {
        get => uid;
        set => SetProperty(ref uid, value);
    }

    protected int? level = null!;
    public int? Level
    {
        get => level;
        set => SetProperty(ref level, value);
    }

    [YamlIgnore] public ContactProgress ResinInfo { get; set; } = new();
    [YamlIgnore] public ContactProgress SignInInfo { get; set; } = new();

    protected bool isRunning = false;
    [YamlIgnore] public bool IsRunning
    {
        get => isRunning;
        set => SetProperty(ref isRunning, value);
    }

    [YamlIgnore] public GenshinRoleInfo? _Role { get; set; }
    [YamlIgnore] public SignInInfo? _SignInInfo { get; set; }
    [YamlIgnore] public TravelNotesSummary? _TravelNotesSummary { get; set; }
    [YamlIgnore] public string? _SpiralAbyss { get; set; }
    [YamlIgnore] public SpiralAbyssInfo? _SpiralAbyssInfo { get; set; }
}
