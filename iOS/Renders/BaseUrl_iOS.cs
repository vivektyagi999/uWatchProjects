using System;
using Foundation;
using uWatch.iOS;
using Xamarin.Forms;

[assembly: Dependency(typeof(BaseUrl_iOS))]
namespace uWatch.iOS
{
	public class BaseUrl_iOS : IBaseUrl
	{
		public string Get()
		{
			return NSBundle.MainBundle.BundlePath;
		}
	}

}

