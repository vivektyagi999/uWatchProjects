using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using UwatchPCL;
using FFImageLoading.Forms;
using Rg.Plugins.Popup.Extensions;

namespace uWatch
{
	public partial class DeviceDetailPage : PopupPage
	{
		public DeviceDetailPageViewModel ViewModel
		{
			get;
			set;
		}
		public Setting CurrentDeviceSetting
		{
			get;
			set;
		}
		public string DeviceName
		{
			get;
			set;
		}
		//public DeviceDetailPage(DeviceDetailPageViewModel _DeviceDetailPageViewModel)
		//{
			
		//	ViewModel = _DeviceDetailPageViewModel;
		//}
		public DeviceDetailPage()
		{
			


		}
		public DeviceDetailPage(string _DeviceName,Setting _CurrentDevivceSetting)
		{
			
			DeviceName = _DeviceName;
			CurrentDeviceSetting = _CurrentDevivceSetting;
            InitilaizaPage();
		}
		protected override void OnAppearing()
		{
			base.OnAppearing();
			
		}
		public void InitilaizaPage()
		{
			try
			{
				InitializeComponent();
				LayoutPopupPage.HeightRequest = App.ScreenHeight * 0.5;
				LayoutPopupPage.WidthRequest = App.ScreenWidth;
				
				double font;
				if (Xamarin.Forms.Device.Idiom == TargetIdiom.Tablet)
				{
					font = 16;
				}
				else
				{
					font = 14;
				}
				Title = "DeviceDetail";
				Label nameLabelDetail = new Label()
				{
					FontSize = font + 2,
					FontAttributes = FontAttributes.Bold,
					TextColor = Color.Black,
					VerticalOptions = LayoutOptions.StartAndExpand,
					HorizontalOptions = LayoutOptions.Start,
					Text = DeviceName
				};

				var namelayoutDetail = new StackLayout { VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.Start };

				namelayoutDetail.Children.Add(nameLabelDetail);

				var clodebuttonstack = new StackLayout { HorizontalOptions = LayoutOptions.EndAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };
				var closeicon = new Image { Source = "CloseIcon.png", HeightRequest = 30, WidthRequest = 30 };
				clodebuttonstack.Children.Add(closeicon);

				var closeimagegesture = new TapGestureRecognizer();
				closeicon.GestureRecognizers.Add(closeimagegesture);
				closeimagegesture.Tapped += async (sender, e) =>
				  {
					  await Navigation.PopPopupAsync();
				  };

				var configrationlayoutDetail = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, WidthRequest = App.ScreenWidth - 10, Padding = 5 };
				configrationlayoutDetail.Children.Add(namelayoutDetail);
				configrationlayoutDetail.Children.Add(clodebuttonstack);
				HeadingStack.Children.Add(configrationlayoutDetail);



				//===============================================//
				Label ConfigrationChanges = new Label()
				{
					FontAttributes = FontAttributes.Bold,
					FontSize = font,
					TextColor = Color.Black,
					HorizontalTextAlignment = TextAlignment.Start,
					HorizontalOptions = LayoutOptions.StartAndExpand,
					VerticalOptions = LayoutOptions.End
					
				};
				ConfigrationChanges.Text = "Latest Configuration at " + DateFormat.GetDateTime(CurrentDeviceSetting.CurrentSetting.cfg_delivered, TimeType.DateAndTime);

				var cameraLayout = new StackLayout { Orientation = StackOrientation.Horizontal, Spacing = 1 };
				Image ImgCamera = new Image { Source = "No.png" };


				ImgCamera.Source = CurrentDeviceSetting.CurrentSetting.chkCamera != false ? "Yes.png" : "No.png";


				Label Camera = new Label()
				{
					FontAttributes = FontAttributes.Bold,
					FontSize = font,
					TextColor = Color.Black,
					HorizontalTextAlignment = TextAlignment.Start,
					HorizontalOptions = LayoutOptions.StartAndExpand,
					VerticalOptions = LayoutOptions.End,
					Text = "Camera"
				};
				cameraLayout.Children.Add(Camera);
				cameraLayout.Children.Add(ImgCamera);


				var GpsLayout = new StackLayout { Orientation = StackOrientation.Horizontal, Spacing = 1 };
				Image ImgGps = new Image { Source = "No.png" };

				ImgGps.Source = CurrentDeviceSetting.CurrentSetting.chkGPS != false ? "Yes.png" : "No.png";


				Label Gps = new Label()
				{
					FontAttributes = FontAttributes.Bold,
					FontSize = font,
					TextColor = Color.Black,
					HorizontalTextAlignment = TextAlignment.Start,
					HorizontalOptions = LayoutOptions.StartAndExpand,
					VerticalOptions = LayoutOptions.End,
					
					Text = "GPS"
				};
				GpsLayout.Children.Add(Gps);
				GpsLayout.Children.Add(ImgGps);

				var PIRLayout = new StackLayout { Orientation = StackOrientation.Horizontal, Spacing = 1 };
				Image ImgPIR = new Image { Source = "No.png" };

				ImgPIR.Source = CurrentDeviceSetting.CurrentSetting.chkMovementSensonPIR != false ? "Yes.png" : "No.png";



				Label PIR = new Label()
				{
					FontAttributes = FontAttributes.Bold,
					FontSize = font,
					TextColor = Color.Black,
					HorizontalTextAlignment = TextAlignment.Start,
					HorizontalOptions = LayoutOptions.StartAndExpand,
					VerticalOptions = LayoutOptions.End,
					
					Text = "PIR"
				};
				PIRLayout.Children.Add(PIR);
				PIRLayout.Children.Add(ImgPIR);

				var ShockedLayout = new StackLayout { Orientation = StackOrientation.Horizontal, Spacing = 1 };
				Image ImgShocked = new Image { Source = "No.png" };
				if (CurrentDeviceSetting.CurrentSetting.chkShockSensor)
				{
					ImgShocked.Source = "Yes.png";
				}
				else
				{
					ImgShocked.Source = "No.png";
				}
				Label Shocked = new Label()
				{
					FontAttributes = FontAttributes.Bold,
					FontSize = font,
					TextColor = Color.Black,
					HorizontalTextAlignment = TextAlignment.Start,
					HorizontalOptions = LayoutOptions.StartAndExpand,
					VerticalOptions = LayoutOptions.End,
					
					Text = "Shock"
				};
				ShockedLayout.Children.Add(Shocked);
				ShockedLayout.Children.Add(ImgShocked);

				var LCDLayout = new StackLayout { Orientation = StackOrientation.Horizontal, Spacing = 1 };
				Image ImgLCD = new Image { Source = "No.png" };
				if (CurrentDeviceSetting.CurrentSetting.chkDeviceLCDDisplayMode)
				{
					ImgLCD.Source = "Yes.png";
				}
				else
				{
					ImgLCD.Source = "No.png";
				}
				Label LCD = new Label()
				{
					FontAttributes = FontAttributes.Bold,
					FontSize = font,
					TextColor = Color.Black,
					HorizontalTextAlignment = TextAlignment.Start,
					HorizontalOptions = LayoutOptions.StartAndExpand,
					VerticalOptions = LayoutOptions.End,
					
					Text = "LCD"
				};
				LCDLayout.Children.Add(LCD);
				LCDLayout.Children.Add(ImgLCD);

				var ForcedAlertLayout = new StackLayout { Orientation = StackOrientation.Horizontal, Spacing = 1 };
				Image ImgForcedAlert = new Image { Source = "No.png" };
				if (CurrentDeviceSetting.CurrentSetting.chkForcedAlert)
				{
					ImgForcedAlert.Source = "Yes.png";
				}
				else
				{
					ImgForcedAlert.Source = "No.png";
				}
				Label ForcedAlert = new Label()
				{
					FontAttributes = FontAttributes.Bold,
					FontSize = font,
					TextColor = Color.Black,
					HorizontalTextAlignment = TextAlignment.Start,
					HorizontalOptions = LayoutOptions.StartAndExpand,
					VerticalOptions = LayoutOptions.End,
					
					Text = "Forced Alert"
				};
				ForcedAlertLayout.Children.Add(ForcedAlert);
				ForcedAlertLayout.Children.Add(ImgForcedAlert);


				var TempRangeLayout = new StackLayout { Orientation = StackOrientation.Horizontal, Spacing = 1 };
				Image ImgTempRange = new Image { Source = "No.png" };
				if (CurrentDeviceSetting.CurrentSetting.chkTemperatureRange)
				{
					ImgTempRange.Source = "Yes.png";
				}
				else
				{
					ImgTempRange.Source = "No.png";
				}
				Label TempRange = new Label()
				{
					FontAttributes = FontAttributes.Bold,
					FontSize = font,
					TextColor = Color.Black,
					HorizontalTextAlignment = TextAlignment.Start,
					HorizontalOptions = LayoutOptions.StartAndExpand,
					VerticalOptions = LayoutOptions.End,
					
					Text = "Temp. Range"
				};
				TempRangeLayout.Children.Add(TempRange);
				TempRangeLayout.Children.Add(ImgTempRange);


				var SwitchOffLayout = new StackLayout { Orientation = StackOrientation.Horizontal, Spacing = 1 };
				Image ImgSwitchOff = new Image { Source = "No.png" };
				if (CurrentDeviceSetting.CurrentSetting.chkSwitchOffAlerts)
				{
					ImgSwitchOff.Source = "Yes.png";
				}
				else
				{
					ImgSwitchOff.Source = "No.png";
				}
				Label SwitchOff = new Label()
				{
					FontAttributes = FontAttributes.Bold,
					FontSize = font,
					TextColor = Color.Black,
					HorizontalTextAlignment = TextAlignment.Start,
					HorizontalOptions = LayoutOptions.StartAndExpand,
					VerticalOptions = LayoutOptions.End,
					
					Text = "Switch OFF"
				};
				SwitchOffLayout.Children.Add(SwitchOff);
				SwitchOffLayout.Children.Add(ImgSwitchOff);


				var SounderLayout = new StackLayout { Orientation = StackOrientation.Horizontal, Spacing = 1 };
				Image ImgSounder = new Image { Source = "No.png" };
				if (CurrentDeviceSetting.CurrentSetting.chkSounder)
				{
					ImgSounder.Source = "Yes.png";
				}
				else
				{
					ImgSounder.Source = "No.png";
				}
				Label Sounder = new Label()
				{
					FontAttributes = FontAttributes.Bold,
					FontSize = font,
					TextColor = Color.Black,
					HorizontalTextAlignment = TextAlignment.Start,
					HorizontalOptions = LayoutOptions.StartAndExpand,
					VerticalOptions = LayoutOptions.End,
					
					Text = "Sounder"
				};
				SounderLayout.Children.Add(Sounder);
				SounderLayout.Children.Add(ImgSounder);

				var ScheduleLayout = new StackLayout { Orientation = StackOrientation.Horizontal, Spacing = 1 };
				Image ImgSchedule = new Image { Source = "No.png" };
				if (CurrentDeviceSetting.CurrentSetting.chkActiveTime)
				{
					ImgSchedule.Source = "Yes.png";
				}
				else
				{
					ImgSchedule.Source = "No.png";
				}
				Label Schedule = new Label()
				{
					FontAttributes = FontAttributes.Bold,
					FontSize = font,
					TextColor = Color.Black,
					HorizontalTextAlignment = TextAlignment.Start,
					HorizontalOptions = LayoutOptions.StartAndExpand,
					VerticalOptions = LayoutOptions.End,
					
					Text = "Schedule"
				};
				ScheduleLayout.Children.Add(Schedule);
				ScheduleLayout.Children.Add(ImgSchedule);

				var SwitchAlertLayout = new StackLayout { Orientation = StackOrientation.Horizontal, Spacing = 1 };
				Image ImgSwitchAlert = new Image { Source = "No.png" };
				if (CurrentDeviceSetting.CurrentSetting.chkSwitchOnAlerts)
				{
					ImgSwitchAlert.Source = "Yes.png";
				}
				else
				{
					ImgSwitchAlert.Source = "No.png";
				}
				Label SwitchAlert = new Label()
				{
					FontAttributes = FontAttributes.Bold,
					FontSize = font,
					TextColor = Color.Black,
					HorizontalTextAlignment = TextAlignment.Start,
					HorizontalOptions = LayoutOptions.StartAndExpand,
					VerticalOptions = LayoutOptions.End,
					
					Text = "Switch Alerts"
				};
				SwitchAlertLayout.Children.Add(SwitchAlert);
				SwitchAlertLayout.Children.Add(ImgSwitchAlert);

				var BatchLayout = new StackLayout { Orientation = StackOrientation.Horizontal, Spacing = 1 };
				Image ImgBatch = new Image { Source = "No.png" };
				if (CurrentDeviceSetting.CurrentSetting.chkBatchAlertUpload)
				{
					ImgBatch.Source = "Yes.png";
				}
				else
				{
					ImgBatch.Source = "No.png";
				}
				Label Batch = new Label()
				{
					FontAttributes = FontAttributes.Bold,
					FontSize = font,
					TextColor = Color.Black,
					HorizontalTextAlignment = TextAlignment.Start,
					HorizontalOptions = LayoutOptions.StartAndExpand,
					VerticalOptions = LayoutOptions.End,
					
					Text = "Batch"
				};
				BatchLayout.Children.Add(Batch);
				BatchLayout.Children.Add(ImgBatch);

				var BluetoothLayout = new StackLayout { Orientation = StackOrientation.Horizontal, Spacing = 1 };
				Image ImgBluetooth = new Image { Source = "No.png" };
				if (CurrentDeviceSetting.CurrentSetting.chkBluetoothLowEnergy)
				{
					ImgBluetooth.Source = "Yes.png";
				}
				else
				{
					ImgBluetooth.Source = "No.png";
				}
				Label Bluetooth = new Label()
				{
					FontAttributes = FontAttributes.Bold,
					FontSize = font,
					TextColor = Color.Black,
					HorizontalTextAlignment = TextAlignment.Start,
					HorizontalOptions = LayoutOptions.StartAndExpand,
					VerticalOptions = LayoutOptions.End,
					
					Text = "Bluetooth"
				};
				BluetoothLayout.Children.Add(Bluetooth);
				BluetoothLayout.Children.Add(ImgBluetooth);

				var stklayout = new StackLayout()
				{
					Children =
				{
					cameraLayout,PIRLayout,ShockedLayout,TempRangeLayout,GpsLayout,ForcedAlertLayout,LCDLayout,SounderLayout,SwitchOffLayout,ScheduleLayout,SwitchAlertLayout,BatchLayout,BluetoothLayout
				}
				};
				



				var Stack1Detail = new StackLayout()
				{

					HorizontalOptions = LayoutOptions.StartAndExpand,
					VerticalOptions = LayoutOptions.Start,
					Padding = new Thickness(10, 10, 0, 10),
					Children =
									{
										ConfigrationChanges,
										stklayout

									}
				};


				var scrlview = new ScrollView { HeightRequest = App.ScreenHeight * 0.45, WidthRequest = App.ScreenWidth };
				scrlview.Content = Stack1Detail;
				LayoutPopupstack.Children.Add(scrlview);
			}
			catch (System.Exception ex)
			{
			}
		}


	}
}
