using System;
using Android.App;
using Android.Content;
using Android.OS;
using uWatch;
using uWatch.Droid;
using Xamarin.Forms;



[assembly: Xamarin.Forms.Dependency(typeof(AndroidMethods))]
namespace uWatch.Droid
{
	public class AndroidMethods: IAndroidMethods
	{
		public void CloseApp()
		{
			Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
			{
				Process.KillProcess(Process.MyPid());
			});
		}
	}
}
