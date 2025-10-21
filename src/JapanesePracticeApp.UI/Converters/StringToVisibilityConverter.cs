using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace JapanesePracticeApp.UI.Converters;

/// <summary>
/// Converts a string to a Visibility value (Visible if non-empty, Collapsed if empty).
/// </summary>
public class StringToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string str)
        {
            return string.IsNullOrWhiteSpace(str) ? Visibility.Collapsed : Visibility.Visible;
        }
        return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
