using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using FFImageLoading.Forms;
using UwatchPCL;
using UwatchPCL.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Maps;


#if __ANDROID__
using Android.App;
using Android.Widget;
using uWatch.Droid;

#endif

namespace uWatch
{
	public partial class AlertDetailsForAgent : ContentPage
	{
		AlertsEsclatedToAgentViewModel Alert;
		Xamarin.Forms.RelativeLayout relativeLayout;
		Xamarin.Forms.ScrollView scrollview;
		public Label lblCountdownValue;
		double w = MyController.VirtualWidth;
		double h = MyController.VirtualHeight;
		bool noimage = false;
		bool noMap = false;
		Geocoder geoCoder;
		AlertsEsclatedToAgentViewModel SendToActionPage;
		int GlobleDevceId;

		public AlertDetailsViewModel ViewModel { get; set; }

		public AlertDetailsForAgent(string str = "")
		{

			try
			{
				if (!string.IsNullOrEmpty(str))
				{
					var lblMsg = new Label { Text = str, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand, FontSize = 15 };
					Content = lblMsg;
				}
				else {

					geoCoder = new Geocoder();
					var alertid = MyController.AlertId;
					AlertsEsclatedToAgentViewModel _AlertsEsclatedToAgentViewModel = new AlertsEsclatedToAgentViewModel();
					_AlertsEsclatedToAgentViewModel.alertlog_idx = alertid == "" ? 0 : Convert.ToInt32(alertid);
					var alert = ApiService.Instance.GetAlert(_AlertsEsclatedToAgentViewModel).Result;
					this.Alert = alert;
					ViewModel = new AlertDetailsViewModel(this.Alert);
					Timer();

					SetLayout();
				}
			}
			catch (Exception ex)
			{
				MyController.ErrorManagement(ex.Message);
			}
		}
		public AlertDetailsForAgent(AlertsEsclatedToAgentViewModel alert)
		{
			try
			{

				var lblloading = new Label { Text = "Loading...", VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.CenterAndExpand };
				Content = lblloading;

				SendToActionPage = alert;
				this.Alert = alert;
				geoCoder = new Geocoder();
				ViewModel = new AlertDetailsViewModel(this.Alert);
				Timer();
				BindingContext = ViewModel.device;
				SetLayout();
			}
			catch (Exception ex)
			{
				MyController.ErrorManagement(ex.Message);
			}
		}
		protected async override void OnAppearing()
		{
			base.OnAppearing();
		}

		protected override void OnDisappearing()
		{
			try
			{
				System.GC.Collect();
				base.OnDisappearing();
			}
			catch { }
		}
		private async void SetLayout()
		{
			try
			{
				Title = "Alert Details";
				relativeLayout = new Xamarin.Forms.RelativeLayout();
				AddLayout();
				scrollview = new Xamarin.Forms.ScrollView();
				scrollview.Content = relativeLayout;
				Content = scrollview;
			}
			catch { }
		}
		private async void AddLayout()
		{

			try
			{

				double position = 0;
				double newx20 = MyUiUtils.getPercentual(w, 20);
				double newx40 = MyUiUtils.getPercentual(w, 40);
				double newx60 = MyUiUtils.getPercentual(w, 60);
				double newx80 = MyUiUtils.getPercentual(w, 80);

				StackLayout layoutstack = new StackLayout { VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand };
				StackLayout mainstack = new StackLayout { };
				mainstack.Padding = new Thickness(12, 0, 12, 10);


				StackLayout mapstack = new StackLayout { VerticalOptions = LayoutOptions.EndAndExpand, HorizontalOptions = LayoutOptions.EndAndExpand };
				StackLayout alertimagestack = new StackLayout { VerticalOptions = LayoutOptions.Start, HorizontalOptions = LayoutOptions.Start };
				StackLayout mapandimagestack = new StackLayout { Orientation = StackOrientation.Horizontal, Spacing = 2, HorizontalOptions = LayoutOptions.FillAndExpand };
				mapandimagestack.Children.Add(alertimagestack);
				mapandimagestack.Children.Add(mapstack);

				StackLayout buttonstack = new StackLayout { Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.EndAndExpand, Padding = new Thickness(12, 0, 12, 10) };
				StackLayout headstack = new StackLayout() { BackgroundColor = Color.FromRgb(244, 244, 244), Padding = new Thickness(0, 6, 0, 6) };
				headstack.Orientation = StackOrientation.Horizontal;

				StackLayout Substack = new StackLayout() { Padding = new Thickness(0, 10, 0, 10) };
				Substack.Orientation = StackOrientation.Horizontal;
				Substack.HorizontalOptions = LayoutOptions.CenterAndExpand;
				Substack.VerticalOptions = LayoutOptions.FillAndExpand;

				var lblAlertText1 = new Label { Text = "!!", FontSize = 19, FontAttributes = FontAttributes.Bold, TextColor = Color.Red, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };
				var imgAlerts = new Image { HeightRequest = 20, WidthRequest = 20, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };
				if (ViewModel != null)
				{
					if (ViewModel.device != null)
						imgAlerts.Source = await MyController.GetAlertTypelImage(ViewModel.device.alert_type);
				}

				var lblAlertText = new Label { FontSize = 19, FontAttributes = FontAttributes.Bold, TextColor = Color.Red, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };
				if (ViewModel.device != null)
				{
					if (MyController.GetAlertTypeName(ViewModel.device.alert_type).ToUpper() == "BLUETOOTH")
					{
						if (ViewModel.device.SlaveFriendlyName.ToString() != null || ViewModel.device.SlaveFriendlyName.ToString() == "")
						{
							lblAlertText.Text = ViewModel.device.SlaveFriendlyName.ToString().ToUpper().TrimStart().TrimEnd() + " !! ";
						}
						else
						{
							lblAlertText.Text = MyController.GetAlertTypeName(ViewModel.device.alert_type).ToUpper() + " !! ";
						}
					}
					else
					{
						lblAlertText.Text = MyController.GetAlertTypeName(ViewModel.device.alert_type).ToUpper() + " !! ";
					}
				}

				Substack.Children.Add(lblAlertText1);
				Substack.Children.Add(imgAlerts);
				Substack.Children.Add(lblAlertText);

				headstack.Children.Add(Substack);
				var lblFirndlyName = new Label();
				lblFirndlyName.Text = SendToActionPage.FriendlyName;
				lblFirndlyName.HorizontalTextAlignment = TextAlignment.Start;
				lblFirndlyName.HorizontalOptions = LayoutOptions.FillAndExpand;
				lblFirndlyName.VerticalOptions = LayoutOptions.StartAndExpand;

				var lblDateText = new Label();
				lblDateText.Text = "Alert Date";
				lblDateText.HorizontalTextAlignment = TextAlignment.Start;
				lblDateText.HorizontalOptions = LayoutOptions.FillAndExpand;
				lblDateText.FontAttributes = FontAttributes.Bold;
				lblDateText.VerticalOptions = LayoutOptions.StartAndExpand;

				var lblDateValue = new Label();
				lblDateValue.Text = DateFormat.GetDateTime(SendToActionPage.DeviceDate, TimeType.DateAndTime);
				lblDateValue.HorizontalTextAlignment = TextAlignment.Start;
				lblDateValue.HorizontalOptions = LayoutOptions.FillAndExpand;
				lblDateValue.VerticalOptions = LayoutOptions.StartAndExpand;


				var addressstacklayout = new StackLayout { };

				var lblAddressText = new Label();
				lblAddressText.Text = "Owner's Address";
				lblAddressText.HorizontalTextAlignment = TextAlignment.Start;
				lblAddressText.FontAttributes = FontAttributes.Bold;
				lblAddressText.HorizontalOptions = LayoutOptions.FillAndExpand;
				lblAddressText.VerticalOptions = LayoutOptions.StartAndExpand;

				var lblAddressline1 = new Label();
				lblAddressline1.Text = SendToActionPage.AddressLine1;
				lblAddressline1.HorizontalTextAlignment = TextAlignment.Start;
				lblAddressline1.HorizontalOptions = LayoutOptions.FillAndExpand;
				lblAddressline1.VerticalOptions = LayoutOptions.StartAndExpand;

				var lblAddressline2 = new Label();
				lblAddressline2.Text = SendToActionPage.AddressLine2;
				lblAddressline2.HorizontalTextAlignment = TextAlignment.Start;
				lblAddressline2.HorizontalOptions = LayoutOptions.FillAndExpand;
				lblAddressline2.VerticalOptions = LayoutOptions.StartAndExpand;


				addressstacklayout.Children.Add(lblFirndlyName);
				addressstacklayout.Children.Add(lblDateText);
				addressstacklayout.Children.Add(lblDateValue);
				addressstacklayout.Children.Add(lblAddressText);
				addressstacklayout.Children.Add(lblAddressline1);
				addressstacklayout.Children.Add(lblAddressline2);

				var contactlayout = new StackLayout { };

				var lblcontactText = new Label();
				lblcontactText.Text = "Contact: " + SendToActionPage.Mobile1;
				lblcontactText.HorizontalTextAlignment = TextAlignment.Start;
				lblcontactText.HorizontalOptions = LayoutOptions.FillAndExpand;
				lblcontactText.VerticalOptions = LayoutOptions.StartAndExpand;


				contactlayout.Children.Add(lblcontactText);

				var lblAlertDateTime = new Label();
				lblAlertDateTime.Text = SendToActionPage.AlertDateTime;
				lblAlertDateTime.HorizontalTextAlignment = TextAlignment.Start;
				lblAlertDateTime.HorizontalOptions = LayoutOptions.FillAndExpand;
				lblAlertDateTime.VerticalOptions = LayoutOptions.StartAndExpand;

				var imgAlert = new CachedImage()
				{
					WidthRequest = w / 2 - 50,
					HeightRequest = w / 2,
					CacheDuration = TimeSpan.FromDays(30),
					DownsampleToViewSize = true,
					RetryCount = 300,
					RetryDelay = 250,
					Aspect = Aspect.Fill,
					TransparencyEnabled = false,
					LoadingPlaceholder = ImageSource.FromFile("noimage.gif"),
					ErrorPlaceholder = ImageSource.FromFile("noimage.gif"),
				};

				alertimagestack.Children.Add(imgAlert);
				if (ViewModel != null)
				{
					if (ViewModel.AlertImage != null)
					{
						if (ViewModel.AlertImage.Image != null)
						{
							imgAlert.Source = BytesArraytoImage(ViewModel.AlertImage.Image).Source as StreamImageSource;
						}
						else
						{
							noimage = true;
							imgAlert.Source = ImageSource.FromFile("noimage.gif");
						}
					}
					else
					{
						noimage = true;
						imgAlert.Source = ImageSource.FromFile("noimage.gif");
					}
				}

				TapGestureRecognizer bigt = new TapGestureRecognizer();
				bigt.Tapped += async (object sender, EventArgs e) =>
				{
					if (!noimage)
					{
						UserDialogs.Instance.ShowLoading("Please wait...", MaskType.Gradient);

						try
						{
							ContentPage p = new ContentPage();
							StackLayout st = new StackLayout { Padding = 10, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
							Xamarin.Forms.RelativeLayout r = new Xamarin.Forms.RelativeLayout();
							CachedImage i = new CachedImage { Aspect = Aspect.Fill, LoadingPlaceholder = ImageSource.FromFile("placeholder.png"), ErrorPlaceholder = ImageSource.FromFile("noimage.gif"), HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
							i.Source = imgAlert.Source;
							i.Aspect = Aspect.AspectFit;
							Xamarin.Forms.AbsoluteLayout a = new Xamarin.Forms.AbsoluteLayout();
							Label l = new Label { TextColor = Color.Black, FontSize = 15 };
							a.Children.Add(l);
							MyUILibrary.AddView(r, a, w - 80, 50, w, 60);
							st.Children.Add(i);

							p.Title = "Alert Image";
							p.Content = st;

							await Navigation.PushAsync(p);

						}
						catch
						{
						}
						await System.Threading.Tasks.Task.Delay(50);
						UserDialogs.Instance.HideLoading();
					}
				};
				imgAlert.GestureRecognizers.Add(bigt);

				var customMap = new Map
				{
					MapType = MapType.Satellite,
					WidthRequest = w / 3,
					HeightRequest = w / 2,
				};
				var pin = new Pin
				{
					Type = PinType.Place,
					Position = ViewModel.AlertPosition,
					Label = ""

				};
				var pos = ViewModel.AlertPosition;




				if (Xamarin.Forms.Device.Idiom == TargetIdiom.Phone)
				{
					var possibleAddresses = await geoCoder.GetAddressesForPositionAsync(pos);
					foreach (var address in possibleAddresses)
					{
						pin.Address += address + "\n";
						pin.Label = pos.Latitude.ToString() + "," + pos.Longitude.ToString();
					}
				}
				customMap.Pins.Add(pin);
				customMap.MoveToRegion(MapSpan.FromCenterAndRadius(pos, Distance.FromMiles(1.0)));
				TapGestureRecognizer bigMap = new TapGestureRecognizer();
				bigMap.Tapped += async (object sender, EventArgs e) =>
				{
					if (!noMap)
					{
						try
						{
							UserDialogs.Instance.ShowLoading("Loading...");
							ContentPage p = new ContentPage();
							StackLayout st = new StackLayout();
							var map = new Map
							{
								MapType = MapType.Satellite,
								WidthRequest = w,
								HeightRequest = h - 80,
							};
							var pinbig = new Pin
							{
								Type = PinType.Place,
								Position = ViewModel.AlertPosition

							};
							var posi = ViewModel.AlertPosition;
							var Addresses = await geoCoder.GetAddressesForPositionAsync(posi);
							foreach (var address in Addresses)
							{
								pinbig.Address += address + "\n";
								pinbig.Label = posi.Latitude.ToString() + "," + posi.Longitude.ToString();
							}
							map.Pins.Add(pinbig);
							map.MoveToRegion(MapSpan.FromCenterAndRadius(posi, Distance.FromMiles(1.0)));
							st.Children.Add(map);

							p.Title = "Alert Position";
							p.Content = st;
							System.GC.Collect();


							await Navigation.PushAsync(p);

							await System.Threading.Tasks.Task.Delay(2000);
							UserDialogs.Instance.HideLoading();
						}
						catch
						{
						}
					}
				};
				if (ViewModel.AlertPosition.Latitude.ToString() == "0" && ViewModel.AlertPosition.Longitude.ToString() == "0")
				{
					noMap = true;

					var imgGps = new CachedImage()
					{
						WidthRequest = w / 2-50,
						HeightRequest = h / 3,
						CacheDuration = TimeSpan.FromDays(30),
						DownsampleToViewSize = true,
						RetryCount = 0,
						RetryDelay = 250,
						Aspect = Aspect.Fill,
						TransparencyEnabled = false,
						LoadingPlaceholder = ImageSource.FromFile("noimage.gif"),
						ErrorPlaceholder = ImageSource.FromFile("noimage.gif"),
					};
					imgGps.Source = ImageSource.FromFile("nogps.gif");

					
					mapstack.Children.Add(imgGps);
				}
				else {

					var layoutTop = new StackLayout { BackgroundColor = Color.Transparent };
					var relative = new Xamarin.Forms.RelativeLayout { BackgroundColor = Color.Green };

					layoutTop.WidthRequest = w / 2;
					layoutTop.HeightRequest = w / 2;

					relative.Children.Add(customMap,
					Constraint.Constant(0),
					Constraint.Constant(0),
					Constraint.RelativeToParent((parent) =>
								{

									return w / 2;

								}),
								Constraint.RelativeToParent((parent) =>
								{

									return w / 2;

								}));
					relative.Children.Add(layoutTop,
				Constraint.Constant(0),
				Constraint.Constant(0),
				Constraint.RelativeToParent((parent) =>
							{

								return w / 2;

							}),
							Constraint.RelativeToParent((parent) =>
							{

								return w / 2;

							}));
					layoutTop.GestureRecognizers.Add(bigMap);
					mapstack.Children.Add(relative);

				}
				var btnAction = new Xamarin.Forms.Button { Text = "Action", WidthRequest = w / 3, BackgroundColor = Color.Red, TextColor = Color.White, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.EndAndExpand };
				var btnArchive = new Xamarin.Forms.Button { Text = "Archive", WidthRequest = w / 3, BackgroundColor = Color.Red, TextColor = Color.White, HorizontalOptions = LayoutOptions.EndAndExpand, VerticalOptions = LayoutOptions.EndAndExpand };

				buttonstack.Children.Add(btnAction);
				buttonstack.Children.Add(btnArchive);

				btnAction.Clicked += (sender, e) =>
				{

					Navigation.PushAsync(new AlertDetailsAction(SendToActionPage));
				};

				btnArchive.Clicked += async (sender, e) =>
			   {

				   try
				   {
					   var networkConnection = DependencyService.Get<INetworkConnection>();
					   networkConnection.CheckNetworkConnection();
					   var networkStatus = networkConnection.IsConnected ? "Connected" : "Not Connected";
					   if (networkStatus != "Connected")
					   {

						   UserDialogs.Instance.Alert("Please check your internet connection.", "Alert", "OK");
						   return;
					   }
					   UserDialogs.Instance.ShowLoading("Loading...");
					   if (Navigation.NavigationStack.Count <= 1)
					   {
						   var mainPage = new MainPage();
						   var alertListViewModel = new AlertsListViewModel(Navigation, Settings.UserID);
						   await alertListViewModel.LoadAlertList().ConfigureAwait(true);
						   mainPage.nav = new NavigationPage(new AlertListPage(0, alertListViewModel));
						   mainPage.Detail = mainPage.nav;
						   mainPage.IsPresented = false;
						   Xamarin.Forms.Application.Current.MainPage = mainPage;
					   }
					   else
					   {
						   await Navigation.PopAsync();
						   System.GC.Collect();
					   }
					   await System.Threading.Tasks.Task.Delay(1000);
					   UserDialogs.Instance.HideLoading();
				   }
				   catch { UserDialogs.Instance.HideLoading(); }
			   };

				mainstack.Children.Add(addressstacklayout);
				mainstack.Children.Add(mapandimagestack);

				layoutstack.Children.Add(headstack);
				layoutstack.Children.Add(mainstack);
				layoutstack.Children.Add(buttonstack);
				Content = layoutstack;
				UserDialogs.Instance.HideLoading();
			}
			catch (System.Exception ex)
			{

				UserDialogs.Instance.HideLoading();
			}
		}
		async void Timer()
		{
			try
			{
				Xamarin.Forms.Device.StartTimer(TimeSpan.FromSeconds(1), () =>
				{
					if (ViewModel != null)
					{

					}
					return true;
				});
			}
			catch { }
		}

		protected override bool OnBackButtonPressed()
		{
			try
			{
				System.GC.Collect();

			}
			catch { }
			return base.OnBackButtonPressed();
		}
		public static string BatteryLevelImage(int? BatteryLevel)
		{
			try
			{
				BatteryLevel = BatteryLevel ?? 0;

				if (BatteryLevel <= 10)
				{
					return "battery0.png";
				}
				else if (BatteryLevel > 10 && BatteryLevel <= 40)
				{
					return "battery1.png";
				}

				else if (BatteryLevel > 40 && BatteryLevel <= 70)
				{
					return "battery2.png";
				}
				else
				{
					return "battery3.png";
				}
			}
			catch { }
			return "";
		}
		public static string TempratureLevelImage(int? TempLevel)
		{
			try
			{
				TempLevel = TempLevel ?? 0;

				if (TempLevel <= 10)
				{
					return "temperature0.png";
				}
				else if (TempLevel > 10 && TempLevel <= 40)
				{
					return "temperature1.png";
				}

				else if (TempLevel > 40 && TempLevel <= 70)
				{
					return "temperature2.png";
				}
				else
				{
					return "temperature3.png";
				}
			}
			catch
			{
			}
			return "";
		}
		private Image BytesArraytoImage(byte[] stream)
		{
			Image img = new Image();
			try
			{

				byte[] imagedata = stream;
				img.Source = ImageSource.FromStream(() => new MemoryStream(imagedata));

			}
			catch { }
			return img;
		}
	}
}

