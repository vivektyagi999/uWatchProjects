using System;
using Xamarin.Forms;

namespace TimeOff247
{
	public class CustomPicker : Picker
	{
		public string Icon { get; set; }

		public string Text { get; set; }

		//set the property of datepicker for text color and font
		public static readonly BindableProperty TitleTextColorProperty =
			BindableProperty.Create ("TitleTextColor", typeof(Color), typeof(CustomPicker), Color.Default);
		
		public static readonly BindableProperty FontProperty =
			BindableProperty.Create ("Font", typeof(Font), typeof(CustomPicker), new Font ());
		

	

		public Color TitleTextColor {
			get { return (Color)GetValue (TitleTextColorProperty); }
			set { SetValue (TitleTextColorProperty, value); }
		}

		public Font Font {
			get { return (Font)GetValue (FontProperty); }
			set { SetValue (FontProperty, value); }
		}

	}
}

