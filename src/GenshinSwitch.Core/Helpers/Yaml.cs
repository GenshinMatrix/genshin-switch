using Newtonsoft.Json;

namespace GenshinSwitch.Core.Helpers;

public static class Yaml
{
    public static async Task<T> ToObjectAsync<T>(string value)
    {
        return await Task.Run<T>(() =>
        {
            // todo
            return JsonConvert.DeserializeObject<T>(value)!;
        });
    }

    public static async Task<string> StringifyAsync(object value)
    {
        return await Task.Run(() =>
        {
            // todo
            return JsonConvert.SerializeObject(value);
        });
    }
}
