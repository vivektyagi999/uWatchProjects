using System;
using System.Collections.Generic; using System.IO; using System.Threading; using System.Threading.Tasks; using Acr.UserDialogs; using FFImageLoading.Forms; using UwatchPCL; using UwatchPCL.Helpers; using Xamarin.Forms; using Xamarin.Forms.Maps;


#if __ANDROID__ using Android.App; using Android.Widget; using uWatch.Droid; 
#endif 

namespace uWatch { 	public partial class AlertDetailsForAgentold : ContentPage 	{ 		AlertsEsclatedToAgentViewModel Alert; 		Xamarin.Forms.RelativeLayout relativeLayout; 		Xamarin.Forms.ScrollView scrollview; 		public Label lblCountdownValue; 		double w = MyController.VirtualWidth; 		double h = MyController.VirtualHeight; 		bool noimage = false; 		bool noMap = false; 		Geocoder geoCoder;

		AlertsEsclatedToAgentViewModel SendToActionPage;

		int GlobleDevceId;  		public AlertDetailsViewModel ViewModel { get; set; }  		public AlertDetailsForAgentold(string str = "") 		{ 			try 			{
				if (!string.IsNullOrEmpty(str)) 				{ 					var lblMsg = new Label { Text = str, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand, FontSize = 15 }; 					Content = lblMsg; 				} 				else 
				{ 					geoCoder = new Geocoder(); 					var alertid = MyController.AlertId; 					AlertsEsclatedToAgentViewModel req = new AlertsEsclatedToAgentViewModel(); 					req.alertlog_idx = alertid == "" ? 0 : Convert.ToInt32(alertid); 					var alert = ApiService.Instance.GetAlert(req).Result; 					this.Alert = alert; 					ViewModel = new AlertDetailsViewModel(this.Alert); 					Timer(); 					SetLayout(); 				} 			} 			catch (Exception ex) 			{ 				MyController.ErrorManagement(ex.Message); 			} 		}  		public AlertDetailsForAgentold(AlertsEsclatedToAgentViewModel alert) 		{ 			try 			{
				SendToActionPage = alert; 				this.Alert = alert;  				geoCoder = new Geocoder(); 				ViewModel = new AlertDetailsViewModel(this.Alert); 				Timer(); 				BindingContext = ViewModel.device; 				SetLayout(); 			} 			catch (Exception ex) 			{ 				MyController.ErrorManagement(ex.Message); 			} 		}  		protected async override void OnAppearing() 		{ 			base.OnAppearing(); 		}  		protected override void OnDisappearing() 		{ 			try 			{ 				System.GC.Collect(); 				base.OnDisappearing(); 			} 			catch { } 		}  		private async void SetLayout() 		{ 			try 			{  				Title = "Alert Details"; 				relativeLayout = new Xamarin.Forms.RelativeLayout(); 				AddLayout(); 				scrollview = new Xamarin.Forms.ScrollView(); 				scrollview.Content = relativeLayout; 				Content = scrollview; 			} 			catch { } 		}  		private async void AddLayout() 		{  			try 			{  				double position = 0; 				double newx20 = MyUiUtils.getPercentual(w, 20); 				double newx40 = MyUiUtils.getPercentual(w, 40); 				double newx60 = MyUiUtils.getPercentual(w, 60); 				double newx80 = MyUiUtils.getPercentual(w, 80);  				StackLayout headstack = new StackLayout() { BackgroundColor = Color.FromRgb(244, 244, 244), }; 				headstack.Orientation = StackOrientation.Horizontal; 				headstack.HorizontalOptions = LayoutOptions.FillAndExpand; 				headstack.VerticalOptions = LayoutOptions.FillAndExpand;  				StackLayout Substack = new StackLayout() { }; 				Substack.Orientation = StackOrientation.Horizontal; 				Substack.HorizontalOptions = LayoutOptions.CenterAndExpand; 				Substack.VerticalOptions = LayoutOptions.CenterAndExpand;  				var lblAlertText1 = new Label { Text = "!!", FontSize= 19, FontAttributes = FontAttributes.Bold, TextColor = Color.Red, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand }; 				var imgAlerts = new Image { HeightRequest = 20, WidthRequest = 20, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand }; 				if (ViewModel != null)
				{
					if (ViewModel.device != null) 					imgAlerts.Source = await MyController.GetAlertTypelImage(ViewModel.device.alert_type);
				}
 				var lblAlertText = new Label { FontSize = 19, FontAttributes = FontAttributes.Bold, TextColor = Color.Red, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand }; 				if (ViewModel.device != null)
					lblAlertText.Text = MyController.GetAlertTypeName(ViewModel.device.alert_type).ToUpper() + " !! ";
				 				Substack.Children.Add(lblAlertText1); 				Substack.Children.Add(imgAlerts); 				Substack.Children.Add(lblAlertText);  				headstack.Children.Add(Substack);  				MyUILibrary.AddLayout(relativeLayout, headstack, 0, position, w, 50);    				position += 75; 				var lblFirndlyName = MyUILibrary.AddLabel(relativeLayout,SendToActionPage.FriendlyName , 10, position, w, 50, Color.Gray, 19);
				lblFirndlyName.HorizontalTextAlignment = TextAlignment.Start; 				lblFirndlyName.HorizontalOptions = LayoutOptions.FillAndExpand; 				lblFirndlyName.VerticalOptions = LayoutOptions.StartAndExpand;
				position += 25;
				var lblDateText = MyUILibrary.AddLabel(relativeLayout, "AlertDate", 10, position, w, 50, Color.Gray, 19);
				lblDateText.HorizontalTextAlignment = TextAlignment.Start;
				lblDateText.HorizontalOptions = LayoutOptions.FillAndExpand;
				lblDateText.FontAttributes = FontAttributes.Bold;
				lblDateText.VerticalOptions = LayoutOptions.StartAndExpand;
				position += 25;
				var lblDeviceName = MyUILibrary.AddLabel(relativeLayout, SendToActionPage.DeviceDate.ToString(), 10, position, w, 50, Color.Gray, 19);
				lblDeviceName.HorizontalTextAlignment = TextAlignment.Start;
				lblDeviceName.HorizontalOptions = LayoutOptions.FillAndExpand;
				lblDeviceName.VerticalOptions = LayoutOptions.StartAndExpand;

				position += 25;
				var lblAddressText = MyUILibrary.AddLabel(relativeLayout, "Owner's Address", 10, position, w, 50, Color.Gray, 19);
				lblAddressText.HorizontalTextAlignment = TextAlignment.Start;
				lblAddressText.FontAttributes = FontAttributes.Bold;
				lblAddressText.HorizontalOptions = LayoutOptions.FillAndExpand;
				lblAddressText.VerticalOptions = LayoutOptions.StartAndExpand;
				position += 10;
				var lblAddressline1 = MyUILibrary.AddLabel(relativeLayout, SendToActionPage.AddressLine1, 10, position, w, 50, Color.Gray, 19);
				lblAddressline1.HorizontalTextAlignment = TextAlignment.Start;
				lblAddressline1.HorizontalOptions = LayoutOptions.FillAndExpand;
				lblAddressline1.VerticalOptions = LayoutOptions.StartAndExpand;
				position += 10;
				var lblAddressline2 = MyUILibrary.AddLabel(relativeLayout, SendToActionPage.AddressLine2, 10, position, w, 50, Color.Gray, 19);
				lblAddressline2.HorizontalTextAlignment = TextAlignment.Start;
				lblAddressline2.HorizontalOptions = LayoutOptions.FillAndExpand;
				lblAddressline2.VerticalOptions = LayoutOptions.StartAndExpand;
				position += 25;
				var addressline = MyUILibrary.AddImage(relativeLayout, "gray_line.png", 0, position, w, 5, Aspect.AspectFit);

				position +=10;
				var lblcontactText = MyUILibrary.AddLabel(relativeLayout, "Contact: "+SendToActionPage.Mobile1, 10, position, w, 50, Color.Gray, 19);
				lblcontactText.HorizontalTextAlignment = TextAlignment.Start;
				lblcontactText.HorizontalOptions = LayoutOptions.FillAndExpand;
				lblcontactText.VerticalOptions = LayoutOptions.StartAndExpand;
 				position += 30;
				var contactline = MyUILibrary.AddImage(relativeLayout, "gray_line.png", 0, position, w, 5, Aspect.AspectFit);  				position += 25; 				var lblAlertDateTime = MyUILibrary.AddLabel(relativeLayout, SendToActionPage.AlertDateTime, 10, position, w, 50, Color.Gray, 19); 				lblAlertDateTime.HorizontalTextAlignment = TextAlignment.Start; 				lblAlertDateTime.HorizontalOptions = LayoutOptions.FillAndExpand; 				lblAlertDateTime.VerticalOptions = LayoutOptions.StartAndExpand;  				position += 30; 				var imgAlert = new CachedImage() 				{ 					WidthRequest = 200, 					HeightRequest = 200, 					CacheDuration = TimeSpan.FromDays(30), 					DownsampleToViewSize = true, 					RetryCount = 300, 					RetryDelay = 250, 					Aspect = Aspect.AspectFit, 					TransparencyEnabled = false, 					LoadingPlaceholder = ImageSource.FromFile("noimage.gif"), 					ErrorPlaceholder = ImageSource.FromFile("noimage.gif"), 				} ;  				imgAlert = MyUILibrary.AddCachedImage(relativeLayout, imgAlert, 20, position, 150, 150, Aspect.AspectFit); 
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
				
					TapGestureRecognizer bigt = new TapGestureRecognizer(); 				bigt.Tapped += async (object sender, EventArgs e) => 				{ 					if (!noimage) 					{ 						UserDialogs.Instance.ShowLoading("Please wait...", MaskType.Gradient);  						try 						{ 							ContentPage p = new ContentPage(); 							StackLayout st = new StackLayout { Padding = 10, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand }; 							Xamarin.Forms.RelativeLayout r = new Xamarin.Forms.RelativeLayout(); 							CachedImage i = new CachedImage { Aspect = Aspect.Fill, LoadingPlaceholder = ImageSource.FromFile("placeholder.png"), ErrorPlaceholder = ImageSource.FromFile("noimage.gif"), HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand }; 							i.Source = imgAlert.Source; 							i.Aspect = Aspect.AspectFit; 							Xamarin.Forms.AbsoluteLayout a = new Xamarin.Forms.AbsoluteLayout(); 							Label l = new Label { TextColor = Color.Black, FontSize = 15 }; 							//l.Text = ViewModel.device.DeviceDate.ToString("f"); 							a.Children.Add(l); 							MyUILibrary.AddView(r, a, w - 80, 50, w, 60); 							st.Children.Add(i);  							p.Title = "Alert Image"; 							p.Content = st;  							await Navigation.PushAsync(p);  						} 						catch  						{   						} 						await System.Threading.Tasks.Task.Delay(50); 						UserDialogs.Instance.HideLoading(); 					} 				} ; 				imgAlert.GestureRecognizers.Add(bigt);  				var customMap = new AlertsMap 				{ 					MapType = MapType.Satellite, 					WidthRequest = w - 40, 					HeightRequest = 500, 				} ; 				var pin = new Pin 				{ 					Type = PinType.Place, 					Position = ViewModel.AlertPosition  				} ; 				var pos = ViewModel.AlertPosition;     				if (Xamarin.Forms.Device.Idiom == TargetIdiom.Phone) 				{ 					var possibleAddresses = await geoCoder.GetAddressesForPositionAsync(pos); 					foreach (var address in possibleAddresses) 					{ 						pin.Address += address + "\n"; 						pin.Label = pos.Latitude.ToString() + "," + pos.Longitude.ToString(); 					} 				} 				customMap.Pins.Add(pin); 				customMap.MoveToRegion(MapSpan.FromCenterAndRadius(pos, Distance.FromMiles(1.0))); 				TapGestureRecognizer bigMap = new TapGestureRecognizer(); 				bigMap.Tapped += async (object sender, EventArgs e) => 				{ 					if (!noMap) 					{ 						try 						{ 							UserDialogs.Instance.ShowLoading("Loading..."); 							ContentPage p = new ContentPage(); 							StackLayout st = new StackLayout(); 							var Map = new AlertsMap 							{ 								MapType = MapType.Satellite, 								WidthRequest = w, 								HeightRequest = h - 80, 							} ; 							var pinbig = new Pin 							{ 								Type = PinType.Place, 								Position = ViewModel.AlertPosition, 								Label = "Xamarin San Francisco Office", 								Address = "394 Pacific Ave, San Francisco CA" 							}; 							var posi = ViewModel.AlertPosition; 							var Addresses = await geoCoder.GetAddressesForPositionAsync(posi); 							foreach (var address in Addresses) 							{ 								pinbig.Address += address + "\n"; 								pinbig.Label = posi.Latitude.ToString() + "," + posi.Longitude.ToString(); 							} 							Map.Pins.Add(pinbig); 							Map.MoveToRegion(MapSpan.FromCenterAndRadius(posi, Distance.FromMiles(1.0))); 							st.Children.Add(Map);   							p.Title = "Alert Position"; 							p.Content = st; 							System.GC.Collect();   							await Navigation.PushAsync(p);  							await System.Threading.Tasks.Task.Delay(2000); 							UserDialogs.Instance.HideLoading(); 					} 					catch 					{ 					} 					} 				} ; 				if (ViewModel.AlertPosition.Latitude.ToString() == "0" && ViewModel.AlertPosition.Longitude.ToString() == "0") 				{ 					noMap = true;  					var imgGps = new CachedImage() 					{ 						WidthRequest = 200, 						HeightRequest = 200, 						CacheDuration = TimeSpan.FromDays(30), 						DownsampleToViewSize = true, 						RetryCount = 0, 						RetryDelay = 250, 						Aspect = Aspect.AspectFit, 						TransparencyEnabled = false, 						LoadingPlaceholder = ImageSource.FromFile("noimage.gif"), 						ErrorPlaceholder = ImageSource.FromFile("noimage.gif"), 					} ; 					imgGps.Source = ImageSource.FromFile("nogps.gif"); 					imgGps = MyUILibrary.AddCachedImage(relativeLayout, imgGps, 200, position, 150, 150, Aspect.AspectFit); 				} 				else {  					MyUILibrary.AddMap(relativeLayout, customMap, 200, position, 150, 150);   				} 				Xamarin.Forms.AbsoluteLayout Maplayout = new Xamarin.Forms.AbsoluteLayout(); 				MyUILibrary.AddView(relativeLayout, Maplayout, 200, position, 150, 150); 				Maplayout.GestureRecognizers.Add(bigMap); 				position += 150 + 10;   				var imgLine2 = MyUILibrary.AddImage(relativeLayout, "gray_line.png", 0, position, w, 5, Aspect.AspectFit); 				position += 5;  				position += 20; 				var imgLine3 = MyUILibrary.AddImage(relativeLayout, "gray_line.png", 0, position, w, 5, Aspect.AspectFit);  				position += 10; 				var btnAction = MyUILibrary.AddButton(relativeLayout, "Action", 15, position, w / 3, 55, Color.Red, Color.Red, Color.White, 15); 				//btnAction.IsEnabled = false;  				var btnArchive = MyUILibrary.AddButton(relativeLayout, "Archive", w / 2 + 40, position, w / 3, 55, Color.Red, Color.Red, Color.White, 15); 
 
				btnAction.Clicked += (sender, e) => {

					Navigation.PushAsync(new AlertDetailsAction(SendToActionPage));
				}; 			 				btnArchive.Clicked += async (sender, e) => 			   { 					 				   try 				   { 						var networkConnection = DependencyService.Get<INetworkConnection>(); 					   networkConnection.CheckNetworkConnection(); 					   var networkStatus = networkConnection.IsConnected ? "Connected" : "Not Connected"; 					   if (networkStatus != "Connected") 					   { 						    						   UserDialogs.Instance.Alert("Please check your internet connection.", "Alert", "OK"); 						   return; 					   } 					   UserDialogs.Instance.ShowLoading("Loading..."); 					   if (Navigation.NavigationStack.Count <= 1) 					   { 						   var mainPage = new MainPage(); 						   var alertListViewModel = new AlertsListViewModel(Navigation, Settings.UserID); 						   await alertListViewModel.LoadAlertList().ConfigureAwait(true); 						   mainPage.nav = new NavigationPage(new AlertListPage(0, alertListViewModel)); 						   mainPage.Detail = mainPage.nav; 						   mainPage.IsPresented = false; 						   Xamarin.Forms.Application.Current.MainPage = mainPage; 					   } 					   else 					   { 						   await Navigation.PopAsync(); 						   System.GC.Collect(); 					   } 						await System.Threading.Tasks.Task.Delay(1000); 					   UserDialogs.Instance.HideLoading(); 				   } 				   catch { UserDialogs.Instance.HideLoading(); } 			   };  				position += 60; 				position += 60; 			} 			catch{} 		}  		async void Timer() 		{ 			try 			{ 				Xamarin.Forms.Device.StartTimer(TimeSpan.FromSeconds(1), () => 				{ 					if (ViewModel != null) 					{ 						 					} 					return true; 				} ); 			} 			catch { } 		}  		protected override bool OnBackButtonPressed() 		{ 			try 			{ 				System.GC.Collect();  			} 			catch { } 			return base.OnBackButtonPressed(); 		}  		public static string BatteryLevelImage(int? BatteryLevel) 		{ 			try 			{ 				BatteryLevel = BatteryLevel ?? 0;  				if (BatteryLevel <= 10) 				{ 					return "battery0.png"; 				} 				else if (BatteryLevel > 10 && BatteryLevel <= 40) 				{ 					return "battery1.png"; 				}  				else if (BatteryLevel > 40 && BatteryLevel <= 70) 				{ 					return "battery2.png"; 				} 				else 				{ 					return "battery3.png"; 				} 			} 			catch { } 			return ""; 		}  		public static string TempratureLevelImage(int? TempLevel) 		{ 			try 			{ 				TempLevel = TempLevel ?? 0;  				if (TempLevel <= 10) 				{ 					return "temperature0.png"; 				} 				else if (TempLevel > 10 && TempLevel <= 40) 				{ 					return "temperature1.png"; 				}  				else if (TempLevel > 40 && TempLevel <= 70) 				{ 					return "temperature2.png"; 				} 				else 				{ 					return "temperature3.png"; 				} 			} 			catch {  			} 			return ""; 		}  		private Image BytesArraytoImage(byte[] stream) 		{ 			Image img = new Image(); 			try 			{ 				 				byte[] imagedata = stream; 				img.Source = ImageSource.FromStream(() => new MemoryStream(imagedata));  			} 			catch { } 			return img; 		} 	} }   