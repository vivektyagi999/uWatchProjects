using System;
using System.Globalization;
using Xamarin.Forms;

namespace uWatch
{
    public class BoolToColorConverter:IValueConverter
    {
        public Color TrueColor
        {
            get;
            set;
        }

        public Color FalseColor
        {
            get;
            set;
        }

        public BoolToColorConverter()
        {
            
        }

        public BoolToColorConverter(Color trueColor, Color falseColor)
        {
            TrueColor = trueColor;
            FalseColor = falseColor;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if ((bool)value)
            {
                return TrueColor;
                //if(parameter!=null)
                //{
                //    if(System.Convert.ToInt32(parameter)==100)
                //    {
                //        return Color.FromHex("#B78DE1");
                //    }
                //}

                //return Color.Transparent;
            }
            return FalseColor;
            //return Color.FromHex("#ddd");

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
