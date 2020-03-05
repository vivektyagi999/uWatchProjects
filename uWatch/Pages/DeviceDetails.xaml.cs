using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using uWatch.ViewModels;
using UwatchPCL;
using uWatch;
using Xamarin.Forms;
using System.IO;
using Acr.UserDialogs;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;

namespace uWatch
{
	public partial class DeviceDetails : ContentPage
	{
		public int Device_Id;
		RelativeLayout relativeLayout;
		ScrollView scrollview;
		int currentimg = 0;
		Image imgAsset;
		public IUserDialogs userdialogs;
		double w = MyController.VirtualWidth;
		double h = MyController.VirtualHeight;

		public DeviceDetailsViewModel ViewModel { get; set; }

		public TakePictureViewModel AssetViewModel { get; set; }


		public DeviceDetails(int deviceId)
		{
			try
			{
				this.Device_Id = deviceId;
				InitializeComponent();
				ViewModel = new DeviceDetailsViewModel(Device_Id);
				AssetViewModel = new TakePictureViewModel();
			}
			catch (System.Exception ex)
			{
			}
		}

		protected async override void OnAppearing()
		{
			try
			{
				await Task.Delay(1000);
				userdialogs = UserDialogs.Instance;
				BindingContext = ViewModel.device;
				SetLayout();
			}
			catch (Exception ex)
			{
			}
			base.OnAppearing();
		}


		protected override bool OnBackButtonPressed()
		{
			System.GC.Collect();
			return base.OnBackButtonPressed();
		}


		private async void SetLayout()
		{
			try
			{
				Title = "Device Details";

				relativeLayout = new RelativeLayout();
				AddLayout();
				scrollview = new ScrollView();
				scrollview.Content = relativeLayout;
				Content = scrollview;
			}
			catch (System.Exception ex)
			{
			}
		}

		private FileImageSource BytesArraytoImage(byte[] stream)
		{
			try
			{
				Image img = new Image();
				byte[] imagedata = stream;
				img.Source = ImageSource.FromStream(() => new MemoryStream(imagedata, 0, imagedata.Length));
				var source = img.Source as FileImageSource;
				return source;
			}
			catch (Exception ex)
			{
				return null;
			}

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
				imgAsset = MyUILibrary.AddImage(relativeLayout, "top_bg.png", 0, position, w, 300, Aspect.Fill);
				TapGestureRecognizer bigt = new TapGestureRecognizer();
				bigt.Tapped += async (object sender, EventArgs e) =>
				{
					ContentPage p = new ContentPage();
					StackLayout st = new StackLayout();
					Image i = new Image();
					i.Source = imgAsset.Source;
					st.Children.Add(i);
					p.Content = st;
					await Navigation.PushAsync(p);
				};
				imgAsset.GestureRecognizers.Add(bigt);
				if (ViewModel != null)
				{
					if (ViewModel.ImageList != null)
					{
						if (ViewModel.ImageList.Count() > 0)
						{
							imgAsset.Source = ViewModel.ImageList[currentimg].Source;
							var x = 10;
							foreach (var item in ViewModel.ImageList)
							{
								var it = MyUILibrary.AddImage(relativeLayout, "", x, position + 300 + 20, 50, 50, Aspect.AspectFit);
								TapGestureRecognizer t = new TapGestureRecognizer();
								t.Tapped += (object sender, EventArgs e) =>
								{
									currentimg = ViewModel.ImageList.ToList().IndexOf(item);
									imgAsset.Source = ViewModel.ImageList[currentimg].Source;
								};
								it.GestureRecognizers.Add(t);
								it.Source = item.Source;
								x += 50 + 10;
							}
							position += 300 + 20 + 50;
						}
						else
						{
							position += 200 + 20;
						}
					}
					else
					{
						position += 200 + 20;
					}
				}
				else
				{
					position += 200 + 20;
				}

				MyUILibrary.AddLabel(relativeLayout, "uWatch unit IMEI : ", 10, position + 20, 150, 40, Color.Black, 15);
				var lblIMEI = MyUILibrary.AddLabel(relativeLayout, "323981279812791", 10 + 150, position + 20 + 3, newx40, 40, Color.Gray, 12);
				lblIMEI.SetBinding(Label.TextProperty, new Binding("imei", BindingMode.TwoWay));

				position += 20 + 40;
				MyUILibrary.AddLabel(relativeLayout, "Date unit initialised", 10, position, newx80, 40, Color.Black, 15);
				var lblInsitilised = MyUILibrary.AddLabel(relativeLayout, "", 10, position + 20 + 2, newx80, 40, Color.Gray, 12);
				lblInsitilised.SetBinding(Label.TextProperty, new Binding("init_date", BindingMode.TwoWay));

				MyUILibrary.AddLabel(relativeLayout, "Date unit activated", 200, position, newx80, 40, Color.Black, 15);
				var lblActivated = MyUILibrary.AddLabel(relativeLayout, "", 200, position + 20, newx80, 40, Color.Gray, 12);
				lblActivated.SetBinding(Label.TextProperty, new Binding("strinit_date", BindingMode.TwoWay));

				position += 40;

				uWatch.ShapeView Alertscircle = new uWatch.ShapeView
				{
					ShapeType = ShapeType.CircleIndicator,
					StrokeColor = Color.Gray,
					StrokeWidth = 2,
					HorizontalOptions = LayoutOptions.Center,
					VerticalOptions = LayoutOptions.Center,
					WidthRequest = 100,
					HeightRequest = 100,
				};
				Label Alertval = new Label
				{
					TextColor = Color.Red,
					Text = "320",
					FontSize = 20,
					FontAttributes = FontAttributes.Bold,
					BackgroundColor = Color.Transparent,
					HorizontalOptions = LayoutOptions.Center,
					VerticalOptions = LayoutOptions.Center,
				};
				Alertval.BindingContext = ViewModel;
				Alertval.SetBinding(Label.TextProperty, new Binding("AlertCounts", BindingMode.TwoWay));
				var btnAlert = MyUILibrary.AddCircle(relativeLayout, Alertscircle, 15, position + 20, 100, 100, "Alerts", Alertval);
				TapGestureRecognizer taa = new TapGestureRecognizer();
				taa.Tapped += GetAletListPage;
				btnAlert.GestureRecognizers.Add(taa);

				uWatch.ShapeView Signalcircle = new uWatch.ShapeView
				{
					ShapeType = ShapeType.CircleIndicator,
					StrokeColor = Color.Gray,
					StrokeWidth = 2,
					HorizontalOptions = LayoutOptions.Center,
					VerticalOptions = LayoutOptions.Center,
					WidthRequest = 100,
					HeightRequest = 100,
				};
				Image signal = new Image();
				if (ViewModel != null)
					signal.BindingContext = ViewModel;
				signal.SetBinding(Image.SourceProperty, new Binding("LastSignalImage", BindingMode.TwoWay));
				MyUILibrary.AddCircle(relativeLayout, Signalcircle, 30 + 100 + 5, position + 20, 100, 100, "Signal", signal);

				uWatch.ShapeView Battarycircle = new uWatch.ShapeView
				{
					ShapeType = ShapeType.CircleIndicator,
					StrokeColor = Color.Gray,
					StrokeWidth = 2,
					HorizontalOptions = LayoutOptions.Center,
					VerticalOptions = LayoutOptions.Center,
					WidthRequest = 100,
					HeightRequest = 100,
				};
				Image battery = new Image();
				if (ViewModel != null)
					battery.BindingContext = ViewModel;
				battery.SetBinding(Image.SourceProperty, new Binding("LastBatteryImage", BindingMode.TwoWay));
				MyUILibrary.AddCircle(relativeLayout, Battarycircle, 30 + 200 + 25, position + 20, 100, 100, "Battery", battery);


				position += 100 + 40;
				MyUILibrary.AddLabel(relativeLayout, "Last reported location", 10, position, newx80, 40, Color.Black, 14);
				var lblLocation = MyUILibrary.AddLabel(relativeLayout, "", 10, position + 20, newx80, 40, Color.Gray, 12);
				//lblLocation.SetBinding(Label.TextProperty, new Binding("geo_coords", BindingMode.TwoWay));

				MyUILibrary.AddLabel(relativeLayout, "Last Heartbeat", 200, position, newx80, 40, Color.Black, 14);
				var lblHeatbeat = MyUILibrary.AddLabel(relativeLayout, "", 200, position + 20, newx80, 40, Color.Gray, 12);
				lblHeatbeat.SetBinding(Label.TextProperty, new Binding("strinit_date", BindingMode.TwoWay));


				var customMap = new AlertsMap
				{
					MapType = MapType.Satellite,
					WidthRequest = w - 40,
					HeightRequest = 500,
				};

				customMap.RouteCoordinates = new List<Position>();

			
				if (ViewModel.AssetsPositions != null)
				{
					if (ViewModel.AssetsPositions.Count > 0)
					{
						customMap.RouteCoordinates.AddRange(ViewModel.AssetsPositions);
					}

					//else
					//{
					//	customMap.RouteCoordinates.Add(new Position(37.785559, -122.396728));
					//	customMap.RouteCoordinates.Add(new Position(37.780624, -122.390541));
					//	customMap.RouteCoordinates.Add(new Position(37.777113, -122.394983));
					//	customMap.RouteCoordinates.Add(new Position(37.776831, -122.394627));
					//}
					foreach (var item in customMap.RouteCoordinates)
					{
						var pin = new Pin
						{
							Type = PinType.Place,
							Position = item,
							Label = item.Longitude.ToString() + " / " + item.Longitude.ToString(),
						};
						customMap.Pins.Add(pin);
					}
					customMap.MoveToRegion(MapSpan.FromCenterAndRadius(ViewModel.AssetsPositions[0], Distance.FromMiles(1.0)));
				}
				MyUILibrary.AddMap(relativeLayout, customMap, 10, position + 40 + 20, w - 20, 300);

				position += 40 + 300;


				var btnAlerts = MyUILibrary.AddButton(relativeLayout, "Alerts", 20 + 200 + 10, position + 50, 100, 50, Color.Red, Color.Gray, Color.White, 15);
				btnAlerts.Clicked += GetAletListPage;
		

			}
			catch(System.Exception ex)
			{
			}
		}

		private async void GetAletListPage(object sender, EventArgs e)
		{
			userdialogs.ShowLoading("Loading...", MaskType.Gradient);
			await Task.Delay(1000);
				await Task.Delay(500);
			userdialogs.HideLoading();

		}

	}
}

