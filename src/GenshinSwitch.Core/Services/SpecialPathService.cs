using GenshinSwitch.Core.Contracts.Services;

namespace GenshinSwitch.Core.Services;

public class SpecialPathService : ISpecialPathService
{
    private const string _defaultApplicationDataFolder = "genshin-switch";
    private readonly static string _localApplicationData = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

    internal static SpecialPathService Provider => new();
    public static string TempPath { get; } = Path.GetTempPath();

    public string GetFolder(string optionFolder = null!)
    {
        return Path.Combine(_localApplicationData, optionFolder ?? _defaultApplicationDataFolder);
    }

    public string GetPath(string baseName)
    {
        string configPath = Path.Combine(GetFolder(), baseName);

        if (!Directory.Exists(new FileInfo(configPath).DirectoryName))
        {
            Directory.CreateDirectory(new FileInfo(configPath).DirectoryName!);
        }
        return configPath;
    }

    public string GetTempPath(string baseName)
    {
        return Path.Combine(TempPath + _defaultApplicationDataFolder, baseName);
    }
}
