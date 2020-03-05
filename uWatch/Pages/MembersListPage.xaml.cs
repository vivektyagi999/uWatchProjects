using System;
using System.Collections.Generic;
using Acr.UserDialogs;
using UwatchPCL;
using UwatchPCL.Model.Response;
using Xamarin.Forms;

namespace uWatch
{
	public partial class MembersListPage : ContentPage
	{
		double pddingH, paddingW;

		public MembersListPage(MemberViewModel viewmodel = null)
		{
			Title = "Member List";

		
            ToolbarItems.Add(new ToolbarItem("Map", "", () =>
            {
                MapToolBarClickAsync(viewmodel);

            }));

			InitializeComponent();
			lstView.ItemsSource = viewmodel.MemberList;
			lstView.LoadMoreCommand = viewmodel.LoadCharactersCommand;

			lstView.ItemTapped += (sender, e) => {
				var alert = e.Item as AddUserModel;

				GetPopupOfNotification(alert);
			};
		}
        private async void MapToolBarClickAsync(MemberViewModel viewmodel)
        {
            UserDialogs.Instance.ShowLoading("Loading...", MaskType.Gradient);
            List<LatLong> items = new List<LatLong>();

            ApiService api = new ApiService();

            foreach (var alert in viewmodel.MemberList)
            {


                var itemsofApi = await api.GetLatLongs(alert.ZipCode).ConfigureAwait(true);
                if (itemsofApi.status == "OVER_QUERY_LIMIT")
                {
                    MapToolBarClickAsync(viewmodel);
                    return;
                }
                foreach (var pos in itemsofApi.results)
                {
                    LatLong value = new LatLong();
                    value.lat = pos.geometry.location.lat;
                    value.lng = pos.geometry.location.lng;
                    items.Add(value);
                }

            }
            await Navigation.PushAsync(new MemberMapPage(items));
            UserDialogs.Instance.HideLoading();
        }
		public void GetPopupOfNotification(AddUserModel selectedmsg)
		{
				var CloseIcon = new Image { Source = "CloseIcon.png", HeightRequest = 25, WidthRequest = 25, HorizontalOptions = LayoutOptions.EndAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };
			var lblTitle = new Label { Text = "Member's Details", TextColor = Color.Black, FontAttributes = FontAttributes.Bold, FontSize = 19, HorizontalOptions = LayoutOptions.StartAndExpand };

			var layoytTitle = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
			layoytTitle.Children.Add(lblTitle);
			layoytTitle.Children.Add(CloseIcon);

			var lblHeading = new Label {Text = selectedmsg.FullName, FontSize = 17 };
			var lblFrndlyName = new Label { Text = selectedmsg.FriendlyName,FontSize = 16 };

			var divider = new BoxView { Color = Color.FromHex("dcdcdc"), HeightRequest = .5, HorizontalOptions = LayoutOptions.FillAndExpand };
			var layoutTitle = new StackLayout
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.StartAndExpand
			};
			layoutTitle.Children.Add(layoytTitle);
			layoutTitle.Children.Add(divider);

			var layoutEmail = new StackLayout { Orientation = StackOrientation.Horizontal };
			var imgEmail = new Image { Source = "email.png", HeightRequest = 25, WidthRequest = 25};
			var lblEmial = new Label { Text = selectedmsg.Email };
			layoutEmail.Children.Add(imgEmail);
			layoutEmail.Children.Add(lblEmial);

			TapGestureRecognizer layoutEmailt = new TapGestureRecognizer();
			layoutEmailt.Tapped += async (object sender, EventArgs e) =>
			{
				UserDialogs.Instance.ShowLoading("Connecting to mail service..!", MaskType.Gradient);
				DependencyService.Get<ISendMessage>().SendMail(selectedmsg.Email);
				UserDialogs.Instance.HideLoading();
			};

			layoutEmail.GestureRecognizers.Add(layoutEmailt);


			var layoutSMS = new StackLayout { Orientation = StackOrientation.Horizontal};
			var imgEmail1 = new Image { Source = "sms.png",HeightRequest = 25, WidthRequest = 25 };
			var lblEmial1 = new Label { Text = selectedmsg.Mobile1 };
			layoutSMS.Children.Add(imgEmail1);
			layoutSMS.Children.Add(lblEmial1);

			TapGestureRecognizer layoutSMSt = new TapGestureRecognizer();
			layoutSMSt.Tapped += async (object sender, EventArgs e) =>
			{
				UserDialogs.Instance.ShowLoading("Connecting to sms service..!", MaskType.Gradient);

				DependencyService.Get<ISendMessage>().SendMessages(selectedmsg.Mobile1);
				UserDialogs.Instance.HideLoading();
			};

			layoutSMS.GestureRecognizers.Add(layoutSMSt);


			var layoutPhone = new StackLayout { Orientation = StackOrientation.Horizontal };
			var imgPhone = new Image { Source = "call.png",HeightRequest = 25, WidthRequest = 25 };
			var lblPhone = new Label { Text = selectedmsg.Mobile1 };
			layoutPhone.Children.Add(imgPhone);
			layoutPhone.Children.Add(lblPhone);

			TapGestureRecognizer layoutPhonet = new TapGestureRecognizer();
			layoutPhonet.Tapped += async (object sender, EventArgs e) =>
			{
				UserDialogs.Instance.ShowLoading("Connecting to call service..!", MaskType.Gradient);

				DependencyService.Get<ISendMessage>().Call(selectedmsg.Mobile1);
				UserDialogs.Instance.HideLoading();
			};

			layoutPhone.GestureRecognizers.Add(layoutPhonet);



			var layouth = new StackLayout { Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.StartAndExpand, VerticalOptions = LayoutOptions.StartAndExpand, Spacing = 18 };

		
			layouth.Children.Add(lblHeading);
			layouth.Children.Add(lblFrndlyName);
			layouth.Children.Add(layoutEmail);
			layouth.Children.Add(layoutPhone);
			layouth.Children.Add(layoutSMS);

			var scrlMssg = new Xamarin.Forms.ScrollView();
			scrlMssg.Content = layouth;


			if (Device.Idiom == TargetIdiom.Phone)
			{
				//PaddingMain = 8;
				pddingH = .70;
				paddingW = .8;
			}
			if (Device.Idiom == TargetIdiom.Tablet)
			{
				//PaddingMain = 40;
				pddingH = .40;
				paddingW = .5;

			}
	;

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

			var mainlayout = new StackLayout
			{
				Padding = new Thickness(2),
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Children = {
					lblAddressLayout
				}
			};


			var popupLayouts = this.Content as CustomPopup;

			TapGestureRecognizer bigt = new TapGestureRecognizer();
			bigt.Tapped += async (object sender, EventArgs e) =>
			{
				popupLayouts.DismissPopup();
			};

			CloseIcon.GestureRecognizers.Add(bigt);

		
			if (popupLayouts.IsPopupActive)
			{

			}
			else {


				var view = new Frame
				{
					Padding = new Thickness(0, 0, 0, 0),
					HasShadow = false,
					OutlineColor = Color.Gray,
					HeightRequest = MyController.ScreenHeight/2,
					WidthRequest = MyController.ScreenWidth - 100,
					VerticalOptions = LayoutOptions.FillAndExpand,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					BackgroundColor = Color.FromHex("f2f2f2"),
					Content = mainlayout
				};

				popupLayouts.ShowPopup(view);
			}
		}
	}
}
