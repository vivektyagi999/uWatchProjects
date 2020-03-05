using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms.Platform.Android;
using Android.Widget;
using Android.Graphics;
using uWatch;
using uWatch.Droid;
using Android.Views.InputMethods;

[assembly: ExportRenderer(typeof(CustomTextView), typeof(TextViewRenderer))]
namespace uWatch.Droid
{
	public class TextViewRenderer: EntryRenderer
	{
		public TextViewRenderer()
		{
		}
		global::Android.Graphics.Drawables.Drawable s;
		object attrs;

		protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged(e);

			var view = (CustomTextView)Element;
            if (view.IsFocus)
            {
                Control.RequestFocus();
                InputMethodManager inputMethodManager = global::Android.App.Application.Context.GetSystemService(global::Android.App.Application.InputMethodService) as InputMethodManager;
                inputMethodManager.ShowSoftInput(Control, ShowFlags.Forced);
                inputMethodManager.ToggleSoftInput(ShowFlags.Forced, HideSoftInputFlags.ImplicitOnly);
               
            }
			Control.SetBackgroundColor(global::Android.Graphics.Color.White);
			SetPlaceholderTextColor(view);
			SetFont(view);


		}
		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			var view = (CustomTextView)Element;

			if (e.PropertyName == CustomTextView.FontProperty.PropertyName)
			{
				SetFont(view);
			}

			if (e.PropertyName == CustomTextView.PlaceholderTextColorProperty.PropertyName)
				SetPlaceholderTextColor(view);
		}
		private void SetPlaceholderTextColor(CustomTextView view)
		{
			if (view.PlaceholderTextColor != Xamarin.Forms.Color.Default)
				Control.SetHintTextColor(view.PlaceholderTextColor.ToAndroid());
		}
		// set the font of entry
		private void SetFont(CustomTextView view)
		{
			if (view.Font != Font.Default)
			{
				Control.TextSize = view.Font.ToScaledPixel();
				Control.Typeface = view.Font.ToTypeface();


				Typeface font = Typeface.CreateFromAsset(Forms.Context.Assets, "");
				Control.Typeface = font;
			}
		}
		// draw the rect for entry
		protected override bool DrawChild(Canvas canvas, global::Android.Views.View child, long drawingTime)
		{
			var paint = new Paint { Color = global::Android.Graphics.Color.White, AntiAlias = true };

			paint.AntiAlias = true;
			paint.StrokeWidth = 3;
			paint.SetStyle(Paint.Style.FillAndStroke);
			paint.Color = global::Android.Graphics.Color.White;
		

			if (Xamarin.Forms.Device.Idiom == TargetIdiom.Phone)
			{
				canvas.DrawRect(2, 10, this.Width - 2, 80, paint);
				Bitmap _bit = BitmapFactory.DecodeResource(Resources, Resource.Drawable.icon);
				canvas.DrawBitmap(_bit, 100, 200, null);

			}
			if (Xamarin.Forms.Device.Idiom == TargetIdiom.Tablet)
			{
				canvas.DrawRect(2, -2, this.Width - 2, 150, paint);
				Bitmap _bit = BitmapFactory.DecodeResource(Resources, Resource.Drawable.icon);
				canvas.DrawBitmap(_bit, 100, 200, null);

			};
			return base.DrawChild(canvas, child, drawingTime);
		}
	}
}
