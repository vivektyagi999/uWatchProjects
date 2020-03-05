using System.ComponentModel;
using Android.Graphics;
using Android.OS;
using Android.Views;
using uWatch;
using uWatch.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using AProgressBar = Android.Widget.ProgressBar;

[assembly: ExportRenderer(typeof(CustomActivityIndicator), typeof(ActivityIndicatorRenderer))]

namespace uWatch.Droid
{
	public class ActivityIndicatorRenderer : ViewRenderer<ActivityIndicator, AProgressBar>
	{
		public ActivityIndicatorRenderer()
		{
			AutoPackage = false;
		}

		protected override void OnElementChanged(ElementChangedEventArgs<ActivityIndicator> e)
		{
			base.OnElementChanged(e);

			AProgressBar progressBar = Control;
			if (progressBar == null)
			{
				SetNativeControl(progressBar);
			}

			UpdateVisibility();
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == ActivityIndicator.IsRunningProperty.PropertyName)
				UpdateVisibility();
			else if (e.PropertyName == ActivityIndicator.ColorProperty.PropertyName)
			{ }
		}
		void UpdateVisibility()
		{
			Control.Visibility = Element.IsRunning ? ViewStates.Visible : ViewStates.Invisible;
		}
	}
}