using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using FFImageLoading.Forms;
using UwatchPCL;
using UwatchPCL.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Rg.Plugins.Popup.Services;
using System.Collections.ObjectModel;
using Device = Xamarin.Forms.Device;
using System.Diagnostics;


#if __ANDROID__
using Android.App;
using Android.Widget;
using uWatch.Droid;
#endif

namespace uWatch
{
    public partial class AlertDetail : ContentPage
    {
        AlertsEsclatedToAgentViewModel Alert;
        public Label lblCountdownValue;
        double pddingH, paddingW;
        int PaddingMain;
        double _ScreenWidth = MyController.VirtualWidth;
        double _ScreenHeight = MyController.VirtualHeight;
        bool noimage = false;
        bool noMap = false;
        Geocoder geoCoder;
        AlertsEsclatedToAgentViewModel SendToActionPage;
        public AlertDetailsViewModel ViewModel { get; set; }
        ToolbarItem _batchgallery;
        bool iconadded = false;
        ObservableCollection<DeviceConfig> profiledetailslist;

        public AlertDetail(string str = "")
        {
            try
            {
                StackLayout stackLoading = new StackLayout();
                stackLoading.Orientation = StackOrientation.Horizontal;

                ActivityIndicator activityIndicatorLoading = new ActivityIndicator();

                var lblLoading = new Label { Text = "Loading...", TextColor = Color.Red, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.CenterAndExpand };
                stackLoading.Children.Add(activityIndicatorLoading);
                stackLoading.Children.Add(lblLoading);
                Content = stackLoading;
                if (!string.IsNullOrEmpty(str))
                {
                    var lblMsg = new Label { Text = str, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand, FontSize = 15 };
                    Content = lblMsg;
                }
                else
                {
                    geoCoder = new Geocoder();
                    var alertid = MyController.AlertId;
                    AlertsEsclatedToAgentViewModel req = new AlertsEsclatedToAgentViewModel();
                    req.alertlog_idx = alertid == "" ? 0 : Convert.ToInt32(alertid);
                    var alert = ApiService.Instance.GetAlert(req).Result;
                    this.Alert = alert;
                    ViewModel = new AlertDetailsViewModel(this.Alert);
                    Timer();
                    BindingContext = ViewModel.device;
                    SetLayout();
                }
            }
            catch (Exception ex)
            {
                MyController.ErrorManagement(ex.Message);
            }
        }
        public AlertDetail(AlertsEsclatedToAgentViewModel alert)
        {
            try
            {

                if (alert.IsBatchUpload)
                {
                    if (!iconadded)
                    {
                        _batchgallery = new ToolbarItem
                        {
                            Text = "Batch Gallery"
                        };
                        this.ToolbarItems.Add(_batchgallery);
                        _batchgallery.Clicked += async (sender, e) =>
                        {
                            UserDialogs.Instance.ShowLoading("Loading...");
                            if (alert.IsBatchUpload)
                            {
                                var batchimagelist = await ApiService.Instance.GetBatchAlertImages(alert.batchImageIdentifier);
                                await Navigation.PushAsync(new BatchAlertGallery(batchimagelist));
                            }
                            else
                            {
                                UserDialogs.Instance.HideLoading();
                                UserDialogs.Instance.Alert("This is not a batch Alert.", "Information", "OK");
                            }
                            UserDialogs.Instance.HideLoading();
                        };

                        iconadded = true;
                    }
                }

                StackLayout stackLoading = new StackLayout();
                stackLoading.Orientation = StackOrientation.Horizontal;

                ActivityIndicator activityIndicatorLoading = new ActivityIndicator();


                var lblLoading = new Label { Text = "Loading...", TextColor = Color.Red, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.CenterAndExpand };
                stackLoading.Children.Add(activityIndicatorLoading);
                stackLoading.Children.Add(lblLoading);
                Content = stackLoading;
                SendToActionPage = alert;
                this.Alert = alert;
                geoCoder = new Geocoder();
                ViewModel = new AlertDetailsViewModel(this.Alert);
                Timer();
                BindingContext = ViewModel.device;
                SetLayout();
            }
            catch (Exception ex)
            {
                MyController.ErrorManagement(ex.Message);
            }
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
                Title = "Alert Details";
                await AddLayout();

            }
            catch { }
        }
        private async System.Threading.Tasks.Task AddLayout()
        {
            try
            {

                double position = 0;
                double newx20 = MyUiUtils.getPercentual(_ScreenWidth, 20);
                double newx40 = MyUiUtils.getPercentual(_ScreenWidth, 40);
                double newx60 = MyUiUtils.getPercentual(_ScreenWidth, 60);
                double newx80 = MyUiUtils.getPercentual(_ScreenWidth, 80);


                var img1 = new Image { Source = "gray_line.png", Aspect = Aspect.Fill, WidthRequest = App.ScreenWidth };
                var img2 = new Image { Source = "gray_line.png", Aspect = Aspect.Fill, WidthRequest = App.ScreenWidth };
                var img3 = new Image { Source = "gray_line.png", Aspect = Aspect.Fill, WidthRequest = App.ScreenWidth };
                var box = new BoxView { HeightRequest = 5 };

                StackLayout layout = new StackLayout { VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand };
                StackLayout mainstack = new StackLayout { VerticalOptions = LayoutOptions.StartAndExpand };
                mainstack.Padding = new Thickness(12, 0, 12, 10);

                StackLayout layoutImageStack = new StackLayout { VerticalOptions = LayoutOptions.Start, HorizontalOptions = LayoutOptions.Start };
                StackLayout mapstack = new StackLayout { VerticalOptions = LayoutOptions.EndAndExpand, HorizontalOptions = LayoutOptions.EndAndExpand, Padding = new Thickness(0, 0, 1, 0) };
                StackLayout mapandimagestack = new StackLayout { Orientation = StackOrientation.Horizontal, Spacing = 2, HorizontalOptions = LayoutOptions.FillAndExpand };
                mapandimagestack.Children.Add(layoutImageStack);
                mapandimagestack.Children.Add(mapstack);

                StackLayout awaketimestack = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.EndAndExpand };
                StackLayout countdownstack = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.EndAndExpand, VerticalOptions = LayoutOptions.EndAndExpand };

                StackLayout timestack = new StackLayout { Orientation = StackOrientation.Horizontal };
                timestack.Children.Add(awaketimestack);
                timestack.Children.Add(countdownstack);

                StackLayout descdetail = new StackLayout { };

                StackLayout buttonstack = new StackLayout { VerticalOptions = LayoutOptions.EndAndExpand };

                StackLayout Templayout = new StackLayout { HorizontalOptions = LayoutOptions.StartAndExpand };
                StackLayout signallayout = new StackLayout { HorizontalOptions = LayoutOptions.CenterAndExpand };
                StackLayout batterylayout = new StackLayout { HorizontalOptions = LayoutOptions.EndAndExpand };

                StackLayout TSBlayout = new StackLayout { Orientation = StackOrientation.Horizontal };
                TSBlayout.Children.Add(Templayout);
                TSBlayout.Children.Add(signallayout);
                TSBlayout.Children.Add(batterylayout);

                StackLayout headstack = new StackLayout() { BackgroundColor = Color.FromRgb(244, 244, 244), Padding = new Thickness(0, 10, 0, 10) };
                headstack.Orientation = StackOrientation.Horizontal;
                headstack.HorizontalOptions = LayoutOptions.FillAndExpand;
                headstack.VerticalOptions = LayoutOptions.FillAndExpand;

                StackLayout Substack = new StackLayout() { };
                Substack.Orientation = StackOrientation.Horizontal;
                Substack.HorizontalOptions = LayoutOptions.CenterAndExpand;
                Substack.VerticalOptions = LayoutOptions.CenterAndExpand;

                var lblAlertText1 = new Label { Text = "!!", FontSize = 19, FontAttributes = FontAttributes.Bold, TextColor = Color.Red, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };
                var imgAlerts = new Image { HeightRequest = 20, WidthRequest = 20, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };
                if (ViewModel != null)
                {
                    if (ViewModel.device != null)
                        imgAlerts.Source = await MyController.GetAlertTypelImage(ViewModel.device.alert_type);
                }

                var lblAlertText = new Label { FontSize = 19, FontAttributes = FontAttributes.Bold, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand, TextColor = Color.Red };

                var lbl = new Label { Text = ";", TextColor = Color.Red };
                var lblsep = new Label { Text = "!!", TextColor = Color.Red, FontSize = 19, FontAttributes = FontAttributes.Bold };
                if (ViewModel.device != null)
                {
                    if (MyController.GetAlertTypeName(ViewModel.device.alert_type).ToUpper() == "BLUETOOTH")
                    {
                        if (ViewModel.device.SlaveFriendlyName.ToString() != null || ViewModel.device.SlaveFriendlyName.ToString() == "")
                        {
                            if (ViewModel.device.SlaveFriendlyName.ToString().Contains(";"))
                            {
                                lblAlertText1.Text = "";
                                var formattedtext = ViewModel.device.SlaveFriendlyName.ToString().Replace(";", "!! \n !!");
                                lblAlertText.Text = lblsep.Text + ViewModel.device.SlaveFriendlyName.ToString() + " !!";
                            }
                            else
                            {

                                lblAlertText.Text = ViewModel.device.SlaveFriendlyName.ToString() + " !!";
                            }
                        }
                        else
                        {
                            lblAlertText.Text = MyController.GetAlertTypeName(ViewModel.device.alert_type).ToUpper() + " !!";
                        }
                    }
                    else
                    {
                        lblAlertText.TextColor = Color.Red;
                        lblAlertText.Text = MyController.GetAlertTypeName(ViewModel.device.alert_type).ToUpper() + " !! ";
                    }
                }
                Substack.Children.Add(lblAlertText1);
                Substack.Children.Add(imgAlerts);
                Substack.Children.Add(lblAlertText);

                headstack.Children.Add(Substack);

                var lblFirndlyName = new Label();
                lblFirndlyName.SetBinding(Label.TextProperty, new Binding("FriendlyName", BindingMode.Default));
                lblFirndlyName.TextColor = Color.Gray;
                lblFirndlyName.HorizontalTextAlignment = TextAlignment.Start;
                lblFirndlyName.HorizontalOptions = LayoutOptions.FillAndExpand;
                lblFirndlyName.VerticalOptions = LayoutOptions.StartAndExpand;
                lblFirndlyName.FontSize = 18;

                var lblAlertDateTime = new Label();
                var alertdate = ViewModel.device.DeviceDate.ToString("ddd") + " " + DateFormat.GetDateTime(ViewModel.device.DeviceDate, TimeType.DateAndTime);
                lblAlertDateTime.SetBinding(Label.TextProperty, new Binding("DeviceDate", BindingMode.Default, stringFormat: alertdate));
                lblAlertDateTime.HorizontalTextAlignment = TextAlignment.Start;
                lblAlertDateTime.TextColor = Color.Gray;
                lblAlertDateTime.HorizontalOptions = LayoutOptions.FillAndExpand;
                lblAlertDateTime.VerticalOptions = LayoutOptions.StartAndExpand;
                lblAlertDateTime.FontSize = 18;

                descdetail.Children.Add(lblFirndlyName);
                descdetail.Children.Add(lblAlertDateTime);

                var lblTemp = new Label();
                lblTemp.Text = "Temp.";
                lblTemp.FontSize = 18;
                lblTemp.TextColor = Color.Gray;
                lblTemp.VerticalOptions = LayoutOptions.CenterAndExpand;

                var lblSignal = new Label();
                lblSignal.Text = "Signal";
                lblSignal.FontSize = 18;
                lblSignal.TextColor = Color.Gray;
                lblSignal.VerticalOptions = LayoutOptions.CenterAndExpand;

                var lblBattery = new Label();
                lblBattery.Text = "Battery";
                lblBattery.FontSize = 18;
                lblBattery.TextColor = Color.Gray;
                lblBattery.VerticalOptions = LayoutOptions.CenterAndExpand;


                var imgTemp = new Image { Source = "Icon.png", Aspect = Aspect.AspectFit, VerticalOptions = LayoutOptions.CenterAndExpand, HeightRequest = 50, WidthRequest = 50 };
                imgTemp.SetBinding(Image.SourceProperty, new Binding("TemperatureImage", BindingMode.Default));

                var imgSignal = new Image { Source = "Icon.png", Aspect = Aspect.AspectFit, VerticalOptions = LayoutOptions.CenterAndExpand, HeightRequest = 50, WidthRequest = 50 };
                imgSignal.SetBinding(Image.SourceProperty, new Binding("SignalImage", BindingMode.Default));

                var imgBattery = new Image { Source = "Icon.png", Aspect = Aspect.AspectFit, VerticalOptions = LayoutOptions.CenterAndExpand, HeightRequest = 50, WidthRequest = 50 };
                imgBattery.SetBinding(Image.SourceProperty, new Binding("BatteryImage", BindingMode.Default));

                var lblTempValue = new Label { VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center };
                lblTempValue.SetBinding(Label.TextProperty, new Binding("DegC", BindingMode.Default, stringFormat: "{0}.C"));

                var lblSignalValue = new Label { VerticalOptions = LayoutOptions.Start, HorizontalOptions = LayoutOptions.Center };
                lblSignalValue.SetBinding(Label.TextProperty, new Binding("Signal", BindingMode.Default, stringFormat: "{0}%"));

                var lblBatteryValue = new Label { VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center };
                lblBatteryValue.SetBinding(Label.TextProperty, new Binding("Battery", BindingMode.Default, stringFormat: "{0}%"));

                Templayout.Children.Add(lblTemp);
                Templayout.Children.Add(imgTemp);
                Templayout.Children.Add(lblTempValue);

                signallayout.Children.Add(lblSignal);
                signallayout.Children.Add(imgSignal);
                signallayout.Children.Add(lblSignalValue);

                batterylayout.Children.Add(lblBattery);
                batterylayout.Children.Add(imgBattery);
                batterylayout.Children.Add(lblBatteryValue);

                var imgAlert = new CachedImage()
                {

                    CacheDuration = TimeSpan.FromDays(30),
                    DownsampleToViewSize = true,
                    RetryCount = 0,
                    RetryDelay = 250,
                    Aspect = Aspect.Fill,
                    TransparencyEnabled = false,
                    LoadingPlaceholder = ImageSource.FromFile("noimage.gif"),
                    ErrorPlaceholder = ImageSource.FromFile("noimage.gif"),
                };

                if (Xamarin.Forms.Device.Idiom == TargetIdiom.Phone)
                {
                    imgAlert.WidthRequest = _ScreenWidth / 2 - 30;
                    imgAlert.HeightRequest = _ScreenWidth / 2;
                }
                else
                {
                    if (Xamarin.Forms.Device.Idiom == TargetIdiom.Tablet)
                    {
                        imgAlert.WidthRequest = App.ScreenWidth / 2 - 10;
                        imgAlert.HeightRequest = App.ScreenWidth / 2 - 9;
                    }
                }
                layoutImageStack.Children.Add(imgAlert);
                if (ViewModel != null)
                {
                    if (ViewModel.AlertImage != null)
                    {
                        if (ViewModel.AlertImage.Image != null)
                        {
                            imgAlert.HeightRequest = _ScreenWidth / 2 - 10;
                            imgAlert.Source = BytesArraytoImage(ViewModel.AlertImage.Image).Source as ImageSource;
                        }
                        else
                        {
                            noimage = true;
                            if (ViewModel.image_id)
                            {
                                //image available
                                imgAlert.Source = BytesArraytoImage(ViewModel.AlertImage.Image).Source as ImageSource;

                            }
                            else
                            {
                                //image not available
                                if (ViewModel.Camera_Enable == true)
                                {
                                    if (ViewModel.alert_Type == (int)MyController.SENSOR_TYPE.HEARTBEAT || ViewModel.alert_Type == (int)MyController.SENSOR_TYPE.SWITCHON || ViewModel.alert_Type == (int)MyController.SENSOR_TYPE.SWITCHOFF)
                                    {

                                        imgAlert.Source = "image_not_available.png";

                                    }
                                    else
                                    {
                                        if (Alert.ImageComing != null)
                                        {
                                            if (Alert.ImageComing.Value)
                                            {
                                                imgAlert.Source = "coming_soon.png";
                                            }
                                            else
                                            {
                                                imgAlert.Source = "image_not_available.png";
                                            }
                                        }
                                        else
                                        {
                                            imgAlert.Source = "image_not_available.png";
                                        }

                                    }


                                }
                                else
                                {
                                    //show camera disable image
                                    imgAlert.Source = "cameradisabled.png";
                                }

                            }
                        }
                    }

                    TapGestureRecognizer bigt = new TapGestureRecognizer();
                    bigt.Tapped += async (object sender, EventArgs e) =>
                    {
                        if (!noimage)
                        {
                            UserDialogs.Instance.ShowLoading("Please wait...", MaskType.Gradient);

                            try
                            {
                                //ContentPage p = new ContentPage();

                                //var uWatchlogo = new ToolbarItem
                                //{
                                //    Icon = "uwatchlogo.png"
                                //};
                                //p.ToolbarItems.Add(uWatchlogo);
                                //var browser = new CustomWebView();

                                //browser.BackgroundColor = Xamarin.Forms.Color.Black;

                                //string htmlsource = "<style>img{display: inline; height: auto; margin: 0 auto; max-width: 100%;}img-container {position: relative;top: 20%; width:100%; height:300px; overflow:hidden; text-align:center;}img{ max-width:100%;width:100%;vertical-align: middle;}\n</style><html>\n<head>\n</head>\n<body style =\"background-color:black\">\n< div class=\"img-container\"> </div>\n<img src = " + String.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(ViewModel.AlertImage.Image)) + " />\n</body>\n</html>";

                                //var htmlSource = new HtmlWebViewSource();

                                //htmlSource.Html = @htmlsource;

                                //if (Xamarin.Forms.Device.OS != TargetPlatform.iOS)
                                //{
                                //    htmlSource.BaseUrl = DependencyService.Get<IBaseUrl>().Get();
                                //}

                                //browser.Source = htmlSource;

                                //Grid grid = new Grid
                                //{
                                //    VerticalOptions = LayoutOptions.FillAndExpand,
                                //    RowDefinitions = {
                                //    new RowDefinition { Height = new GridLength (40, GridUnitType.Absolute) },
                                //    new RowDefinition { Height = new GridLength (1, GridUnitType.Star) }
                                //   },
                                //    ColumnDefinitions = {
                                //    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },
                                //            }
                                //};


                                //grid.Children.Add(browser, 0, 3, 0, 3);
                                //p.Title = "Alert Image";
                                //p.Content = grid;


                                //await Navigation.PushAsync(p);
                                await Navigation.PushAsync(new AlertImagePage(ViewModel.AlertImage, Title));
                            }
                            catch (System.Exception ex)
                            {
                            }
                            await System.Threading.Tasks.Task.Delay(50);
                            UserDialogs.Instance.HideLoading();
                        }
                    };
                    imgAlert.GestureRecognizers.Add(bigt);


                    var customMap = new Map();

                    customMap.MapType = MapType.Satellite;


                    if (Xamarin.Forms.Device.Idiom == TargetIdiom.Phone)
                    {
                        customMap.WidthRequest = _ScreenWidth / 2 - 50;
                        customMap.HeightRequest = _ScreenWidth / 2;
                    }
                    else
                    {
                        if (Xamarin.Forms.Device.Idiom == TargetIdiom.Tablet)
                        {
                            customMap.WidthRequest = App.ScreenWidth / 2 - 10;
                            customMap.HeightRequest = App.ScreenWidth / 2 - 10;
                        }
                    }

                    var pin = new Pin
                    {
                        Type = PinType.Place,
                        Position = ViewModel.AlertPosition

                    };
                    var pos = ViewModel.AlertPosition;
                    if (Xamarin.Forms.Device.Idiom == TargetIdiom.Phone)
                    {
                        var possibleAddresses = await geoCoder.GetAddressesForPositionAsync(pos);
                        foreach (var address in possibleAddresses)
                        {
                            pin.Address += address + "\n";
                            pin.Label = pos.Latitude.ToString() + "," + pos.Longitude.ToString();
                        }
                    }
                    if (pin.Label != null)
                    {
                        customMap.Pins.Add(pin);
                    }
                    customMap.MoveToRegion(MapSpan.FromCenterAndRadius(pos, Distance.FromMiles(1.0)));
                    TapGestureRecognizer bigMap = new TapGestureRecognizer();
                    bigMap.Tapped += async (object sender, EventArgs e) =>
                    {
                        if (!noMap)
                        {
                            try
                            {
                                UserDialogs.Instance.ShowLoading("Loading...");
                                ContentPage p = new ContentPage();
                                StackLayout st = new StackLayout();
                                var Map = new Map
                                {
                                    MapType = MapType.Satellite,
                                    WidthRequest = _ScreenWidth,
                                    HeightRequest = _ScreenHeight - 80,
                                };
                                var pinbig = new Pin
                                {
                                    Type = PinType.Place,
                                    Position = ViewModel.AlertPosition

                                };
                                var posi = ViewModel.AlertPosition;
                                var Addresses = await geoCoder.GetAddressesForPositionAsync(posi);
                                foreach (var address in Addresses)
                                {
                                    pinbig.Address += address + "\n";
                                    pinbig.Label = posi.Latitude.ToString() + "," + posi.Longitude.ToString();
                                }
                                Map.Pins.Add(pinbig);
                                Map.MoveToRegion(MapSpan.FromCenterAndRadius(posi, Distance.FromMiles(1.0)));
                                st.Children.Add(Map);


                                p.Title = "Alert Position";
                                p.Content = st;
                                System.GC.Collect();


                                await Navigation.PushAsync(p);

                                await System.Threading.Tasks.Task.Delay(2000);
                                UserDialogs.Instance.HideLoading();
                            }
                            catch
                            {
                            }
                        }
                    };

                    if (ViewModel.AlertPosition.Latitude == 0 && ViewModel.AlertPosition.Longitude == 0)
                    {
                        noMap = true;

                        var imgGps = new CachedImage()
                        {

                            CacheDuration = TimeSpan.FromDays(30),
                            DownsampleToViewSize = true,
                            RetryCount = 0,
                            RetryDelay = 250,
                            Aspect = Aspect.Fill,
                            TransparencyEnabled = false,
                            LoadingPlaceholder = ImageSource.FromFile("noimage.gif"),
                            ErrorPlaceholder = ImageSource.FromFile("noimage.gif"),
                        };
                        if (Xamarin.Forms.Device.Idiom == TargetIdiom.Phone)
                        {
                            imgGps.WidthRequest = _ScreenWidth / 2 - 20;
                            imgGps.HeightRequest = _ScreenWidth / 2;
                        }
                        else
                        {
                            if (Xamarin.Forms.Device.Idiom == TargetIdiom.Tablet)
                            {
                                imgGps.WidthRequest = App.ScreenWidth / 2 - 10;
                                imgGps.HeightRequest = App.ScreenWidth / 2 - 10;
                            }
                        }
                        if (ViewModel.Gps_enable == true)
                        {
                            imgGps.Source = "no_GPS.png";
                        }
                        else
                        {
                            imgGps.Source = "gpsdisabled.png";
                        }
                        mapstack.Children.Add(imgGps);
                    }
                    else
                    {

                        var layoutTop = new StackLayout { BackgroundColor = Color.Transparent };
                        var relative = new Xamarin.Forms.RelativeLayout { BackgroundColor = Color.Green };
                        if (Xamarin.Forms.Device.Idiom == TargetIdiom.Phone)
                        {
                            layoutTop.WidthRequest = _ScreenWidth / 2 - 10;
                            layoutTop.HeightRequest = _ScreenWidth / 2 - 10;


                            relative.Children.Add(customMap,
                            Constraint.Constant(0),
                            Constraint.Constant(0),
                            Constraint.RelativeToParent((parent) =>
                            {

                                return _ScreenWidth / 2 - 10;

                            }),
                                        Constraint.RelativeToParent((parent) =>
                                        {

                                            return _ScreenWidth / 2 - 10;

                                        }));
                            relative.Children.Add(layoutTop,
                        Constraint.Constant(0),
                        Constraint.Constant(0),
                        Constraint.RelativeToParent((parent) =>
                        {

                            return _ScreenWidth / 2 - 10;

                        }),
                                    Constraint.RelativeToParent((parent) =>
                                    {

                                        return _ScreenWidth / 2 - 10;

                                    }));
                        }
                        else
                        {
                            if (Xamarin.Forms.Device.Idiom == TargetIdiom.Tablet)
                            {
                                layoutTop.WidthRequest = App.ScreenWidth / 2 - 10;
                                layoutTop.HeightRequest = App.ScreenWidth / 2 - 10;


                                relative.Children.Add(customMap,
                                Constraint.Constant(0),
                                Constraint.Constant(0),
                                Constraint.RelativeToParent((parent) =>
                                {

                                    return App.ScreenWidth / 2 - 10;

                                }),
                                            Constraint.RelativeToParent((parent) =>
                                            {

                                                return App.ScreenWidth / 2 - 10;

                                            }));
                                relative.Children.Add(layoutTop,
                            Constraint.Constant(0),
                            Constraint.Constant(0),
                            Constraint.RelativeToParent((parent) =>
                            {

                                return App.ScreenWidth / 2 - 10;

                            }),
                                        Constraint.RelativeToParent((parent) =>
                                        {

                                            return App.ScreenWidth / 2 - 10;

                                        }));
                            }
                        }



                        layoutTop.GestureRecognizers.Add(bigMap);
                        mapstack.Children.Add(relative);

                    }


                    //imp
                    var lblAwakeTime = new Label { Text = "Awake Time:", TextColor = Color.Gray, FontSize = 18 };
                    var AwakeSec = ViewModel.device.WakeTime * 60;
                    var lblAwakeTimeValue = new Label { TextColor = Color.Black, FontSize = 18 };
                    lblAwakeTimeValue.SetBinding(Label.TextProperty, new Binding("WakeTime", BindingMode.TwoWay, stringFormat: "" + AwakeSec.ToString() + " Secs"));
                    awaketimestack.Children.Add(lblAwakeTime);
                    awaketimestack.Children.Add(lblAwakeTimeValue);

                    //imp
                    var lblCountdown = new Label { Text = "Countdown:", TextColor = Color.Gray, FontSize = 18 };
                    lblCountdownValue = new Label { TextColor = Color.Black, FontSize = 18 };
                    lblCountdownValue.SetBinding(Label.TextProperty, new Binding("CountDown", BindingMode.TwoWay, stringFormat: "{0} Secs"));
                    countdownstack.Children.Add(lblCountdown);
                    countdownstack.Children.Add(lblCountdownValue);

                    var btnAction = new Xamarin.Forms.Button { Text = "Action", BackgroundColor = Color.Red, TextColor = Color.White, VerticalOptions = LayoutOptions.EndAndExpand };
                    var btnArchive = new Xamarin.Forms.Button { Text = "Archive", BackgroundColor = Color.Red, TextColor = Color.White };

                    var bottomStack = new StackLayout() { VerticalOptions = LayoutOptions.EndAndExpand, Padding = new Thickness(20, 0, 20, 10) };

                    var btnEscalate = new Xamarin.Forms.Button { Text = "Escalate", FontSize = 18, BackgroundColor = Color.Red, TextColor = Color.White, HeightRequest = 70 };
                    var btnAgent = new MultiLineButton { Text = "Notify Agent", FontSize = 16, BackgroundColor = Color.Red, TextColor = Color.White, HeightRequest = 70 };
                    var btnCubeConfiguration = new MultiLineButton { Text = "Change Cube Configuration", FontSize = 16, BackgroundColor = Color.Red, TextColor = Color.White, HeightRequest = 70 };
                    if (Xamarin.Forms.Device.Idiom == TargetIdiom.Phone)
                    {
                        buttonstack.Padding = new Thickness(12, 0, 12, 10);
                        btnAction.WidthRequest = _ScreenWidth / 3;
                        btnArchive.WidthRequest = _ScreenWidth / 3;
                        buttonstack.Orientation = StackOrientation.Horizontal;
                        btnAction.HorizontalOptions = LayoutOptions.Start;
                        btnArchive.HorizontalOptions = LayoutOptions.EndAndExpand;
                    }
                    else
                    {

                        buttonstack.Padding = new Thickness(35, 0, 35, 20);
                        btnAction.WidthRequest = MyController.ScreenWidth / 2 + 40;
                        //btnAction.WidthRequest = _ScreenWidth / 2;
                        btnArchive.HeightRequest = 50;
                        btnAction.HeightRequest = 50;

                        btnArchive.WidthRequest = _ScreenWidth / 2;
                        buttonstack.WidthRequest = _ScreenWidth / 2;
                        buttonstack.Orientation = StackOrientation.Vertical;
                    }

                    var bottomBtnGrid = new Grid() { VerticalOptions = LayoutOptions.EndAndExpand };
                    bottomBtnGrid.Children.Add(btnAgent, 0, 0);
                    bottomBtnGrid.Children.Add(btnCubeConfiguration, 1, 0);

                    buttonstack.Children.Add(btnAction);
                    buttonstack.Children.Add(btnArchive);

                    bottomStack.Children.Add(btnEscalate);
                    bottomStack.Children.Add(bottomBtnGrid);

                    buttonstack.WidthRequest = 20;

                    btnCubeConfiguration.Clicked += async (sender, e) =>
                    {
                        GetPopupOfChangeCubeConfigAsync();
                    };

                    async void GetPopupOfChangeCubeConfigAsync()
                    {
                        try
                        {
                            var alertList = new AlertsListViewModel(Navigation, Settings.UserID);
                            profiledetailslist = new ObservableCollection<DeviceConfig>();
                            if (Settings.RoleID != 3)
                            {
                                profiledetailslist = await alertList.FetchConfigurationProfileDetails().ConfigureAwait(true);
                            }
                            var popupLayouts = this.Content as CustomPopup;
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

                            var LiueRatingPicker = new CustomPicker { Title = "Configuration Store list", TitleTextColor = Color.Black, WidthRequest = MyController.ScreenWidth - _ScreenWidth / 3, HeightRequest = 40, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };
                            var imgLiueRatingArrow = new Image { Source = "downArrow.png", HorizontalOptions = LayoutOptions.EndAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand, HeightRequest = 40 + 3, WidthRequest = 40 + 3 };
                            var configrationlist = new InfiniteListView();
                            configrationlist.HasUnevenRows = true;
                            configrationlist.ItemTemplate = new DataTemplate(typeof(ConfigrationViewCell));
                            configrationlist.ItemsSource = profiledetailslist;

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

                            BtnEscalateToAgent.Clicked += async (sender, e) =>
                            {

                                try
                                {

                                    if (!isConfigSelected)
                                    {

                                        await UserDialogs.Instance.AlertAsync("Please choose the configuration!! ", "Information", "OK");
                                        return;
                                    }
                                    if (SendToActionPage.CountDown == 0)
                                    {
                                        string address = "Configuration will be updated at next Heartbeat or Alert!!";
                                        var answer = await UserDialogs.Instance.ConfirmAsync(address, "Confirmation", "Continue", "Cancel");
                                        if (answer == true)
                                        {
                                            var alertList2 = new AlertsListViewModel(Navigation, Settings.UserID);

                                            UserDialogs.Instance.ShowLoading("Please wait...", MaskType.Gradient);

                                            bool lei1 = await alertList.ChangeCubeConfigration(SendToActionPage.device_idx, profileid, DateTime.Now);

                                            if (lei1)
                                            {
                                                UserDialogs.Instance.HideLoading();
                                                await UserDialogs.Instance.AlertAsync("Configuration changed Successfully", "Cube configuration", "OK");
                                                UserDialogs.Instance.ShowLoading("Please wait...", MaskType.Gradient);
                                            }
                                            else
                                            {
                                                UserDialogs.Instance.HideLoading();
                                                await UserDialogs.Instance.AlertAsync("Uwatch server error, try again after 5 minutes !! ", "Cube configuration", "OK");
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

                                                //await System.Threading.Tasks.Task.Delay(500);



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
                                        bool _ChangeCubeConfigration = await alertList.ChangeCubeConfigration(SendToActionPage.device_idx, profileid, DateTime.Now).ConfigureAwait(true);

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




                            TapGestureRecognizer bigtt = new TapGestureRecognizer();
                            bigtt.Tapped += async (object sender, EventArgs e) =>
                            {
                                await popupLayouts.DismissPopup();
                            };

                            CloseIcon.GestureRecognizers.Add(bigtt);

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
                                await popupLayouts.ShowPopup(view);
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex);
                        }


                    }

                    btnEscalate.Clicked += async (sender, e) =>
                    {

                        try
                        {
                            bool s = true;
                            UserDialogs.Instance.ShowLoading("Connecting to social system...", Acr.UserDialogs.MaskType.Gradient);
                            await Task.Delay(100);
                            if (Xamarin.Forms.Device.OS.ToString() == "Android")
                            {
                                s = await CheckStoragePermisions();
                            }

                            if (s)
                            {
                                var GpsImage = SendToActionPage.GpsImage;
                                var AlertImage = SendToActionPage.strAlertImage;


                                var GpsImagepath = await DependencyService.Get<IPicture>().DownloadImage(GpsImage);

                                var AlertImagepath = await DependencyService.Get<IPicture>().DownloadImage(AlertImage);

                                if (GpsImagepath == null)
                                {
                                    UserDialogs.Instance.HideLoading();
                                    await DisplayAlert("Oops..!", "Unable to download image.", "Ok");
                                    return;
                                }
                                var listOfContent = new List<string>();

                                if (SendToActionPage.Gps_enable == true)
                                {
                                    listOfContent.Add(GpsImagepath);
                                }
                                else
                                {
                                    listOfContent.Add("");
                                }
                                if (SendToActionPage.Camera_Enable == true)
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

                                    //await System.Threading.Tasks.Task.Delay(1000);

                                    Xamarin.Forms.Application.Current.MainPage = mainPage;
                                }
                                 await PopupNavigation.Instance.PushAsync(new EscalatePopUp(listOfContent, SendToActionPage), true);
                                UserDialogs.Instance.HideLoading();
                            }
                        }
                        catch (Exception ex)
                        {
                            UserDialogs.Instance.HideLoading();
                        }
                        //                        Device.BeginInvokeOnMainThread(async() =>
                        //                        {
                        //                            UserDialogs.Instance.ShowLoading("Connecting to social system...", Acr.UserDialogs.MaskType.Gradient);


                        //                            // Device.BeginInvokeOnMainThread(() => UserDialogs.Instance.ShowLoading("Connecting to social system...", MaskType.Gradient));
                        //                            await Task.Delay(100);
                        //                            try
                        //                            {
                        //                                bool s = true;
                        //#if __ANDROID__
                        //                            s = await CheckStoragePermisions();
                        //#endif

                        //        if (s)
                        //        {
                        //            var GpsImage = SendToActionPage.GpsImage;
                        //            var AlertImage = SendToActionPage.strAlertImage;


                        //            var GpsImagepath = await DependencyService.Get<IPicture>().DownloadImage(GpsImage);

                        //            var AlertImagepath = await DependencyService.Get<IPicture>().DownloadImage(AlertImage);

                        //            if (GpsImagepath == null)
                        //            {
                        //                UserDialogs.Instance.HideLoading();
                        //                await DisplayAlert("Oops..!", "Unable to download image.", "Ok");
                        //                return;
                        //            }
                        //            var listOfContent = new List<string>();

                        //            if (SendToActionPage.Gps_enable == true)
                        //            {
                        //                listOfContent.Add(GpsImagepath);
                        //            }
                        //            else
                        //            {
                        //                listOfContent.Add("");
                        //            }
                        //            if (SendToActionPage.Camera_Enable == true)
                        //            {
                        //                listOfContent.Add(AlertImagepath);
                        //            }
                        //            else
                        //            {
                        //                listOfContent.Add("");
                        //            }
                        //            if (Navigation.NavigationStack.Count <= 3)
                        //            {
                        //                var mainPage = new MainPage();
                        //                var alertListViewModel = new AlertsListViewModel(Navigation, Settings.UserID);
                        //                if (Settings.RoleID == 3)
                        //                {
                        //                    await alertListViewModel.LoadAlertListOfAgent().ConfigureAwait(false);
                        //                }
                        //                else
                        //                {
                        //                    await alertListViewModel.LoadAlertList().ConfigureAwait(false);
                        //                }
                        //                mainPage.nav = new NavigationPage(new AlertListPage(0, alertListViewModel));
                        //                mainPage.Detail = mainPage.nav;
                        //                mainPage.IsPresented = false;

                        //                //await System.Threading.Tasks.Task.Delay(1000);

                        //                //UserDialogs.Instance.HideLoading();
                        //                var day = SendToActionPage.DeviceDate.ToString("ddd");
                        //                var date = DateFormat.GetDateTime(SendToActionPage.DeviceDate, TimeType.DateAndTime);
                        //                //   await PopupNavigation.Instance.PushAsync(new EscalatePopUp(listOfContent,
                        //                // SendToActionPage), true);
                        //                string type = "";

                        //                var result = await UserDialogs.Instance.ActionSheetAsync("Select to escalate", "Cancel", null, new string[] { "Escalate Alert Image", "Escalate Alert Details" }).ConfigureAwait(true);
                        //                if (result == "Escalate Alert Image")
                        //                {
                        //                    type = "Image";
                        //                }
                        //                else if (result == "Escalate Alert Details")
                        //                {
                        //                    type = "Details";
                        //                }
                        //                else
                        //                {
                        //                    UserDialogs.Instance.HideLoading();
                        //                    return;
                        //                }


                        //                DependencyService.Get<IMMS>().SendMMS(type, "From: " + SendToActionPage.OwnerFullName + "," + SendToActionPage.AddressLine1 + " " + SendToActionPage.AddressLine2 + "\n" + SendToActionPage.Mobile1 + " " + SendToActionPage.Mobile2 + "\n" + "Alert Device: " + SendToActionPage.FriendlyName + "\n" + "Type: " + MyController.GetAlertTypeName(SendToActionPage.alert_type) + ", " + day + " " + date, listOfContent, SendToActionPage);
                        //            }
                        //        }
                        //    }
                        //    catch (Exception ex)
                        //    {
                        //        Debug.WriteLine(ex);
                        //    }
                        //    UserDialogs.Instance.HideLoading();
                        //});
                    };


                    //                    btnEscalate.Clicked += async (object sender, EventArgs e) =>
                    //                    {
                    //                        try
                    //                        {
                    //                            UserDialogs.Instance.ShowLoading();
                    //                            await Task.Delay(100);
                    //#if _ANDROID_
                    //var storagePermission = await CheckStoragePermisions();
                    //if (!storagePermission)
                    //return;
                    //#endif
                    //        var GpsImage = SendToActionPage.GpsImage;
                    //        var AlertImage = SendToActionPage.strAlertImage;
                    //        var GpsImagepath = await DependencyService.Get<IPicture>().DownloadImage(GpsImage);
                    //        var AlertImagepath = await DependencyService.Get<IPicture>().DownloadImage(AlertImage);
                    //        var listOfContent = new List<string>();

                    //        if (SendToActionPage.Gps_enable == true)
                    //        {
                    //            listOfContent.Add(GpsImagepath);
                    //        }
                    //        if (SendToActionPage.Camera_Enable == true)
                    //        {
                    //            listOfContent.Add(AlertImagepath);
                    //        }
                    //        else
                    //        {
                    //            listOfContent.Add("");
                    //        }
                    //        if (Navigation.NavigationStack.Count <= 3)
                    //        {
                    //            var mainPage = new MainPage();
                    //            var alertListViewModel = new AlertsListViewModel(Navigation, Settings.UserID);
                    //            if (Settings.RoleID == 3)
                    //            {
                    //                await alertListViewModel.LoadAlertListOfAgent().ConfigureAwait(false);
                    //            }
                    //            else
                    //            {
                    //                await alertListViewModel.LoadAlertList().ConfigureAwait(false);
                    //            }
                    //            mainPage.nav = new NavigationPage(new AlertListPage(0, alertListViewModel));
                    //            mainPage.Detail = mainPage.nav;
                    //            mainPage.IsPresented = false;


                    //        }
                    //        await PopupNavigation.Instance.PushAsync(new EscalatePopUp(listOfContent, SendToActionPage), true);
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        Debug.WriteLine(ex);
                    //    }
                    //    UserDialogs.Instance.HideLoading();

                    //};
                    btnAgent.Clicked += async (object sender, EventArgs e) =>
                    {


                        try
                        {
                            int res;
                            var networkConnection = DependencyService.Get<INetworkConnection>();
                            networkConnection.CheckNetworkConnection();
                            var networkStatus = networkConnection.IsConnected ? "Connected" : "Not Connected";
                            if (networkStatus != "Connected")
                            {
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


                            }
                            else
                            {

                            }

                        }
                        catch { }


                    };

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

                    btnAction.Clicked += async (sender, e) =>
                    {
                        var networkConnection = DependencyService.Get<INetworkConnection>();
                        networkConnection.CheckNetworkConnection();
                        var networkStatus = networkConnection.IsConnected ? "Connected" : "Not Connected";
                        if (networkStatus != "Connected")
                        {

                            UserDialogs.Instance.Alert("Please check your internet connection.", "Alert", "OK");
                            return;
                        }
                        UserDialogs.Instance.ShowLoading("Loading...");
                        await System.Threading.Tasks.Task.Delay(200);
                        await Navigation.PushAsync(new AlertDetailsAction(SendToActionPage));
                    };


                    btnArchive.Clicked += async (sender, e) =>
                    {

                        try
                        {
                            var networkConnection = DependencyService.Get<INetworkConnection>();
                            networkConnection.CheckNetworkConnection();
                            var networkStatus = networkConnection.IsConnected ? "Connected" : "Not Connected";
                            if (networkStatus != "Connected")
                            {

                                UserDialogs.Instance.Alert("Please check your internet connection.", "Alert", "OK");
                                return;
                            }
                            UserDialogs.Instance.ShowLoading("Loading...");
                            if (Navigation.NavigationStack.Count <= 1)
                            {
                                var mainPage = new MainPage();
                                var alertListViewModel = new AlertsListViewModel(Navigation, Settings.UserID);
                                await alertListViewModel.LoadAlertList().ConfigureAwait(true);
                                mainPage.nav = new NavigationPage(new AlertListPage(0, alertListViewModel));
                                mainPage.Detail = mainPage.nav;
                                mainPage.IsPresented = false;
                                Xamarin.Forms.Application.Current.MainPage = mainPage;
                            }
                            else
                            {
                                await Navigation.PopAsync();
                                System.GC.Collect();
                            }
                            await System.Threading.Tasks.Task.Delay(1000);
                            UserDialogs.Instance.HideLoading();
                        }
                        catch { UserDialogs.Instance.HideLoading(); }
                    };

                    mainstack.Children.Add(box);
                    mainstack.Children.Add(descdetail);
                    mainstack.Children.Add(img1);
                    mainstack.Children.Add(TSBlayout);
                    mainstack.Children.Add(mapandimagestack);
                    mainstack.Children.Add(img2);
                    mainstack.Children.Add(timestack);
                    mainstack.Children.Add(img3);

                    var scrl = new Xamarin.Forms.ScrollView { };
                    scrl.Content = mainstack;

                    var Toplayout = new StackLayout { VerticalOptions = LayoutOptions.StartAndExpand };
                    Toplayout.Children.Add(headstack);

                    var Top2layout = new StackLayout { VerticalOptions = LayoutOptions.StartAndExpand };
                    Toplayout.Children.Add(scrl);

                    layout.Children.Add(Toplayout);
                    if (Settings.RoleID == 3)
                    {
                        btnAgent.Text = "Notify Area Manager";
                    }
                    else
                    {
                        btnAgent.Text = "Notify Agent";
                    }
                    layout.Children.Add(bottomStack);
                    var custom = new CustomPopup();
                    Content = custom;
                    custom.Content = layout;
                }
            }
            catch (System.Exception ex)
            {
                UserDialogs.Instance.HideLoading();

            }
        }
        async void Timer()
        {
            try
            {

                DateTime AlertDate = Convert.ToDateTime(ViewModel.device.DeviceDate);
                ViewModel.device.CountDown = ViewModel.device.WakeTime * 60;

                var renainigRtIME = DateTime.Now - AlertDate;

                var CalculatedCountDown = Convert.ToInt32(ViewModel.device.CountDown) - Convert.ToInt32(renainigRtIME.Minutes * 60);
                if (CalculatedCountDown >= 0 && Convert.ToInt32(ViewModel.device.CountDown) >= CalculatedCountDown)
                {
                    ViewModel.device.CountDown = CalculatedCountDown;
                }
                else
                {
                    ViewModel.device.CountDown = 0;
                }


                Xamarin.Forms.Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                {
                    if (ViewModel != null)
                    {
                        if (ViewModel.device != null)
                        {
                            if (ViewModel.device.CountDown == null)
                            {
                                return false;
                            }

                            if (ViewModel.device.CountDown < 0)
                            {
                                if (lblCountdownValue != null)
                                {
                                    lblCountdownValue.Text = "0 Secs";
                                }
                                return false;
                            }
                            else
                            {
                                if (ViewModel.device.CountDown > 0)
                                    ViewModel.device.CountDown -= 1;
                                if (lblCountdownValue != null)
                                {
                                    lblCountdownValue.Text = ViewModel.device.CountDown.ToString() + " Secs";
                                }
                            }
                        }
                        else
                        {
                            return false;
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
                img.Source = ImageSource.FromStream(() => new MemoryStream(stream));
            }
            catch { }
            return img;
        }
    }
}

