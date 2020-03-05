using System;
using System.Threading.Tasks;
using Android.Locations;
using FindLocally.Droid;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using uWatch.Droid;
using UwatchPCL;
using Acr.UserDialogs;

[assembly: Xamarin.Forms.Dependency(typeof(CurrentLocation))]
namespace uWatch.Droid
{
	public class CurrentLocation : ICurrentLocation
	{
		public  void GetCurrentLocation()
		{
			LocationApp.StartLocationService();
				LocationApp.Current.LocationServiceConnected += (object sender, ServiceConnectedEventArgs e) =>
						{

							// notifies us of location changes from the system
							LocationApp.Current.LocationService.LocationChanged += HandleLocationChanged;
							//notifies us of user changes to the location provider (ie the user disables or enables GPS)
							LocationApp.Current.LocationService.ProviderDisabled += HandleProviderDisabled;
							LocationApp.Current.LocationService.ProviderEnabled += HandleProviderEnabled;
							// notifies us of the changing status of a provider (ie GPS no longer available)
							LocationApp.Current.LocationService.StatusChanged += HandleStatusChanged;
						};
				
			

		}
		public void StopLocationServices()
		{
			try
			{
				LocationApp.StopLocationService();
			}
			catch (System.Exception ex)
			{
			}
		}
		#region Android Location Service methods

		///<summary>
		/// Updates UI with location data
		/// </summary>
		public void HandleLocationChanged(object sender, LocationChangedEventArgs e)
		{
			global::Android.Locations.Location location = e.Location;


			// these events are on a background thread, need to update on the UI thread


				LocationServices.Latitude = location.Latitude;
				LocationServices.Longitude = location.Longitude;




		}

		public void HandleProviderDisabled(object sender, ProviderDisabledEventArgs e)
		{
			//Log.Debug(logTag, "Location provider disabled event raised");
		}

		public void HandleProviderEnabled(object sender, ProviderEnabledEventArgs e)
		{
			//Log.Debug(logTag, "Location provider enabled event raised");
		}

		public void HandleStatusChanged(object sender, StatusChangedEventArgs e)
		{
			//Log.Debug(logTag, "Location status changed, event raised");
		}

		#endregion

	}
}
