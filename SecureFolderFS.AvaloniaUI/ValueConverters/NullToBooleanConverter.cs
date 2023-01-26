﻿using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace SecureFolderFS.AvaloniaUI.ValueConverters
{
    internal sealed class NullToBooleanConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (parameter is string strParam && strParam.ToLower() == "invert")
            {
                if (value is string str1)
                {
                    return string.IsNullOrEmpty(str1);
                }

                return value is null;
            }

            if (value is string str)
            {
                return !string.IsNullOrEmpty(str);
            }

            return value is not null;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
