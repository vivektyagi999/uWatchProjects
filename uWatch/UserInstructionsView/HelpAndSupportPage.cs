using System;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using Acr.UserDialogs;
using System.Linq;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using UwatchPCL;

namespace uWatch
{
	public class HelpAndSupportPage : ContentPage
	{
		public Action<Page> OnMenuTap;
		ListView orderListView;
		string UpldatedOn;
		StackLayout layoutDatePicker, layoutMainContent;
		ToolbarItem Nav;
		double Left, Right, lengthStatus, lengthaddress, lengthAction, left, right;
		Label lblstatusOfUpload;
		Label IsorderList;
		public int SyncTime;
		public bool OnAppearFlag = true;
		int width1, width2, width3;
		static int i;

		string checkForFirstTimeLOGIN = "False";
		string checkForFirstTime = "True";
		string checkForFirstTimeSync = "True";

		string LastUpdatedDate, SelectedTab = "All";
		Image awaitingconfirmationStatus, awaitingUploadStatus, awaitingBookedStatus, awaitingAppointmnetStatus, imguserguide;



		public HelpAndSupportPage()
		{

			try
			{

				Title = "Help & Support";

				//handle toolbar item

				NavigationPage.SetHasNavigationBar(this, true);
				NavigationPage.SetBackButtonTitle(this, "");
				NavigationPage.SetHasBackButton(this, false);



				Init();
			}
			catch (Exception ex)
			{
				
			}
		}
		private async Task Init()
		{
			getdata();


		}
		public async void getdata()
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


				var confirmationlayout = new StackLayout
				{
					Padding = new Thickness(14, 8, 14, 8),
					BackgroundColor = Color.White,
					VerticalOptions = LayoutOptions.StartAndExpand,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					Orientation = StackOrientation.Horizontal
				};




				var ConfImgLayout = new StackLayout { HorizontalOptions = LayoutOptions.StartAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand, Padding = 5 };
				var ImageAawaitConf = new Image { Source = "Navigations.png", HeightRequest = 40, WidthRequest = 40 };
				ConfImgLayout.Children.Add(ImageAawaitConf);


				var ConfContLayout = new StackLayout { HorizontalOptions = LayoutOptions.StartAndExpand, Spacing = 2, VerticalOptions = LayoutOptions.CenterAndExpand, Orientation = StackOrientation.Horizontal };

				var confirmationDetail = new Label { Text = "Navigating The Screens", WidthRequest = MyController.ScreenWidth - 200, TextColor = Color.Black, FontSize = 14, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.StartAndExpand };
				confirmationDetail.FontFamily = Device.OnPlatform(
					"Gibson-Regular",
					null,
					null
				);

				awaitingconfirmationStatus = new Image { Source = "right_arrow1.png", HeightRequest = 17, WidthRequest = 17, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.EndAndExpand };


				ConfContLayout.Children.Add(confirmationDetail);
				ConfContLayout.Children.Add(awaitingconfirmationStatus);

				var ConfiLayoutMain = new StackLayout
				{
					Spacing = 12,
					HorizontalOptions = LayoutOptions.StartAndExpand,
					VerticalOptions = LayoutOptions.CenterAndExpand,
					Orientation = StackOrientation.Horizontal
				};

				ConfiLayoutMain.Children.Add(ConfImgLayout);
				ConfiLayoutMain.Children.Add(ConfContLayout);

				confirmationlayout.Children.Add(ConfiLayoutMain);


				//Appointment

				var Appointmnetlayout = new StackLayout
				{
					Padding = new Thickness(14, 8, 14, 8),
					
					BackgroundColor = Color.White,
					VerticalOptions = LayoutOptions.StartAndExpand,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					Orientation = StackOrientation.Horizontal
				};


				var AppointmnetImgLayout = new StackLayout { HorizontalOptions = LayoutOptions.StartAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand, Padding = 5 };
				var ImageAawaitAppointmnet = new Image { Source = "Glossarys.png", HeightRequest = 40, WidthRequest = 40 };
				AppointmnetImgLayout.Children.Add(ImageAawaitAppointmnet);


				var AppointmnetContLayout = new StackLayout { HorizontalOptions = LayoutOptions.StartAndExpand, Spacing = 2, VerticalOptions = LayoutOptions.CenterAndExpand, Orientation = StackOrientation.Horizontal };

				var AppointmnetDetail = new Label { Text = "Glossary", WidthRequest = MyController.ScreenWidth - 200, TextColor = Color.Black, FontSize = 14, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.StartAndExpand };
				AppointmnetDetail.FontFamily = Device.OnPlatform(
					"Gibson-Regular",
					null,
					null
				);

				awaitingAppointmnetStatus = new Image { Source = "right_arrow1.png", HeightRequest = 17, WidthRequest = 17, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.EndAndExpand };


				AppointmnetContLayout.Children.Add(AppointmnetDetail);
				AppointmnetContLayout.Children.Add(awaitingAppointmnetStatus);

				var AppointmnetLayoutMain = new StackLayout
				{
					Spacing = 12,
					HorizontalOptions = LayoutOptions.StartAndExpand,
					VerticalOptions = LayoutOptions.CenterAndExpand,
					Orientation = StackOrientation.Horizontal
				};

				AppointmnetLayoutMain.Children.Add(AppointmnetImgLayout);
				AppointmnetLayoutMain.Children.Add(AppointmnetContLayout);

				Appointmnetlayout.Children.Add(AppointmnetLayoutMain);

				//----


				var Uploadlayout = new StackLayout
				{
					Padding = new Thickness(14, 8, 14, 8),
					
					BackgroundColor = Color.White,
					VerticalOptions = LayoutOptions.StartAndExpand,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					Orientation = StackOrientation.Horizontal
				};


				var UploadImgLayout = new StackLayout { HorizontalOptions = LayoutOptions.StartAndExpand, VerticalOptions = LayoutOptions.StartAndExpand, Padding = 5 };
				var ImageAawaitUpload = new Image { Source = "ConfigNew.png", HeightRequest = 40, WidthRequest = 40 };
				UploadImgLayout.Children.Add(ImageAawaitUpload);


				var UploadContLayout = new StackLayout { HorizontalOptions = LayoutOptions.StartAndExpand, Spacing = 2, VerticalOptions = LayoutOptions.CenterAndExpand, Orientation = StackOrientation.Horizontal };

				var UploadDetail = new Label { Text = "Configuration", WidthRequest = MyController.ScreenWidth - 200, VerticalOptions = LayoutOptions.CenterAndExpand, FontSize = 14, TextColor = Color.Black, HorizontalOptions = LayoutOptions.StartAndExpand };
				UploadDetail.FontFamily = Device.OnPlatform(
					"Gibson-Regular",
					null,
					null
				);

				awaitingUploadStatus = new Image { Source = "right_arrow1.png", HeightRequest = 17, WidthRequest = 17, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.EndAndExpand };


				UploadContLayout.Children.Add(UploadDetail);
				UploadContLayout.Children.Add(awaitingUploadStatus);


				var uploadlayoutMain = new StackLayout { VerticalOptions = LayoutOptions.CenterAndExpand, Spacing = 12, HorizontalOptions = LayoutOptions.StartAndExpand, Orientation = StackOrientation.Horizontal };

				uploadlayoutMain.Children.Add(UploadImgLayout);
				uploadlayoutMain.Children.Add(UploadContLayout);

				Uploadlayout.Children.Add(uploadlayoutMain);


				//Booked order

				var Bookedlayout = new StackLayout
				{
					Padding = new Thickness(14, 8, 14, 8),
					
					BackgroundColor = Color.White,
					VerticalOptions = LayoutOptions.StartAndExpand,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					Orientation = StackOrientation.Horizontal
				};


				var BookedImgLayout = new StackLayout { HorizontalOptions = LayoutOptions.StartAndExpand, VerticalOptions = LayoutOptions.StartAndExpand, Padding = 5 };
				var ImageAawaitBooked = new Image { Source = "TroubleShootings.png", HeightRequest = 40, WidthRequest = 40 };
				BookedImgLayout.Children.Add(ImageAawaitBooked);


				var BookedContLayout = new StackLayout { HorizontalOptions = LayoutOptions.StartAndExpand, Spacing = 2, VerticalOptions = LayoutOptions.CenterAndExpand, Orientation = StackOrientation.Horizontal };

				var BookedDetail = new Label { Text = "Trouble Shooting", WidthRequest = MyController.ScreenWidth - 200, FontSize = 14, TextColor = Color.Black, HorizontalOptions = LayoutOptions.StartAndExpand };
				BookedDetail.FontFamily = Device.OnPlatform(
					"Gibson-Regular",
					null,
					null
				);

				awaitingBookedStatus = new Image { Source = "right_arrow1.png", HeightRequest = 17, WidthRequest = 17, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.EndAndExpand };


				BookedContLayout.Children.Add(BookedDetail);
				BookedContLayout.Children.Add(awaitingBookedStatus);


				var BookedlayoutMain = new StackLayout { VerticalOptions = LayoutOptions.CenterAndExpand, Spacing = 12, HorizontalOptions = LayoutOptions.StartAndExpand, Orientation = StackOrientation.Horizontal };

				BookedlayoutMain.Children.Add(BookedImgLayout);
				BookedlayoutMain.Children.Add(BookedContLayout);

				Bookedlayout.Children.Add(BookedlayoutMain);


			
				//User Guide

				var UserGuideDetaillayout = new StackLayout
				{
					Padding = new Thickness(14, 8, 14, 8),
					
					BackgroundColor = Color.White,
					VerticalOptions = LayoutOptions.StartAndExpand,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					Orientation = StackOrientation.Horizontal
				};


				var UserGuideDetailImgLayout = new StackLayout { HorizontalOptions = LayoutOptions.StartAndExpand, VerticalOptions = LayoutOptions.StartAndExpand, Padding = 5 };
				var ImageUserGuideDetail = new Image { Source = "usermanual.png", HeightRequest = 40, WidthRequest = 40 };
				UserGuideDetailImgLayout.Children.Add(ImageUserGuideDetail);


				var UserGuideDetailContLayout = new StackLayout { HorizontalOptions = LayoutOptions.StartAndExpand, Spacing = 2, VerticalOptions = LayoutOptions.CenterAndExpand, Orientation = StackOrientation.Horizontal };

				var UserGuideDetail = new Label { Text = "User Guide", WidthRequest = MyController.ScreenWidth - 200, FontSize = 14, TextColor = Color.Black, HorizontalOptions = LayoutOptions.StartAndExpand };
				UserGuideDetail.FontFamily = Device.OnPlatform(
					"Gibson-Regular",
					null,
					null
				);

				imguserguide = new Image { Source = "right_arrow1.png", HeightRequest = 17, WidthRequest = 17, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.EndAndExpand };


				UserGuideDetailContLayout.Children.Add(UserGuideDetail);
				UserGuideDetailContLayout.Children.Add(imguserguide);


				var UserGuideDetailMain = new StackLayout { VerticalOptions = LayoutOptions.CenterAndExpand, Spacing = 12, HorizontalOptions = LayoutOptions.StartAndExpand, Orientation = StackOrientation.Horizontal };

				UserGuideDetailMain.Children.Add(UserGuideDetailImgLayout);
				UserGuideDetailMain.Children.Add(UserGuideDetailContLayout);

				UserGuideDetaillayout.Children.Add(UserGuideDetailMain);



				//---------- 


				var layoutContain = new StackLayout { Orientation = StackOrientation.Vertical, Padding = new Thickness(18, 13, 18, 0), Spacing = 26, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand };
				layoutContain.Children.Add(confirmationlayout);
				layoutContain.Children.Add(Appointmnetlayout);
				layoutContain.Children.Add(Uploadlayout);
				//layoutContain.Children.Add(Bookedlayout);
				layoutContain.Children.Add(UserGuideDetaillayout);


				var ConftapGestureRecognizer = new TapGestureRecognizer();

				ConfiLayoutMain.GestureRecognizers.Add(ConftapGestureRecognizer);

				ConftapGestureRecognizer.Tapped += async(object sender, EventArgs e) =>
				{
					UserDialogs.Instance.ShowLoading("Please wait...");
					await Task.Delay(50);
					await Navigation.PushAsync(new PageNavigation());
					UserDialogs.Instance.HideLoading();
				};


				var ApptapGestureRecognizer = new TapGestureRecognizer();

				Appointmnetlayout.GestureRecognizers.Add(ApptapGestureRecognizer);

				ApptapGestureRecognizer.Tapped += async(object sender, EventArgs e) =>
				{
					UserDialogs.Instance.ShowLoading("Please wait...");
					await Task.Delay(50);
					await Navigation.PushAsync(new GlossaryPage());
					UserDialogs.Instance.HideLoading();
				};



				var UploadGestureRecognizer = new TapGestureRecognizer();

				Uploadlayout.GestureRecognizers.Add(UploadGestureRecognizer);

				UploadGestureRecognizer.Tapped += async(object sender, EventArgs e) =>
				{
					UserDialogs.Instance.ShowLoading("Please wait...");
					await Task.Delay(50);
					await Navigation.PushAsync(new ConfigrationPage());
					UserDialogs.Instance.HideLoading();

				};

				var BookedGestureRecognizer = new TapGestureRecognizer();

				awaitingBookedStatus.GestureRecognizers.Add(BookedGestureRecognizer);

				BookedGestureRecognizer.Tapped += (object sender, EventArgs e) =>
				{

				


				};





				lblstatusOfUpload = new Label();
				lblstatusOfUpload.FontSize = 10;

				var tapGestureRecognizer = new TapGestureRecognizer();

				lblstatusOfUpload.GestureRecognizers.Add(tapGestureRecognizer);

				tapGestureRecognizer.Tapped += (object sender, EventArgs e) =>
				{
					
				};

				//Navigate to User Guide
				var UserGuideGestureRecognizer = new TapGestureRecognizer();

				UserGuideDetaillayout.GestureRecognizers.Add(UserGuideGestureRecognizer);

				UserGuideGestureRecognizer.Tapped += async(object sender, EventArgs e) =>
				{
					UserDialogs.Instance.ShowLoading("Please wait...");
					await Task.Delay(50);
					await Navigation.PushAsync(new UserGuide());
					UserDialogs.Instance.HideLoading();
				};

				var layoutContent = new StackLayout
				{
					Spacing = 0,
					BackgroundColor = Color.FromRgb(245, 245, 245),
					VerticalOptions = LayoutOptions.FillAndExpand,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					Padding = new Thickness(0, 0, 0, 0),
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
				LayoutstatusOfUpload.Children.Add(lblstatusOfUpload);
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

		// when page is dissapear the refresh process is stop till the page is appear again
		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			OnAppearFlag = false;
		}
	}
}