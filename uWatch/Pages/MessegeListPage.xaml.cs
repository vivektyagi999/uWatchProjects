using System;
using System.Collections.Generic;
using Xamarin.Forms;
using UwatchPCL;
using System.Collections.ObjectModel;
using Acr.UserDialogs;
using System.Threading.Tasks;
using UwatchPCL.Helpers;
using System.Diagnostics;
using FFImageLoading.Forms;
using System.Linq;
using System.Text;
using System.Globalization;
using uWatch.ViewModels;

#if __ANDROID__
using Android.App;
using Android.Widget;
using uWatch.Droid;

#endif


namespace uWatch
{
	public partial class MessegeListPage : ContentPage
	{
		public static int _numberOfUnreadMessage;
		int j;
		int selectedindex;
		int PaddingMain;
		Label Device_idxLabel, Notification_idxLabel;
		Xamarin.Forms.RelativeLayout relativeLayout;
		Xamarin.Forms.ScrollView scrollview;
		double w = MyController.VirtualWidth;
		double h = MyController.VirtualHeight;
		Label lblerror;
		public InfiniteListView listViewInbox, listViewSentBox;
		public static string ListType = "";
		double pddingH, paddingW;
		DateTimeFormatInfo _DateTimeFormatInfo = new DateTimeFormatInfo();
		public ObservableCollection<MessageStatics> PopupData { get; set; }
		public DataTemplate Cell { get; private set; }
		public MessageViewModel ViewModel { get; set; }
		StackLayout ListViewLyout;
		public int filtervalue = 0;
		Label MessageLabel, Date;

		public MessegeListPage(MessageViewModel viewmodel = null)
		{
			ToolbarItem items = new ToolbarItem
			{
				Text = "Drafts"
			};
			NavigationPage.SetBackButtonTitle(this, "");
			items.Clicked += async (sender, e) =>
			 {
				 UserDialogs.Instance.ShowLoading("Loading...");
				 await Task.Delay(50);
				 await viewmodel.LoadDevicesDrafts();
				 await Navigation.PushAsync(new DraftListPage(viewmodel));
				 UserDialogs.Instance.HideLoading();
			 };

			this.ToolbarItems.Add(items);

			try
			{

				Title = "Messages";
				this.ViewModel = viewmodel;
				InitializeComponent();
				BindingContext = ViewModel;
				SetLayout();
			}
			catch (Exception ex)
			{
				MyController.ErrorManagement(ex.Message);
			}
		}

		private void SetLayout()
		{
			relativeLayout = new Xamarin.Forms.RelativeLayout { VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand };
			AddLayout();
		}

		private void AddLayout()
		{
			try
			{
				double _Padding = 1;

				if (Xamarin.Forms.Device.Idiom == TargetIdiom.Phone)
				{
					_Padding = 1;
				}
				else {
					_Padding = 5;
				}

				var layoutTbMain = new StackLayout { Padding = new Thickness(0, 5, 0, 0), VerticalOptions = LayoutOptions.StartAndExpand, BackgroundColor = Color.White };
				var layoutTb = new StackLayout { HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand, BackgroundColor = Color.Red, Orientation = StackOrientation.Horizontal };
				var lblInboxlayout = new StackLayout { Padding = _Padding, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.CenterAndExpand };
				var lblInbox = new Label { TextColor = Color.White, WidthRequest = 80, Text = "Inbox", HorizontalTextAlignment = TextAlignment.Center, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.CenterAndExpand };
				var devider = new BoxView { BackgroundColor = Color.White, WidthRequest = 1, VerticalOptions = LayoutOptions.FillAndExpand };
				var lblSendBoxlayout = new StackLayout { Padding = _Padding, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.CenterAndExpand };
				var lblSendBox = new Label { TextColor = Color.FromRgb(216, 181, 178), WidthRequest = 80, Text = "Sent", HorizontalTextAlignment = TextAlignment.Center, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.CenterAndExpand };
				lblInboxlayout.Children.Add(lblInbox);
				lblSendBoxlayout.Children.Add(lblSendBox);
				layoutTb.Children.Add(lblInboxlayout);
				layoutTb.Children.Add(devider);
				layoutTb.Children.Add(lblSendBoxlayout);
				layoutTbMain.Children.Add(layoutTb);

				TapGestureRecognizer lblInboxTap = new TapGestureRecognizer();
				lblInboxTap.Tapped += async (object sender, EventArgs e) =>
				{
					try
					{
						Acr.UserDialogs.UserDialogs.Instance.ShowLoading();
						await Task.Delay(100);
						await ViewModel.LoadDevices("True");
						listViewInbox.ItemsSource = null;
						listViewInbox.BindingContext = ViewModel;
						Acr.UserDialogs.UserDialogs.Instance.HideLoading();
						listViewInbox.SetBinding(Xamarin.Forms.ListView.ItemsSourceProperty, new Binding("MessageListInbox"));

						listViewInbox.IsVisible = true;
						listViewSentBox.IsVisible = false;

						lblInbox.TextColor = Color.White;
						lblSendBox.TextColor = Color.FromRgb(216, 181, 178);

						if (ViewModel.MessageListInbox.Count > 0)
						{
							lblerror.IsVisible = false;
							listViewInbox.IsVisible = true;
							listViewSentBox.IsVisible = false;
							ListViewLyout.VerticalOptions = LayoutOptions.StartAndExpand;

						}
						else
						{
							lblerror.IsVisible = true;
							listViewInbox.IsVisible = false;
							listViewSentBox.IsVisible = false;
							ListViewLyout.VerticalOptions = LayoutOptions.FillAndExpand;
						}
						UserDialogs.Instance.HideLoading();
					}
					catch
					{
						UserDialogs.Instance.HideLoading();
					}

				};

				lblInboxlayout.GestureRecognizers.Add(lblInboxTap);

				TapGestureRecognizer lblSendBoxTap = new TapGestureRecognizer();
				lblSendBoxTap.Tapped += async (object sender, EventArgs e) =>
				{
					try
					{
						Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Loading...");
						await Task.Delay(100);
						await ViewModel.LoadDevicesSendBox("True");
						listViewSentBox.ItemsSource = null;
						listViewSentBox.BindingContext = ViewModel;
						listViewSentBox.SetBinding(Xamarin.Forms.ListView.ItemsSourceProperty, new Binding("MessageListSendbox"));
						Acr.UserDialogs.UserDialogs.Instance.HideLoading();

						listViewInbox.IsVisible = false;
						listViewSentBox.IsVisible = true;


						lblInbox.TextColor = Color.FromRgb(216, 181, 178);
						lblSendBox.TextColor = Color.White;
						UserDialogs.Instance.HideLoading();

						if (ViewModel.MessageListSendbox.Count > 0)
						{
							lblerror.IsVisible = false;
							listViewInbox.IsVisible = false;
							listViewSentBox.IsVisible = true;
							ListViewLyout.VerticalOptions = LayoutOptions.StartAndExpand;

						}
						else
						{
							lblerror.IsVisible = true;
							listViewInbox.IsVisible = false;
							listViewSentBox.IsVisible = false;
							ListViewLyout.VerticalOptions = LayoutOptions.FillAndExpand;
						}
						UserDialogs.Instance.HideLoading();
					}
					catch
					{

					}
				};

				lblSendBoxlayout.GestureRecognizers.Add(lblSendBoxTap);

				//for inbox

				listViewInbox = new InfiniteListView();
				listViewInbox.BindingContext = ViewModel;
				listViewInbox.SetBinding(Xamarin.Forms.ListView.ItemsSourceProperty, new Binding("MessageListInbox"));
				listViewInbox.LoadMoreCommand = ViewModel.LoadCharactersCommand;
				listViewInbox.HeightRequest = App.ScreenHeight - 140;
				listViewInbox.SeparatorColor = Color.Transparent;
				listViewInbox.RowHeight = 100;

				listViewInbox.ItemTemplate = new DataTemplate(typeof(MessegesListInboxCell));


				listViewInbox.ItemSelected += (sender, e) =>
				{
					try
					{

						if (e.SelectedItem != null)
						{
							selectedindex = ViewModel.MessageListInbox.IndexOf((InMailModel)e.SelectedItem);
							var message = (InMailModel)e.SelectedItem;
							ListType = "1";
							GetPopupOfNotification(message);
							listViewInbox.SelectedItem = null;

						}
					}
					catch (System.Exception ex)
					{
					}

				};


				//for sentbox listview

				listViewSentBox = new InfiniteListView();
				listViewSentBox.IsVisible = false;
				listViewSentBox.BindingContext = ViewModel;
				listViewSentBox.SetBinding(Xamarin.Forms.ListView.ItemsSourceProperty, new Binding("MessageListSendbox"));
				listViewSentBox.LoadMoreCommand = ViewModel.LoadCharactersCommandSend;
				listViewSentBox.RowHeight = 100;
				listViewSentBox.HeightRequest = App.ScreenHeight - 140;
				listViewSentBox.SeparatorColor = Color.Transparent;
				listViewSentBox.ItemTemplate = new DataTemplate(typeof(MessegesListSentBoxCell));

				listViewSentBox.ItemTapped += (sender, e) =>
				{
					try
					{

						if (e.Item != null)
						{
							selectedindex = ViewModel.MessageListSendbox.IndexOf((InMailModel)e.Item);
						    var message = (InMailModel)e.Item; //as MessageViewModel;
							listViewSentBox.SelectedItem = null;
							ListType = "2";
							GetPopupOfNotification(message);
							//
						}
					}
					catch(Exception ex) { 
					}

				};


				double newx80 = MyUiUtils.getPercentual(w, 80);
				double newy30 = MyUiUtils.getPercentual(h, 30);

				var imgAddButton = new Image { Source = "addbtnUwatch.png", HeightRequest = 50, WidthRequest = 50 };
				TapGestureRecognizer imgAddButtonTap = new TapGestureRecognizer();
				imgAddButtonTap.Tapped += async (object sender, EventArgs e) =>
				{
					UserDialogs.Instance.ShowLoading("Please wait..", MaskType.Gradient);
					await Task.Delay(500);
					try
					{
						GetPopupOfCreateMessage();
						UserDialogs.Instance.HideLoading();

					}
					catch
					{
						UserDialogs.Instance.HideLoading();
					}
				};

				imgAddButton.GestureRecognizers.Add(imgAddButtonTap);
				ListViewLyout = new StackLayout { BackgroundColor = Color.Transparent, HorizontalOptions = LayoutOptions.FillAndExpand };

				ListViewLyout.Children.Add(layoutTbMain);
				lblerror = new Label { IsVisible = false, Text = "No Message to display", FontSize = 18, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };


				if (ViewModel.MessageListInbox.Count > 0)
				{
					ListViewLyout.VerticalOptions = LayoutOptions.StartAndExpand;
					lblerror.IsVisible = false;
					listViewInbox.IsVisible = true;
					listViewSentBox.IsVisible = false;
				}
				else
				{
					ListViewLyout.VerticalOptions = LayoutOptions.FillAndExpand;
					lblerror.IsVisible = true;
					listViewInbox.IsVisible = false;
					listViewSentBox.IsVisible = false;
				}

				var layoutListView = new StackLayout { VerticalOptions = LayoutOptions.StartAndExpand };

				layoutListView.Children.Add(listViewInbox);
				layoutListView.Children.Add(listViewSentBox);
				ListViewLyout.Children.Add(lblerror);
				ListViewLyout.Children.Add(layoutListView);


				var layoutAddButton = new Xamarin.Forms.RelativeLayout { VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand };
				relativeLayout.Children.Add(ListViewLyout, Constraint.Constant(0), Constraint.Constant(0), Constraint.Constant(MyController.ScreenWidth), Constraint.Constant(MyController.ScreenHeight + 70));
				layoutAddButton.Children.Add(relativeLayout, Constraint.Constant(0), Constraint.Constant(0), Constraint.Constant(MyController.ScreenWidth), Constraint.Constant(MyController.ScreenHeight + 70));
				relativeLayout.Children.Add(imgAddButton, Constraint.Constant(MyController.ScreenWidth - 80), Constraint.Constant(MyController.ScreenHeight - 150), Constraint.Constant(50), Constraint.Constant(50));

				var custom = new CustomPopup();
				var popupLayouts = this.Content as CustomPopup;
				Content = custom;
				custom.Content = layoutAddButton;
			}
			catch (Exception ex)
			{

			}
		}
		public async Task DeleteMessage(string MailId)
		{
			try
			{
				var result = await ApiService.Instance.DeleteMessage(MailId);
				if (result)
				{
					if (ListType == "1")
					{
						await ViewModel.LoadDevices();
						await Task.Delay(500);
						var lst = listViewInbox.ItemsSource as ObservableCollection<InMailModel>;
						lst.RemoveAt(selectedindex);

					}
					else
					{
						await ViewModel.LoadDevicesSendBox();
						await Task.Delay(500);
						var lst = listViewSentBox.ItemsSource as ObservableCollection<InMailModel>;
						lst.RemoveAt(selectedindex);
					}
					}
				else
				{
					UserDialogs.Instance.HideLoading();
					await UserDialogs.Instance.AlertAsync("Failed please try after some time!!", "Information", "Ok");
				}

			}
			catch (System.Exception)
			{
				UserDialogs.Instance.HideLoading();
			}
		}
		public async void ignoreThisFunction(int NotificationId, bool ThisOnly)
		{
			UserDialogs.Instance.ShowLoading("Loading...");

			ApiService service = new ApiService();

			IgnoreNotificationReq req = new IgnoreNotificationReq();
			req.NotificationID = Convert.ToInt32(NotificationId);
			req.AllNotification = ThisOnly;



			var result = await service.IgnoreNotification(req).ConfigureAwait(true);
			if (result.ToString() == "0")
			{
				if (ThisOnly)
				{
					ListViewLyout.Children.Remove(listViewInbox);

					var MessageviewModel = new MessageViewModel();
					await MessageviewModel.LoadDevices();
					this.ViewModel = MessageviewModel;
					BindingContext = ViewModel;
					AddLayout();

				}
				else {
					ViewModel.MessageListSendbox.RemoveAt(selectedindex);
				}
				UserDialogs.Instance.HideLoading();
				UserDialogs.Instance.ShowSuccess("Done", 2000);
			}

			UserDialogs.Instance.HideLoading();
		}


		public async void AlwaysignoreFunction()
		{
			UserDialogs.Instance.ShowLoading("Loading...");
			var DeviceId = Device_idxLabel.Text;
			ApiService service = new ApiService();

			var result = await service.AlwaysIgnoreNotification(Convert.ToInt32(DeviceId)).ConfigureAwait(true);
			if (result.ToString() == "1")
			{
				var MessageviewModel = new MessageViewModel();
				await MessageviewModel.LoadDevices();
				UserDialogs.Instance.HideLoading();
				UserDialogs.Instance.ShowSuccess("Done", 2000);
			}

			UserDialogs.Instance.HideLoading();
		} 
		public async Task GetPopupOfNotification(InMailModel selectedmsg)
		{
			Label lbldatetime = new Label();
			var CloseIcon = new Image { Source = "CloseIcon.png", HeightRequest = 35, WidthRequest = 35, HorizontalOptions = LayoutOptions.EndAndExpand };
			var lblTitle = new Label { Text = "Message Details", TextColor = Color.Black, FontAttributes = FontAttributes.Bold, FontSize = 19, HorizontalOptions = LayoutOptions.StartAndExpand };

			var stklbl = new StackLayout { Padding = new Thickness(0, 10, 0, 0), HorizontalOptions = LayoutOptions.StartAndExpand, VerticalOptions = LayoutOptions.StartAndExpand };
			stklbl.Children.Add(lblTitle);

			var stkclose = new StackLayout { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
			stkclose.Children.Add(CloseIcon);

			var layoytTitle = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
			layoytTitle.Children.Add(stklbl);
			layoytTitle.Children.Add(stkclose);

			var divider = new BoxView { Color = Color.FromHex("dcdcdc"), HeightRequest = .5, HorizontalOptions = LayoutOptions.FillAndExpand };
			var layoutTitle = new StackLayout
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.StartAndExpand
			};
			layoutTitle.Children.Add(layoytTitle);
			layoutTitle.Children.Add(divider);


			var lblDetails = new Label { Text = "Are you sure you want to Ignore Notification from this Device?", TextColor = Color.Black, FontAttributes = FontAttributes.Bold, FontSize = 15, XAlign = TextAlignment.Start, VerticalOptions = LayoutOptions.StartAndExpand, HorizontalOptions = LayoutOptions.StartAndExpand };

			var lblMessage = new Label { Text = "Title: " + selectedmsg.Title, FontAttributes = FontAttributes.Bold, FontSize = 15, XAlign = TextAlignment.Start, VerticalOptions = LayoutOptions.StartAndExpand, HorizontalOptions = LayoutOptions.StartAndExpand };
			var lblMessageDesc = new Label { Text = "Body: " + selectedmsg.Body, FontSize = 14, XAlign = TextAlignment.Start, VerticalOptions = LayoutOptions.StartAndExpand, HorizontalOptions = LayoutOptions.StartAndExpand };

            var lblfrndly = new Label { Text = "From: " + selectedmsg.FullName.ToString(), FontSize = 14, XAlign = TextAlignment.Start, VerticalOptions = LayoutOptions.StartAndExpand, HorizontalOptions = LayoutOptions.StartAndExpand };
			var lblreciveddate = new Label { Text = "Receive Date: " + DateFormat.GetDateTime(selectedmsg.Date, TimeType.OnlyDate), FontSize = 14, XAlign = TextAlignment.Start, VerticalOptions = LayoutOptions.StartAndExpand, HorizontalOptions = LayoutOptions.StartAndExpand };
			var lblsentdate = new Label { Text = "Send Date: " + DateFormat.GetDateTime(selectedmsg.Date, TimeType.OnlyDate), FontSize = 14, XAlign = TextAlignment.Start, VerticalOptions = LayoutOptions.StartAndExpand, HorizontalOptions = LayoutOptions.StartAndExpand };
			var Tolbl = new Label {FontSize = 14, XAlign = TextAlignment.Start, VerticalOptions = LayoutOptions.StartAndExpand, HorizontalOptions = LayoutOptions.StartAndExpand };

			if (selectedmsg.SendDate != null)
			{
				Tolbl.Text = "To: " + selectedmsg.FullName.ToString();
				//lbldatetime = new Label { Text = "Date: " + selectedmsg.SendDate.ToString(), FontSize = 14, XAlign = TextAlignment.Start, VerticalOptions = LayoutOptions.StartAndExpand, HorizontalOptions = LayoutOptions.StartAndExpand };
				lbldatetime.Text = "Date: " + selectedmsg.SendDate.ToString();
				lbldatetime.FontSize = 14;
				lbldatetime.XAlign = TextAlignment.Start;
				lbldatetime.VerticalOptions = LayoutOptions.StartAndExpand;
				lbldatetime.HorizontalOptions = LayoutOptions.StartAndExpand;
			}
			

		var text = selectedmsg.FullName != null ? Tolbl.Text = "To: " + selectedmsg.FullName.ToString() : Tolbl.Text = "";

			var BtnDeleteMessage = new Xamarin.Forms.Button
			{
				Text = "    Delete Message    ",
				FontSize = 14,
				FontAttributes = FontAttributes.Bold,
				BackgroundColor = Color.Red,
				TextColor = Color.White,
			};

			var BtnReadMessage = new Xamarin.Forms.Button
			{
				Text = "    Mark As Read    ",
				FontSize = 14,
				FontAttributes = FontAttributes.Bold,
				BackgroundColor = Color.Red,
				TextColor = Color.White
			};

			var btnstack = new StackLayout
			{
				Orientation = StackOrientation.Vertical,
				Spacing = 8,
				Padding = new Thickness(0, 0, 0, 5),
				VerticalOptions = LayoutOptions.EndAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand
			};

			if (selectedmsg.IsRead)
			{
				BtnReadMessage.IsEnabled = false;
			}

			if (ListType == "1")
			{
				btnstack.Children.Add(BtnReadMessage);
			}
			btnstack.Children.Add(BtnDeleteMessage);

			var layouth = new StackLayout { Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.StartAndExpand, VerticalOptions = LayoutOptions.StartAndExpand, Spacing = 18 };

			layouth.Children.Add(lblMessage);
			layouth.Children.Add(lblMessageDesc);
			if (ListType == "1")
			{
				layouth.Children.Add(lblfrndly);
				layouth.Children.Add(lblreciveddate);
			}
			else
			{
				layouth.Children.Add(Tolbl);
				layouth.Children.Add(lblsentdate);
			}
			//
			layouth.Children.Add(lbldatetime);

			var scrlMssg = new Xamarin.Forms.ScrollView();
			scrlMssg.Content = layouth;


			if (Xamarin.Forms.Device.Idiom == TargetIdiom.Phone)
			{
				PaddingMain = 8;
				pddingH = .70;
				paddingW = .8;
			}
			if (Xamarin.Forms.Device.Idiom == TargetIdiom.Tablet)
			{
				PaddingMain = 40;
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
				HeightRequest = MyController.ScreenHeight / 2 - 10
			};
			lblAddressLayout.Children.Add(lblAddressTopLayout);
			lblAddressLayout.Children.Add(btnstack);

			var mainlayout = new StackLayout
			{
				Padding = new Thickness(2,0,2,2),
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

			BtnDeleteMessage.Clicked += async (object sender, EventArgs e) =>
			 {
				 try
				 {
					 UserDialogs.Instance.ShowLoading("Loading...");
					 await DeleteMessage(ListType + selectedmsg.MailID.ToString());
					 await Task.Delay(1000);
					 UserDialogs.Instance.HideLoading();
					 await popupLayouts.DismissPopup();
				 }
				 catch
				 {
					 UserDialogs.Instance.HideLoading();

				 }

			 };
			BtnReadMessage.Clicked += async (object sender, EventArgs e) =>
			 {
				 try
				 {

					 if (!selectedmsg.IsRead)
					 {
						 selectedmsg.IsRead = true;
						 BtnReadMessage.IsEnabled = false;
						 var result = await ApiService.Instance.ReadMessage(selectedmsg.MailID.ToString());
						 _numberOfUnreadMessage = Convert.ToInt32(await ApiService.Instance.GetUnreadMessageCount(Settings.UserID));

					 }

				 }
				 catch (System.Exception ex)
				 {

				 }

			 };
			if (popupLayouts.IsPopupActive)
			{

			}
			else {
				Double WidthOfPopup = 1;

				if (Xamarin.Forms.Device.Idiom == TargetIdiom.Phone)
				{
					WidthOfPopup = MyController.ScreenWidth - 30;

				}
				else
				{
					WidthOfPopup = MyController.ScreenWidth / 2 + 50;
				}



				var view = new Frame
				{
					Padding = new Thickness(0, 0, 0, 0),
					HasShadow = false,
					OutlineColor = Color.Gray,
					HeightRequest = MyController.ScreenHeight / 2,
					WidthRequest = WidthOfPopup,
					VerticalOptions = LayoutOptions.FillAndExpand,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					BackgroundColor = Color.FromHex("f2f2f2"),
					Content = mainlayout
				};
				popupLayouts.ShowPopup(view);
			}
		}
		public async void GetPopupOfCreateMessage()
		{
			ListOfUsersViewmodel viewModelUsers = new ListOfUsersViewmodel();
			await viewModelUsers.LoadDevices().ConfigureAwait(true);


			var CloseIcon = new Image { Source = "CloseIcon.png", HeightRequest = 35, WidthRequest = 35, HorizontalOptions = LayoutOptions.EndAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };
			var lblTitle = new Label { Text = "New Message", TextColor = Color.Black, FontAttributes = FontAttributes.Bold, FontSize = 19, HorizontalOptions = LayoutOptions.StartAndExpand };


			var stklbl = new StackLayout { Padding = new Thickness(0, 10, 0, 0), HorizontalOptions = LayoutOptions.StartAndExpand, VerticalOptions = LayoutOptions.StartAndExpand };
			stklbl.Children.Add(lblTitle);

			var stkclose = new StackLayout { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
			stkclose.Children.Add(CloseIcon);

			var layoytTitle = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
			layoytTitle.Children.Add(stklbl);
			layoytTitle.Children.Add(stkclose);



			var divider = new BoxView { Color = Color.FromHex("dcdcdc"), HeightRequest = .5, HorizontalOptions = LayoutOptions.FillAndExpand };
			var layoutTitle = new StackLayout
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.StartAndExpand
			};
			layoutTitle.Children.Add(layoytTitle);
			layoutTitle.Children.Add(divider);

			var Titlelayout = new StackLayout { Orientation = StackOrientation.Horizontal };
			var lblTitle1 = new Label { Text = "Title : ", VerticalOptions = LayoutOptions.StartAndExpand };
			var EntryTitle = new Entry { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.StartAndExpand };
			Titlelayout.Children.Add(lblTitle1);
			Titlelayout.Children.Add(EntryTitle);

			var Bodylayout = new StackLayout { Orientation = StackOrientation.Horizontal };
			var lblBody = new Label { Text = "Body : ", VerticalOptions = LayoutOptions.StartAndExpand };
			var EntryBody = new Entry { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.StartAndExpand };
			Bodylayout.Children.Add(lblBody);
			Bodylayout.Children.Add(EntryBody);


			var SendDatelayout = new StackLayout { Orientation = StackOrientation.Horizontal };
			var lblSendDate = new Label { Text = "Send Date : ", VerticalOptions = LayoutOptions.StartAndExpand };
			var DatePickersendDate = new Xamarin.Forms.DatePicker { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.StartAndExpand, Format = "dd-MMM-yy",MinimumDate=DateTime.Now };
			SendDatelayout.Children.Add(lblSendDate);
			SendDatelayout.Children.Add(DatePickersendDate);

			var AppearOnlayout = new StackLayout { Orientation = StackOrientation.Horizontal };
			var lblAppearOn = new Label { Text = "Appear On : ", VerticalOptions = LayoutOptions.StartAndExpand };
			var layoutAppearOnOption = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.StartAndExpand };

			//Adding type picker for Displaying 
			var AppearonPicker = new CustomPicker { Title = "Please Select", WidthRequest = MyController.ScreenWidth / 2, TitleTextColor = Color.Black };
			AppearonPicker.Items.Add("User's home page");
			AppearonPicker.Items.Add("App");
			AppearonPicker.Items.Add("Both");
			layoutAppearOnOption.Children.Add(AppearonPicker);


			AppearOnlayout.Children.Add(lblAppearOn);
			AppearOnlayout.Children.Add(layoutAppearOnOption);

			var Filterlayout = new StackLayout { };
			var lblFilter = new Label { Text = "Filter : " };


			var layoutList = new StackLayout
			{
				Padding = new Thickness(6, 5, 5, 5),
				Spacing = 15,
				Orientation = StackOrientation.Vertical,
				VerticalOptions = LayoutOptions.StartAndExpand,
				HorizontalOptions = LayoutOptions.StartAndExpand,

			};

			var scrl = new Xamarin.Forms.ScrollView();
			scrl.Content = layoutList;

			List<ListOfUsersResponce> dictionary = new List<ListOfUsersResponce>();

			int i = 1;

			foreach (var item in viewModelUsers.UserList)
			{
				string Name = i.ToString();
				dictionary.Add(new ListOfUsersResponce() { SNo = item.SNo, Name = "uncheck" });


				var layoutListh = new StackLayout
				{
					Orientation = StackOrientation.Horizontal,
					HorizontalOptions = LayoutOptions.StartAndExpand,
					VerticalOptions = LayoutOptions.StartAndExpand
				};


				var checkimages = new Image { Source = "Checkbox.png", HeightRequest = 23, WidthRequest = 23 };
				var conditions = new Label { Text = item.Name, FontSize = 13, VerticalOptions = LayoutOptions.CenterAndExpand };


				layoutListh.Children.Add(checkimages);
				layoutListh.Children.Add(conditions);
				i++;

				var tapGestureRecognizerlbl = new TapGestureRecognizer();

				tapGestureRecognizerlbl.Tapped += (object sender, EventArgs e) =>
				{
					var Result = (Label)sender;
					dictionary = dictionary.Where(x =>
					{
						if (x.SNo == item.SNo)
						{
							if (x.Name == "uncheck")
							{
								checkimages.Source = "CheckBoxTick.png";
								x.Name = "check";
								dictionary[item.SNo - 1].Image = "check";
								item.FlagValue = true;
								j++;
							}
							else if (x.Name == "check")
							{
								checkimages.Source = "Checkbox.png";
								x.Name = "uncheck";
								dictionary[item.SNo - 1].Image = "uncheck";
								item.FlagValue = false;
								j--;
							}


						}
						return true;
					}).ToList<ListOfUsersResponce>();

				};

				conditions.GestureRecognizers.Add(tapGestureRecognizerlbl);

				var tapGestureRecognizerpref = new TapGestureRecognizer();
				tapGestureRecognizerpref.Tapped += (object sender, EventArgs e) =>
				{
					try
					{
						var Result = (Image)sender;

						var rr = (FileImageSource)Result.Source;
						if (rr.File == "Checkbox.png")
						{
							dictionary = dictionary.Where(x =>
							{
								if (x.SNo == item.SNo)
								{
									x.Name = "uncheck";
									dictionary[item.SNo - 1].Name = "check";
									item.FlagValue = true;
								}
								return true;
							}).ToList<ListOfUsersResponce>();

							j--;

							checkimages.Source = "CheckBoxTick.png";
						}
						else
						{
							dictionary = dictionary.Where(x =>
							{
								if (x.SNo == item.SNo)
								{
									x.Name = "check";
									dictionary[item.SNo - 1].Image = "uncheck";
									item.FlagValue = false;
								}
								return true;
							}).ToList<ListOfUsersResponce>();

							j++;
							checkimages.Source = "Checkbox.png";
						}

					
					}
					catch
					{
					}

				};

				checkimages.GestureRecognizers.Add(tapGestureRecognizerpref);
				layoutList.Children.Add(layoutListh);

			}

			Filterlayout.Children.Add(lblFilter);
			Filterlayout.Children.Add(scrl);


			var BtnSend = new Xamarin.Forms.Button
			{
				Text = "Send",
				FontSize = 14,
				WidthRequest = MyController.ScreenWidth / 3 - 30,
				FontAttributes = FontAttributes.Bold,
				BackgroundColor = Color.Red,
				TextColor = Color.White,
			};
			var BtnDraft = new Xamarin.Forms.Button
			{
				Text = "Draft",
				FontSize = 14,
				WidthRequest = MyController.ScreenWidth / 3 - 30,
				FontAttributes = FontAttributes.Bold,
				BackgroundColor = Color.Red,
				TextColor = Color.White,
			};
			var BtnCancel = new Xamarin.Forms.Button
			{
				Text = "Cancel",
				FontSize = 14,
				WidthRequest = MyController.ScreenWidth / 3 - 30,
				FontAttributes = FontAttributes.Bold,
				BackgroundColor = Color.Red,
				TextColor = Color.White,
			};


			var btnstack = new StackLayout
			{
				Orientation = StackOrientation.Horizontal,
				Spacing = 3,
				Padding = new Thickness(0, 0, 0, 5),
				VerticalOptions = LayoutOptions.EndAndExpand,
				HorizontalOptions = LayoutOptions.CenterAndExpand
			};

		   btnstack.Children.Add(BtnSend);
			
			btnstack.Children.Add(BtnDraft);

			btnstack.Children.Add(BtnCancel);


			var layouth = new StackLayout { Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.StartAndExpand, Spacing = 8 };

			layouth.Children.Add(layoutTitle);
			layouth.Children.Add(Titlelayout);
			layouth.Children.Add(Bodylayout);
			layouth.Children.Add(SendDatelayout);
			layouth.Children.Add(AppearOnlayout);
			layouth.Children.Add(Filterlayout);

			var scrlMssg = new Xamarin.Forms.ScrollView();
			scrlMssg.Content = layouth;


			if (Xamarin.Forms.Device.Idiom == TargetIdiom.Phone)
			{
				PaddingMain = 8;
				pddingH = .70;
				paddingW = .8;
			}
			if (Xamarin.Forms.Device.Idiom == TargetIdiom.Tablet)
			{
				PaddingMain = 40;
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
			lblAddressTopLayout.Children.Add(layouth);


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
				Padding = new Thickness(2,0,2,2),
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

			BtnSend.Clicked += async (object sender, EventArgs e) =>
			 {
				 try
				 {
					 DateTime currentDateTime = DateTime.Now;

					 var item = viewModelUsers.UserList.Where(x => x.FlagValue == true).ToList();
					 string date = DatePickersendDate.Date.Date.ToString("d");
					 string time = DateTime.Now.TimeOfDay.ToString("c");
					 DateTime Odate = Convert.ToDateTime(date);
					 TimeSpan Otime = Convert.ToDateTime(time).TimeOfDay;
					 DateTime dateofOrder = Odate + Otime;


					 if (string.IsNullOrEmpty(EntryTitle.Text) || string.IsNullOrEmpty(EntryBody.Text) || item.Count == 0 || AppearonPicker.SelectedIndex < 0)
					 {
						 UserDialogs.Instance.AlertAsync("Please fill the all Info..!", "Information", "Ok");
						 return;
					 }
					 else if (dateofOrder < currentDateTime)
					 {
						 UserDialogs.Instance.AlertAsync("You cant sent a message of past date..!", "Information", "Ok");
						 return;
					 }
					 else

						 filtervalue = AppearonPicker.SelectedIndex + 1;

					 UserDialogs.Instance.ShowLoading("Please wait..!", MaskType.Gradient);
					 StringBuilder AllusernameinTo = new StringBuilder();
					 List<string> stringArray = new List<string>();

					 foreach (var req in item)
					 {
						 AllusernameinTo.Append(req.Value + ",");
						 stringArray.Add(req.Value);

					 }

					 var TitleValue = EntryTitle.Text;
					 var bodyValue = EntryBody.Text;
					 var selected = DatePickersendDate.Date;

					InMailModel itemreq = new InMailModel();
					 itemreq.CreatedBy = Settings.UserID;
					 itemreq.CreatedDate = DateTime.Now;
					 itemreq.Title = EntryTitle.Text;
					 itemreq.Body = EntryBody.Text;
					 itemreq.SendDate = DatePickersendDate.Date;
                     itemreq.strSendDate = DateFormat.GetDateTime(DatePickersendDate.Date, TimeType.OnlyDate);
					 itemreq.AppearOn = (short)filtervalue;
					 itemreq.UserNameList = stringArray.ToArray();
					 itemreq.From = Settings.UserName;
					 itemreq.SendBy = Settings.UserID.ToString();
					 itemreq.To = AllusernameinTo.ToString();


					 var result = await ApiService.Instance.SendMessage(itemreq).ConfigureAwait(true);
					 if (result)
					 {
						 UserDialogs.Instance.AlertAsync("Message send successfully!!", "Information", "Ok");
					 }
					 else
					 {
						 UserDialogs.Instance.AlertAsync("Failed, please send after some time!!", "Information", "Ok");

					 }
					 UserDialogs.Instance.HideLoading();
					 popupLayouts.DismissPopup();
				 }
				 catch
				 {

				 }
			 };

			BtnDraft.Clicked += async (object sender, EventArgs e) =>
			 {
				 try
				 {
					 DateTime currentDateTime = DateTime.Now;

					 var item = viewModelUsers.UserList.Where(x => x.FlagValue == true).ToList();
					 string date = DatePickersendDate.Date.Date.ToString("d");
					 string time = DateTime.Now.TimeOfDay.ToString("c");
					 DateTime Odate = Convert.ToDateTime(date);
					 TimeSpan Otime = Convert.ToDateTime(time).TimeOfDay;
					 DateTime dateofOrder = Odate + Otime;

					 if (!(string.IsNullOrEmpty(EntryTitle.Text)) || !(string.IsNullOrEmpty(EntryBody.Text)) || !(item.Count == 0) || !(AppearonPicker.SelectedIndex < 0))
					 {
						 UserDialogs.Instance.ShowLoading("Please wait..!", MaskType.Gradient);
						 StringBuilder AllusernameinTo = new StringBuilder();
						 List<string> stringArray = new List<string>();

						 foreach (var req in item)
						 {
							 AllusernameinTo.Append(req.UserId + ",");
							 stringArray.Add(req.UserId);
						 }

						 var TitleValue = EntryTitle.Text;
						 var bodyValue = EntryBody.Text;
						 var selected = DatePickersendDate.Date;
						 InMailModel itemreq = new InMailModel();
						 itemreq.CreatedBy = Settings.UserID;
						 itemreq.CreatedDate = DateTime.Now;
						 itemreq.Title = EntryTitle.Text;
						 itemreq.Body = EntryBody.Text;
						 itemreq.SendDate = DatePickersendDate.Date;
						 itemreq.strSendDate = DateFormat.GetDateTime(DatePickersendDate.Date, TimeType.OnlyDate);
						 itemreq.AppearOn = (short)filtervalue;
						 itemreq.UserNameList = stringArray.ToArray();
						 itemreq.From = Settings.UserName;
						 itemreq.SendBy = Settings.UserID.ToString();
						 itemreq.To = AllusernameinTo.ToString();
						 var result = await ApiService.Instance.DraftSendInMail(itemreq).ConfigureAwait(true);
						 if (result)
						 {
							popupLayouts.DismissPopup();
							 UserDialogs.Instance.HideLoading();
							await UserDialogs.Instance.AlertAsync("Message saved to Draft successfully!!", "Information", "Ok");
							UserDialogs.Instance.ShowLoading("Loading...");
							 await Task.Delay(50);
							 await this.ViewModel.LoadDevicesDrafts();
							 await Navigation.PushAsync(new DraftListPage(this.ViewModel));
							 UserDialogs.Instance.HideLoading();
						 }
						 else
						 {
							 UserDialogs.Instance.HideLoading();
							 UserDialogs.Instance.AlertAsync("Failed, please try after some time!!", "Information", "Ok");

						 }
					 }
				 }
				 catch
				 {

				 }
			 };

			BtnCancel.Clicked += async (object sender, EventArgs e) =>
			 {
				 try
				 {
					 popupLayouts.DismissPopup();
				 }
				 catch
				 {
				 }
			 };
			if (popupLayouts.IsPopupActive)
			{

			}
			else {

				Double WidthOfPopup = 1;
				Double HeightOfPopup = 1;


				if (Xamarin.Forms.Device.Idiom == TargetIdiom.Phone)
				{
					WidthOfPopup = MyController.ScreenWidth - 30;
					HeightOfPopup = MyController.ScreenHeight - 100;
				}
				else
				{
					WidthOfPopup = MyController.ScreenWidth - MyController.ScreenWidth / 3;
					HeightOfPopup = MyController.ScreenHeight / 2 + 50;
				}


				var view = new Frame
				{
					Padding = new Thickness(0, 0, 0, 0),
					HasShadow = false,
					OutlineColor = Color.Gray,
					HeightRequest = HeightOfPopup,
					WidthRequest = WidthOfPopup,
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



