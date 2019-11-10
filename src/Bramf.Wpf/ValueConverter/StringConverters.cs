using Bramf.Extensions;
using System;
using System.Globalization;

namespace Bramf.Wpf.ValueConverter
{
    /// <summary>
    /// Takes a <see cref="Int64"/> and returns its string representation with size format. 
    /// Useful for file sizes
    /// </summary>
    public class LengthToReadableLength : BaseValueConverter<LengthToReadableLength>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => ((long)value).SizeSuffix();

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => long.MinValue;
    }
}
