using System;
using System.ComponentModel;
using CoreGraphics;
using Foundation;
using UIKit;
using uWatch.Controls;
using uWatch;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(CustomWebView), typeof(CustomWebViewRenderer))]
namespace XLabs.Forms.Controls
{
	/// <summary>
	/// A renderer for the CustomWebView control.
	/// </summary>
	public class CustomWebViewRenderer : WebViewRenderer
	{

		protected override void OnElementChanged(VisualElementChangedEventArgs e)
		{
			base.OnElementChanged(e);
			var view = (UIWebView)NativeView;
			view.ScrollView.ScrollEnabled = true;
			view.ScalesPageToFit = true;
		}

	}
}