using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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
using Android.Gms.Gcm;
#endif


namespace uWatch
{
	public partial class AlertDetailsAction : ContentPage
	{
		AlertsEsclatedToAgentViewModel Alert;
		Xamarin.Forms.RelativeLayout relativeLayout;
		Xamarin.Forms.ScrollView scrollview;
		public Label lblCountdownValue;
		public Label lblCountdownValue1;
		//public IUserDialogs userdialogs;
		double w = MyController.VirtualWidth;
		double h = MyController.VirtualHeight;
		bool noimage = false;
		bool noMap = false;
		Geocoder geoCoder;
		int _DeviceId;
		public AlertDetailsViewModel ViewModel { get; set; }
		ObservableCollection<DeviceConfig> lei;

		public AlertDetailsAction(AlertsEsclatedToAgentViewModel alert)
		{
			try
			{
				_DeviceId = alert.device_idx;
				this.Alert = alert;
				InitializeComponent();
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
#if __ANDROID__

			//var activity = Forms.Context as Activity;
			//var tt = activity.FindViewById<TextView>(Resource.Id.toolbar_title);
			//tt.Text = "Alerts Details";
#endif
#if __IOS__
				Title = "Alert Action";
#endif
				Title = "Alert Action";
			
				AddLayout();

			}
			catch { }
		}

		private async void AddLayout()
		{

			try
			{
				

				StackLayout headstack = new StackLayout() { Padding = new Thickness(0,10,0,10), BackgroundColor = Color.FromRgb(244, 244, 244), VerticalOptions = LayoutOptions.Start, HorizontalOptions = LayoutOptions.FillAndExpand };
				headstack.Orientation = StackOrientation.Horizontal;


				StackLayout Substack = new StackLayout() {};
				Substack.Orientation = StackOrientation.Horizontal;
				Substack.HorizontalOptions = LayoutOptions.CenterAndExpand;
				Substack.VerticalOptions = LayoutOptions.CenterAndExpand;


				var lblAwakeTime = new Label { Text = "Awake Time:", FontSize = 14};
				var lblAwakeTimeValue = new Label { Text = "180 secs", FontSize = 14};
				var AwakeSec = ViewModel.device.WakeTime * 60;
				lblAwakeTimeValue.SetBinding(Label.TextProperty, new Binding("WakeTime", BindingMode.TwoWay, stringFormat: "" + AwakeSec.ToString() + " Secs"));
				var lblCountdown = new Label { Text = "Countdown:", TextColor = Color.Gray, FontSize = 14};
				lblCountdownValue = new Label { Text = "98 secs", TextColor = Color.Black, FontSize = 14};
				lblCountdownValue.SetBinding(Label.TextProperty, new Binding("CountDown", BindingMode.TwoWay, stringFormat: "{0} Secs"));

				var AwakeTimeLyout = new StackLayout { Orientation = StackOrientation.Horizontal };
				AwakeTimeLyout.Children.Add(lblAwakeTime);
				AwakeTimeLyout.Children.Add(lblAwakeTimeValue);


				var CountDowmTimeLyout = new StackLayout { Orientation = StackOrientation.Horizontal };
				CountDowmTimeLyout.Children.Add(lblCountdown);
				CountDowmTimeLyout.Children.Add(lblCountdownValue);
				double HeightOfImage;
				HeightOfImage = 18;

				Double HeightRequestControl;
				HeightRequestControl = 40;

				//headstack.Children.Add(imgTitle);
				Substack.Children.Add(AwakeTimeLyout);
				Substack.Children.Add(CountDowmTimeLyout);

				headstack.Children.Add(Substack);


				//var imageLiueRating = new Image { Source = "UpdatedStartEndTime.png", HeightRequest = HeightOfImage, WidthRequest = HeightOfImage };
				var LiueRatingPicker = new CustomPicker { Title = "Profile",WidthRequest=MyController.ScreenWidth/2, TitleTextColor = Color.Black, HeightRequest = HeightRequestControl, HorizontalOptions = LayoutOptions.StartAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };
				var imgLiueRatingArrow = new Image { Source = "downArrow.png", HorizontalOptions = LayoutOptions.EndAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand, HeightRequest = HeightOfImage + 3, WidthRequest = HeightOfImage + 3 };

				var alertList = new AlertsListViewModel(Navigation, Settings.UserID);
				 lei = new ObservableCollection<DeviceConfig>();
				lei= await alertList.FetchConfigurationProfileDetails().ConfigureAwait(true);

					

				foreach (var item in lei)
				{
					if (item.configType == "Basic")
					{
						LiueRatingPicker.Items.Add(item.ConfigurationName);
					}
				}

				var LayoutLiueRating = new StackLayout { Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.Fill, Padding = new Thickness(10, 0, 10, 0) };

				StackLayout headstack1 = new StackLayout() { Padding = new Thickness(20, 10, 20, 10), BackgroundColor = Color.FromRgb(244, 244, 244), VerticalOptions = LayoutOptions.Start, HorizontalOptions = LayoutOptions.StartAndExpand };

				headstack1.Orientation = StackOrientation.Horizontal;


				LayoutLiueRating.Children.Add(LiueRatingPicker);
				LayoutLiueRating.Children.Add(imgLiueRatingArrow);

				StackLayout Substack1 = new StackLayout() { Padding = new Thickness(20, 10, 20, 50)};
				Substack1.Orientation = StackOrientation.Vertical;
				Substack1.HorizontalOptions = LayoutOptions.CenterAndExpand;
				Substack1.VerticalOptions = LayoutOptions.CenterAndExpand;



				var lblAwakeTime1 = new Label { Text = "Are you sure you want to change the cube configration of app", FontSize = 21,HorizontalOptions=LayoutOptions.Fill};


				Substack1.Children.Add(lblAwakeTime1);
				Substack1.Children.Add(LayoutLiueRating);
				//Substack1.Children.Add(imgLiueRatingArrow);

				headstack1.Children.Add(Substack1);

				var btnEsclate = new  Xamarin.Forms.Button { Text = "Esclate", WidthRequest = MyController.ScreenWidth/2+40, BackgroundColor = Color.Red, TextColor = Color.White, FontSize = 15};
				btnEsclate.IsEnabled = false;

				var btnChangeConfig = new Xamarin.Forms.Button { Text = "Change Cube Configuration",WidthRequest = MyController.ScreenWidth / 2+40 , BackgroundColor = Color.Red, TextColor = Color.White, FontSize = 15};


				if (ViewModel.device.EscalateTo == null && ViewModel.device.EscalateToAgentID != null)
				{
					btnEsclate.IsEnabled = true;
					btnEsclate.Clicked += async (sender, e) =>
					{
						btnEsclate.IsEnabled = false;
						btnChangeConfig.IsEnabled = false;
							try
							{
								var networkConnection = DependencyService.Get<INetworkConnection>();
								networkConnection.CheckNetworkConnection();
								var networkStatus = networkConnection.IsConnected ? "Connected" : "Not Connected";
								if (networkStatus != "Connected")
								{
									btnEsclate.IsEnabled = true;
									btnChangeConfig.IsEnabled = true;
									UserDialogs.Instance.Alert("Please check your internet connection.", "Alert", "OK");
									return;
								}
								string address = "Are you sure you want to send this Alert to Agent? ?";
								var answer = await UserDialogs.Instance.ConfirmAsync(address, "Confirmation", "Yes", "No");

							if (answer == true)
							{
								UserDialogs.Instance.ShowLoading("Escalating Alert...", Acr.UserDialogs.MaskType.Gradient);
								await System.Threading.Tasks.Task.Delay(100);

								AlertsEsclatedToAgentViewModel req = new AlertsEsclatedToAgentViewModel();
								req.alertlog_idx = ViewModel.device.alertlog_idx;
								req.CreatedBy = Settings.UserID;
								req.strCreatedDate = DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tttt");

								var res = await ApiService.Instance.EsclateAlert(req);
								if (res > 0)
								{
									UserDialogs.Instance.HideLoading();
									await UserDialogs.Instance.AlertAsync("Alert Escalated Successfully", "Information", "OK");
									UserDialogs.Instance.ShowLoading("Please wait...", MaskType.Gradient);
								}
								else
								{
									UserDialogs.Instance.HideLoading();
									await UserDialogs.Instance.AlertAsync("Alert Not Escalated, Please Consult your System Administrator", "Information", "OK");
								    UserDialogs.Instance.ShowLoading("Please wait...", MaskType.Gradient);
								}
								MyController.fromAssetsToGallery = true;

								if (Navigation.NavigationStack.Count <= 1)
								{
									var mainPage = new MainPage();
									var alertListViewModel = new AlertsListViewModel(Navigation, Settings.UserID);
									await alertListViewModel.LoadAlertList().ConfigureAwait(true);
									mainPage.nav = new NavigationPage(new AlertListPage(0, alertListViewModel));
									mainPage.Detail = mainPage.nav;
									mainPage.IsPresented = false;

									await System.Threading.Tasks.Task.Delay(2000);

									Xamarin.Forms.Application.Current.MainPage = mainPage;

									UserDialogs.Instance.HideLoading();
								}
								else
								{
									await Navigation.PopAsync();
									System.GC.Collect();
									await System.Threading.Tasks.Task.Delay(1000);
									UserDialogs.Instance.HideLoading();
								}

								btnEsclate.IsEnabled = true;
								btnChangeConfig.IsEnabled = true;

							}
							else
							{ 
							btnEsclate.IsEnabled = true;
							btnChangeConfig.IsEnabled = true;
							}

							}
							catch { }
					};
				}
				else
				{
					btnEsclate.Text = "Escalated";
				}


				btnChangeConfig.Clicked += async (sender, e) =>
			   {

					   try
					   {
						
						var alertList2 = new AlertsListViewModel(Navigation, Settings.UserID);


					 var selectedProfile =  LiueRatingPicker.SelectedIndex+1;



						var xx = lei.ElementAt(0);
					   int profileid = xx.DeviceCustomConfig_Idx;
						bool lei1 = await alertList.ChangeCubeConfigration(_DeviceId,profileid,DateTime.Now).ConfigureAwait(true);



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

				var btnLayout = new StackLayout { Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.EndAndExpand };
				//btnLayout.Children.Add(LiueRatingPicker);
				btnLayout.Children.Add(btnEsclate);
				btnLayout.Children.Add(btnChangeConfig);

				var mainStackLayout = new StackLayout { Padding = new Thickness(0,10,0,10), HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
				mainStackLayout.Children.Add(headstack);
				mainStackLayout.Children.Add(headstack1);

				mainStackLayout.Children.Add(btnLayout);


				Content = mainStackLayout;

			}
			catch{}


		}

		async void Timer()
		{
			try
			{
				Device.StartTimer(TimeSpan.FromSeconds(1), () =>
				{
					if (ViewModel != null)
					{
						if (ViewModel.device.CountDown < 0)
						{
							return false;
						}
						if (ViewModel.device.CountDown > 0)
							ViewModel.device.CountDown -= 1;
						if (lblCountdownValue != null)
						{
							lblCountdownValue.Text = ViewModel.device.CountDown.ToString() + " Secs";
						}
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
			catch { 
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

