
using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms.Platform.Android;
using Android.Graphics;
using uWatch;
using uWatch.Droid;

[assembly: ExportRenderer(typeof(CustomPicker), typeof(PickerRenderers))]
namespace uWatch.Droid
{
	public class PickerRenderers: PickerRenderer
	{
		public PickerRenderers()
		{
		}
		global::Android.Graphics.Drawables.Drawable s;
		object attrs;
		protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
		{
			base.OnElementChanged(e);

			var view = (CustomPicker)Element;
			Control.SetBackgroundColor(global::Android.Graphics.Color.White);
			//SetPlaceholderTextColor(view);

		}
		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			var view = (CustomPicker)Element;


			
		}
		
		protected override bool DrawChild(Canvas canvas, global::Android.Views.View child, long drawingTime)
		{
			var paint = new Paint { Color = global::Android.Graphics.Color.White, AntiAlias = true };

			paint.AntiAlias = true;
			paint.StrokeWidth = 3;
			paint.SetStyle(Paint.Style.FillAndStroke);
			paint.Color = global::Android.Graphics.Color.White;
			canvas.DrawRect(2, 15, this.Width - 2, 20, paint);
			
			return base.DrawChild(canvas, child, drawingTime);
		}
	}
}
