using System;
using System.ComponentModel;
using uWatch.iOS;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using uWatch;

[assembly: ExportRenderer(typeof(HtmlLabel), typeof(HtmlLabelRenderer))]
namespace uWatch.iOS
{
	public class HtmlLabelRenderer: LabelRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
		{
			base.OnElementChanged(e);

			if (Control != null && Element != null && !string.IsNullOrWhiteSpace(Element.Text))
			{
				
				var attr = new NSAttributedStringDocumentAttributes();
				var nsError = new NSError();
				attr.DocumentType = NSDocumentType.HTML;
				string htmlContents =  Element.Text ;
				var myHtmlData = NSData.FromString(htmlContents, NSStringEncoding.Unicode);
				Control.Lines = 0;
				Control.AttributedText = new NSAttributedString(myHtmlData, attr, ref nsError);


			}
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == Label.TextProperty.PropertyName)
			{
				if (Control != null && Element != null && !string.IsNullOrWhiteSpace(Element.Text))
				{
					
					var attr = new NSAttributedStringDocumentAttributes();
					var nsError = new NSError();
					attr.DocumentType = NSDocumentType.HTML;
					string htmlContents =  Element.Text ;
					var myHtmlData = NSData.FromString(htmlContents, NSStringEncoding.Unicode);
					Control.Lines = 0;
					Control.AttributedText = new NSAttributedString(myHtmlData, attr, ref nsError);
				}
			}
		}
	}
}

