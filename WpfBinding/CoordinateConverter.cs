using System;
using System.Globalization;
using System.Windows.Data;
using WpfBinding.Geomerty;

namespace WpfBinding
{
    public class CoordinateConverter : IValueConverter
    {
        private readonly IScaleTransformer _scaleTransformer;

        public CoordinateConverter(IScaleTransformer scaleTransformer)
        {
            _scaleTransformer = scaleTransformer;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return _scaleTransformer.Transform((double) value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return _scaleTransformer.Untransform((double) value);
        }
    }
}