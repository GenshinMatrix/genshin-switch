using GenshinSwitch.Fetch.Regedit;
using Newtonsoft.Json;
using System.IO.Pipes;

namespace GenshinSwitch.Fetch.Service;

public static class MainService
{
    public static bool ServiceEnabled { get; set; } = false;

    internal static void SetGameAccountRegisty(string key, string value, GameType type = GameType.CN)
    {
        using NamedPipeClientStream pipeClient = new(".", "GenshinSwitch.WindowsService", PipeDirection.InOut);
        pipeClient.Connect(2000);
        using StreamWriter writer = new StreamWriter(pipeClient);
        writer.WriteLine(JsonConvert.SerializeObject(new
        {
            Command = MainServiceCommmand.SetGameAccountRegisty.GetHashCode(),
            Type = type.ToString(),
            Key = key,
            Value = value,
        }));
        writer.Flush();
        return;
    }
}
