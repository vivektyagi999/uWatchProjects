using System;
using Android.Graphics;
using uWatch;
using uWatch.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using System.Text;

[assembly: ExportRenderer(typeof(CustomDatePicker), typeof(CustomDatePickerRenderer))]
namespace uWatch.Droid
{
	public class CustomDatePickerRenderer : DatePickerRenderer
	{

		public CustomDatePickerRenderer()
		{

		}

		//Android.Graphics.Drawables.Drawable s;
		object attrs;

		protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
		{
			base.OnElementChanged(e);

			var view = (CustomDatePicker)Element;
			Control.SetBackgroundColor(global::Android.Graphics.Color.White);
			this.Control.SetHintTextColor(global::Android.Graphics.Color.LightGray);
			Control.Text = view.PlaceHolderText;
	


			Control.SetTextColor(global::Android.Graphics.Color.Black);
			SetTitleTextColor(view);
			SetFont(view);
		}

		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			var view = (CustomDatePicker)Element;


			if (e.PropertyName == CustomDatePicker.TitleTextColorProperty.PropertyName)
				SetTitleTextColor(view);

			if (e.PropertyName == CustomDatePicker.FontProperty.PropertyName)
			{
				SetFont(view);
			}
			if (e.PropertyName == "Date")
			{

			}
			else {
				if (e.PropertyName == "IsFocused")
				{
                    if (Control.Text == "Purchase Date" || Control.Text == "Warranty Expiry" || Control.Text == "OverTime Date")
					{
						Control.Text = DateTime.Now.ToString("dd-MMM-yyy");
					}

                    if(Control.Text == "Date Of Birth")
                    {
                        Control.Text = DateTime.Now.AddYears(-13).ToString("dd-MMM-yyy");
                        MessagingCenter.Send<string>("DateSelect", "DateSelected");
                    }
				}
			}
		}
		//set the placeholder text color
		private void SetTitleTextColor(CustomDatePicker view)
		{
			if (view.TitleTextColor != Xamarin.Forms.Color.Default)
				Control.SetTextColor(view.TitleTextColor.ToAndroid());

		}
		//set the font of picker
		private void SetFont(CustomDatePicker view)
		{
			if (view.Font != Font.Default)
			{
				Control.TextSize = view.Font.ToScaledPixel();
				Control.Typeface = view.Font.ToTypeface();

				
			}
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
