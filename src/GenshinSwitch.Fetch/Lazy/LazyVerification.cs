using System.Diagnostics;

namespace GenshinSwitch.Fetch.Lazy;

public static class LazyVerification
{
    public static async Task<bool> VerifyAssembly(string path)
    {
        FileVersionInfo fvi = await Task.Run(() => FileVersionInfo.GetVersionInfo(path));
        return fvi.ProductName == "GenshinLazy" && fvi.OriginalFilename == "GenshinLazy.dll" && fvi.CompanyName == "GenshinMatrix";
    }
}
