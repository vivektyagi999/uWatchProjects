
using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms.Platform.Android;
using Android.Graphics;
using uWatch;
using uWatch.Droid;
using Android.Graphics.Drawables;
using Android;

[assembly: ExportRenderer(typeof(RoundedButton), typeof(RoundedButtonRenderer))]
namespace uWatch.Droid
{
	public class RoundedButtonRenderer : ButtonRenderer
	{
		private GradientDrawable _normal, _pressed;

		protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
		{
			base.OnElementChanged(e);

			if (Control != null)
			{
				var button = (RoundedButton)e.NewElement;
				button.HeightRequest = 38;
				button.WidthRequest = 38;
				button.FontSize = 12;
			
			}
		}
		protected override bool DrawChild(Canvas canvas, global::Android.Views.View child, long drawingTime)
		{
			try
			{
				var radius = Math.Min(Width, Height) / 2;
				
				radius -= 5;
				//Create path to clip
				var path = new Path();
				path.AddCircle(Width / 2, Height / 2, radius, Path.Direction.Ccw);
				canvas.Save();
				canvas.ClipPath(path);

				var result = base.DrawChild(canvas, child, drawingTime);

				canvas.Restore();

				// Create path for circle border
				path = new Path();
				path.AddCircle(Width / 2, Height / 2, radius, Path.Direction.Ccw);

				var paint = new Paint();
				paint.AntiAlias = true;
				//paint.StrokeWidth = 5;
				paint.SetStyle(Paint.Style.Stroke);
				paint.Color = global::Android.Graphics.Color.Gray;

				canvas.DrawPath(path, paint);

				//Properly dispose
				paint.Dispose();
				path.Dispose();
				return result;
			}
			catch (Exception ex)
			{

			}

			return base.DrawChild(canvas, child, drawingTime);
		}
	}
}