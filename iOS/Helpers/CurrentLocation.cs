using System;
using uWatch.iOS;

[assembly: Xamarin.Forms.Dependency(typeof(CurrentLocation))]
namespace uWatch.iOS
{
	
	public class CurrentLocation:ICurrentLocation
	{
		public static LocationManager Manager = null;
		public void GetCurrentLocation()
		{
			try
			{
				Manager = new LocationManager();

				Manager.StartLocationUpdates();
			}
			catch (System.Exception ex)
			{
			}
		}
		public void StopLocationServices()
		{
			try
			{
			}
			catch (System.Exception ex)
			{
			}
		}
	}
}
