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
	public partial class DraftListPage : ContentPage
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
		public InfiniteListView listViewDraft;
		public static string ListType = "";
		double pddingH, paddingW;
		DateTimeFormatInfo _DateTimeFormatInfo = new DateTimeFormatInfo();
		public ObservableCollection<MessageStatics> PopupData { get; set; }
		public DataTemplate Cell { get; private set; }
		public MessageViewModel ViewModel { get; set; }
		StackLayout ListViewLyout;
		public int filtervalue = 0;
		Label MessageLabel, Date;

		public DraftListPage(MessageViewModel viewmodel = null)
		{
			NavigationPage.SetBackButtonTitle(this, "");

			try
			{
				Title = "Draft Messages";
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
			relativeLayout = new Xamarin.Forms.RelativeLayout();
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

				//for inbox

				listViewDraft = new InfiniteListView();
				listViewDraft.BindingContext = ViewModel;
				listViewDraft.ItemsSource = ViewModel.DraftMessageList;
				listViewDraft.ItemTemplate = new DataTemplate(typeof(DraftListCell));
				listViewDraft.HeightRequest = App.ScreenHeight - 140;
				listViewDraft.SeparatorColor = Color.Transparent;
				listViewDraft.RowHeight = 100;
				listViewDraft.ItemTapped += async (object sender, ItemTappedEventArgs e) =>
				  {
					  try
					  {
						  UserDialogs.Instance.ShowLoading("Loading...");
						  selectedindex = ViewModel.DraftMessageList.IndexOf((DraftMailModel)e.Item);
						  var message = (DraftMailModel)e.Item;
						  GetPopupOfCreateMessage(message);
						  UserDialogs.Instance.HideLoading();
					  }
					  catch (System.Exception ex)
					  {
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
						UserDialogs.Instance.HideLoading();
					}
					catch
					{
						UserDialogs.Instance.HideLoading();
					}
				};

				imgAddButton.GestureRecognizers.Add(imgAddButtonTap);

				ListViewLyout = new StackLayout { BackgroundColor = Color.Transparent, VerticalOptions = LayoutOptions.FillAndExpand, HeightRequest = MyController.ScreenHeight, HorizontalOptions = LayoutOptions.FillAndExpand };

				lblerror = new Label { IsVisible = false, Text = "No Message to display", FontSize = 18, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };


				if (ViewModel.DraftMessageList.Count > 0)
				{
					ListViewLyout.VerticalOptions = LayoutOptions.StartAndExpand;
					lblerror.IsVisible = false;
					listViewDraft.IsVisible = true;
				}
				else
				{
					ListViewLyout.VerticalOptions = LayoutOptions.FillAndExpand;
					lblerror.IsVisible = true;
					listViewDraft.IsVisible = false;
				}

				var layoutListView = new StackLayout { VerticalOptions = LayoutOptions.FillAndExpand };

				layoutListView.Children.Add(listViewDraft);
				ListViewLyout.Children.Add(lblerror);
				ListViewLyout.Children.Add(layoutListView);

				var custom = new CustomPopup();
				var popupLayouts = this.Content as CustomPopup;
				Content = custom;
				custom.Content = ListViewLyout;
			}
			catch (Exception ex)
			{

			}
		}

		public async void GetPopupOfCreateMessage(DraftMailModel selectedDraftItem)
		{
			ListOfUsersViewmodel viewModelUsers = new ListOfUsersViewmodel();
			await viewModelUsers.LoadDevices().ConfigureAwait(true);
			
			var CloseIcon = new Image { Source = "CloseIcon.png", HeightRequest = 25, WidthRequest = 25, HorizontalOptions = LayoutOptions.EndAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };
			var lblTitle = new Label { Text = "New Message", TextColor = Color.Black, FontAttributes = FontAttributes.Bold, FontSize = 19, HorizontalOptions = LayoutOptions.StartAndExpand };

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

			var Titlelayout = new StackLayout { Orientation = StackOrientation.Horizontal };
			var lblTitle1 = new Label { Text = "Title : ", VerticalOptions = LayoutOptions.StartAndExpand };
			var EnrtyTitle = new Entry { Text = selectedDraftItem.Title, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.StartAndExpand };
			Titlelayout.Children.Add(lblTitle1);
			Titlelayout.Children.Add(EnrtyTitle);

			var Bodylayout = new StackLayout { Orientation = StackOrientation.Horizontal };
			var lblBody = new Label { Text = "Body : ", VerticalOptions = LayoutOptions.StartAndExpand };
			var EnrtyBody = new Entry { Text = selectedDraftItem.Body, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.StartAndExpand };
			Bodylayout.Children.Add(lblBody);
			Bodylayout.Children.Add(EnrtyBody);


			var SendDatelayout = new StackLayout { Orientation = StackOrientation.Horizontal };
			var lblSendDate = new Label { Text = "Send Date : ", VerticalOptions = LayoutOptions.StartAndExpand };
			var DatePickersendDate = new Xamarin.Forms.DatePicker { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.StartAndExpand, Format = "dd-MMM-yy",MinimumDate=System.DateTime.Now, Date=(DateTime)selectedDraftItem.SendDate };
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

				if (item.UserId == selectedDraftItem.SendTo)
				{
					item.FlagValue = true;
					item.Image = "CheckBoxTick.png";
					dictionary.Add(new ListOfUsersResponce() { SNo = item.SNo, Name = "check" });

				}
				else {
					dictionary.Add(new ListOfUsersResponce() { SNo = item.SNo, Name = "uncheck" });

				}

				string Name = i.ToString();


				var layoutListh = new StackLayout
				{
					Orientation = StackOrientation.Horizontal,
					HorizontalOptions = LayoutOptions.StartAndExpand,
					VerticalOptions = LayoutOptions.StartAndExpand
				};


				var checkimages = new Image { Source = item.Image, HeightRequest = 23, WidthRequest = 23 };
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
				Text = "Delete",
				FontSize = 14,
				WidthRequest = MyController.ScreenWidth / 3 - 30,
				FontAttributes = FontAttributes.Bold,
				BackgroundColor = Color.Red,
				TextColor = Color.White,
				IsVisible=false
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
				//Padding = new Thickness(10, 12, 10, 10),
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
			//lblAddressLayout.Children.Add(layoutTitle);
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
					 var item = viewModelUsers.UserList.Where(x => x.FlagValue == true).ToList();
					 if (string.IsNullOrEmpty(EnrtyTitle.Text) || string.IsNullOrEmpty(EnrtyBody.Text) || item.Count == 0 || AppearonPicker.SelectedIndex < 0)
					 {
						 UserDialogs.Instance.AlertAsync("Please fill the all Info..!", "Info", "Ok");
						 return;
					 }
					 else if (DatePickersendDate.Date < DateTime.Now.Date)
					 {
						 UserDialogs.Instance.AlertAsync("You cant sent a message of past date..!", "Info", "Ok");
						 return;
					 }
					 else

						 filtervalue = AppearonPicker.SelectedIndex + 1;

					 UserDialogs.Instance.ShowLoading("Please wait..!", MaskType.Gradient);
					 StringBuilder AllusernameinTo = new StringBuilder();
					 List<string> stringArray = new List<string>();

					 foreach (var req in item)
					 {
						 AllusernameinTo.Append(req.UserId + ",");
						 stringArray.Add(req.UserId);

					 }

					 var TitleValue = EnrtyTitle.Text;
					 var bodyValue = EnrtyBody.Text;
					 var selected = DatePickersendDate.Date;

					 InMailModel itemreq = new InMailModel();
					 itemreq.CreatedBy = Settings.UserID;
					 itemreq.CreatedDate = DateTime.Now;
					
					 itemreq.Title = EnrtyTitle.Text;
					 itemreq.Body = EnrtyBody.Text;
					 itemreq.SendDate = DatePickersendDate.Date;



					 itemreq.strSendDate = DateFormat.GetDateTime(DatePickersendDate.Date, TimeType.OnlyDate);

					 itemreq.AppearOn = (short)filtervalue;
					 itemreq.UserNameList = stringArray.ToArray();
					 itemreq.From = Settings.UserName;
					 itemreq.SendBy = Settings.UserID.ToString();
					 itemreq.To = AllusernameinTo.ToString();

					 itemreq.DraftMailID = selectedDraftItem.DraftMailID;

					 var result = await ApiService.Instance.SendMessage(itemreq).ConfigureAwait(true);
					 if (result)
					 {
						 UserDialogs.Instance.HideLoading();
						await UserDialogs.Instance.AlertAsync("Message send successfully!!", "Info", "Ok");
						 popupLayouts.DismissPopup();
						 UserDialogs.Instance.ShowLoading("Loading...");
						 await Task.Delay(500);
						 var mainPage = new MainPage();
						 var MessageviewModel = new MessageViewModel();
						 await MessageviewModel.LoadDevices();
						 await MessageviewModel.LoadDevicesSendBox();
						 mainPage.nav = new NavigationPage(new MessegeListPage(MessageviewModel));
						 mainPage.Detail = mainPage.nav;
						 mainPage.IsPresented = false;

						 Xamarin.Forms.Application.Current.MainPage = mainPage;
						UserDialogs.Instance.HideLoading();
					 }
					 else
					 {
						 UserDialogs.Instance.HideLoading();
						 UserDialogs.Instance.AlertAsync("Failed, please send after some time!!", "Info", "Ok");

					 }
				 }
				 catch
				 {
					 UserDialogs.Instance.HideLoading();
				 }
			 };

			BtnDraft.Clicked += async (object sender, EventArgs e) =>
			 {
				 try
				 {

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



