// ValueConverters.cs
// Contains WinUI 3 IValueConverter implementations used throughout the app for
// XAML data-binding conversions (date formatting, string/bool to Visibility, URL to image).

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media.Imaging;
using System;

namespace NationalPark.Converters
{
    /// <summary>
    /// Converts a <see cref="DateTime"/> value to a human-readable date string
    /// formatted as "MMMM dd, yyyy" (e.g., "January 01, 2024").
    /// <see cref="ConvertBack"/> is not supported.
    /// </summary>
    public class DateTimeToStringConverter : IValueConverter
    {
        /// <summary>
        /// Formats a <see cref="DateTime"/> as "MMMM dd, yyyy".
        /// Returns <see cref="string.Empty"/> when <paramref name="value"/> is not a <see cref="DateTime"/>.
        /// </summary>
        /// <param name="value">The source value; expected to be a <see cref="DateTime"/>.</param>
        /// <param name="targetType">The target binding type (unused).</param>
        /// <param name="parameter">An optional converter parameter (unused).</param>
        /// <param name="language">The language/culture for the conversion (unused).</param>
        /// <returns>A formatted date string, or <see cref="string.Empty"/> if conversion is not possible.</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTime dateTime)
            {
                return dateTime.ToString("MMMM dd, yyyy");
            }
            return string.Empty;
        }

        /// <summary>
        /// Not supported. Always throws <see cref="NotImplementedException"/>.
        /// </summary>
        /// <param name="value">The value to convert back (unused).</param>
        /// <param name="targetType">The target binding type (unused).</param>
        /// <param name="parameter">An optional converter parameter (unused).</param>
        /// <param name="language">The language/culture for the conversion (unused).</param>
        /// <returns>Never returns; always throws.</returns>
        /// <exception cref="NotImplementedException">Always thrown.</exception>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converts a string value to a <see cref="Visibility"/> value.
    /// Returns <see cref="Visibility.Visible"/> when the string is non-empty and non-whitespace;
    /// returns <see cref="Visibility.Collapsed"/> otherwise.
    /// <see cref="ConvertBack"/> is not supported.
    /// </summary>
    public class StringToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Returns <see cref="Visibility.Visible"/> when <paramref name="value"/> is a non-empty,
        /// non-whitespace string; returns <see cref="Visibility.Collapsed"/> otherwise.
        /// </summary>
        /// <param name="value">The source value; expected to be a <see cref="string"/>.</param>
        /// <param name="targetType">The target binding type (unused).</param>
        /// <param name="parameter">An optional converter parameter (unused).</param>
        /// <param name="language">The language/culture for the conversion (unused).</param>
        /// <returns><see cref="Visibility.Visible"/> or <see cref="Visibility.Collapsed"/>.</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return string.IsNullOrWhiteSpace(value?.ToString()) ? Visibility.Collapsed : Visibility.Visible;
        }

        /// <summary>
        /// Not supported. Always throws <see cref="NotImplementedException"/>.
        /// </summary>
        /// <param name="value">The value to convert back (unused).</param>
        /// <param name="targetType">The target binding type (unused).</param>
        /// <param name="parameter">An optional converter parameter (unused).</param>
        /// <param name="language">The language/culture for the conversion (unused).</param>
        /// <returns>Never returns; always throws.</returns>
        /// <exception cref="NotImplementedException">Always thrown.</exception>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converts a <see cref="bool"/> value to a <see cref="Visibility"/> value.
    /// <c>true</c> maps to <see cref="Visibility.Visible"/>; <c>false</c> maps to
    /// <see cref="Visibility.Collapsed"/>. <see cref="ConvertBack"/> is also supported.
    /// </summary>
    public class BoolToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Maps <c>true</c> to <see cref="Visibility.Visible"/> and <c>false</c> (or a non-bool value)
        /// to <see cref="Visibility.Collapsed"/>.
        /// </summary>
        /// <param name="value">The source value; expected to be a <see cref="bool"/>.</param>
        /// <param name="targetType">The target binding type (unused).</param>
        /// <param name="parameter">An optional converter parameter (unused).</param>
        /// <param name="language">The language/culture for the conversion (unused).</param>
        /// <returns><see cref="Visibility.Visible"/> when <paramref name="value"/> is <c>true</c>;
        /// <see cref="Visibility.Collapsed"/> otherwise.</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool boolValue)
            {
                return boolValue ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        /// <summary>
        /// Maps <see cref="Visibility.Visible"/> to <c>true</c> and any other visibility value to <c>false</c>.
        /// Returns <c>false</c> when <paramref name="value"/> is not a <see cref="Visibility"/>.
        /// </summary>
        /// <param name="value">The source value; expected to be a <see cref="Visibility"/>.</param>
        /// <param name="targetType">The target binding type (unused).</param>
        /// <param name="parameter">An optional converter parameter (unused).</param>
        /// <param name="language">The language/culture for the conversion (unused).</param>
        /// <returns><c>true</c> when <paramref name="value"/> is <see cref="Visibility.Visible"/>;
        /// <c>false</c> otherwise.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is Visibility visibility)
            {
                return visibility == Visibility.Visible;
            }
            return false;
        }
    }

    /// <summary>
    /// Converts a URL string to a <see cref="BitmapImage"/> suitable for thumbnail display.
    /// An optional <c>ConverterParameter</c> can specify the decode width in logical pixels;
    /// defaults to 400 when omitted or unparseable.
    /// <see cref="ConvertBack"/> is not supported.
    /// </summary>
    public class UrlToThumbnailImageConverter : IValueConverter
    {
        /// <summary>
        /// Creates a <see cref="BitmapImage"/> from a URL string, decoded at the width specified by
        /// <paramref name="parameter"/> (parsed as an <see cref="int"/>), or 400 logical pixels if
        /// no valid width is provided. Returns <c>null</c> when <paramref name="value"/> is not a
        /// non-empty string.
        /// </summary>
        /// <param name="value">The source value; expected to be a non-empty URL <see cref="string"/>.</param>
        /// <param name="targetType">The target binding type (unused).</param>
        /// <param name="parameter">
        /// An optional string containing the desired decode pixel width (e.g., <c>"200"</c>).
        /// Defaults to 400 when absent or not parseable as an integer.
        /// </param>
        /// <param name="language">The language/culture for the conversion (unused).</param>
        /// <returns>A <see cref="BitmapImage"/> loaded from the URL, or <c>null</c> if the URL is empty.</returns>
        public object? Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string url && !string.IsNullOrEmpty(url))
            {
                var bitmapImage = new BitmapImage
                {
                    UriSource = new Uri(url),
                    DecodePixelWidth = parameter is string w && int.TryParse(w, out var width) ? width : 400,
                    DecodePixelType = DecodePixelType.Logical
                };
                return bitmapImage;
            }
            return null;
        }

        /// <summary>
        /// Not supported. Always throws <see cref="NotImplementedException"/>.
        /// </summary>
        /// <param name="value">The value to convert back (unused).</param>
        /// <param name="targetType">The target binding type (unused).</param>
        /// <param name="parameter">An optional converter parameter (unused).</param>
        /// <param name="language">The language/culture for the conversion (unused).</param>
        /// <returns>Never returns; always throws.</returns>
        /// <exception cref="NotImplementedException">Always thrown.</exception>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}