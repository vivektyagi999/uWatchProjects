using System;
using Xamarin.Forms;

namespace uWatch
{
	public class AddNewImagePopUp
	{
		public AddNewImagePopUp()
		{
			var CloseIcon = new Image { Source = "CloseIcon.png", HeightRequest = 25, WidthRequest = 25, HorizontalOptions = LayoutOptions.EndAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };

			var lblTitle = new Label { Text = "Choose Option", TextColor = Color.Black, FontAttributes = FontAttributes.Bold, FontSize = 19, HorizontalOptions = LayoutOptions.StartAndExpand };

			var layoytTitle = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
			layoytTitle.Children.Add(lblTitle);
			layoytTitle.Children.Add(CloseIcon);

			var divider = new BoxView { Color = Color.FromHex("dcdcdc"), HeightRequest = .5, HorizontalOptions = LayoutOptions.FillAndExpand };
			var layoutTitle = new StackLayout
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.StartAndExpand
			};
			layoutTitle.Children.Add(layoytTitle);
			layoutTitle.Children.Add(divider);


			var lblDetails = new Label { Text = "Choose your option :", TextColor = Color.Black, FontAttributes = FontAttributes.Bold, FontSize = 15, XAlign = TextAlignment.Start, VerticalOptions = LayoutOptions.StartAndExpand, HorizontalOptions = LayoutOptions.StartAndExpand };

			var BtnAddImage = new Xamarin.Forms.Button
			{
				Text = "   Add image    ",
				FontSize = 14,
				FontAttributes = FontAttributes.Bold,
				BackgroundColor = Color.Red,
				TextColor = Color.White,
			};

			var BtnAddImageWithLocation = new Xamarin.Forms.Button
			{
				Text = "    Add image with location   ",
				FontSize = 14,
				FontAttributes = FontAttributes.Bold,
				BackgroundColor = Color.Red,
				TextColor = Color.White,
			};

			var BtnCancelMessage = new Xamarin.Forms.Button
			{
				Text = "    Cancel    ",
				FontSize = 14,
				FontAttributes = FontAttributes.Bold,
				BackgroundColor = Color.Red,
				TextColor = Color.White,
			};

			var btnstack = new StackLayout
			{
				Orientation = StackOrientation.Vertical,
				Spacing = 8,
				Padding = new Thickness(0, 0, 0, 5),
				VerticalOptions = LayoutOptions.EndAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand
			};

			btnstack.Children.Add(BtnAddImage);
			btnstack.Children.Add(BtnAddImageWithLocation);
			//	btnstack.Children.Add(BtnEscalateToOtherByEmail);


			var layouth = new StackLayout { Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.StartAndExpand, VerticalOptions = LayoutOptions.StartAndExpand, Spacing = 18 };

			layouth.Children.Add(lblDetails);

			var scrlMssg = new Xamarin.Forms.ScrollView();
			scrlMssg.Content = layouth;


			var lblAddressTopLayout = new StackLayout
			{
				Spacing = 10,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.StartAndExpand,
				BackgroundColor = Color.FromHex("f2f2f2"),
			};
			lblAddressTopLayout.Children.Add(layoutTitle);
			lblAddressTopLayout.Children.Add(scrlMssg);


			var lblAddressLayout = new StackLayout
			{
				Padding = new Thickness(10, 12, 10, 10),
				Spacing = 10,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				BackgroundColor = Color.FromHex("f2f2f2"),
			};
			lblAddressLayout.Children.Add(lblAddressTopLayout);
			lblAddressLayout.Children.Add(btnstack);

			var mainlayout = new StackLayout
			{
				Padding = new Thickness(2),
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Children = {
					lblAddressLayout
				}
			};
		}
	}
	}