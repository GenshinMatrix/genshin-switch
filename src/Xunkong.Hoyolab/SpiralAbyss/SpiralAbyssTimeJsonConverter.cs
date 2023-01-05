﻿namespace Xunkong.Hoyolab.SpiralAbyss;

public class SpiralAbyssTimeJsonConverter : JsonConverter<DateTimeOffset>
{
    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var str = reader.GetString();
        if (int.TryParse(str, out var seconds))
        {
            return DateTimeOffset.FromUnixTimeSeconds(seconds);
        }
        else
        {
            return DateTimeOffset.Now;
        }
    }

    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToUnixTimeSeconds().ToString());
    }
}