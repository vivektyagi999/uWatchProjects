using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Views;
using Android.OS;
//using RomaxCubePCL;
using Plugin.Permissions;
using Android.Graphics;
using Xamarin.Forms.Platform.Android;
using Acr.UserDialogs;
using Android.Gms.Common;
using Android.Widget;
using Xamarin.Forms;
using Firebase.Messaging;
using Firebase.Iid;
using UwatchPCL.Helpers;
using System.IO;
using Uri = Android.Net.Uri;
using Android.Database;
using Android.Provider;
using System.Threading.Tasks;
using Android.Locations;
using Android.Gms.Common.Apis;
using Android.Gms.Location;
using Firebase;

namespace uWatch.Droid
{
    public static class AppClass
    {
        public static Java.IO.File _file;
        public static Java.IO.File _dir;
        public static Bitmap bitmap;
    }
	[Activity (Label = "uWatch", Icon = "@drawable/icon",Theme = "@style/MyTheme.Base", ScreenOrientation = ScreenOrientation.Portrait)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public bool IsCamera;
		protected override void OnStop()
		{
			base.OnStop();
            UwatchPCL.MyController.isAppClosed = true;
           
		}

		protected override void OnStart()
		{
			base.OnStart();
            UwatchPCL.MyController.isAppClosed = false;
		}

		private Action<int, Result, Intent> _activityResultCallback;
        public static MainActivity Instance { get; private set; }
        public const int REQUEST_LOCATION = 2;
        GoogleApiClient googleApiClient;

		protected override void OnCreate (Bundle bundle)
		{
			//Title = "Android";

			FormsAppCompatActivity.ToolbarResource = Resource.Layout.toolbar;
			FormsAppCompatActivity.TabLayoutResource = Resource.Layout.tabs;

			App.ScreenWidth = (int)(Resources.DisplayMetrics.WidthPixels / Resources.DisplayMetrics.Density);
			App.ScreenHeight = (int)(Resources.DisplayMetrics.HeightPixels / Resources.DisplayMetrics.Density);

			var tabtext = this.FindViewById<TextView>(Resource.Id.action_bar_title);
            Instance = this;
			base.OnCreate (bundle);
            StrictMode.VmPolicy.Builder builder = new StrictMode.VmPolicy.Builder();
            StrictMode.SetVmPolicy(builder.Build());
            global::Xamarin.Forms.Forms.Init (this, bundle);
            ZXing.Net.Mobile.Forms.Android.Platform.Init();
            Xamarin.Essentials.Platform.Init(this, bundle);
            Xamarin.FormsMaps.Init(this, bundle);
            UserDialogs.Init(() => (Activity)Forms.Context);

            Rg.Plugins.Popup.Popup.Init(this, bundle);

            UwatchPCL.MyController.ScreenWidth = (int)(Resources.DisplayMetrics.WidthPixels / Resources.DisplayMetrics.Density);
            UwatchPCL.MyController.ScreenHeight = (int)(Resources.DisplayMetrics.HeightPixels / Resources.DisplayMetrics.Density);

            FirebaseApp app = FirebaseApp.InitializeApp(this);

            if (Build.VERSION.SdkInt >= Build.VERSION_CODES.Lollipop)
			{
				Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
				Window.SetStatusBarColor(global::Android.Graphics.Color.Red);
			}

			IsPlayServicesAvailable();

            DependencyService.Register<UwatchPCL.IAdapter, Adapter>();
			LoadApplication (new App ());
		}

		public bool IsPlayServicesAvailable()
		{
			int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
			if (resultCode != ConnectionResult.Success)
			{
				if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
				{ 
				} //msgText.Text = GoogleApiAvailability.Instance.GetErrorString(resultCode);
				else
				{
					//msgText.Text = "This device is not supported";
					Finish();
				}
				return false;
			}
			else
			{
				//msgText.Text = "Google Play Services is available.";
				return true;
			}
		}
       
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            IsCamera = InventoryDetails.IsCamera;

            if (requestCode==2)
            {
                
                    switch (resultCode)
                    {
                        case Result.Canceled:
                            {
                                Toast.MakeText(this, "Gps is not enabled", ToastLength.Short).Show();
                                break;
                            }
                        case Result.Ok:
                            {
                                Toast.MakeText(this, "Gps is enabled", ToastLength.Short).Show();
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                    
            }
            else if (IsCamera == true)
            {
                Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
                Uri contentUri = Uri.FromFile(AppClass._file);
                mediaScanIntent.SetData(contentUri);
                SendBroadcast(mediaScanIntent);

                //int height = Resources.DisplayMetrics.HeightPixels;
                int width = Resources.DisplayMetrics.WidthPixels;

                AppClass.bitmap = AppClass._file.Path.LoadAndResizeBitmap(300, 300);


                byte[] bitmapData = new byte[0];

                if (AppClass.bitmap != null)
                {
                    using (var stream = new MemoryStream())
                    {
                        AppClass.bitmap.Compress(Bitmap.CompressFormat.Png, 50, stream);
                        bitmapData = stream.ToArray();
                    }

                    AppClass.bitmap = null;
                }

                GC.Collect();

                InventoryDetails.CameraPreview = bitmapData;
                MessagingCenter.Send<string>("CameraImage", "CameraPreview");

            }
            else
            {
                if (requestCode == 1)
                {
                    if (resultCode == Result.Ok)
                    {
                        if (data.Data != null)
                        {
                            global::Android.Net.Uri uri = data.Data;

                            int orientation = getOrientation(uri);
                            BitmapWorkerTask task = new BitmapWorkerTask(this.ContentResolver, uri);
                            task.Execute(orientation);
                        }
                    }
                }
            }
            base.OnActivityResult(requestCode, resultCode, data);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            try
            {
                Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
                PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            }
            catch
            {

            }
        }
        public int getOrientation(global::Android.Net.Uri photoUri)
        {
            ICursor cursor = Application.ApplicationContext.ContentResolver.Query(photoUri, new String[] { MediaStore.Images.ImageColumns.Orientation }, null, null, null);

            if (cursor.Count != 1)
            {
                return -1;
            }

            cursor.MoveToFirst();
            return cursor.GetInt(0);
        }
        public void PushNotificationClear()
        {
            NotificationManager notificationManager = GetSystemService(Context.NotificationService) as NotificationManager;
            notificationManager.CancelAll();
        }

        #region GpsEnabled Functionality

        public async Task<bool> NoLocation()
        {
            LocationManager manager = global::Android.App.Application.Context.GetSystemService("location") as LocationManager;
            if (!manager.IsProviderEnabled(LocationManager.GpsProvider))
            {
                EnableLoc();
                return false;
            }
            return true;
        }

        private void EnableLoc()
        {
            try
            {
                googleApiClient = new GoogleApiClient.Builder(global::Android.App.Application.Context)
                   .AddApi(LocationServices.API).Build();
                googleApiClient.Connect();
                LocationRequest locationRequest = LocationRequest.Create();
                locationRequest.SetPriority(LocationRequest.PriorityHighAccuracy);
                locationRequest.SetInterval(30 * 1000);
                locationRequest.SetFastestInterval(5 * 1000);
                LocationSettingsRequest.Builder builder = new LocationSettingsRequest.Builder()
                    .AddLocationRequest(locationRequest);
                builder.SetAlwaysShow(true);
                builder.SetNeedBle(true);
                var result = LocationServices.SettingsApi.CheckLocationSettings(googleApiClient, builder.Build());
                IPendingResultExtensions.SetResultCallback<LocationSettingsResult>(result, OnSetResult);
            }
            catch (Exception ex)
            {

            }
        }

        private void OnSetResult(LocationSettingsResult result)
        {

            var status = result.Status;
            switch (status.StatusCode)
            {
                case LocationSettingsStatusCodes.ResolutionRequired:
                    try
                    {
                        status.StartResolutionForResult(this, REQUEST_LOCATION);
                    }
                    catch (IntentSender.SendIntentException ex)
                    {

                    }
                    break;
                default:
                    Console.WriteLine("Default case");
                    break;
            }

        }

      

        public void OnConnected(Bundle connectionHint)
        {

        }

        public void OnConnectionSuspended(int cause)
        {
            Console.WriteLine(cause);
        }

        public void OnConnectionFailed(ConnectionResult result)
        {
            Console.WriteLine(result.ErrorCode);
        }

        
        #endregion

    }
}

