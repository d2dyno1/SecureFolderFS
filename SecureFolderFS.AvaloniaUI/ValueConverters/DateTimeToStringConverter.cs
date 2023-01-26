﻿using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace SecureFolderFS.AvaloniaUI.ValueConverters
{
    internal sealed class DateTimeToStringConverter : IValueConverter
    {

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not DateTime dateTime)
                return string.Empty;

            string dateString;
            if (dateTime.Date == DateTime.Today)
            {
                dateString = $"Today, {dateTime.ToString("HH:mm")}"; // TODO: Localize
            }
            else
            {
                dateString = dateTime.Year == 1 ? "Unspecified" : dateTime.ToString("MM/dd/yyyy, HH:mm");
            }

            if (parameter is string formatString)
            {
                var split = formatString.Split('|');
                if (split[0] == "LOCALIZE")
                {
                    return string.Format(split[1], dateString); // TODO: Localize
                }
                else
                {
                    return string.Format(split[1], dateString);
                }
            }

            return dateString;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
