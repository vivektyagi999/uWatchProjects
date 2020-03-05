using System;
using Xamarin.Forms;

namespace uWatch
{
	public class CustomDatePicker : DatePicker
	{
		public string Icon { get; set; }

		public string Text { get; set; }

		//set the property of datepicker for text color and font
		public static readonly BindableProperty TitleTextColorProperty =
			BindableProperty.Create("TitleTextColor", typeof(Color), typeof(CustomPicker), Color.Default);

		public static readonly BindableProperty FontProperty =
			BindableProperty.Create("Font", typeof(Font), typeof(CustomPicker), new Font());


		public static readonly BindableProperty PlaceHolderTextProperty =
			BindableProperty.Create("PlaceHolderText", typeof(string), typeof(CustomEditor), "Enter Here");


		public string PlaceHolderText
		{
			get { return (string)GetValue(PlaceHolderTextProperty); }
			set { SetValue(PlaceHolderTextProperty, value); }
		}

		public Color TitleTextColor
		{
			get { return (Color)GetValue(TitleTextColorProperty); }
			set { SetValue(TitleTextColorProperty, value); }
		}

		public Font Font
		{
			get { return (Font)GetValue(FontProperty); }
			set { SetValue(FontProperty, value); }
		}


	}
}
