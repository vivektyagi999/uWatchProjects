//using ImageCircle.Forms.Plugin.Abstractions;
using System;
using Xamarin.Forms;


namespace UwatchPCL
{
    public class MyUiUtils
    {

        public static string OnlyTempSymbol = "\u00B0";
        public static string CelsiusdegreeSymbol = "\u2103";
        public static string FahrenheitdegreeSymbol = "\u2109";
        public static string emptySpace = "\u0020";



		public static int getPercentual(double dim, int perc)
		{
			if (dim <= MyController.VirtualHeight && dim > MyController.VirtualWidth)
			{
				return Convert.ToInt32((MyController.VirtualHeight * perc) / 100);
			}
			else
			{
				return Convert.ToInt32((MyController.VirtualWidth * perc) / 100);
			}
		}


        public static double GetInWPercent(View p1, double perc)
        {
			var p =p1.Width / 100 * perc; 
			return p;
        }

        public static double GetInHPercent(View p1, double perc)
        {
			var p =p1.Height / 100 * perc; 
			return p;
        }

        public static double GetInWPercent(double w, double perc)
        {
			return (w / 100) * perc;
        }

        public static double GetInHPercent(double h, double perc)
        {
			return (h / 100) * perc;
        }


        public static double GetFontHeight(double fontsize)
        {
            return Device.OnPlatform(1.2,1.2,1.3) * fontsize;
        }

        public static double GetFontHalfHeight(double fontsize)
        {
            return Device.OnPlatform(1.2, 1.2, 1.3) * fontsize / 2;
        }

        internal static double GetWFromSize(double size, double numcar)
        {
            return size * 0.5 * numcar;
        }



        internal static double GetFontSizePerc(double w, string numcar, int perc = 70)
        {
            return (GetInPercent(w, perc)) / (0.5 * numcar.Length);
        }

        internal static double GetFontSizePerc(double w, int numcar, int perc = 70)
        {
            return (GetInPercent(w, perc)) / (0.5 * numcar);
        }


        public static double GetInPercent(double val, double perc)
        {
            return val / 100 * perc;
        }

        internal static double GetBtnWidth(string testo, NamedSize namedSize, double moltiplicator = 1.0)
        {
            var fs = Device.GetNamedSize(namedSize, typeof(Button));
            return 0.5 * fs * testo.Length * moltiplicator;
        }

        internal static double GetBtnWidth(string testo, double Size, double moltiplicator = 1.0)
        {
            return 0.5 * Size * testo.Length * moltiplicator;
        }

        internal static void FormatBtn(Button btn, bool isEnabled)
        {
            if (isEnabled)
            {
				btn.BackgroundColor = Color.Red;
                btn.TextColor = Color.White;
            }
            else
            {
                btn.BackgroundColor = Color.Transparent;
				btn.TextColor = Color.Black;
            }
        }

        internal static int GetCircleThickness(int level)
        {
            switch (level)
            {
                case 0:
                    return Device.OnPlatform(1, 3, 3);
                case 1:
                    return Device.OnPlatform(2, 4, 4);
                case 2:
                    return Device.OnPlatform(3, 7, 7);
                case 3:
                    return Device.OnPlatform(4, 9, 9);
                case 4:
                    return Device.OnPlatform(5, 11, 11);
                default:
                    return Device.OnPlatform(3, 7, 7);
            }
              
        }

        internal static void SetFont(Label l1)
        {
            l1.FontFamily = Device.OnPlatform("HelveticaNeue-Light", "sans-serif-light", "HelveticaNeue-Light");
        }



        internal static ActivityIndicator addActivityIndicator(RelativeLayout rl)
        {
            var loadingIndicator = new ActivityIndicator();
            rl.Children.Add(loadingIndicator,
            Constraint.RelativeToParent(p => MyUiUtils.GetInWPercent(p, 50)),
            Constraint.RelativeToParent(p => MyUiUtils.GetInHPercent(p, 50)));
            return loadingIndicator;
        }
    }
}
