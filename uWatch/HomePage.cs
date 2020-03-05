using System;
using uWatch ;
using Xamarin.Forms;
using System.Threading.Tasks;
using UwatchPCL;

#if __ANDROID__
using Xamarin.Forms.Platform.Android;
using uWatch.Droid; 
using Android.Views;
#endif
#if __IOS__
using Xamarin.Forms.Platform.iOS;
using UIKit;
using CoreGraphics;
#endif

namespace uWatch
{
	public class HomePage : ContentPage
	{
		Image imgIcon, ImgRightIcon;
		StackLayout HeaderLayout, UpdateLayout,ListLayout, MainContentLayout, BottomLayout, TopContentLayout;
		BoxView DividerLine1,DividerLine2;
		Label lblInfoLogin;
		Button btnUpdate, btnInfoLogin;
		ActivityIndicator Loading;
		NativeListView nativeListView=new NativeListView(); 
		ListView lstView=new ListView();

		string user="";
		public HomePage ()
		{
			this.BackgroundColor = Color.White;
			#region test list view 
//			lstView.ItemsSource = ApiService.dl;;
//			lstView.ItemTemplate = new DataTemplate (() => {
//				var cell = new ViewCell ();
//				Grid grid = new Grid {
//
//					Padding = new Thickness (
//						left: 13, 
//						top: Device.OnPlatform (iOS: 0, Android: 0, WinPhone: 0),
//						right: 0, 
//						bottom: Device.OnPlatform (iOS: 2, Android: 0, WinPhone: 0)),
//
//					ColumnSpacing = 13,
//					ColumnDefinitions = {
//						new ColumnDefinition { Width = new GridLength (0, GridUnitType.Auto) },
//						new ColumnDefinition { Width = new GridLength (3, GridUnitType.Star) },
//					}
//
//				};
//				var Condition = new Label ();
//				Condition.FontSize = 13;
//				Condition.TextColor = Color.Black;
//				Condition.VerticalOptions = LayoutOptions.CenterAndExpand;
//				Condition.FontFamily = Device.OnPlatform (
//					"Roboto-Bold",
//					null,
//					null
//				);
//
//
////				var Images = new Image ();
////				//Images.Source = "checkImg.png";
////				Images.VerticalOptions = LayoutOptions.CenterAndExpand;
////				Images.HeightRequest = 30;
////				Images.WidthRequest = 30;
//				Condition.SetBinding (Label.TextProperty, new Binding ("FilterOptions"));
//				//Images.SetBinding (Image.SourceProperty, new Binding ("ImageName"));
//
//				//grid.Children.Add (Images, 0, 0);
//				gri\d.Children.Add(Condition, 1, 0);
//
//				cell.View = grid;
//				return cell;
//			});
			#endregion


			imgIcon = new Image{Source = ImageSource.FromFile("Icon.png"), HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand, WidthRequest= 50, HeightRequest=50 };
			ImgRightIcon = new Image{Source = ImageSource.FromFile("logoHeader.png"), HorizontalOptions = LayoutOptions.StartAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand,WidthRequest=250,HeightRequest=100    };
			HeaderLayout = new StackLayout{Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.StartAndExpand  };
			HeaderLayout.Children.Add (ImgRightIcon);
			HeaderLayout.Children.Add (imgIcon);

			DividerLine1 = new BoxView{HorizontalOptions = LayoutOptions.FillAndExpand, HeightRequest = 1, BackgroundColor = Color.Black};

				

			Loading = new ActivityIndicator ();
//			Loading.IsRunning = true;
			Loading.IsEnabled = true;
			Loading.BindingContext = this.BindingContext;
			Loading.Color = Color.Green;
			Loading.BackgroundColor = Color.Black;
			Loading.SetBinding (ActivityIndicator.IsVisibleProperty, "IsBusy");
			Loading.SetBinding (ActivityIndicator.IsRunningProperty,"IsBusy");

			btnUpdate = new Button{Text = "Touch to update",HeightRequest = 40, FontSize = 13, TextColor = Color.Gray, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };
			UpdateLayout = new StackLayout{HorizontalOptions = LayoutOptions.CenterAndExpand, Padding = new Thickness(6), Orientation = StackOrientation.Horizontal };
			UpdateLayout.Children.Add (Loading);
			UpdateLayout.Children.Add (btnUpdate);

			DividerLine2 = new BoxView{HorizontalOptions = LayoutOptions.FillAndExpand, HeightRequest = 1, BackgroundColor = Color.Black};

			TopContentLayout = new StackLayout {
				VerticalOptions = LayoutOptions.StartAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Spacing = 0
			};

			TopContentLayout.Children.Add (HeaderLayout);
			TopContentLayout.Children.Add (DividerLine1);
			TopContentLayout.Children.Add (UpdateLayout);
			TopContentLayout.Children.Add (DividerLine2);

			//nativeListView.Items =DataSource.GetList ();
			nativeListView.Items =ApiService.dl;
			ListLayout = new StackLayout {
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand
			};

			nativeListView.ItemSelected += OnItemSelected;

			ListLayout.Children.Add (nativeListView);   
					 

			btnInfoLogin = new Button{Text = "Not Logged in",HeightRequest = 40, FontSize = 13, TextColor = Color.White, HorizontalOptions = LayoutOptions.EndAndExpand, VerticalOptions = LayoutOptions.EndAndExpand };
			if (Application.Current.Properties.ContainsKey("Userid"))
			{
				btnInfoLogin.Text = Application.Current.Properties ["Userid"] as string ;
				// do something with id
			}
			BottomLayout = new StackLayout {
				Padding = new Thickness(5,8,5,8),
				VerticalOptions = LayoutOptions.EndAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				BackgroundColor = Color.Gray
			};
			BottomLayout.Children.Add (btnInfoLogin);


			MainContentLayout = new StackLayout {
				Padding = new Thickness(0,8,0,0),
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand
			};

			MainContentLayout.Children.Add (TopContentLayout);
			MainContentLayout.Children.Add (ListLayout);
			MainContentLayout.Children.Add (BottomLayout);

			btnUpdate.Clicked+= (object sender, EventArgs e) => {
				if (!Application.Current.Properties.ContainsKey("Userid"))
					return; 
				this.IsBusy = true;
				ApiService api = new ApiService();
				if(Loading.Color == Color.Green)
					Loading.Color = Color.White;
                 else
					Loading.Color = Color.Green;
				//var v= ApiService.Instance.GetDeviceList();  
				Device.BeginInvokeOnMainThread(() =>
					{
				nativeListView.Items =ApiService.dl;// .get.Instance.GetDeviceList();//    DataSource.GetList ();
					});
				this.IsBusy = false;
			};
			btnInfoLogin.Clicked+= (object sender, EventArgs e) => {
				GetPopupOfLogin();
			};
			var custom = new CustomPopup ();
			var popupLayouts = this.Content as CustomPopup;
			Content = custom;
			custom.Content = MainContentLayout;

		
		}
		async void OnItemSelected (object sender, SelectedItemChangedEventArgs e)
		{
			
			//await Navigation.PushModalAsync (new alertListPage((DeviceList)e.SelectedItem));
		}
		public async Task ExecuteLoginCommand()
		{

//			bool success =await ApiService.Instance.Login();
//				//bool success = await longinTask;
//				if (success)
//				{
//				Device.BeginInvokeOnMainThread(() =>
//						{
//							btnInfoLogin.Text = "hi andre"; 
//						});
//	
//
//				}
				

		}
		public void GetPopupOfLogin ()
		{
			var lblTitle = new Label{ Text = "Enter your log in details", FontAttributes = FontAttributes.Bold, FontSize = 18,TextColor =Color.Red,    HorizontalOptions = LayoutOptions.CenterAndExpand};

			var lblUserName = new Label{Text = "Username:", TextColor = Color.Gray, VerticalOptions = LayoutOptions.CenterAndExpand};
			var txtUsername = new CustomTextView{HorizontalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.FromRgb(242,242,242),WidthRequest = 150 };
			var dividerUserName = new BoxView {Color = Color.Gray, HorizontalOptions = LayoutOptions.FillAndExpand, HeightRequest = 1 };
			var txtLayoutUserName = new StackLayout{ Spacing = 0 };
			txtLayoutUserName.Children.Add (txtUsername);
			txtLayoutUserName.Children.Add (dividerUserName);
			var mainLayoutUserName = new StackLayout{ Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand };
			mainLayoutUserName.Children.Add (lblUserName);
			mainLayoutUserName.Children.Add (txtLayoutUserName);

			var lblPassword = new Label{Text = "Password:", TextColor = Color.Gray, VerticalOptions = LayoutOptions.CenterAndExpand};
			var txtPassword = new CustomTextView{HorizontalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.FromRgb(242,242,242),WidthRequest = 150 };
			var dividerPassword = new BoxView {Color = Color.Gray, HorizontalOptions = LayoutOptions.FillAndExpand, HeightRequest = 1 };
			var txtLayoutPassword= new StackLayout{ Spacing = 0 };
			txtLayoutPassword.Children.Add (txtPassword);
			txtLayoutPassword.Children.Add (dividerPassword);
			var mainLayoutPassword = new StackLayout{ Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand };
			mainLayoutPassword.Children.Add (lblPassword);
			mainLayoutPassword.Children.Add (txtLayoutPassword);

			var btnCancel = new Button{ Text = "CANCEL", TextColor = Color.Black,FontSize = 11,HeightRequest = 30, BorderRadius = 0, BackgroundColor = Color.Silver };
			var btnOk = new Button{ Text = "OK", TextColor = Color.Black,FontSize = 11,BorderRadius = 0,HeightRequest = 30, BackgroundColor = Color.Silver };
			var Layout = new StackLayout{ Orientation = StackOrientation.Horizontal,Spacing = 2, HorizontalOptions = LayoutOptions.FillAndExpand };
			Layout.Children.Add (btnCancel);
			Layout.Children.Add (btnOk);


			var mainlayout = new StackLayout { 
				Padding = new Thickness (4),
				Spacing = 7,
				HorizontalOptions= LayoutOptions.FillAndExpand,
				VerticalOptions= LayoutOptions.FillAndExpand,
				Children = {
					lblTitle,
					mainLayoutUserName,
					mainLayoutPassword,
					Layout
				}
			};

			var popupLayouts = this.Content as CustomPopup;

			btnCancel.Clicked+= (object sender, EventArgs e) => 
			{
				NavigationPage.SetHasBackButton (this, true);
				popupLayouts.DismissPopup ();
				//popupLayouts=null;  
			};
		
			btnOk.Clicked+= (object sender, EventArgs e) => 
			{
				NavigationPage.SetHasBackButton (this, true);
				popupLayouts.DismissPopup ();
				//popupLayouts.ac 
				user="nguiver@btinternet.com";//txtUsername.Text.Trim();
				//ApiService.Instance.Login( txtUsername.Text.Trim() , txtPassword.Text.Trim(), loginDone);  
//				ApiService.Instance.Login("nguiver@btinternet.com" ,"pa55w0rd", loginDone);  
				//Task.Run(()=> ExecuteLoginCommand()); 
			};

//			if (popupLayouts.IsPopupActive) {
//				
//			}
//			else {
//				btnCancel.WidthRequest = (this.Width * .8 / 2) - 6;
//				btnOk.WidthRequest = (this.Width * .8 / 2) - 6;
//				#if __IOS__
//				var view = new Frame {
//				Padding = new Thickness (0, 0, 0, 0),
//				HasShadow = true,
//				HeightRequest = this.Height * .3,
//				WidthRequest = this.Width * .8,
//				VerticalOptions= LayoutOptions.FillAndExpand,
//				HorizontalOptions = LayoutOptions.FillAndExpand,
//				BackgroundColor = Color.FromHex ("f2f2f2"),
//				Content = mainlayout
//				};
//				#endif
//				#if __ANDROID__
//				var view = new StackLayout {
//					Padding = new Thickness (0, 0, 0, 0),
//					HeightRequest = this.Height * .3,
//					WidthRequest = this.Width * .8,
//					BackgroundColor = Color.FromHex ("f2f2f2"),
//					VerticalOptions= LayoutOptions.FillAndExpand,
//					HorizontalOptions = LayoutOptions.FillAndExpand
//				};
//				view.Children.Add (mainlayout);
//				#endif
//
//				this.ToolbarItems.Clear ();
//				NavigationPage.SetHasBackButton (this, false);
//				popupLayouts.ShowPopup (view);
//			}
		}
		public void loginDone()
		{
			if (ApiService.UserId.Trim()!= "") {
				 
				Application.Current.Properties ["Userid"] = ApiService.UserId.Trim();
				Device.BeginInvokeOnMainThread(() =>
					{
						btnInfoLogin.Text = user; 
					});
			}
		}
	}
}


