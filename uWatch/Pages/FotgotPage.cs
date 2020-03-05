using System;
using Xamarin.Forms;
using UwatchPCL;
using UwatchPCL.Helpers;
using System.Threading.Tasks;
using Acr.UserDialogs;
#if __ANDROID__
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
#endif

#if __ANDROID__
using Android.OS;
#endif


namespace uWatch
{
	public partial class ForgotPage : ContentPage
	{
		const int rowHeight = 190;
		RelativeLayout relativeLayout;
		ScrollView scrollview;

		int w = MyController.VirtualWidth;
		int H = MyController.VirtualHeight;
       
        public Button btnLogin;
		public Image Img_TopRedLine = null;
		public Image imgIcon = null;

		public Entry txtUsername;
		public Entry txtPassword;



		public double newx20 = 0;

		public ForgotPage ()
		{
			try
			{
			InitializeComponent ();
                NavigationPage.SetHasNavigationBar(this, false);
                SetLayout();
				}
			catch (Exception ex)
			{
				MyController.ErrorManagement(ex.Message);
			}
		}

		protected override async void OnAppearing()
		{
			try
			{
				base.OnAppearing();
			}
			catch (Exception ex)
			{
				MyController.ErrorManagement(ex.Message);
			}

		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
		}

		private void SetLayout()
		{
			relativeLayout = new RelativeLayout();
			relativeLayout.BackgroundColor = Color.White;
			AddLayout();
			scrollview = new ScrollView();
			scrollview.Content = relativeLayout;
			Content = scrollview;
		}

		private async void AddLayout()
		{
			double position = 0;
			newx20 = MyUiUtils.getPercentual(w, 20);
			double newx40 = MyUiUtils.getPercentual(w, 40);
			double newx60 = MyUiUtils.getPercentual(w, 60);
			double newx80 = MyUiUtils.getPercentual(w, 80);

			var imgTopRedLine = MyUILibrary.AddImage (relativeLayout, "red_line.png", 0, position+12, w, 10,Aspect.Fill);
			var imgBottomRedLine = MyUILibrary.AddImage (relativeLayout, "red_line.png", 0, H-36, w, 10,Aspect.Fill);
			Device.OnPlatform 
			(
				iOS: () => 
				{
					imgTopRedLine.TranslationY = position+12;

				}
			);
			
			position += 3;

			MyUILibrary.AddImage (relativeLayout, "uwatch_logo.png", (w-newx40)/2, position+40, newx40, newx40, Aspect.AspectFit);
			position +=20 + newx40;

			var lblLogin = MyUILibrary.AddLabel (relativeLayout, "Forgot Password", 0, position, w,55, Color.Black, 18);
			position += 55;
			lblLogin.HorizontalOptions = LayoutOptions.CenterAndExpand;
				
			var	msgInfo = MyUILibrary.AddLabel(relativeLayout, "Please follow link in your email to reset password",  (w - newx80) / 2, position + 15, newx80, 55, Color.FromRgb(173,83,81),15);
			msgInfo.IsVisible = false;

			txtUsername = MyUILibrary.AddEntry (relativeLayout, "", "Username", (w - newx80) / 2, position + 50, newx80,45, Color.Black, Keyboard.Default, TextAlignment.Start,fontsize:15);
			position += 45 + 25;
			txtUsername.SetBinding(Entry.TextProperty, new Binding("Username", BindingMode.TwoWay));
		
			btnLogin  = MyUILibrary.AddButton (relativeLayout, "Submit", (w - newx60) / 2, position + 80, newx60, 40, Color.Red, Color.Red, Color.White,15);
			position += 80 + 40;

			btnLogin.Clicked += async (object sender, EventArgs e) =>
			{
				if (string.IsNullOrEmpty(txtUsername.Text))
				{
					UserDialogs.Instance.Alert("Enter your username", "Alert", "OK");
				}
				else 
				{
					var r = await ApiService.Instance.SendMailForForgotPassword(txtUsername.Text).ConfigureAwait(true);
					if (string.IsNullOrEmpty(r.Email))
					{
						msgInfo.Text = "Invalid Username, please enter a valid username";
					}
					else
					{
						
					}
					msgInfo.IsVisible = true;
				}
			};

			var lblforgot = MyUILibrary.AddLabel (relativeLayout, "Go To Login", 0, position+20, w,55, Color.Blue, 15);
			TapGestureRecognizer TForgot = new TapGestureRecognizer();
			TForgot.Tapped += async (object sender, EventArgs e) =>
			{
				UserDialogs.Instance.ShowLoading("Loading...", MaskType.Gradient);
				await Navigation.PopModalAsync(true);
				UserDialogs.Instance.HideLoading();
			};
			lblforgot.GestureRecognizers.Add(TForgot);
			position += 20 + 55;
			lblforgot.HorizontalOptions = LayoutOptions.CenterAndExpand;

		}

	}
}

	