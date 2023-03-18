using YamlDotNet.Serialization;

namespace GenshinSwitch.Core.Helpers;

public static class Json
{
    public static async Task<T> ToObjectAsync<T>(string value)
    {
        return await Task.Run(() =>
        {
            Deserializer deserializer = new();
            return deserializer.Deserialize<T>(value);
        });
    }

    public static async Task<string> StringifyAsync(object value)
    {
        return await Task.Run(() =>
        {
            Serializer serializer = new();
            return serializer.Serialize(value!);
        });
    }
}