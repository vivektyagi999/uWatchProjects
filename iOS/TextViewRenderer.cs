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
	public class TextViewRenderer :EntryRenderer
	{
		public TextViewRenderer ()
		{
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged(e);

			if (Control != null)
			{

				#region set attributes of TextField


				CustomTextView cv = (CustomTextView)this.Element;
//				if (!string.IsNullOrEmpty(cv.Icon))
//				SetFont (cv);
				#endregion
			}
			Control.Layer.BorderWidth = 2;
			Control.Layer.CornerRadius = 0;
			Control.Layer.BorderColor = UIColor.FromRGB(242,242,242).CGColor;
			Control.Layer.BackgroundColor = UIColor.FromRGB(242,242,242).CGColor;
		
		
		}
		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			CustomTextView cv = (CustomTextView)this.Element;

			if (e.PropertyName == CustomTextView.FontProperty.PropertyName)
				SetFont(cv);
			
		}
		private void SetFont(CustomTextView view)
		{
			UIFont uiFont;
			if (view.Font != Font.Default && (uiFont = view.Font.ToUIFont ()) != null)
				Control.Font = uiFont;
			else if (view.Font == Font.Default) {
				
				Control.Font = UIFont.SystemFontOfSize(17f);
			}

		}
	}
}

