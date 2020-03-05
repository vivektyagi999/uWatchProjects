using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Acr.UserDialogs;
using UwatchPCL;
using Xamarin.Forms;

namespace uWatch
{
	public partial class EscalateAlert : ContentPage
	{
		RelativeLayout relativeLayout;
		ScrollView scrollview;
		int alertlogid = 0;
		public IUserDialogs userdialogs;
		EscalateAlertViewModel ViewModel;
		double w = MyController.VirtualWidth;
		double h = MyController.VirtualHeight;

		public EscalateAlert()
		{
			InitializeComponent();
			BackgroundColor = Color.Gray;
			userdialogs = UserDialogs.Instance;
		}

		public EscalateAlert(int AlertlogId)
		{
			this.alertlogid = AlertlogId;
			InitializeComponent();
			userdialogs = UserDialogs.Instance;
		}

		protected override void OnAppearing()
		{
			SetLayout();
			base.OnAppearing();
		}

		protected override void OnDisappearing()
		{
			System.GC.Collect();
			base.OnDisappearing();
		}

		private async void SetLayout()
		{
			Title = "Escalate Alert";

			relativeLayout = new RelativeLayout();
			AddLayout();
			scrollview = new ScrollView();
			scrollview.Content = relativeLayout;
			Content = scrollview;
		}

		private async void AddLayout()
		{
			double position = 50;
			double newx20 = MyUiUtils.getPercentual(w, 20);
			double newx40 = MyUiUtils.getPercentual(w, 40);
			double newx60 = MyUiUtils.getPercentual(w, 60);
			double newx80 = MyUiUtils.getPercentual(w, 80);

			var imgback = MyUILibrary.AddImage(relativeLayout, "backNewArrow.png", 0, position, 80, 80, Aspect.AspectFit);
			TapGestureRecognizer TapBack = new TapGestureRecognizer();
			TapBack.Tapped += (object sender, EventArgs e) =>
		   	{
				   Navigation.PopModalAsync(true);
			   };
			imgback.GestureRecognizers.Add(TapBack);

			position += 30;

			var lblAgentId = MyUILibrary.AddLabel(relativeLayout, "Agent Code :", 10, position+10, newx40, 50, Color.Black,18);
			var EntAgentId = MyUILibrary.AddEntry(relativeLayout, "", "Enter Agent Code", newx40 + 10, position, w-60, 50, Color.Black, Keyboard.Text, TextAlignment.Start,false,16);
			EntAgentId.TextChanged +=  (sender, e) =>
			{
				var AgentUserName = EntAgentId.Text;
				ViewModel = new EscalateAlertViewModel();
				ViewModel.AgentUserName = AgentUserName;
				ViewModel.AlertLogId = this.alertlogid;
				BindingContext = ViewModel;
			};
			position += 50;

			var lblAgentName = MyUILibrary.AddLabel(relativeLayout, "Agent Name", 10, position + 10, newx40, 50, Color.Black, 18);
			var AgentName = MyUILibrary.AddLabel(relativeLayout, "name", 10, position + 40, newx40, 50, Color.Black, 18);
			AgentName.SetBinding(Label.TextProperty, new Binding("FullName", BindingMode.Default));
			position += 50;

			var lblAgentJoinDate = MyUILibrary.AddLabel(relativeLayout, "Member Since", 10, position + 10, newx40, 50, Color.Black, 18);
			var AgentJoinDate = MyUILibrary.AddLabel(relativeLayout, "date", 10, position + 40, newx40, 50, Color.Black, 18);

			position += 50;

			var lblAgentDetails = MyUILibrary.AddLabel(relativeLayout, "Agent Details", 10, position + 10, newx40, 50, Color.Black, 18);
			var AgentDetails = MyUILibrary.AddLabel(relativeLayout, "email/mobile", 10, position + 40, newx40, 50, Color.Black, 18);
			AgentDetails.SetBinding(Label.TextProperty, new Binding("Email", BindingMode.Default));
			position += 50;

			var btnDelete = MyUILibrary.AddButton(relativeLayout, "Esclate", 20 + 200 + 10, position + 50, 100, 50, Color.Red, Color.Gray, Color.White, 15);
			btnDelete.Clicked += async (object sender, EventArgs e) =>
			{
				string address = "Are you sure you want to Escalate this Alert?";
				var answer = await DisplayAlert("Escalate Alert", address, "Yes", "No");
				if (answer == true)
				{
					userdialogs.ShowLoading("Escalating Alert...", Acr.UserDialogs.MaskType.Gradient);
					await Task.Delay(1000);

					AlertsEsclatedToAgentViewModel req = new AlertsEsclatedToAgentViewModel();
					req.alertlog_idx = this.alertlogid;

					var res = await ApiService.Instance.EsclateAlert(req);
					userdialogs.HideLoading();

					await userdialogs.AlertAsync("Alert Escalated Successfully", "Information", "OK");

					var page = new MainPage();

					var masterPage = new MenuPages();
					page.Master = masterPage;
					page.nav = new NavigationPage(new DevicesPage()); 
					page.Detail = page.nav;
					Application.Current.MainPage = page;
				}
			};

		}
	}
}

