using System;
using Acr.UserDialogs;
using CoreLocation;
using Foundation;
using UIKit;
using UwatchPCL;
using Xamarin.Forms;

namespace uWatch.iOS
{
	
    public class LocationManager
    {
        protected CLLocationManager locMgr;
        // event for the location changing
        public event EventHandler<LocationUpdatedEventArgs> LocationUpdated = delegate { };

        public bool IsAuthorized
        {
            get { return CLLocationManager.Status == CLAuthorizationStatus.AuthorizedAlways; }
        }

        public LocationManager()
        {
            this.locMgr = new CLLocationManager();

            //ErrorLogs.WriteLogNative("Initialize LocationManager");
            LocMgr.AuthorizationChanged += (object sender, CLAuthorizationChangedEventArgs e) =>
            {
                if (CLLocationManager.Status == CLAuthorizationStatus.Authorized || CLLocationManager.Status == CLAuthorizationStatus.AuthorizedWhenInUse)
                {
                    //ErrorLogs.WriteLogNative("Allowing Location services");
                    StartLocationUpdates();

                }
                if (CLLocationManager.Status == CLAuthorizationStatus.Denied)
                {
                   
                }

            };

            this.locMgr.PausesLocationUpdatesAutomatically = false;

            // iOS 8 has additional permissions requirements
            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                locMgr.RequestAlwaysAuthorization(); // works in background
                locMgr.RequestWhenInUseAuthorization(); // only in foreground
                                                        //UIAlertView _error = new UIAlertView();
                                                        //_error.Title = "In Location";
                                                        //_error.Message = "In ";
                                                        //_error.AddButton("Ok");
                                                        //_error.Show();

            }
            if (UIDevice.CurrentDevice.CheckSystemVersion(9, 0))
            {
                locMgr.AllowsBackgroundLocationUpdates = false;

                //var locationManager = CLLocationManager();
            }

            LocationUpdated += PrintLocation;
        }

        // create a location manager to get system location updates to the application
        public CLLocationManager LocMgr
        {
            get { return this.locMgr; }
        }


        public void StartLocationUpdates()
        {
            if (CLLocationManager.LocationServicesEnabled)
            {


                if (CLLocationManager.Status == CLAuthorizationStatus.Authorized || CLLocationManager.Status == CLAuthorizationStatus.AuthorizedWhenInUse)
                {
                   // UserDialogs.Instance.ShowLoading("Fetching Location...");
                    LocMgr.DesiredAccuracy = 1; // sets the accuracy that we want in meters


                    if (UIDevice.CurrentDevice.CheckSystemVersion(6, 0))
                    {
                        try
                        {
                            LocMgr.LocationsUpdated += (object sender, CLLocationsUpdatedEventArgs e) =>
                            {
                                // fire our custom Location Updated event

                                this.LocationUpdated(this, new LocationUpdatedEventArgs(e.Locations[e.Locations.Length - 1]));

                            };
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    else
                    {
                        LocMgr.UpdatedLocation += (object sender, CLLocationUpdatedEventArgs e) =>
                        {
                            this.LocationUpdated(this, new LocationUpdatedEventArgs(e.NewLocation));

                        };

                    }

                    if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
                    {
                        //ErrorLogs.WriteLogNative("CheckSystemVersion(8, 0) is called");

                        LocMgr.RequestWhenInUseAuthorization();

                        //no idea

                    }
                    if (UIDevice.CurrentDevice.CheckSystemVersion(9, 0))
                    {
                        LocMgr.AllowsBackgroundLocationUpdates = false;



                    }
                    LocMgr.StartUpdatingLocation();


                    //LocMgr.LocationsUpdated += (sender, e) =>
                    //{
                    //    //CLLocation[] location = e.Locations;

                    //    //try
                    //    //{
                    //    //    var latitude = Convert.ToDouble(CrossSettings.Current.GetValueOrDefault("Latitude", "0.00"));
                    //    //    var longitude = Convert.ToDouble(CrossSettings.Current.GetValueOrDefault("Longitude", "0.00"));
                    //    //    var location2 = new { Location = latitude, Longitude = longitude };
                    //    //    if (Convert.ToDouble(location2.Longitude) == 0.00)
                    //    //    {
                    //    //        MessagingCenter.Send<App, object>(new App(), "UserCurrentLocation", location2);
                    //    //        CrossSettings.Current.AddOrUpdateValue("Latitude", location[0].Coordinate.Latitude);
                    //    //        CrossSettings.Current.AddOrUpdateValue("Longitude", location[0].Coordinate.Longitude);
                    //    //    }
                    //    //}
                    //    //catch (Exception ex)
                    //    //{

                    //    //}
                    //};





                    // Get some output from our manager in case of failure
                    LocMgr.Failed += (object sender, NSErrorEventArgs e) =>
                    {


                    };

                }
                else if (CLLocationManager.Status == CLAuthorizationStatus.Denied)
                {
                       UIAlertView _error = new UIAlertView();
                    _error.Title = "Location Permission";
                    _error.Message = "Please allow location permission to use features in FindLocally";
                    _error.AddButton("Settings");
                    _error.Clicked += (object s, UIButtonEventArgs ev) =>
                    {
                        var settingsString = UIKit.UIApplication.OpenSettingsUrlString;
                        var url = new NSUrl(settingsString);
                        UIApplication.SharedApplication.OpenUrl(url);

                    };
                    _error.Show();
                    UserDialogs.Instance.HideLoading();
                }
                else
                {
                    // StartLocationUpdates();
                    //LocMgr.LocationsUpdated += (sender, e) =>
                    //{
                    //    //CLLocation[] location = e.Locations;


                    //    //var latitude = Convert.ToDouble(CrossSettings.Current.GetValueOrDefault("Latitude", ""));
                    //    //var longitude = Convert.ToDouble(CrossSettings.Current.GetValueOrDefault("Longitude", ""));
                    //    //var location2 = new { Location = latitude, Longitude = longitude };
                    //    //MessagingCenter.Send<App, object>(new App(), "UserCurrentLocation", location2);
                    //    //CrossSettings.Current.AddOrUpdateValue("Latitude", location[0].Coordinate.Latitude);
                    //    //CrossSettings.Current.AddOrUpdateValue("Longitude", location[0].Coordinate.Longitude);

                    //};
                }


            }
            else
            {
                UIAlertView _error = new UIAlertView();
                _error.Title = "Location Services not enabled";
                _error.Message = "Please enable Location Services to use features in FindLocally";
                _error.AddButton("Settings");
                _error.Clicked += (object s, UIButtonEventArgs ev) =>
                {
                    var settingsString = UIKit.UIApplication.OpenSettingsUrlString;
                    var url = new NSUrl(settingsString);
                    UIApplication.SharedApplication.OpenUrl(url);
                };
                _error.Show();
                UserDialogs.Instance.HideLoading();
            }

        }
        public void PrintLocation(object sender, LocationUpdatedEventArgs e)
        {

            CLLocation location = e.Location;

            if (Convert.ToString(LocationServices.Latitude) == "0" && Convert.ToString(LocationServices.Longitude) == "0")
            {
                LocationServices.Latitude = location.Coordinate.Latitude;
                LocationServices.Longitude = location.Coordinate.Longitude;
            }
           // UserDialogs.Instance.HideLoading();
            //Console.WriteLine("Print Location received");
            //CLLocation location = e.Location;
            //LocationServices.Latitude = location.Coordinate.Latitude;
            //LocationServices.Longitude = location.Coordinate.Longitude;
            //LocMgr.StopUpdatingLocation();
            //MessagingCenter.Send<CurrentLocation>(new CurrentLocation(), "LocationServices");
         
        }
    
    }

}
