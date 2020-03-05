using System;
using Xamarin.Forms;

namespace uWatch
{
	public class CustomTextView :Entry
	{
		public string Icon { get; set; }

		public string Text { get; set; }

		// set the property of entry
		public static readonly BindableProperty PlaceholderTextColorProperty =
			BindableProperty.Create ("PlaceholderTextColor", typeof(Color), typeof(CustomTextView), Color.Default);

		public static readonly BindableProperty FontProperty =
			BindableProperty.Create ("Font", typeof(Font), typeof(CustomTextView), new Font ());

		public Color PlaceholderTextColor {
			get { return (Color)GetValue (PlaceholderTextColorProperty); }
			set { SetValue (PlaceholderTextColorProperty, value); }
		}

		public Font Font {
			get { return (Font)GetValue (FontProperty); }
			set { SetValue (FontProperty, value); }
		}
	
	}
}

