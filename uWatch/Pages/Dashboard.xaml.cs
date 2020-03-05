
using Xamarin.Forms;
using UwatchPCL;
using Acr.UserDialogs;
using UwatchPCL.Helpers;

namespace uWatch
{
	public partial class Dashboard : TabbedPage
	{
		RelativeLayout relativeLayout;

		public Page alertPage;
		public Page posPage;
		public Page monitorPage;
		public Page settingPage;

		double w = MyController.VirtualWidth;
		double h = MyController.VirtualHeight;

		public Dashboard ()
		{
			try
			{
			InitializeComponent ();
			SetLayout();
				}
			catch (System.Exception ex)
			{
				MyController.ErrorManagement(ex.Message);
			}
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
		}

		private async void SetLayout()
		{
			Title = "Cubes";

			alertPage = new DevicesPage ();
			alertPage.Icon = "alert.png";
			alertPage.Title = "Alerts";


			settingPage = new Login ();
			settingPage.Icon = "logout.png";
			settingPage.Title = "Log Out";

			Children.Add (alertPage);
			Children.Add (monitorPage);
			Children.Add (settingPage);

			CurrentPage = alertPage;
		}

		protected override async void OnCurrentPageChanged()
		{

			try
			{

			

			if (CurrentPage == settingPage)
			{
				string address = "Are you sure you want to Logout ?";
				var answer = await DisplayAlert("Confirmation", address, "Yes", "No");
				if (answer == true)
				{
					Settings.UserName = "";
					Settings.Password = "";
					Application.Current.MainPage = new Login();
				}
				else 
				{
					CurrentPage = alertPage;
				}
			}
				}
			catch 
			{

			}
		}

	}
}

