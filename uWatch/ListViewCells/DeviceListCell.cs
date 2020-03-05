using System;

using Xamarin.Forms;
using UwatchPCL;
using FFImageLoading.Forms;

namespace uWatch
{
	public class DeviceListCell : MyViewCell
	{
		double font;


		protected override void OnBindingContextChanged()
		{

			try
			{

				if (Device.Idiom == TargetIdiom.Tablet)
				{
					font = 16;
				}
				else
				{
					font = 14;
				}


				base.OnBindingContextChanged();
				var account = BindingContext as DeviceStatic;

				var nameLabel = new Label()
				{
					FontSize = font + 2,
					FontAttributes = FontAttributes.Bold,
					TextColor = Color.Black,
					VerticalOptions = LayoutOptions.StartAndExpand,
					HorizontalOptions = LayoutOptions.Start,
				};
				nameLabel.SetBinding(Label.TextProperty, new Binding("FriendlyName", BindingMode.Default, stringFormat: "#{0}"));

				var friendlyNameLabel = new Label()
				{
					FontAttributes = FontAttributes.Bold,
					FontSize = font - 2,
					TextColor = Color.FromHex("#666"),
					VerticalOptions = LayoutOptions.End,
					HorizontalOptions = LayoutOptions.StartAndExpand,
					HorizontalTextAlignment = TextAlignment.Start,
					WidthRequest = 200,
				};
				friendlyNameLabel.SetBinding(Label.TextProperty, new Binding("imei", BindingMode.TwoWay, stringFormat: "IMEI: {0}"));


				var modelLabel = new Label()
				{
					FontAttributes = FontAttributes.Bold,
					FontSize = font - 2,
					TextColor = Color.FromHex("#666"),
					HorizontalTextAlignment = TextAlignment.Start,
					HorizontalOptions = LayoutOptions.StartAndExpand,
					VerticalOptions = LayoutOptions.Center
				};
				modelLabel.SetBinding(Label.TextProperty, new Binding("model_no", BindingMode.TwoWay, stringFormat: "Model: {0}"));


				var serialLabel = new Label()
				{
					FontAttributes = FontAttributes.Bold,
					FontSize = font - 2,
					TextColor = Color.FromHex("#666"),
					HorizontalTextAlignment = TextAlignment.Start,
					HorizontalOptions = LayoutOptions.StartAndExpand,
					VerticalOptions = LayoutOptions.End
				};
				serialLabel.SetBinding(Label.TextProperty, new Binding("serial_no", BindingMode.TwoWay, stringFormat: "S/N: {0}"));

				var layoutSerialAndModel = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.StartAndExpand };
				layoutSerialAndModel.Children.Add(modelLabel);
				layoutSerialAndModel.Children.Add(serialLabel);

				var lblDeviceSwitchStatus = new Label()
				{
					FontAttributes = FontAttributes.Bold,
					FontSize = font - 2,
					TextColor = Color.FromHex("#666"),
					HorizontalTextAlignment = TextAlignment.Start,
					HorizontalOptions = LayoutOptions.StartAndExpand,
					VerticalOptions = LayoutOptions.End
				};
				var devicestatus = account.DeviceSwitchStatus == null ? " " : account.DeviceSwitchStatus;
				lblDeviceSwitchStatus.SetBinding(Label.TextProperty, new Binding("DeviceSwitchStatus", BindingMode.Default, stringFormat: "DeviceSwitchStatus: " + devicestatus));

				var simExpiryDate = new Label()
				{
					FontAttributes = FontAttributes.Bold,
					FontSize = font - 2,
					TextColor = Color.FromHex("#666"),
					HorizontalTextAlignment = TextAlignment.Start,
					HorizontalOptions = LayoutOptions.StartAndExpand,
					VerticalOptions = LayoutOptions.End
				};
				var exdate = account.SimExpireDate==null ? "No Date Available":DateFormat.GetDateTime(account.SimExpireDate.Value.Date, TimeType.OnlyDate);
				simExpiryDate.SetBinding(Label.TextProperty, new Binding("SimExpireDate", BindingMode.Default, stringFormat: "Sim Expiry Date: " + exdate));


				var firmware_version = new Label()
				{
					FontAttributes = FontAttributes.Bold,
					FontSize = font - 2,
					TextColor = Color.FromHex("#666"),
					HorizontalTextAlignment = TextAlignment.Start,
					HorizontalOptions = LayoutOptions.StartAndExpand,
					VerticalOptions = LayoutOptions.End
				};
				firmware_version.SetBinding(Label.TextProperty, new Binding("fw_version", BindingMode.Default, stringFormat: "FW: {0}"));

				var LastUpdatedfirmware_version = new Label()
				{
					FontAttributes = FontAttributes.Bold,
					FontSize = font - 2,
					TextColor = Color.FromHex("#666"),
					HorizontalTextAlignment = TextAlignment.Start,
					HorizontalOptions = LayoutOptions.StartAndExpand,
					VerticalOptions = LayoutOptions.End
				};

				var lastUpdated = account.LastUpdated == null ? "No Date Available" : DateFormat.GetDateTime(account.LastAlert, TimeType.DateAndTime);
				LastUpdatedfirmware_version.SetBinding(Label.TextProperty, new Binding("LastUpdated", BindingMode.Default, stringFormat: "Updated: " + lastUpdated));

				var layoutFirmWare = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.StartAndExpand };
				layoutFirmWare.Children.Add(firmware_version);
				layoutFirmWare.Children.Add(LastUpdatedfirmware_version);

				// Vet rating label and star image
				var starLabel = new Label()
				{
					FontSize = font - 2,
					TextColor = Color.Gray
				};
				

				var starImage = new CachedImage()
				{
					Source = "right_arrow.png",
					HeightRequest = 50,
					WidthRequest = 50,
					LoadingPlaceholder = ImageSource.FromFile("placeholder.png"),
				};



				

				var BatteryCondition = new Label()
				{
					FontAttributes = FontAttributes.Bold,
					FontSize = font - 2,
					TextColor = Color.FromHex("#666"),
					HorizontalTextAlignment = TextAlignment.Start,
					HorizontalOptions = LayoutOptions.StartAndExpand,
					VerticalOptions = LayoutOptions.End
				};
				var condition = account.Battery <= 0 ? 0 : account.Battery;
				BatteryCondition.SetBinding(Label.TextProperty, new Binding("Battery", BindingMode.Default, stringFormat: "Battery: " + condition + "%"));




				var Stack1 = new StackLayout()
				{
					HorizontalOptions = LayoutOptions.StartAndExpand,
					VerticalOptions = LayoutOptions.Start,
					Padding = new Thickness(10, 10, 0, 10),
					Children =
				{
					nameLabel,
					layoutSerialAndModel,
					friendlyNameLabel,
				    lblDeviceSwitchStatus,
					layoutFirmWare,
					simExpiryDate,
					BatteryCondition,
					
				}
				};

				var Stack2 = new StackLayout()
				{
					HorizontalOptions = LayoutOptions.StartAndExpand,
					VerticalOptions = LayoutOptions.Start,
					Padding = new Thickness(0, 10, 0, 10),
					MinimumWidthRequest = 100,
					Children =
				{


				}
				};
				var Stack3 = new StackLayout()
				{
					HorizontalOptions = LayoutOptions.EndAndExpand,
					VerticalOptions = LayoutOptions.CenterAndExpand,
					Padding = new Thickness(0, 0, 8, 0),
					Children =
				{
					starImage,
				}
				};

				var MainStackLayout = new StackLayout()
				{
					Orientation = StackOrientation.Horizontal,
					HorizontalOptions = LayoutOptions.Fill,
					VerticalOptions = LayoutOptions.Fill,
					Children =
				{
					Stack1,
					Stack3,
				}
				};

				View = MainStackLayout;
			}
			catch (System.Exception ex)
			{

			}

			}
		}

	}



