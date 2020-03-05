using System;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundation;
using UIKit;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using CoreGraphics;
using System.ComponentModel;
using uWatch;
using uWatch.iOS;

[assembly: ExportRenderer(typeof(CustomTextView), typeof(TextViewRenderer))]
namespace uWatch.iOS
{
	public class TextViewRenderer: EntryRenderer
	{
		public TextViewRenderer()
		{
		}
		protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged(e);

			if (Control != null)
			{
				CustomTextView cv;

				#region set attributes of TextField
				if (e.NewElement == null)
				{
					cv = (CustomTextView)e.OldElement;
				}
				else
				{

					cv = (CustomTextView)this.Element;
				}
                if(cv.IsFocus)
                {
                    Control.BecomeFirstResponder();
                }

				if (!string.IsNullOrEmpty(cv.Icon))
					SetFont(cv);
				SetPlaceholderTextColor(cv);
				#endregion
			}
			Control.Layer.BorderWidth = 2;
			Control.Layer.CornerRadius = 0;
			Control.Layer.BorderColor = UIColor.FromRGB(255, 255, 255).CGColor;


		}
		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			CustomTextView cv = (CustomTextView)this.Element;

			if (e.PropertyName == CustomTextView.FontProperty.PropertyName)
				SetFont(cv);
			if (e.PropertyName == CustomTextView.PlaceholderTextColorProperty.PropertyName)
				SetPlaceholderTextColor(cv);

		}
		private void SetFont(CustomTextView view)
		{
			UIFont uiFont;
			if (view.Font != Font.Default && (uiFont = view.Font.ToUIFont()) != null)
				Control.Font = uiFont;
			else if (view.Font == Font.Default)
			{

				Control.Font = UIFont.SystemFontOfSize(17f);
			}

		}

		void SetPlaceholderTextColor(CustomTextView view)
		{

			if (string.IsNullOrEmpty(view.Placeholder) == false && view.PlaceholderTextColor != Color.Default)
			{
				NSAttributedString placeholderString = new NSAttributedString(view.Placeholder, new UIStringAttributes() { ForegroundColor = view.PlaceholderTextColor.ToUIColor() });
				Control.AttributedPlaceholder = placeholderString;
			}
		}
	}
}
