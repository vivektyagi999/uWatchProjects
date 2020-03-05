using System;
using System.ComponentModel;
using Android.Text;
using Android.Widget;
using uWatch;
using uWatch.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(HtmlLabel), typeof(HtmlLabelRenderer))]
namespace uWatch.Droid
{
	public class HtmlLabelRenderer: LabelRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
		{
			base.OnElementChanged(e);
			string htmlContents = Element.Text ;
			Control?.SetText(Html.FromHtml(htmlContents), TextView.BufferType.Spannable);
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == Label.TextProperty.PropertyName)
			{
				string htmlContents =  Element.Text ;
				Control?.SetText(Html.FromHtml(htmlContents), TextView.BufferType.Spannable);
			}
		}

	}
}

