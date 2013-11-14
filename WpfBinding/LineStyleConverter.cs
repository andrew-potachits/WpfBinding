using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WpfBinding
{
    internal class LineStyleConverter : IValueConverter
    {
        private readonly ResourceDictionary _resources;
        private readonly string _selectedStyleName;
        private readonly string _unselectedStyleName;

        public LineStyleConverter(ResourceDictionary resources, 
            string selectedStyleName, 
            string unselectedStyleName)
        {
            _resources = resources;
            _selectedStyleName = selectedStyleName;
            _unselectedStyleName = unselectedStyleName;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            LineDef data = value as LineDef;
            if (data == null || !data.Selected)
                return _resources[_unselectedStyleName];

            return _resources[_selectedStyleName];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}