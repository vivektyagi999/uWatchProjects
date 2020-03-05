using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Plugin.Geolocator;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using UwatchPCL;

namespace uWatch
{
	public partial class MapPage : ContentPage
	{
		public double Latitudec, Longitudec;
		public MapPage(double Latitude,double Longitude)
		{
			Title = "Location Page";
			this.Latitudec = Latitude;
			this.Longitudec = Longitude;
		}
		protected override void OnAppearing()
		{
			ForMap(Latitudec, Longitudec);
		}
		public void ForMap(double Latitude, double Longitude)
		{

			var map = new Map(
			MapSpan.FromCenterAndRadius(new Position(Latitude, Longitude), Distance.FromMiles(0.3)))
			{
				IsShowingUser = true,
				HeightRequest = 100,
				WidthRequest = 960,
				VerticalOptions = LayoutOptions.FillAndExpand,
				MapType=MapType.Satellite
			};
			var position = new Position(Latitude, Longitude); 

			//Creating Pins to be shown in MyMap
			var pin = new Pin
			{
				Type = PinType.Place,
				Position = position,
				Label = "My Location",
				Address = ""
			};

			//Adding Pins in MyMap
			map.Pins.Add(pin);
			var stack = new StackLayout { Spacing = 0 };
			stack.Children.Add(map);
			Content = stack;
		}
		public async Task GetFocusOnCurrentLocation()
		{
			try
			{
				//For focusing the current location
				var locator = CrossGeolocator.Current;
				locator.DesiredAccuracy = 100;
				UserDialogs.Instance.ShowLoading("Getting your current location");
#if __ANDROID__
				 var currentPosition = await locator.GetPositionAsync(new TimeSpan(10000));
				MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Xamarin.Forms.Maps.Position(currentPosition.Latitude, currentPosition.Longitude), Distance.FromMiles(1)).WithZoom(20));
#elif __IOS__
				var currentPosition = await locator.GetPositionAsync(10000);
				MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(currentPosition.Latitude, currentPosition.Longitude), Distance.FromMiles(0)));
#endif


				UserDialogs.Instance.HideLoading();
			}
			catch { }
		}
	}
}
