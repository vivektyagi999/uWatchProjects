using System;
using UwatchPCL;
using UwatchPCL.Model;
using Xamarin.Forms;

namespace uWatch
{
	public class AssetListViewCell: MyViewCell
	{
		double font;
		protected override void OnBindingContextChanged()
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
			var asset = BindingContext as DeviceAssetsModel;
			var nameLabel = new Label()
			{
				FontSize = font + 2,
				FontAttributes = FontAttributes.Bold,
				TextColor = Color.Black,
				VerticalOptions = LayoutOptions.StartAndExpand,
				HorizontalOptions = LayoutOptions.Start,
			};
			nameLabel.SetBinding(Label.TextProperty, new Binding("FriendlyName", BindingMode.Default, stringFormat: "#{0}"));

			var EmeiLbl = new Label()
			{
				
				FontSize = font-2,
				TextColor = Color.FromHex("#666"),
				VerticalOptions = LayoutOptions.StartAndExpand,
			};
			EmeiLbl.SetBinding(Label.TextProperty, new Binding("imei", BindingMode.Default, stringFormat: "Cube: #{0}"));

			var starImage = new Image()
			{
				Source = "right_arrow.png",
				HeightRequest = 50,
				WidthRequest = 50
			};

			var Stack1 = new StackLayout()
			{
				HorizontalOptions = LayoutOptions.StartAndExpand,
				VerticalOptions = LayoutOptions.Fill,
				Padding = new Thickness(10, 10, 0, 10),
				Children =
				{
					nameLabel,
					EmeiLbl
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
	}
}

