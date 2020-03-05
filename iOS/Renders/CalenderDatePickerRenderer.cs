using System;
using uWatch;
using uWatch.iOS;
using Xamarin.Forms;
using UIKit;
using Xamarin.Forms.Platform.iOS;
using System.ComponentModel;

[assembly: ExportRenderer(typeof(CustomDatePicker), typeof(CalenderDatePickerRenderer))]
namespace uWatch.iOS
{
	public class CalenderDatePickerRenderer : DatePickerRenderer
	{
		UILabel labelPlaceHolder;
		UITextView replacingControl;
		private string Placeholder { get; set; }

		public CalenderDatePickerRenderer()
		{
		}
		protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
		{
			base.OnElementChanged(e);

			if (Control != null)
			{
                //this.Control.BackgroundColor = UIColor.LightTextColor;
                //this.Control.Layer.CornerRadius = 5;
                //this.Control.Layer.MasksToBounds = true;
              
                #region set attributes of PickerRendererdField

				CustomDatePicker cv = (CustomDatePicker)this.Element;
				if (cv != null)
				{
					cv.BackgroundColor = Color.Transparent;
					Control.TextColor = UIColor.Black;
					Control.ReturnKeyType = UIReturnKeyType.Default;
					SetFont(cv);
				}
				#endregion


				//SetBorder (cv);
				//SetPlaceholderTextColor (cv);

			}
			replacingControl = new UITextView(Control.Bounds);
			//var adelegate = new myTextViewDelegate();
			var element = this.Element as CustomDatePicker;
			if (Control != null && element != null)
			{
				Placeholder = element.PlaceHolderText;
				Control.TextColor = UIColor.LightGray;
				Control.Text = Placeholder;
				Control.ShouldBeginEditing += (UITextField textView) =>
				{
					if (textView.Text == Placeholder)
					{
						textView.Text = DateTime.Now.AddYears(-13).ToString("dd-MMM-yyy");
						textView.TextColor = UIColor.Black; // Text Color
                        MessagingCenter.Send<string>("DateSelect", "DateSelected");
					}

					return true;
				};

				Control.ShouldEndEditing += (UITextField textView) =>
				{
					if (textView.Text == "")
					{
						textView.Text = Placeholder;
						textView.TextColor = UIColor.LightGray; // Placeholder Color
					}

					return true;
				};
			}


			Control.TextColor = UIColor.Black;
            Control.Layer.BorderWidth = new nfloat(0.25);
			Control.Layer.CornerRadius = 5;
			//Control.Font = FontAttributes.Bold;
			Control.Layer.BorderColor = UIColor.LightGray.CGColor;
			Control.ClipsToBounds = true;

		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			var view = (CustomDatePicker)Element;

			if (e.PropertyName == CustomDatePicker.FontProperty.PropertyName)
				SetFont(view);

			if (e.PropertyName == CustomDatePicker.TitleTextColorProperty.PropertyName)
			{
			}//SetPlaceholderTextColor(view);

		}


		private void SetFont(CustomDatePicker view)
		{
			UIFont uiFont;
			if (view.Font != Font.Default && (uiFont = view.Font.ToUIFont()) != null)
				Control.Font = uiFont;
			else if (view.Font == Font.Default)
				Control.Font = UIFont.SystemFontOfSize(17f);

		}


	}
}
