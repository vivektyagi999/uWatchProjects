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
using System.Net;

[assembly: ExportRenderer(typeof(CustomWebView), typeof(WebViewRenderers))]
namespace uWatch.Droid
{
	public class WebViewRenderers : WebViewRenderer
	{

		public WebViewRenderers()
		{

		}
		//Android.Graphics.Drawables.Drawable s;
		object attrs;
		// set the property of webview for zoomin zoomout images
		protected override void OnElementChanged(ElementChangedEventArgs<WebView> e)
		{
			base.OnElementChanged(e);
			if (Control != null)
			{
				Control.Settings.BuiltInZoomControls = true;
				Control.Settings.DisplayZoomControls = false;
				
			}
		}

		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			var view = (CustomWebView)Element;

		}



	}
}


