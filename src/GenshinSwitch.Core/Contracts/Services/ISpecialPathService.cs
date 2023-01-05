namespace GenshinSwitch.Core.Contracts.Services;

public interface ISpecialPathService
{
    string GetFolder(string optionFolder);
    string GetPath(string baseName);
    string GetTempPath(string baseName);
}
