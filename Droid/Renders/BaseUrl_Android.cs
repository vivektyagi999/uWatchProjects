using System;
using Xamarin.Forms;

using uWatch;
using uWatch.Droid;

[assembly: Dependency (typeof (BaseUrl_Android))]
namespace uWatch.Droid 
{
	public class BaseUrl_Android : IBaseUrl 
	{
		public string Get () 
		{
			return "file:///android_asset/";
		}
	}
}