using System;
using System.Globalization;
using Xamarin.Forms;

namespace uWatch
{
    public class BoolToImageConverter:IValueConverter
    {
        public string TrueImage
        {
            get;
            set;
        }

        public string FalseImage
        {
            get;
            set;
        }
        public BoolToImageConverter(string trueImage, string falseImage)
        {
            TrueImage = trueImage;
            FalseImage = falseImage;
        }

        public BoolToImageConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
            {

                return TrueImage;
            }

            return FalseImage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
