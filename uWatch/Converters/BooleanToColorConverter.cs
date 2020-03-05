using System;
using System.Globalization;
using Xamarin.Forms;

namespace uWatch
{
    class BooleanToColorConverter : IValueConverter
    {
        Color TrueColor;
        Color FalseColor;

        public BooleanToColorConverter(Color trueColor, Color falseColor)
        {
            TrueColor = trueColor;
            FalseColor = falseColor;
        }

   
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value) ? TrueColor : FalseColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
