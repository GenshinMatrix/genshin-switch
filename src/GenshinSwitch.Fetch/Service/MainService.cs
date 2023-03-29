using GenshinSwitch.Fetch.Regedit;
using Newtonsoft.Json;
using System.IO.Pipes;
using System.Text;

namespace GenshinSwitch.Fetch.Service;

public static class MainService
{
    public static bool ServiceEnabled { get; set; } = false;

    internal static void SetGameAccountRegisty(string key, string value, GameType type = GameType.CN)
    {
        using NamedPipeClientStream pipeClient = new(".", "GenshinSwitch.WindowsService", PipeDirection.InOut);
        pipeClient.Connect(2000);
        using StreamWriter writer = new(pipeClient);
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

    internal static string GetGameAccountRegisty(string key, GameType type = GameType.CN)
    {
        using NamedPipeClientStream pipeClient = new(".", "GenshinSwitch.WindowsService", PipeDirection.InOut);
        pipeClient.Connect(2000);
        StreamWriter writer = new(pipeClient);
        using StreamReader reader = new(pipeClient);
        writer.WriteLine(JsonConvert.SerializeObject(new
        {
            Command = MainServiceCommmand.GetGameAccountRegisty.GetHashCode(),
            Type = type.ToString(),
            Key = key,
        }));
        writer.Flush();

        string? ret = string.Empty;
        ret = reader.ReadLine();
        writer.Close();

        if (ret is not null)
        {
            try
            {
                dynamic? retObj = JsonConvert.DeserializeObject(ret);

                if (Convert.FromBase64String((string)retObj!.Value) is byte[] bytes)
                {
                    return Encoding.UTF8.GetString(bytes);
                }
            }
            catch
            {
            }
        }
        return null!;
    }
}
