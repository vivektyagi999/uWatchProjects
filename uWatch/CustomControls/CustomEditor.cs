using System;
using Xamarin.Forms;

namespace uWatch
{
	public class CustomEditor : Editor
	{
		public string Icon { get; set; }

		public string Text { get; set; }

		public static readonly BindableProperty FontProperty =
			BindableProperty.Create("Font", typeof(Font), typeof(CustomEditor), new Font());

		//set the property of datepicker for text color and font
		public static readonly BindableProperty TitleTextColorProperty =
			BindableProperty.Create("TitleTextColor", typeof(Color), typeof(CustomEditor), Color.Default);

		public static readonly BindableProperty HasBorderProperty =
			BindableProperty.Create("HasBorder", typeof(bool), typeof(CustomEditor), false);

		public static readonly BindableProperty PlaceHolderTextColorProperty =
			BindableProperty.Create("PlaceHolderTextColor", typeof(Color), typeof(CustomEditor), Color.Default);

		public static readonly BindableProperty PlaceHolderTextProperty =
			BindableProperty.Create("PlaceHolderText", typeof(string), typeof(CustomEditor), "Enter Here");


		public string PlaceHolderText
		{
			get { return (string)GetValue(PlaceHolderTextProperty); }
			set { SetValue(PlaceHolderTextProperty, value); }
		}

		public bool HasBorder
		{
			get { return (bool)GetValue(HasBorderProperty); }
			set { SetValue(HasBorderProperty, value); }
		}

		public Color TitleTextColor
		{
			get { return (Color)GetValue(TitleTextColorProperty); }
			set { SetValue(TitleTextColorProperty, value); }
		}

		public Color PlaceHolderTextColor
		{
			get { return (Color)GetValue(PlaceHolderTextColorProperty); }
			set { SetValue(PlaceHolderTextColorProperty, value); }
		}

		public Font Font
		{
			get { return (Font)GetValue(FontProperty); }
			set { SetValue(FontProperty, value); }
		}

	}
}
