using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace GenshinSwitch.WindowsService;

internal partial class MainService : ServiceBase
{
    private IContainer components = null!;
    private bool isRunning = false;

    public MainService()
    {
        InitializeComponent();
    }

    public void StartTest()
    {
        OnStart(null!);
    }

    private void InitializeComponent()
    {
        components = new Container();
        ServiceName = "GenshinSwitch.WindowsService";
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    protected override void OnStart(string[] args)
    {
        _ = Task.Run(() =>
        {
            try
            {
                isRunning = true;

                while (isRunning)
                {
                    using NamedPipeServerStream pipeServer = new("GenshinSwitch.WindowsService", PipeDirection.InOut);
                    pipeServer.WaitForConnection();
                    using StreamReader reader = new(pipeServer);

                    try
                    {
                        string? message = reader.ReadLine();
                        Debug.WriteLine("Received message: " + message);

                        dynamic? obj = JsonConvert.DeserializeObject(message!);

                        _ = obj ?? JsonConvert.DeserializeObject(
                            """
                            {
                                "Command": 1,
                                "Type": "CN",
                                "Key": "",
                                "Value": "",
                            }
                            """
                        );

                        if (obj != null)
                        {
                            if ((int)obj.Command == (int)MainServiceCommmand.SetGameAccountRegisty)
                            {
                                Registry.SetValue(((GameType)Enum.Parse(typeof(GameType), (string)obj.Type)).GetRegKeyName(), (string)obj.Key, Encoding.UTF8.GetBytes((string)obj.Value));
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        });
    }

    protected override void OnStop()
    {
        isRunning = false;
    }
}

file enum MainServiceCommmand
{
    None = 0x00,
    SetGameAccountRegisty = 0x01,
}

file enum GameType
{
    CN,
    OVERSEA,
    CNCloud,
}

file static class RegeditKeys
{
    public const string CN = "原神";
    public const string PROD_CN = "MIHOYOSDK_ADL_PROD_CN_h3123967166";
    public const string DATA = "GENERAL_DATA_h2389025596";

    public const string OVERSEA = "Genshin Impact";
    public const string PROD_OVERSEA = "MIHOYOSDK_ADL_PROD_OVERSEA_h1158948810";

    public const string CNCloud = "云·原神";
    public const string PROD_CNCloud = "MIHOYOSDK_ADL_0";

    public static string GetRegKeyName(this GameType type)
    {
        return @"HKEY_CURRENT_USER\SOFTWARE\miHoYo\" + ParseGameType(type);
    }

    public static string ParseGameType(this GameType type)
    {
        return type switch
        {
            GameType.OVERSEA => OVERSEA,
            GameType.CNCloud => CNCloud,
            GameType.CN or _ => CN,
        };
    }
}
