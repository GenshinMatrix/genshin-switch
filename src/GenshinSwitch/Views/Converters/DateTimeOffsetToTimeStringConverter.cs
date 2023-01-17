﻿using Microsoft.UI.Xaml.Data;

namespace GenshinSwitch.Views.Converters;

internal class DateTimeOffsetToTimeStringConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is DateTimeOffset time)
        {
            return time.LocalDateTime.ToString("HH:mm:ss");
        }
        if (value is DateTime time1)
        {
            return time1.ToString("HH:mm:ss");
        }
        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
