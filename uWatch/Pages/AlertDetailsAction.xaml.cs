using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using FFImageLoading.Forms;
using UwatchPCL;
using UwatchPCL.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using System.Net;
#if __ANDROID__
using Android.App;
using Android.Widget;
using uWatch.Droid;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
#endif


namespace uWatch
{
    public partial class AlertDetailsAction : ContentPage
    {
        Xamarin.Forms.Button btnEsclate, btnChangeConfig;
        int PaddingMain;
        double pddingH, paddingW;
        AlertsEsclatedToAgentViewModel Alert;
        Xamarin.Forms.RelativeLayout relativeLayout;
        Xamarin.Forms.ScrollView scrollview;
        public Label lblCountdownValue;
        public Label lblCountdownValue1;
        double w = MyController.VirtualWidth;
        double h = MyController.VirtualHeight;
        bool noimage = false;
        bool noMap = false;
        int _DeviceId;
        public AlertDetailsViewModel ViewModel { get; set; }
        ObservableCollection<DeviceConfig> profiledetailslist;
        int res; string[] GpsImageName, AlertImageName;
        public bool bGpsImage, bAlertImage = true;
        public AlertDetailsAction(AlertsEsclatedToAgentViewModel alert)
        {
            try
            {
                _DeviceId = alert.device_idx;
                this.Alert = alert;
                InitializeComponent();
                ViewModel = new AlertDetailsViewModel(this.Alert);
                Timer();
                BindingContext = ViewModel.device;
                SetLayout();

            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                MyController.ErrorManagement(ex.Message);
            }
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            try
            {
                System.GC.Collect();
                base.OnDisappearing();
            }
            catch { }
        }

        private async void SetLayout()
        {
            try
            {

                Title = "Alert Action";

                AddLayout();

            }
            catch
            {

            }
        }

        private async void AddLayout()
        {

            try
            {
                StackLayout headstack = new StackLayout() { Padding = new Thickness(0, 10, 0, 10), BackgroundColor = Color.FromRgb(244, 244, 244), VerticalOptions = LayoutOptions.Start, HorizontalOptions = LayoutOptions.FillAndExpand };
                headstack.Orientation = StackOrientation.Horizontal;

                StackLayout Substack = new StackLayout() { Spacing = 1 };
                Substack.Orientation = StackOrientation.Horizontal;
                Substack.HorizontalOptions = LayoutOptions.CenterAndExpand;
                Substack.VerticalOptions = LayoutOptions.CenterAndExpand;

                var lblAwakeTime = new Label { Text = "Awake Time:", TextColor = Color.Gray, FontSize = 17 };
                var lblAwakeTimeValue = new Label { Text = "180 secs", FontSize = 17 };
                var AwakeSec = ViewModel.device.WakeTime * 60;
                lblAwakeTimeValue.SetBinding(Label.TextProperty, new Binding("WakeTime", BindingMode.TwoWay, stringFormat: "" + AwakeSec.ToString() + " Secs"));
                var lblCountdown = new Label { Text = "Countdown:", TextColor = Color.Gray, FontSize = 17 };
                lblCountdownValue = new Label { Text = "98 secs", TextColor = Color.Black, FontSize = 17 };
                lblCountdownValue.SetBinding(Label.TextProperty, new Binding("CountDown", BindingMode.TwoWay, stringFormat: "{0} Secs"));

                var AwakeTimeLyout = new StackLayout { Orientation = StackOrientation.Horizontal, WidthRequest = MyController.ScreenWidth / 2 - 10 };

                if (Xamarin.Forms.Device.Idiom == TargetIdiom.Phone)
                {
                    AwakeTimeLyout.HorizontalOptions = LayoutOptions.Start;
                }
                else
                {
                    AwakeTimeLyout.HorizontalOptions = LayoutOptions.FillAndExpand;

                }

                AwakeTimeLyout.Children.Add(lblAwakeTime);
                AwakeTimeLyout.Children.Add(lblAwakeTimeValue);


                var CountDowmTimeLyout = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.End, WidthRequest = MyController.ScreenWidth / 2 - 10 };
                CountDowmTimeLyout.Children.Add(lblCountdown);
                CountDowmTimeLyout.Children.Add(lblCountdownValue);
                double HeightOfImage;
                HeightOfImage = 18;

                Double HeightRequestControl;
                HeightRequestControl = 40;
                Substack.Children.Add(AwakeTimeLyout);
                Substack.Children.Add(CountDowmTimeLyout);

                headstack.Children.Add(Substack);
                var LiueRatingPicker = new CustomPicker { Title = "Profile", WidthRequest = MyController.ScreenWidth / 2, TitleTextColor = Color.Black, HeightRequest = HeightRequestControl, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };
                var imgLiueRatingArrow = new Image { Source = "downArrow.png", HorizontalOptions = LayoutOptions.EndAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand, HeightRequest = HeightOfImage + 3, WidthRequest = HeightOfImage + 3 };

                var alertList = new AlertsListViewModel(Navigation, Settings.UserID);
                profiledetailslist = new ObservableCollection<DeviceConfig>();
                if (Settings.RoleID != 3)
                {
                    profiledetailslist = await alertList.FetchConfigurationProfileDetails().ConfigureAwait(true);
                }

                if (profiledetailslist != null)
                {
                    foreach (var item in profiledetailslist)
                    {
                        LiueRatingPicker.Items.Add(item.ConfigurationName);
                    }
                }
                var lblAwakeTime1 = new Label { Text = "Select Configuration to Apply", FontSize = 16, HorizontalOptions = LayoutOptions.FillAndExpand };
                var LayoutLiueRating = new StackLayout { Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.Fill, Padding = new Thickness(10, 0, 10, 0) };
                StackLayout headstack1 = new StackLayout() { Padding = new Thickness(20, 10, 20, 10), BackgroundColor = Color.FromRgb(244, 244, 244), VerticalOptions = LayoutOptions.Start, HorizontalOptions = LayoutOptions.FillAndExpand };
                headstack1.Orientation = StackOrientation.Horizontal;

                LayoutLiueRating.Children.Add(LiueRatingPicker);
                LayoutLiueRating.Children.Add(imgLiueRatingArrow);

                StackLayout Substack1 = new StackLayout() { Padding = new Thickness(1, 10, 1, 10) };
                Substack1.Orientation = StackOrientation.Vertical;
                Substack1.HorizontalOptions = LayoutOptions.CenterAndExpand;
                Substack1.VerticalOptions = LayoutOptions.CenterAndExpand;

                var lblFirndlyName = new Label { Text = "Shed at back of garrage.......", TextColor = Color.Gray, FontSize = 19 };
                lblFirndlyName.SetBinding(Label.TextProperty, new Binding("FriendlyName", BindingMode.TwoWay, stringFormat: "{0}"));
                lblFirndlyName.HorizontalTextAlignment = TextAlignment.Center;
                lblFirndlyName.HorizontalOptions = LayoutOptions.CenterAndExpand;
                lblFirndlyName.VerticalOptions = LayoutOptions.StartAndExpand;

                var lblAlertDateTime = new Label { TextColor = Color.Gray, FontSize = 19 };
                var alertdate = ViewModel.device.DeviceDate.ToString("ddd") + " " + DateFormat.GetDateTime(ViewModel.device.DeviceDate, TimeType.DateAndTime);
                lblAlertDateTime.SetBinding(Label.TextProperty, new Binding("DeviceDate", BindingMode.Default, stringFormat: alertdate));
                lblAlertDateTime.HorizontalTextAlignment = TextAlignment.Center;
                lblAlertDateTime.HorizontalOptions = LayoutOptions.CenterAndExpand;
                lblAlertDateTime.VerticalOptions = LayoutOptions.StartAndExpand;

                Substack1.Children.Add(lblFirndlyName);
                Substack1.Children.Add(lblAlertDateTime);

                headstack1.Children.Add(Substack1);

                btnEsclate = new Xamarin.Forms.Button { Text = "Escalate", WidthRequest = MyController.ScreenWidth / 2 + 40, BackgroundColor = Color.Red, TextColor = Color.White, FontSize = 15 };
                btnEsclate.Clicked += async (sender, e) =>
                {
                    GetPopupOfEscalate(ViewModel.device);
                };
                btnChangeConfig = new Xamarin.Forms.Button { Text = "Change Cube Configuration", WidthRequest = MyController.ScreenWidth / 2 + 40, BackgroundColor = Color.Red, TextColor = Color.White, FontSize = 15 };

                btnChangeConfig.Clicked += async (sender, e) =>
                {
                    GetPopupOfChangeCubeConfig(ViewModel.device, profiledetailslist, alertList);
                };

                var btnLayout = new StackLayout { Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.EndAndExpand };
                btnLayout.Children.Add(btnEsclate);
                if (Settings.RoleID != 3)
                {
                    btnLayout.Children.Add(btnChangeConfig);
                }

                var mainStackLayout = new StackLayout { Padding = new Thickness(0, 0, 0, 5), HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
                mainStackLayout.Children.Add(headstack);
                mainStackLayout.Children.Add(headstack1);

                mainStackLayout.Children.Add(btnLayout);

                var custom = new CustomPopup();
                var popupLayouts = this.Content as CustomPopup;
                Content = custom;
                custom.Content = mainStackLayout;
                UserDialogs.Instance.HideLoading();

            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        async void Timer()
        {
            try
            {
                Xamarin.Forms.Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                {
                    if (ViewModel != null)
                    {
                        if (ViewModel.device.CountDown < 0)
                        {
                            return false;
                        }
                        if (ViewModel.device.CountDown > 0)
                            ViewModel.device.CountDown -= 1;
                        if (lblCountdownValue != null)
                        {
                            lblCountdownValue.Text = ViewModel.device.CountDown.ToString() + " Secs";
                        }
                    }
                    return true;
                });
            }
            catch { }
        }

        protected override bool OnBackButtonPressed()
        {
            try
            {
                System.GC.Collect();

            }
            catch { }
            return base.OnBackButtonPressed();
        }

        public static string BatteryLevelImage(int? BatteryLevel)
        {
            try
            {
                BatteryLevel = BatteryLevel ?? 0;

                if (BatteryLevel <= 10)
                {
                    return "battery0.png";
                }
                else if (BatteryLevel > 10 && BatteryLevel <= 40)
                {
                    return "battery1.png";
                }

                else if (BatteryLevel > 40 && BatteryLevel <= 70)
                {
                    return "battery2.png";
                }
                else
                {
                    return "battery3.png";
                }
            }
            catch { }
            return "";
        }

        public static string TempratureLevelImage(int? TempLevel)
        {
            try
            {
                TempLevel = TempLevel ?? 0;

                if (TempLevel <= 10)
                {
                    return "temperature0.png";
                }
                else if (TempLevel > 10 && TempLevel <= 40)
                {
                    return "temperature1.png";
                }

                else if (TempLevel > 40 && TempLevel <= 70)
                {
                    return "temperature2.png";
                }
                else
                {
                    return "temperature3.png";
                }
            }
            catch
            {
            }
            return "";
        }

        private Image BytesArraytoImage(byte[] stream)
        {
            Image img = new Image();
            try
            {

                byte[] imagedata = stream;
                img.Source = ImageSource.FromStream(() => new MemoryStream(imagedata));

            }
            catch { }
            return img;
        }
        public void GetPopupOfEscalate(AlertsEsclatedToAgentViewModel _alertsEsclatedToAgentViewModel)
        {

            //int DevId = (int)selectedmsg.Device_idx;
            var CloseIcon = new Image { Source = "CloseIcon.png", HeightRequest = 25, WidthRequest = 25, HorizontalOptions = LayoutOptions.EndAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };

            var lblTitle = new Label { Text = "Escalate Alert", TextColor = Color.Black, FontAttributes = FontAttributes.Bold, FontSize = 19, HorizontalOptions = LayoutOptions.StartAndExpand };

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


            var lblDetails = new Label { Text = "Choose your option to Escalate this Alert", TextColor = Color.Black, FontAttributes = FontAttributes.Bold, FontSize = 15, XAlign = TextAlignment.Start, VerticalOptions = LayoutOptions.StartAndExpand, HorizontalOptions = LayoutOptions.StartAndExpand };

            var BtnEscalateToAgent = new Xamarin.Forms.Button
            {
                Text = "   Escalate to Agent    ",
                FontSize = 14,
                FontAttributes = FontAttributes.Bold,
                BackgroundColor = Color.Red,
                TextColor = Color.White,
            };

            var BtnEscalateToOther = new Xamarin.Forms.Button
            {
                Text = "    Escalate to Others   ",
                FontSize = 14,
                FontAttributes = FontAttributes.Bold,
                BackgroundColor = Color.Red,
                TextColor = Color.White,
            };

            var BtnEscalateToOtherByEmail = new Xamarin.Forms.Button
            {
                Text = "    Escalate to Others By Email   ",
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

            btnstack.Children.Add(BtnEscalateToAgent);
            btnstack.Children.Add(BtnEscalateToOther);
            // btnstack.Children.Add(BtnEscalateToOtherByEmail);


            var layouth = new StackLayout { Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.StartAndExpand, VerticalOptions = LayoutOptions.StartAndExpand, Spacing = 18 };

            layouth.Children.Add(lblDetails);

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

            };

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
            var popupLayouts = this.Content as CustomPopup;

            BtnEscalateToAgent.IsEnabled = false;
            if (Settings.RoleID == 3)
            {
                BtnEscalateToAgent.Text = "Escalate to Area Manager";

                if (ViewModel.device.EscalateToAreaManager == null && ViewModel.device.EscalateToAgentID != null)
                {
                    BtnEscalateToAgent.IsEnabled = true;

                }

            }
            else
            {
                if (ViewModel.device.EscalateTo == null && ViewModel.device.EscalateToAgentID != null)
                {
                    BtnEscalateToAgent.IsEnabled = true;

                }
                else
                {
                    BtnEscalateToAgent.Text = "Escalated to Agent";
                }
            }
            TapGestureRecognizer bigt = new TapGestureRecognizer();
            bigt.Tapped += async (object sender, EventArgs e) =>
            {
                popupLayouts.DismissPopup();
            };

            CloseIcon.GestureRecognizers.Add(bigt);

            BtnEscalateToAgent.Clicked += async (object sender, EventArgs e) =>
             {
                 try
                 {

                     btnEsclate.IsEnabled = false;
                     btnChangeConfig.IsEnabled = false;
                     try
                     {

                         var networkConnection = DependencyService.Get<INetworkConnection>();
                         networkConnection.CheckNetworkConnection();
                         var networkStatus = networkConnection.IsConnected ? "Connected" : "Not Connected";
                         if (networkStatus != "Connected")
                         {
                             btnEsclate.IsEnabled = true;
                             btnChangeConfig.IsEnabled = true;
                             UserDialogs.Instance.Alert("Please check your internet connection.", "Alert", "OK");
                             return;
                         }
                         string address = "Are you sure you want to send this alert to agent?";
                         var answer = await UserDialogs.Instance.ConfirmAsync(address, "Confirmation", "Yes", "No");

                         if (answer == true)
                         {
                             UserDialogs.Instance.ShowLoading("Escalating Alert...", Acr.UserDialogs.MaskType.Gradient);
                             await System.Threading.Tasks.Task.Delay(100);
                             AlertsEsclatedToAgentViewModel req = new AlertsEsclatedToAgentViewModel();
                             req.alertlog_idx = ViewModel.device.alertlog_idx;
                             req.CreatedBy = Settings.UserID;
                             if (Settings.RoleID == 3)
                             {
                                 res = await ApiService.Instance.EsclateAlertToAreaManager(req);
                                 if (res > 0)
                                 {
                                     UserDialogs.Instance.HideLoading();
                                     UserDialogs.Instance.ShowLoading("Alert Escalated Redirecting...", MaskType.Gradient);
                                     await System.Threading.Tasks.Task.Delay(500);
                                 }
                                 else
                                 {
                                     UserDialogs.Instance.HideLoading();
                                     await UserDialogs.Instance.AlertAsync("Alert Not Escalated, Please Consult your System Administrator", "Information", "OK");
                                 }
                             }
                             else
                             {

                                 req.strCreatedDate = DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tttt");

                                 res = await ApiService.Instance.EsclateAlert(req);
                                 if (res > 0)
                                 {
                                     UserDialogs.Instance.HideLoading();
                                     UserDialogs.Instance.ShowLoading("Alert Escalated Redirecting...", MaskType.Gradient);
                                     await System.Threading.Tasks.Task.Delay(500);
                                 }
                                 else
                                 {
                                     UserDialogs.Instance.HideLoading();
                                     await UserDialogs.Instance.AlertAsync("Alert Not Escalated, Please Consult your System Administrator", "Information", "OK");
                                 }
                             }
                             MyController.fromAssetsToGallery = true;

                             if (Navigation.NavigationStack.Count <= 3)
                             {
                                 if (res > 0)
                                 {
                                     var mainPage = new MainPage();
                                     var alertListViewModel = new AlertsListViewModel(Navigation, Settings.UserID);

                                     if (Settings.RoleID == 3)
                                     {
                                         await alertListViewModel.LoadAlertListOfAgent().ConfigureAwait(true);
                                     }
                                     else
                                     {
                                         await alertListViewModel.LoadAlertList().ConfigureAwait(true);
                                     }

                                     mainPage.nav = new NavigationPage(new AlertListPage(0, alertListViewModel));
                                     mainPage.Detail = mainPage.nav;
                                     mainPage.IsPresented = false;



                                     Xamarin.Forms.Application.Current.MainPage = mainPage;
                                     await System.Threading.Tasks.Task.Delay(500);
                                     UserDialogs.Instance.HideLoading();
                                 }
                             }
                             else
                             {
                                 await Navigation.PopAsync();
                                 System.GC.Collect();
                                 await System.Threading.Tasks.Task.Delay(1000);
                                 UserDialogs.Instance.HideLoading();
                             }

                             btnEsclate.IsEnabled = true;
                             btnChangeConfig.IsEnabled = true;
                             popupLayouts.DismissPopup();
                         }
                         else
                         {
                             btnEsclate.IsEnabled = true;
                             btnChangeConfig.IsEnabled = true;
                         }

                     }
                     catch { }

                 }
                 catch
                 {
                     UserDialogs.Instance.HideLoading();
                 }
             };
            BtnEscalateToOther.Clicked += async (object sender, EventArgs e) =>
             {
                 try
                 {
                     bool s = true;
                     UserDialogs.Instance.ShowLoading("Connecting to social system...", Acr.UserDialogs.MaskType.Gradient);
                     await System.Threading.Tasks.Task.Delay(100);
                     NavigationPage.SetHasBackButton(this, true);
                     await popupLayouts.DismissPopup();
                     if (Xamarin.Forms.Device.OS.ToString() == "Android")
                     {
                         s = await CheckStoragePermisions();
                     }

                     if (s)
                     {
                         var GpsImage = _alertsEsclatedToAgentViewModel.GpsImage;
                         var AlertImage = _alertsEsclatedToAgentViewModel.strAlertImage;


                         var GpsImagepath = await DependencyService.Get<IPicture>().DownloadImage(GpsImage);


                         var AlertImagepath = await DependencyService.Get<IPicture>().DownloadImage(AlertImage);

                         if (GpsImagepath == null)
                         {
                             UserDialogs.Instance.HideLoading();
                             await DisplayAlert("Oops..!", "Unable to download image.", "Ok");
                             return;
                         }
                         var listOfContent = new List<string>();

                         if (_alertsEsclatedToAgentViewModel.Gps_enable == true)
                         {
                             listOfContent.Add(GpsImagepath);
                         }
                         else
                         {
                             listOfContent.Add("");
                         }
                         if (_alertsEsclatedToAgentViewModel.Camera_Enable == true)
                         {
                             listOfContent.Add(AlertImagepath);
                         }
                         else
                         {
                             listOfContent.Add("");
                         }
                         if (Navigation.NavigationStack.Count <= 3)
                         {
                             var mainPage = new MainPage();
                             var alertListViewModel = new AlertsListViewModel(Navigation, Settings.UserID);
                             if (Settings.RoleID == 3)
                             {
                                 await alertListViewModel.LoadAlertListOfAgent().ConfigureAwait(true);
                             }
                             else
                             {
                                 await alertListViewModel.LoadAlertList().ConfigureAwait(true);
                             }
                             mainPage.nav = new NavigationPage(new AlertListPage(0, alertListViewModel));
                             mainPage.Detail = mainPage.nav;
                             mainPage.IsPresented = false;

                             await System.Threading.Tasks.Task.Delay(1000);

                             Xamarin.Forms.Application.Current.MainPage = mainPage;

                             UserDialogs.Instance.HideLoading();
                         }
                         var day = _alertsEsclatedToAgentViewModel.DeviceDate.ToString("ddd");
                         var date = DateFormat.GetDateTime(_alertsEsclatedToAgentViewModel.DeviceDate, TimeType.DateAndTime);
                         string type = "";
#if __IOS__
                        var result =await UserDialogs.Instance.ActionSheetAsync(null,"Cancel",null, new string[]{"Escalate Alert Image","Escalate Alert Details"});
                        if(result=="Escalate Alert Image")
                        {
                             type = "Image";
                        }
                         else
                        {
                             type = "Details";
                        }
#endif
                         DependencyService.Get<IMMS>().SendMMS(type, "From: " + _alertsEsclatedToAgentViewModel.OwnerFullName + "," + _alertsEsclatedToAgentViewModel.AddressLine1 + " " + _alertsEsclatedToAgentViewModel.AddressLine2 + "\n" + _alertsEsclatedToAgentViewModel.Mobile1 + " " + _alertsEsclatedToAgentViewModel.Mobile2 + "\n" + "Alert Device: " + _alertsEsclatedToAgentViewModel.FriendlyName + "\n" + "Type: " + MyController.GetAlertTypeName(_alertsEsclatedToAgentViewModel.alert_type) + ", " + day + " " + date, listOfContent, _alertsEsclatedToAgentViewModel);
                         UserDialogs.Instance.HideLoading();
                     }
                 }
                 catch (Exception ex)
                 {
                     UserDialogs.Instance.HideLoading();
                 }

             };
            BtnEscalateToOtherByEmail.Clicked += async (object sender, EventArgs e) =>
 {
     try
     {
         bool s = true;
         UserDialogs.Instance.ShowLoading("Connecting to E-Mail System...", Acr.UserDialogs.MaskType.Gradient);
         await System.Threading.Tasks.Task.Delay(100);

         NavigationPage.SetHasBackButton(this, true);
         await popupLayouts.DismissPopup();
         if (Xamarin.Forms.Device.OS.ToString() == "Android")
         {
             s = await CheckStoragePermisions();
         }

         if (s)
         {
             var GpsImage = _alertsEsclatedToAgentViewModel.GpsImage;
             var GpsImagepath = await DependencyService.Get<IPicture>().DownloadImage(GpsImage);

             var AlertImage = _alertsEsclatedToAgentViewModel.strAlertImage;
             var AlertImagepath = await DependencyService.Get<IPicture>().DownloadImage(AlertImage);

             if (GpsImagepath == null)
             {
                 UserDialogs.Instance.HideLoading();
                 await DisplayAlert("Oops..!", "Unable to download image.", "Ok");
                 return;
             }
             var listOfContent = new List<string>();
             if (_alertsEsclatedToAgentViewModel.Gps_enable == true)
             {
                 listOfContent.Add(GpsImagepath);
             }
             else
             {
                 listOfContent.Add("");
             }
             if (_alertsEsclatedToAgentViewModel.Camera_Enable == true)
             {
                 listOfContent.Add(AlertImagepath);
             }
             else
             {
                 listOfContent.Add("");
             }

             var listOfRecepients = new List<string>();
             listOfRecepients.Add("");

             var listOfRecepientsBCC = new List<string>();
             listOfRecepientsBCC.Add("");

             var listOfRecepientsCC = new List<string>();
             listOfRecepientsCC.Add("");

             if (Navigation.NavigationStack.Count <= 3)
             {
                 var mainPage = new MainPage();
                 var alertListViewModel = new AlertsListViewModel(Navigation, Settings.UserID);
                 if (Settings.RoleID == 3)
                 {
                     await alertListViewModel.LoadAlertListOfAgent().ConfigureAwait(true);
                 }
                 else
                 {
                     await alertListViewModel.LoadAlertList().ConfigureAwait(true);
                 }
                 mainPage.nav = new NavigationPage(new AlertListPage(0, alertListViewModel));
                 mainPage.Detail = mainPage.nav;
                 mainPage.IsPresented = false;

                 await System.Threading.Tasks.Task.Delay(1500);

                 Xamarin.Forms.Application.Current.MainPage = mainPage;

                 UserDialogs.Instance.HideLoading();
             }

             if (Xamarin.Forms.Device.OS.ToString() == "Android")
             {
                 DependencyService.Get<IMMS>().SendMMS("", "From: " + _alertsEsclatedToAgentViewModel.OwnerFullName + ", " + _alertsEsclatedToAgentViewModel.AddressLine1 + " " + _alertsEsclatedToAgentViewModel.AddressLine2 + "\n" + _alertsEsclatedToAgentViewModel.Mobile1 + " " + _alertsEsclatedToAgentViewModel.Mobile2 + "\n" + "Alert Device: " + _alertsEsclatedToAgentViewModel.FriendlyName + "\n" + "Type: " + MyController.GetAlertTypeName(_alertsEsclatedToAgentViewModel.alert_type) + ", " + _alertsEsclatedToAgentViewModel.DeviceDate, listOfContent, _alertsEsclatedToAgentViewModel);
             }
             else
             {
                 if (Xamarin.Forms.Device.OS == TargetPlatform.iOS)
                 {
                     DependencyService.Get<IMail>().SendMail(listOfRecepients, listOfRecepientsBCC, listOfRecepientsCC, "", "From: " + _alertsEsclatedToAgentViewModel.OwnerFullName + ", " + _alertsEsclatedToAgentViewModel.AddressLine1 + " " + _alertsEsclatedToAgentViewModel.AddressLine2 + ",\n" + _alertsEsclatedToAgentViewModel.Mobile1 + " " + _alertsEsclatedToAgentViewModel.Mobile2 + "\n" + "Alert Device: " + _alertsEsclatedToAgentViewModel.FriendlyName + "\n" + "Type: " + MyController.GetAlertTypeName(_alertsEsclatedToAgentViewModel.alert_type) + ", " + _alertsEsclatedToAgentViewModel.DeviceDate, listOfContent);
                 }
             }


             UserDialogs.Instance.HideLoading();
         }

     }
     catch
     {
         UserDialogs.Instance.HideLoading();
     }

 };
            if (popupLayouts.IsPopupActive)
            {

            }
            else
            {


                var view = new Frame
                {
                    Padding = new Thickness(0, 0, 0, 0),
                    HasShadow = true,
                    HeightRequest = this.Height * pddingH,
                    WidthRequest = this.Width * paddingW,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    BackgroundColor = Color.FromHex("f2f2f2"),
                    Content = mainlayout
                };



                this.ToolbarItems.Clear();
                //NavigationPage.SetHasBackButton(this, false);
                popupLayouts.ShowPopup(view);
            }
        }

        async Task<bool> CheckStoragePermisions()
        {
            var results = false;
            try
            {
#if __ANDROID__
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Plugin.Permissions.Abstractions.Permission.Storage);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Plugin.Permissions.Abstractions.Permission.Storage))
                    {
                    }

                    var r = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Plugin.Permissions.Abstractions.Permission.Storage });
                    status = r[Plugin.Permissions.Abstractions.Permission.Storage];
                }
                if (status == PermissionStatus.Granted)
                {
                    results = true;
                }
                else if (status != PermissionStatus.Unknown)
                {
                }
#endif
            }
            catch (Exception ex)
            {
                results = false;
                MyController.ErrorManagement(ex.Message);
            }
            return results;
        }


        public void GetPopupOfChangeCubeConfig(AlertsEsclatedToAgentViewModel _alertsEsclatedToAgentViewModel, ObservableCollection<DeviceConfig> profiledetailslist, AlertsListViewModel alertList)
        {

            //int DevId = (int)selectedmsg.Device_idx;
            var CloseIcon = new Image { Source = "CloseIcon.png", HeightRequest = 25, WidthRequest = 25, HorizontalOptions = LayoutOptions.EndAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };

            var lblTitle = new Label { Text = "Change Configuration", TextColor = Color.Black, FontAttributes = FontAttributes.Bold, FontSize = 19, HorizontalOptions = LayoutOptions.StartAndExpand };

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

            var LiueRatingPicker = new CustomPicker { Title = "Configuration Store list", TitleTextColor = Color.Black, WidthRequest = MyController.ScreenWidth - w / 3, HeightRequest = 40, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };
            var imgLiueRatingArrow = new Image { Source = "downArrow.png", HorizontalOptions = LayoutOptions.EndAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand, HeightRequest = 40 + 3, WidthRequest = 40 + 3 };
            var configrationlist = new InfiniteListView();
            configrationlist.HasUnevenRows = true;
            //configrationlist.RowHeight = 35;
            configrationlist.ItemTemplate = new DataTemplate(typeof(ConfigrationViewCell));
            configrationlist.ItemsSource = profiledetailslist;
            //if (profiledetailslist != null)
            //{
            //    foreach (var item in profiledetailslist)
            //    {
            //        LiueRatingPicker.Items.Add(item.ConfigurationName);
            //    }
            //}
            int profileid = 0; bool isConfigSelected = false;
            configrationlist.ItemTapped += (object sender, ItemTappedEventArgs e) =>
              {
                  try
                  {
                      isConfigSelected = true;
                      var alert = e.Item as DeviceConfig;
                      profileid = alert.DeviceCustomConfig_Idx;
                  }
                  catch (Exception ex)
                  {

                  }
              };
            var LayoutLiueRating = new StackLayout { Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.CenterAndExpand };

            StackLayout headstack1 = new StackLayout() { Padding = new Thickness(20, 10, 20, 10), BackgroundColor = Color.FromRgb(244, 244, 244), VerticalOptions = LayoutOptions.Start, HorizontalOptions = LayoutOptions.FillAndExpand };

            headstack1.Orientation = StackOrientation.Horizontal;


            // LayoutLiueRating.Children.Add(LiueRatingPicker);
            //LayoutLiueRating.Children.Add(imgLiueRatingArrow);
            LayoutLiueRating.Children.Add(configrationlist);
            var lblDetails = new Label { Text = "Select Configuration to Apply", TextColor = Color.Black, FontAttributes = FontAttributes.Bold, FontSize = 15, XAlign = TextAlignment.Start, VerticalOptions = LayoutOptions.StartAndExpand, HorizontalOptions = LayoutOptions.StartAndExpand };


            var BtnEscalateToAgent = new Xamarin.Forms.Button
            {
                Text = "Apply New configuration",
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

            btnstack.Children.Add(BtnEscalateToAgent);



            var layouth = new StackLayout { Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.StartAndExpand, Spacing = 18 };

            layouth.Children.Add(lblDetails);
            layouth.Children.Add(LayoutLiueRating);

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


            var popupLayouts = this.Content as CustomPopup;




            TapGestureRecognizer bigt = new TapGestureRecognizer();
            bigt.Tapped += async (object sender, EventArgs e) =>
            {
                popupLayouts.DismissPopup();
            };

            CloseIcon.GestureRecognizers.Add(bigt);

            BtnEscalateToAgent.Clicked += async (sender, e) =>
              {

                  try
                  {

                      if (!isConfigSelected)
                      {

                          await UserDialogs.Instance.AlertAsync("Please choose the configuration!! ", "Information", "OK");
                          return;
                      }
                      if (_alertsEsclatedToAgentViewModel.CountDown == 0)
                      {
                          string address = "Configuration will be updated at next Heartbeat or Alert!!";
                          var answer = await UserDialogs.Instance.ConfirmAsync(address, "Confirmation", "Continue", "Cancel");
                          if (answer == true)
                          {
                              var alertList2 = new AlertsListViewModel(Navigation, Settings.UserID);


                              //var selectedProfile = LiueRatingPicker.SelectedIndex + 1;
                              //var itemBasic = profiledetailslist.ToList();
                              //var selectedindex = itemBasic.ElementAt(LiueRatingPicker.SelectedIndex);
                              //int profileid = selectedindex.DeviceCustomConfig_Idx;
                              bool lei1 = await alertList.ChangeCubeConfigration(_DeviceId, profileid, DateTime.Now).ConfigureAwait(true);

                              if (lei1)
                              {
                                  UserDialogs.Instance.HideLoading();
                                  await UserDialogs.Instance.AlertAsync("Configuration changed Successfully", "Information", "OK");
                                  UserDialogs.Instance.ShowLoading("Please wait...", MaskType.Gradient);
                              }
                              else
                              {
                                  UserDialogs.Instance.HideLoading();
                                  await UserDialogs.Instance.AlertAsync("Uwatch server error, try again after 5 minutes !! ", "Information", "OK");
                                  UserDialogs.Instance.ShowLoading("Please wait...", MaskType.Gradient);
                              }
                              MyController.fromAssetsToGallery = true;

                              if (Navigation.NavigationStack.Count <= 3)
                              {
                                  var mainPage = new MainPage();
                                  var alertListViewModel = new AlertsListViewModel(Navigation, Settings.UserID);
                                  await alertListViewModel.LoadAlertList().ConfigureAwait(true);
                                  mainPage.nav = new NavigationPage(new AlertListPage(0, alertListViewModel));
                                  mainPage.Detail = mainPage.nav;
                                  mainPage.IsPresented = false;

                                  Xamarin.Forms.Application.Current.MainPage = mainPage;

                                  await System.Threading.Tasks.Task.Delay(1500);



                                  UserDialogs.Instance.HideLoading();
                              }
                              else
                              {
                                  await Navigation.PopAsync();
                                  System.GC.Collect();
                                  await System.Threading.Tasks.Task.Delay(1000);
                                  UserDialogs.Instance.HideLoading();
                              }


                              await System.Threading.Tasks.Task.Delay(1000);
                              UserDialogs.Instance.HideLoading();

                          }
                          else
                          {
                              popupLayouts.DismissPopup();
                          }
                      }
                      else
                      {
                          var alertList2 = new AlertsListViewModel(Navigation, Settings.UserID);


                          var selectedProfile = LiueRatingPicker.SelectedIndex + 1;



                          //var itemBasic = profiledetailslist.Where(x => x.configType == "Basic").ToList();
                          //var selectedindex = itemBasic.ElementAt(LiueRatingPicker.SelectedIndex);
                          //int profileid = selectedindex.DeviceCustomConfig_Idx;
                          bool _ChangeCubeConfigration = await alertList.ChangeCubeConfigration(_DeviceId, profileid, DateTime.Now).ConfigureAwait(true);

                          if (_ChangeCubeConfigration)
                          {
                              UserDialogs.Instance.HideLoading();
                              await UserDialogs.Instance.AlertAsync("Configuration changed Successfully", "Information", "OK");
                              UserDialogs.Instance.ShowLoading("Please wait...", MaskType.Gradient);
                          }
                          else
                          {
                              UserDialogs.Instance.HideLoading();
                              await UserDialogs.Instance.AlertAsync("Uwatch server error, try again after 5 minutes !! ", "Information", "OK");
                              UserDialogs.Instance.ShowLoading("Please wait...", MaskType.Gradient);
                          }
                          MyController.fromAssetsToGallery = true;

                          if (Navigation.NavigationStack.Count <= 3)
                          {
                              var mainPage = new MainPage();
                              var alertListViewModel = new AlertsListViewModel(Navigation, Settings.UserID);
                              await alertListViewModel.LoadAlertList().ConfigureAwait(true);
                              mainPage.nav = new NavigationPage(new AlertListPage(0, alertListViewModel));
                              mainPage.Detail = mainPage.nav;
                              mainPage.IsPresented = false;



                              Xamarin.Forms.Application.Current.MainPage = mainPage;

                              await System.Threading.Tasks.Task.Delay(1500);

                              UserDialogs.Instance.HideLoading();
                          }
                          else
                          {
                              await Navigation.PopAsync();
                              System.GC.Collect();
                              await System.Threading.Tasks.Task.Delay(1000);
                              UserDialogs.Instance.HideLoading();
                          }


                          await System.Threading.Tasks.Task.Delay(1000);
                          UserDialogs.Instance.HideLoading();
                      }
                  }
                  catch { UserDialogs.Instance.HideLoading(); }

              };



            if (popupLayouts.IsPopupActive)
            {

            }
            else
            {


                var view = new Frame
                {
                    Padding = new Thickness(0, 0, 0, 0),
                    HasShadow = true,
                    HeightRequest = App.ScreenHeight - 100,
                    WidthRequest = this.Width * paddingW,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    BackgroundColor = Color.FromHex("f2f2f2"),
                    Content = mainlayout
                };



                this.ToolbarItems.Clear();
                popupLayouts.ShowPopup(view);
            }
        }

    }
}

