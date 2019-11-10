using System;
using System.Globalization;
using System.Windows;

namespace Bramf.Wpf.ValueConverter
{
    /// <summary>
    /// Takes a <see cref="bool"/> and returns its negative
    /// </summary>
    public class BooleanInverterConverter : BaseValueConverter<BooleanInverterConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => !(bool)value;

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => (bool)value;
    }

    /// <summary>
    /// Takes a <see cref="bool"/> and return its <see cref="Visibility"/> representation
    /// </summary>
    public class BooleanToVisibilityConverter : BaseValueConverter<BooleanToVisibilityConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
                return (bool)value ? Visibility.Visible : Visibility.Collapsed;
            else
                return (bool)value ? Visibility.Collapsed : Visibility.Visible;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
                return (Visibility)value == Visibility.Visible ? true : false;
            else
                return (Visibility)value == Visibility.Visible ? false : true;
        }
    }
}
