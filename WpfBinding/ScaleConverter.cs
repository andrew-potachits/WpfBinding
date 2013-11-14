using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfBinding
{
    public class ScaleConverter : IValueConverter
    {
        private readonly IScaleProvider _scaleProvider;

        public ScaleConverter(IScaleProvider scaleProvider)
        {
            _scaleProvider = scaleProvider;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}