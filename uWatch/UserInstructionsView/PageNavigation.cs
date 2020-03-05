
using System;
using Xamarin.Forms;
using System.IO;
using System.Reflection;
using Acr.UserDialogs;
using UwatchPCL;
using UwatchPCL.Helpers;

namespace uWatch
{
	

	public class PageNavigation : ContentPage
	{
		ListView HeplSupport;
		
		
		StackLayout layoutDatePicker, layoutMainContent;
		public PageNavigation()
		{
			Title = "Navigating The Screens";
	        NavigationPage.SetHasNavigationBar(this, true);
			NavigationPage.SetBackButtonTitle(this, "");
			NavigationPage.SetHasBackButton(this, true);
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			InitializeComponent();
		}
		void InitializeComponent()
		{
			try
			{
				
				var layout = new StackLayout
				{
					VerticalOptions = LayoutOptions.StartAndExpand,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					BackgroundColor = Color.FromRgb(229, 229, 230)
				};
				var Summerylayout = new StackLayout
				{
					Padding = new Thickness(0, 8, 0, 8)
				};

				var SummeryStatus = new Label { Text = "Summary", TextColor = Color.Black, HorizontalOptions = LayoutOptions.CenterAndExpand };
				SummeryStatus.FontFamily = Device.OnPlatform(
					"Gibson-Regular",
					null,
					null
				);
				Summerylayout.Children.Add(SummeryStatus);
				layout.Children.Add(Summerylayout);



				//MenuPage


				var confirmationlayout = new StackLayout
				{
					Padding = new Thickness(14, 8, 14, 8),
					
					BackgroundColor = Color.White,
					VerticalOptions = LayoutOptions.StartAndExpand,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					Orientation = StackOrientation.Horizontal
				};




				var ConfImgLayout = new StackLayout { HorizontalOptions = LayoutOptions.StartAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand, Padding = 5 };
				var ImageAawaitConf = new Image { Source = "MenuNav.png", HeightRequest = 40, WidthRequest = 40 };
				ConfImgLayout.Children.Add(ImageAawaitConf);


				var ConfContLayout = new StackLayout { HorizontalOptions = LayoutOptions.StartAndExpand, Spacing = 2, VerticalOptions = LayoutOptions.CenterAndExpand, Orientation = StackOrientation.Horizontal };

				var confirmationDetail = new Label { Text = "Menu", WidthRequest = MyController.ScreenWidth - 200, TextColor = Color.Black, FontSize = 14, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.StartAndExpand };
				confirmationDetail.FontFamily = Device.OnPlatform(
					"Gibson-Regular",
					null,
					null
				);

				var awaitingconfirmationStatus = new Image { Source = "right_arrow1.png", HeightRequest = 17, WidthRequest = 17, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.EndAndExpand };


				ConfContLayout.Children.Add(confirmationDetail);
				ConfContLayout.Children.Add(awaitingconfirmationStatus);

				var ConfiLayoutMain = new StackLayout
				{
					Spacing = 12,
					HorizontalOptions = LayoutOptions.StartAndExpand,
					VerticalOptions = LayoutOptions.StartAndExpand,
					Orientation = StackOrientation.Horizontal
				};

				ConfiLayoutMain.Children.Add(ConfImgLayout);
				ConfiLayoutMain.Children.Add(ConfContLayout);

				confirmationlayout.Children.Add(ConfiLayoutMain);


				//AlertListPage
			    
					var AlertListlayout = new StackLayout
				    {
					Padding = new Thickness(14, 8, 14, 8),
					
					BackgroundColor = Color.White,
					VerticalOptions = LayoutOptions.StartAndExpand,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					Orientation = StackOrientation.Horizontal
				    };




				var AlertListImgLayout = new StackLayout { HorizontalOptions = LayoutOptions.StartAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand, Padding = 5 };
				var AlertListConf = new Image { Source = "AlertNav.png", HeightRequest = 40, WidthRequest = 40 };
				AlertListImgLayout.Children.Add(AlertListConf);


				var AlertListContLayout = new StackLayout { HorizontalOptions = LayoutOptions.StartAndExpand, Spacing = 2, VerticalOptions = LayoutOptions.CenterAndExpand, Orientation = StackOrientation.Horizontal };

				var AlertListDetail = new Label { Text = "Alerts", WidthRequest = MyController.ScreenWidth - 200, TextColor = Color.Black, FontSize = 14, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.StartAndExpand };
				AlertListDetail.FontFamily = Device.OnPlatform(
					"Gibson-Regular",
					null,
					null
				);

				var AlertListStatus = new Image { Source = "right_arrow1.png", HeightRequest = 17, WidthRequest = 17, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.EndAndExpand };


				AlertListContLayout.Children.Add(AlertListDetail);
				AlertListContLayout.Children.Add(AlertListStatus);

				var AlertListLayoutMain = new StackLayout
				{
					Spacing = 12,
					HorizontalOptions = LayoutOptions.StartAndExpand,
					VerticalOptions = LayoutOptions.StartAndExpand,
					Orientation = StackOrientation.Horizontal
				};

				AlertListLayoutMain.Children.Add(AlertListImgLayout);
				AlertListLayoutMain.Children.Add(AlertListContLayout);

				AlertListlayout.Children.Add(AlertListLayoutMain);



				//DevicePage
				var DevicePagelayout = new StackLayout
				{
					Padding = new Thickness(14, 8, 14, 8),
					
					BackgroundColor = Color.White,
					VerticalOptions = LayoutOptions.StartAndExpand,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					Orientation = StackOrientation.Horizontal
				};




				var DeviceListPageImgLayout = new StackLayout { HorizontalOptions = LayoutOptions.StartAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand, Padding = 5 };
				var ImageDevicePageConf = new Image { Source = "DeviceNav.png", HeightRequest = 40, WidthRequest = 40 };
				DeviceListPageImgLayout.Children.Add(ImageDevicePageConf);


				var ImageDevicePageContLayout = new StackLayout { HorizontalOptions = LayoutOptions.StartAndExpand, Spacing = 2, VerticalOptions = LayoutOptions.CenterAndExpand, Orientation = StackOrientation.Horizontal };

				var ImageDevicePageConfDetail = new Label { Text = "Cubes", WidthRequest = MyController.ScreenWidth - 200, TextColor = Color.Black, FontSize = 14, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.StartAndExpand };
				ImageDevicePageConfDetail.FontFamily = Device.OnPlatform(
					"Gibson-Regular",
					null,
					null
				);

				var DevicePageconfirmationStatus = new Image { Source = "right_arrow1.png", HeightRequest = 17, WidthRequest = 17, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.EndAndExpand };


				ImageDevicePageContLayout.Children.Add(ImageDevicePageConfDetail);
				ImageDevicePageContLayout.Children.Add(DevicePageconfirmationStatus);

				var DevicePageLayoutMain = new StackLayout
				{
					Spacing = 12,
					HorizontalOptions = LayoutOptions.StartAndExpand,
					VerticalOptions = LayoutOptions.StartAndExpand,
					Orientation = StackOrientation.Horizontal
				};

				DevicePageLayoutMain.Children.Add(DeviceListPageImgLayout);
				DevicePageLayoutMain.Children.Add(ImageDevicePageContLayout);

				DevicePagelayout.Children.Add(DevicePageLayoutMain);
			
				//MessagePage
				var MessagePagelayout = new StackLayout
				{
					Padding = new Thickness(14, 8, 14, 8),
					
					BackgroundColor = Color.White,
					VerticalOptions = LayoutOptions.StartAndExpand,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					Orientation = StackOrientation.Horizontal
				};




				var MessageListPageImgLayout = new StackLayout { HorizontalOptions = LayoutOptions.StartAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand, Padding = 5 };
				var ImageMessagePageConf = new Image { Source = "MessageNav.png", HeightRequest = 40, WidthRequest = 40 };
				MessageListPageImgLayout.Children.Add(ImageMessagePageConf);


				var ImageMessagePageContLayout = new StackLayout { HorizontalOptions = LayoutOptions.StartAndExpand, Spacing = 2, VerticalOptions = LayoutOptions.CenterAndExpand, Orientation = StackOrientation.Horizontal };

				var ImageMessagePageConfDetail = new Label { Text = "Messages", WidthRequest = MyController.ScreenWidth - 200, TextColor = Color.Black, FontSize = 14, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.StartAndExpand };
				ImageDevicePageConfDetail.FontFamily = Device.OnPlatform(
					"Gibson-Regular",
					null,
					null
				);

				var MessagePageconfirmationStatus = new Image { Source = "right_arrow1.png", HeightRequest = 17, WidthRequest = 17, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.EndAndExpand };


				ImageMessagePageContLayout.Children.Add(ImageMessagePageConfDetail);
				ImageMessagePageContLayout.Children.Add(MessagePageconfirmationStatus);

				var MessagePageLayoutMain = new StackLayout
				{
					Spacing = 12,
					HorizontalOptions = LayoutOptions.StartAndExpand,
					VerticalOptions = LayoutOptions.StartAndExpand,
					Orientation = StackOrientation.Horizontal
				};

				MessagePageLayoutMain.Children.Add(MessageListPageImgLayout);
				MessagePageLayoutMain.Children.Add(ImageMessagePageContLayout);

				MessagePagelayout.Children.Add(MessagePageLayoutMain);

				//
				//MemberPage
		
				//

				TapGestureRecognizer _messagegesture = new TapGestureRecognizer();
				_messagegesture.Tapped += (sender, e) =>
				  {
					  Navigation.PushAsync(new DetailsPage("messages.png","Message Page"));
				  };
				MessagePagelayout.GestureRecognizers.Add(_messagegesture);
				TapGestureRecognizer _Devicegesture = new TapGestureRecognizer();
				_Devicegesture.Tapped += (sender, e) =>
				  {
					  Navigation.PushAsync(new DetailsPage("devicepage.png","Cubes"));
				  };
				DevicePagelayout.GestureRecognizers.Add(_Devicegesture);
				TapGestureRecognizer _menugesture = new TapGestureRecognizer();
				_menugesture.Tapped += (sender, e) =>
				  {
					 
						  Navigation.PushAsync(new DetailsPage("menuHelp.png","Menu Page"));
					 
				  };
				confirmationlayout.GestureRecognizers.Add(_menugesture);

				TapGestureRecognizer _AlertListPagelayout = new TapGestureRecognizer();
				_AlertListPagelayout.Tapped += (sender, e) =>
				  {
					  Navigation.PushAsync(new DetailsPage("alertpage.png","Alerts"));
				  };
				AlertListlayout.GestureRecognizers.Add(_AlertListPagelayout);
				var layoutContain = new StackLayout { Orientation = StackOrientation.Vertical, Padding = new Thickness(18, 13, 18, 0), Spacing = 26, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand };
				layoutContain.Children.Add(confirmationlayout);
				if (Settings.RoleID != 8)
				{
					layoutContain.Children.Add(AlertListlayout);
					layoutContain.Children.Add(DevicePagelayout);
				}
				layoutContain.Children.Add(MessagePagelayout);

				var layoutContent = new StackLayout
				{
					Spacing = 0,
					BackgroundColor = Color.FromRgb(245, 245, 245),
					VerticalOptions = LayoutOptions.FillAndExpand,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					Padding = new Thickness(0, 0, 0, 0)
				};
				layoutContent.Children.Add(layout);
				layoutContent.Children.Add(layoutContain);


				ScrollView sc = new ScrollView();
				sc.Content = layoutContent;

				var LayoutstatusOfUpload = new StackLayout
				{
					VerticalOptions = LayoutOptions.EndAndExpand,
					HorizontalOptions = LayoutOptions.CenterAndExpand,
					Padding = new Thickness(0, 5, 0, 5)
				};
				
				layoutMainContent = new StackLayout
				{
					Spacing = 0,
					BackgroundColor = Color.FromRgb(245, 245, 245),
					VerticalOptions = LayoutOptions.FillAndExpand,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					Padding = new Thickness(0),
				};


				layoutMainContent.Children.Add(sc);
				layoutMainContent.Children.Add(LayoutstatusOfUpload);
				this.Content = layoutMainContent;

				
			}
			catch (System.Exception ex)
			{
				
			}
		}
	}
}