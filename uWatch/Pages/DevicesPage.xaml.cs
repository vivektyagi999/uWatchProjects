using System;
using System.Linq;
using uWatch.ViewModels;
using UwatchPCL;
using Xamarin.Forms;
using System.IO;
using Acr.UserDialogs;
using System.Threading.Tasks;
using UwatchPCL.Model;
using FFImageLoading.Forms;
using UwatchPCL.Helpers;
using Plugin.BLE;
using Rg.Plugins.Popup.Services;
using System.Collections.ObjectModel;
using UwatchPCL.WebServices;


#if __ANDROID__
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Android.App;
using Android.Widget;
using uWatch.Droid;
#endif

#if __IOS__
using Xamarin.Media;
#endif
#if __ANDROID__
using Xamarin.Media;
#endif

namespace uWatch
{
    public partial class DevicesPage : ContentPage
    {
        WebServiceManager webServiceManager;
        public static bool IsList;
        public INavigation _navigation { get; set; }
        double pddingH, paddingW;
        private ImageSource picture;
        Label lblHelp;
        int PaddingMain;
        StackLayout mainlayoutGallery;
        Xamarin.Forms.Button btnAdd;
        ToolbarItem _bluetooth, bluetoothText;
        bool iconadded = false;
        public ImageSource Picture
        {
            get
            {
                return this.picture;
            }
            set
            {
                if (Equals(value, this.picture))
                {
                    return;
                }
                this.picture = value;
                OnPropertyChanged();
            }
        }

        public int countAssets = 0;
        bool backpressed = false;
        Xamarin.Forms.RelativeLayout relativeLayout;
        Xamarin.Forms.ScrollView scrollview;
        int currentimg = 0;
        Image imgAsset;
        ObservableCollection<DeviceConfig> profiledetailslist;
        double w = MyController.VirtualWidth;
        double h = MyController.VirtualHeight;
        public InfiniteListView listView;

        public bool MoreThanOneDevices { get; set; }

        public DeviceStatic device { get; set; }
        public DataTemplate Cell { get; private set; }

        public DashboardViewModel ViewModelDevices { get; set; }

        public AlertsListViewModel ViewModelTestAlertList { get; set; }

        public TakePictureViewModel AssetViewModel { get; set; }
        public DeviceDetailsViewModel ViewModel { get; set; }

        public DevicesPage(DashboardViewModel assetsViewModel = null)
        {
            try
            {
                webServiceManager = new WebServiceManager();
                MyController.fromAssetsToGallery = false;
                this.ViewModelDevices = assetsViewModel;
                InitializeComponent();
                AssetViewModel = new TakePictureViewModel();

                if (ViewModelDevices.DeviceList.Count > 1)
                {
                    IsList = true;
                    SetLayout(true);
                }
                else if (ViewModelDevices.DeviceList.Count == 1)
                {

                    IsList = true;
                    this.device = ViewModelDevices.DeviceList[0];

                    SetLayout(true);
                }
                else
                {
                    relativeLayout = new Xamarin.Forms.RelativeLayout();
                    var lblError = MyUILibrary.AddLabel(relativeLayout, "No Assets to Display", 20, 150, w - 40, 55, Color.Blue, 20);
                    lblError.HorizontalOptions = LayoutOptions.CenterAndExpand;
                    scrollview = new Xamarin.Forms.ScrollView();
                    scrollview.Content = relativeLayout;


                    var custom = new CustomPopup();
                    var popupLayouts = this.Content as CustomPopup;
                    Content = custom;
                    custom.Content = scrollview;

                }
            }
            catch (Exception ex)
            {
                MyController.ErrorManagement(ex.Message);
            }
        }

        public DevicesPage(DeviceDetailsViewModel assetViewModel)
        {
            try
            {
                this.ViewModel = assetViewModel;
                if (ViewModel.device != null)
                    this.device = ViewModel.device;
                AssetViewModel = new TakePictureViewModel();
                // AlertList = new AlertsListViewModel();
                InitializeComponent();
                IsList = false;
                SetLayout();
            }
            catch (Exception ex)
            {
                MyController.ErrorManagement(ex.Message);
            }
        }

#if __ANDROID__
        async Task<bool> CheckCameraPermisions()
        {
            var results = false;
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Plugin.Permissions.Abstractions.Permission.Storage);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Plugin.Permissions.Abstractions.Permission.Storage))
                    {
                        await DisplayAlert("Need Storage", "uWatch need to access your device storage", "OK");
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
                    await DisplayAlert("Storage access Denied", "Can not continue, try again.", "OK");
                }
            }
            catch (Exception ex)
            {
                results = false;
                MyController.ErrorManagement(ex.Message);
            }
            return results;
        }
#endif
        protected async override void OnAppearing()
        {
#if __ANDROID__
            var s = await CheckCameraPermisions();
            if (s)
            {
                base.OnAppearing();
            }
            else
            {
                await Navigation.PopAsync(true);
            }
#endif
#if __IOS__
            base.OnAppearing();
#endif

            try
            {
                if (MyController.fromAssetsToGallery == true)
                {
                    var networkConnection = DependencyService.Get<INetworkConnection>();
                    networkConnection.CheckNetworkConnection();
                    var networkStatus = networkConnection.IsConnected ? "Connected" : "Not Connected";
                    if (networkStatus != "Connected")
                    {
                        MyController.fromAssetsToGallery = false;
                        UserDialogs.Instance.Alert("Please check your internet connection.", "Alert", "OK");
                        return;
                    }
                    using (UserDialogs.Instance.Loading("Loading..."))
                    {

                        ViewModel = new DeviceDetailsViewModel(this.device.device_idx);
                        var Assets = await ViewModel.GetDeviceAssets(device.device_idx);
                        if (this.device != null)

                            if (!IsList)
                            {
                                SetLayout();
                            }
                            else
                            {
                                //GetPopupofAssetsImage(true);
                            }

                    }
                    MyController.fromAssetsToGallery = false;
                }
            }
            catch (Exception ex)
            {
                MyController.ErrorManagement(ex.Message);
            }

        }

        private async Task AddLayoutForDevices()
        {
            try
            {
                profiledetailslist = new ObservableCollection<DeviceConfig>();
                profiledetailslist = await AssetViewModel.FetchConfigurationProfileDetails().ConfigureAwait(true);


                listView = new InfiniteListView();
                listView.BackgroundColor = Color.FromRgb(247, 247, 247);
                listView.BindingContext = ViewModelDevices;
                listView.SetBinding(Xamarin.Forms.ListView.ItemsSourceProperty, new Binding("DeviceList", BindingMode.TwoWay));
                listView.LoadMoreCommand = ViewModelDevices.LoadCharactersCommand;
                listView.HasUnevenRows = true;

                listView.ItemTemplate = new DataTemplate(() =>
                {
                    var cell = new ViewCell();
                    double font;
                    if (Xamarin.Forms.Device.Idiom == TargetIdiom.Tablet)
                    {
                        font = 16;
                    }
                    else
                    {
                        font = 14;
                    }
                    Label nameLabel = new Label()
                    {
                        FontSize = font + 2,
                        FontAttributes = FontAttributes.Bold,
                        TextColor = Color.Black,
                        VerticalOptions = LayoutOptions.StartAndExpand,
                        HorizontalOptions = LayoutOptions.Start,
                    };
                    nameLabel.SetBinding(Label.TextProperty, new Binding("FriendlyName", BindingMode.Default, stringFormat: "#{0}"));
                    var namelayout = new StackLayout { VerticalOptions = LayoutOptions.CenterAndExpand };
                    namelayout.Children.Add(nameLabel);
                    Label deviceid = new Label()
                    {
                        FontSize = font + 2,
                        FontAttributes = FontAttributes.Bold,
                        TextColor = Color.Black,
                        VerticalOptions = LayoutOptions.StartAndExpand,
                        HorizontalOptions = LayoutOptions.Start,
                    };

                    var configrationimage = new Image
                    {
                        Source = "configration.png",
                        HeightRequest = 40,
                        WidthRequest = 40

                    };
                    var configrationimagelayout = new StackLayout { HorizontalOptions = LayoutOptions.End };
                    // configrationimagelayout.Children.Add(configrationimage);
                    //TapGestureRecognizer _configrationgesture = new TapGestureRecognizer();
                    //configrationimage.GestureRecognizers.Add(_configrationgesture);
                    //_configrationgesture.Tapped += (s, ex) =>
                    //{
                    //    DeviceConfigration(deviceid.Text);
                    //};
                    var configrationlayout = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start };
                    configrationlayout.Children.Add(namelayout);
                    Label friendlyNameLabel = new Label()
                    {

                        VerticalOptions = LayoutOptions.End,
                        HorizontalOptions = LayoutOptions.StartAndExpand,
                        HorizontalTextAlignment = TextAlignment.Start,
                        WidthRequest = 200,
                    };
                    var formattedImeiString = new FormattedString();
                    formattedImeiString.Spans.Add(new Span
                    {
                        Text = "IMEI: ",
                        FontSize = font - 2,
                        ForegroundColor = Color.Black,
                    });
                    var Imeispan = new Span()
                    {
                        FontAttributes = FontAttributes.Bold,
                        FontSize = font - 1,
                        ForegroundColor = Color.Black,
                    };
                    formattedImeiString.Spans.Add(Imeispan);
                    Imeispan.SetBinding(Span.TextProperty, new Binding("imei", BindingMode.TwoWay));
                    friendlyNameLabel.FormattedText = formattedImeiString;


                    Label modelLabel = new Label()
                    {

                        HorizontalTextAlignment = TextAlignment.Start,
                        HorizontalOptions = LayoutOptions.StartAndExpand,
                        VerticalOptions = LayoutOptions.Center
                    };
                    var formattedModelString = new FormattedString();
                    formattedModelString.Spans.Add(new Span
                    {
                        Text = "Model: ",
                        FontSize = font - 1,
                        ForegroundColor = Color.Black,
                    });
                    var span = new Span()
                    {
                        FontAttributes = FontAttributes.Bold,
                        FontSize = font - 1,
                        ForegroundColor = Color.Black,
                    };
                    formattedModelString.Spans.Add(span);
                    span.SetBinding(Span.TextProperty, new Binding("model_no", BindingMode.TwoWay));
                    modelLabel.FormattedText = formattedModelString;


                    Label serialLabel = new Label()
                    {

                        HorizontalTextAlignment = TextAlignment.Start,
                        HorizontalOptions = LayoutOptions.StartAndExpand,
                        VerticalOptions = LayoutOptions.End
                    };

                    var formattedSerialString = new FormattedString();
                    formattedSerialString.Spans.Add(new Span
                    {
                        Text = "S/N: ",
                        FontSize = font - 1,
                        ForegroundColor = Color.Black,
                    });
                    var spanSerial = new Span()
                    {
                        FontAttributes = FontAttributes.Bold,
                        FontSize = font - 1,
                        ForegroundColor = Color.Black,
                    };
                    formattedSerialString.Spans.Add(spanSerial);
                    spanSerial.SetBinding(Span.TextProperty, new Binding("serial_no", BindingMode.TwoWay));
                    serialLabel.FormattedText = formattedSerialString;

                    var layoutSerialAndModel = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.StartAndExpand };
                    layoutSerialAndModel.Children.Add(modelLabel);
                    layoutSerialAndModel.Children.Add(serialLabel);

                    Label lblDeviceSwitchStatus = new Label()
                    {
                        FontAttributes = FontAttributes.Bold,
                        FontSize = font - 1,
                        TextColor = Color.Black,
                        HorizontalTextAlignment = TextAlignment.Start,
                        HorizontalOptions = LayoutOptions.StartAndExpand,
                        VerticalOptions = LayoutOptions.End
                    };


                    Label simExpiryDate = new Label()
                    {
                        FontAttributes = FontAttributes.Bold,
                        FontSize = font - 1,
                        TextColor = Color.Black,
                        HorizontalTextAlignment = TextAlignment.Start,
                        HorizontalOptions = LayoutOptions.StartAndExpand,
                        VerticalOptions = LayoutOptions.End
                    };
                    var formattedSimexpString = new FormattedString();
                    formattedSimexpString.Spans.Add(new Span
                    {
                        Text = "Sim Expiry Date: ",
                        FontSize = font - 1,
                        ForegroundColor = Color.Black,
                    });
                    var Simexpspan = new Span()
                    {
                        FontAttributes = FontAttributes.Bold,
                        FontSize = font - 1,
                        ForegroundColor = Color.Black,
                    };
                    formattedSimexpString.Spans.Add(Simexpspan);
                    simExpiryDate.FormattedText = formattedSimexpString;


                    Label firmware_version = new Label()
                    {
                        FontAttributes = FontAttributes.Bold,
                        FontSize = font - 1,
                        TextColor = Color.Black,
                        HorizontalTextAlignment = TextAlignment.Start,
                        HorizontalOptions = LayoutOptions.StartAndExpand,
                        VerticalOptions = LayoutOptions.End
                    };
                    var formattedFirmString = new FormattedString();
                    formattedFirmString.Spans.Add(new Span
                    {
                        Text = "FW: ",
                        FontSize = font - 1,
                        ForegroundColor = Color.Black,
                    });
                    var Firmspan = new Span()
                    {
                        FontAttributes = FontAttributes.Bold,
                        FontSize = font - 1,
                        ForegroundColor = Color.Black,
                    };
                    formattedFirmString.Spans.Add(Firmspan);
                    Firmspan.SetBinding(Span.TextProperty, new Binding("fw_version", BindingMode.TwoWay));
                    firmware_version.FormattedText = formattedFirmString;
                    //  firmware_version.SetBinding(Label.TextProperty, new Binding("fw_version", BindingMode.Default, stringFormat: "FW: {0}"));

                    Label LastUpdatedfirmware_version = new Label()
                    {
                        FontAttributes = FontAttributes.Bold,
                        FontSize = font - 1,
                        TextColor = Color.Black,
                        HorizontalTextAlignment = TextAlignment.Start,
                        HorizontalOptions = LayoutOptions.StartAndExpand,
                        VerticalOptions = LayoutOptions.End
                    };
                    var formattedUpdatedString = new FormattedString();
                    formattedUpdatedString.Spans.Add(new Span
                    {
                        Text = "Updated: ",
                        FontSize = font - 1,
                        ForegroundColor = Color.Black,
                    });
                    var Updatedspan = new Span()
                    {
                        FontAttributes = FontAttributes.Bold,
                        FontSize = font - 1,
                        ForegroundColor = Color.Black,
                    };
                    formattedUpdatedString.Spans.Add(Updatedspan);
                    LastUpdatedfirmware_version.FormattedText = formattedUpdatedString;


                    var layoutFirmWare = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.StartAndExpand };
                    layoutFirmWare.Children.Add(firmware_version);
                    layoutFirmWare.Children.Add(LastUpdatedfirmware_version);

                    Label starLabel = new Label()
                    {
                        FontSize = font - 1,
                        TextColor = Color.Gray
                    };
                    CachedImage starImage = new CachedImage()
                    {
                        Source = "right_arrow.png",
                        HeightRequest = 50,
                        WidthRequest = 50,
                        LoadingPlaceholder = ImageSource.FromFile("placeholder.png"),
                    };
                    Label BatteryCondition = new Label()
                    {
                        FontAttributes = FontAttributes.Bold,
                        FontSize = font - 1,
                        TextColor = Color.FromHex("#666"),
                        HorizontalTextAlignment = TextAlignment.Start,
                        HorizontalOptions = LayoutOptions.StartAndExpand,
                        VerticalOptions = LayoutOptions.End
                    };

                    var moredetail = new Label { Text = "Current configuration", TextColor = Color.Blue, FontSize = 14, VerticalOptions = LayoutOptions.CenterAndExpand };
                    var _viewmore = new StackLayout();
                    _viewmore.HorizontalOptions = LayoutOptions.FillAndExpand;
                    _viewmore.VerticalOptions = LayoutOptions.CenterAndExpand;
                    // _viewmore.BackgroundColor = Color.Red;
                    _viewmore.Padding = new Thickness(10, 0, 0, 0);
                    _viewmore.Children.Add(moredetail);
                    var viewmoreframe = new Frame();
                    viewmoreframe.HasShadow = false;
                    viewmoreframe.OutlineColor = Color.Transparent;
                    viewmoreframe.Padding = new Thickness(0);
                    viewmoreframe.HeightRequest = 30;
                    viewmoreframe.BackgroundColor = Color.Transparent;
                    viewmoreframe.Content = _viewmore;
                    var _tapRecogniser = new TapGestureRecognizer();
                    viewmoreframe.GestureRecognizers.Add(_tapRecogniser);

                    var Stack1 = new StackLayout()
                    {
                        HorizontalOptions = LayoutOptions.StartAndExpand,
                        VerticalOptions = LayoutOptions.Start,
                        Padding = new Thickness(10, 10, 0, 0),

                        Children =
                                    {
                                        configrationlayout,
                                        layoutSerialAndModel,
                                        friendlyNameLabel,
                                       // lblDeviceSwitchStatus,
                                        layoutFirmWare,
                                        simExpiryDate,
                                       // BatteryCondition,
                                    }
                    };
                    var Stack2 = new StackLayout()
                    {
                        HorizontalOptions = LayoutOptions.StartAndExpand,
                        VerticalOptions = LayoutOptions.Start,
                        Padding = new Thickness(0, 10, 0, 10),
                        MinimumWidthRequest = 100,
                        Children =
                                        {


                                        }
                    };
                    var boxview = new BoxView { HeightRequest = 20 };
                    var Stack3 = new StackLayout()
                    {
                        HorizontalOptions = LayoutOptions.EndAndExpand,
                        VerticalOptions = LayoutOptions.Start,
                        Padding = new Thickness(0, 0, 8, 0),
                        Children =
                                    {
                            configrationimagelayout,
                            boxview
                                        //starImage
                                    }
                    };


                    var MainStackLayout = new StackLayout()
                    {
                        Orientation = StackOrientation.Horizontal,
                        HorizontalOptions = LayoutOptions.Fill,
                        VerticalOptions = LayoutOptions.Fill,
                        Children =
                                    {
                                        Stack1,
                                        Stack3

                                    }
                    };
                    var btnConfigurationChange = new Xamarin.Forms.Button()
                    {
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Text = "Configuration",
                        BackgroundColor = Color.Red,
                        TextColor = Color.White
                    };

                    btnConfigurationChange.Clicked += async (s, e) =>
                    {

                        var data = btnConfigurationChange.BindingContext as DeviceStatic;
                        GetPopupOfChangeCubeConfigAsync(data.device_idx);
                    };

                    var btnLinkTags = new Xamarin.Forms.Button()
                    {
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Text = "Link Tags",
                        BackgroundColor = Color.Red,
                        TextColor = Color.White
                    };

                    btnLinkTags.Clicked += async (s, e) =>
                    {
                        try
                        {
                            Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
                            {
                                UserDialogs.Instance.ShowLoading("Loading...");
                                var result = await webServiceManager.GetDeviceConfigurationLinkAsync(Convert.ToInt32(deviceid.Text));
                                if (result.ErrorCode == 0)
                                {
                                    if (result.IsSuccess)
                                    {
                                        if (!result.Data.CurrentSetting.ChkBluetoothLowEnergy)
                                        {
                                            UserDialogs.Instance.HideLoading();
                                            var selectedResult = await UserDialogs.Instance.ConfirmAsync("BLE is not activated for this Cube", "BLE Tag Link", "Continue", "Exit");
                                            if (!selectedResult)
                                            {
                                                UserDialogs.Instance.HideLoading();
                                                return;
                                            }
                                        }

                                        await PopupNavigation.Instance.PushAsync(new LinkTagsPopUp(result.Data, Convert.ToInt32(deviceid.Text)), true);
                                    }
                                    UserDialogs.Instance.HideLoading();
                                }
                                else if (result.ErrorCode > 0)
                                {
                                    UserDialogs.Instance.Alert(result.ErrorMessage, "Alert", "Ok");
                                    UserDialogs.Instance.HideLoading();
                                }
                                else if (result.ErrorCode < 0)
                                {
                                    UserDialogs.Instance.Alert(result.ErrorMessage, "Alert", "Ok");
                                    UserDialogs.Instance.HideLoading();
                                }
                                else
                                {
                                    UserDialogs.Instance.Alert(result.ErrorMessage, "Alert", "Ok");
                                    UserDialogs.Instance.HideLoading();
                                }
                            });
                        }
                        catch (Exception ex)
                        {
                            UserDialogs.Instance.ShowLoading("Loading...");

                        }

                    };
                    var btnSwitchOff = new Xamarin.Forms.Button()
                    {
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Text = "Switch off",
                        BackgroundColor = Color.Red,
                        TextColor = Color.White
                    };

                    btnSwitchOff.Clicked += (s, e) =>
                    {
                        DeviceConfigration(deviceid.Text);
                    };

                    var configurationBoxStack = new StackLayout()
                    {
                        Orientation = StackOrientation.Horizontal,
                        VerticalOptions = LayoutOptions.StartAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Spacing = 5,
                        Padding = new Thickness(10, 7, 10, 0),
                        Children ={
                            btnConfigurationChange,btnLinkTags,btnSwitchOff
                        }
                    };

                    var boxView = new BoxView() { BackgroundColor = Color.Gray, VerticalOptions = LayoutOptions.EndAndExpand, Margin = new Thickness(0, 5, 0, 0), HorizontalOptions = LayoutOptions.FillAndExpand, HeightRequest = 1.5f };

                    var Mainviewcellstack = new StackLayout()
                    {
                        Orientation = StackOrientation.Vertical,
                        HorizontalOptions = LayoutOptions.Fill,
                        VerticalOptions = LayoutOptions.Fill,
                        Spacing = 2,
                        Children =
                                    {
                                        MainStackLayout,
                                        viewmoreframe,
                            configurationBoxStack,boxView
                                    }
                    };

                    _tapRecogniser.Tapped += async (sender, e) =>
                    {
                        try
                        {
                            UserDialogs.Instance.ShowLoading("Loading...");
                            await Task.Delay(1000);
                            var DeviceConfigrationDetail = await ApiService.Instance.FetchDeviceConfigurationDetails(deviceid.Text.ToString());
                            if (DeviceConfigrationDetail != null)
                            {
                                UserDialogs.Instance.HideLoading();
                                await PopupNavigation.PushAsync(new DeviceDetailPage(nameLabel.Text.ToString(), DeviceConfigrationDetail), true);

                            }
                            else
                            {
                                UserDialogs.Instance.HideLoading();
                                UserDialogs.Instance.Alert("There was an error communicating with the server. Please try again!", "Server Error", "Ok");
                            }
                        }
                        catch (Exception ex)
                        {

                        }

                    };

                    cell.BindingContextChanged += (sender, e) =>
                    {
                        base.OnBindingContextChanged();
                        ViewCell theViewCell = ((ViewCell)sender);

                        var account = (DeviceStatic)theViewCell.BindingContext;

                        deviceid.Text = account.device_idx.ToString();

                        var devicestatus = account.DeviceSwitchStatus == null ? " " : account.DeviceSwitchStatus;
                        lblDeviceSwitchStatus.SetBinding(Label.TextProperty, new Binding("DeviceSwitchStatus", BindingMode.Default, stringFormat: "Cube Switch Status: " + devicestatus));

                        var exdate = account.SimExpireDate == null ? "No Date Available" : DateFormat.GetDateTime(account.SimExpireDate.Value.Date, TimeType.OnlyDate);
                        // simExpiryDate.SetBinding(Label.TextProperty, new Binding("SimExpireDate", BindingMode.Default, stringFormat: "Sim Expiry Date: " + exdate));
                        Simexpspan.SetBinding(Span.TextProperty, new Binding("SimExpireDate", BindingMode.TwoWay, stringFormat: exdate));


                        var lastUpdated = account.LastUpdated == null ? "No Date Available" : DateFormat.GetDateTime(account.LastAlert, TimeType.DateAndTime);
                        Updatedspan.SetBinding(Span.TextProperty, new Binding("LastUpdated", BindingMode.TwoWay, stringFormat: lastUpdated));

                        var condition = account.Battery <= 0 ? 0 : account.Battery;
                        BatteryCondition.SetBinding(Label.TextProperty, new Binding("Battery", BindingMode.Default, stringFormat: "Battery: " + condition + "%"));
                    };

                    cell.View = Mainviewcellstack;

                    return cell;
                });


                listView.ItemTapped += async (sender, e) =>
                {
                    listView.SelectedItem = null;

                    try
                    {

                        //var networkConnection = DependencyService.Get<INetworkConnection>();
                        //networkConnection.CheckNetworkConnection();
                        //var networkStatus = networkConnection.IsConnected ? "Connected" : "Not Connected";
                        //if (networkStatus != "Connected")
                        //{
                        // UserDialogs.Instance.Alert("Please check your internet connection.", "Alert", "OK");
                        // return;
                        //}
                        //UserDialogs.Instance.ShowLoading("Loading...");
                        //await Task.Delay(5);

                        //if (e.Item == null)
                        // return;

                        //this.device = e.Item as DeviceStatic;
                        //this.device.OwnerUserID = Settings.UserID;

                        //ViewModel = new DeviceDetailsViewModel(this.device.device_idx);
                        //if (ViewModel.AssetsList == null)
                        // ViewModel.AssetsList = new System.Collections.ObjectModel.ObservableCollection<DeviceAssetsModel>();
                        //ViewModel.AssetsList = await ViewModel.GetDeviceAssets(device.device_idx);
                        //ViewModel.device = this.device;

                        //System.GC.Collect();

                        // GetPopupofAssetsImage(false);
                        // UserDialogs.Instance.HideLoading();


                    }
                    catch (Exception ex)
                    {
                    }
                };
                double newx80 = MyUiUtils.getPercentual(w, 80);
                double newy30 = MyUiUtils.getPercentual(h, 30);

                if (ViewModelDevices.DeviceList.Count > 0)
                {
                    MyUILibrary.AddListView(relativeLayout, listView, 0, 0, w, h - 80, listView.SeparatorColor, 220);
                }
                else
                {
                    var lblerror = MyUILibrary.AddLabel(relativeLayout, "No devices to display, please register your device", (w - newx80) / 2, newy30, w - 30, 500, listView.SeparatorColor, 18);
                }

            }
            catch (System.Exception ex)
            {
                UserDialogs.Instance.HideLoading();
            }
        }
        private async void SetLayout(bool moredevices = false)
        {
            try
            {

                Title = "Cubes";
                NavigationPage.SetHasNavigationBar(this, true);
                //NavigationPage.SetBackButtonTitle(this, "");
                NavigationPage.SetHasBackButton(this, true);
                if (!iconadded)
                {
                    _bluetooth = new ToolbarItem
                    {
                        Icon = "bluetoothicon.png"
                    };

                    bluetoothText = new ToolbarItem
                    {
                        //Icon = "bluetoothtext.png"
                        Text = "Bluetooth Search"
                    };

                    this.ToolbarItems.Add(bluetoothText);
                    this.ToolbarItems.Add(_bluetooth);

                    _bluetooth.Clicked += OnBluetoothClickAsync;
                    bluetoothText.Clicked += OnBluetoothClickAsync;
                    iconadded = true;
                }

                relativeLayout = new Xamarin.Forms.RelativeLayout();

                if (!moredevices)
                {
                    AddLayout();
                }
                else
                {
                    await AddLayoutForDevices();
                }
                scrollview = new Xamarin.Forms.ScrollView();
                scrollview.Content = relativeLayout;
                var custom = new CustomPopup();
                var popupLayouts = this.Content as CustomPopup;
                Content = custom;
                custom.Content = relativeLayout;
            }
            catch { }
        }

        private async void OnBluetoothClickAsync(object sender, EventArgs e)
        {
            var _bluetoothAdapter = CrossBluetoothLE.Current;
            if (!_bluetoothAdapter.IsOn)
            {
                UserDialogs.Instance.Alert("This app needs access to bluetooth Please Turn on!", "Info", "Ok");
                return;

            }
            else
            {

                UserDialogs.Instance.ShowLoading("Searching for uWatch Bluetooth Devices!");
                await Task.Delay(1000);
                await Navigation.PushAsync(new BluetoothDeviceListPage());

            }
        }

        private async Task AddLayout()
        {
            try
            {
                Xamarin.Forms.StackLayout relativeLayouts = new Xamarin.Forms.StackLayout();
                relativeLayouts.VerticalOptions = LayoutOptions.StartAndExpand;
                double position = 0;
                double newx20 = MyUiUtils.getPercentual(w, 20);
                double newx40 = MyUiUtils.getPercentual(w, 40);
                double newx60 = MyUiUtils.getPercentual(w, 60);
                double newx80 = MyUiUtils.getPercentual(w, 80);

                var x = 10;
                var z = 10;
                var i = 0;
                var lblname = new Label { Text = "Cube:", TextColor = Color.Black, FontSize = 16, WidthRequest = 75, };
                var LblDvice = new Label { TextColor = Color.Black, FontSize = 15, VerticalOptions = LayoutOptions.CenterAndExpand };
                LblDvice.LineBreakMode = LineBreakMode.WordWrap;
                if (MyController.Roll_id != 8)
                {
                    if (device != null && (!string.IsNullOrEmpty(device.FriendlyName)))
                        LblDvice.Text = "Cube: " + device.FriendlyName.ToString();
                    else
                        LblDvice.Text = "Cube: " + "Asset Images";
                }
                else
                {
                    LblDvice.Text = "Cube: " + "Asset Images";
                }

                Image ConfigrationIcon = new Image { Source = "configration.png", HeightRequest = 50, WidthRequest = 50, VerticalOptions = LayoutOptions.CenterAndExpand };

                StackLayout layoutConfigration = new StackLayout { };
                layoutConfigration.Children.Add(ConfigrationIcon);

                var devicelayout = new StackLayout { Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand };
                devicelayout.Children.Add(LblDvice);


                var layoutGllry1 = new StackLayout { Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.StartAndExpand };
                var layoutGllry2 = new StackLayout { Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.StartAndExpand };

                StackLayout st = new StackLayout() { BackgroundColor = Color.Red, Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };
                //st.Children.Add(lblname);
                st.Children.Add(devicelayout);

                TapGestureRecognizer layoutConfigrationpopup = new TapGestureRecognizer();
                layoutConfigration.GestureRecognizers.Add(layoutConfigrationpopup);
                layoutConfigrationpopup.Tapped += (object sender, EventArgs e) =>
                {
                    DeviceConfigration();
                };

                MyUILibrary.AddLayout(relativeLayout, layoutConfigration, w - 50, position + 5, 50, 50);

                MyUILibrary.AddLayout(relativeLayout, devicelayout, x, position, App.ScreenWidth, 50);
                if (ViewModel != null && ViewModel.AssetsList != null)
                {
                    countAssets = ViewModel.AssetsList.Count;
                    if (ViewModel.ImageList != null)
                    {
                        if (ViewModel.ImageList.Count() > 0)
                        {
                            int CountOfImage = 1;
                            foreach (var item in ViewModel.AssetsList)
                            {
                                var it = new CachedImage()
                                {
                                    WidthRequest = MyController.ScreenWidth / 4 - 10,
                                    HeightRequest = MyController.ScreenWidth / 4 - 10,
                                    CacheDuration = TimeSpan.FromDays(30),
                                    DownsampleToViewSize = true,
                                    RetryCount = 0,
                                    RetryDelay = 250,
                                    TransparencyEnabled = false,
                                    Aspect = Aspect.AspectFit,
                                    Source = ImageSource.FromFile("noimage.gif"),
                                };
                                var layout = new Xamarin.Forms.RelativeLayout();
                                layout.Padding = 2;
                                layout.Children.Add(it, Constraint.Constant(0), Constraint.Constant(0));
                                layout.BackgroundColor = Color.White;

                                var sou = await ModeltoImage(item);
                                it.Source = sou.Source as ImageSource;
                                var frm = new Frame { Padding = 1, HasShadow = false, BackgroundColor = Color.Silver };
                                frm.Content = it;
                                if (CountOfImage < 5)
                                {
                                    layoutGllry1.Children.Add(frm);
                                }
                                else
                                {
                                    layoutGllry2.Children.Add(frm);
                                    z += 80;
                                }
                                relativeLayouts.Children.Add(layoutGllry1);
                                relativeLayouts.Children.Add(layoutGllry2);

                                TapGestureRecognizer bigt = new TapGestureRecognizer();
                                bigt.Tapped += async (object sender, EventArgs e) =>
                                {
                                    //popupLayouts.DismissPopup();
                                    var networkConnection = DependencyService.Get<INetworkConnection>();
                                    networkConnection.CheckNetworkConnection();
                                    var networkStatus = networkConnection.IsConnected ? "Connected" : "Not Connected";
                                    if (networkStatus != "Connected")
                                    {
                                        UserDialogs.Instance.Alert("Please check your internet connection.", "Alert", "OK");
                                        return;
                                    }
                                    UserDialogs.Instance.ShowLoading("Loading...");
                                    await Task.Delay(100);
                                    var AssetDetailsViewModel = new AssetDetailsViewModel();
                                    await AssetDetailsViewModel.GetActualAssetImage(item);

                                    await Navigation.PushAsync(new NewAssetsDetailPage(item.Position_no, item, AssetDetailsViewModel));
                                    UserDialogs.Instance.HideLoading();
                                };
                                it.GestureRecognizers.Add(bigt);
                                x += 80;
                                i++;
                                CountOfImage++;
                            }
                            position += 20 + 50;
                        }
                        else
                        {
                            var it = new CachedImage()
                            {
                                WidthRequest = 80,
                                HeightRequest = 80,
                                CacheDuration = TimeSpan.FromDays(30),
                                DownsampleToViewSize = true,
                                RetryCount = 0,
                                RetryDelay = 250,
                                TransparencyEnabled = false,
                                Aspect = Aspect.AspectFit,
                                Source = ImageSource.FromFile("noimage.gif"),
                            };
                            relativeLayouts.Children.Add(it);

                            position += 100 + 20;
                        }
                    }
                    else
                    {
                        var it = new CachedImage()
                        {
                            WidthRequest = 80,
                            HeightRequest = 80,
                            CacheDuration = TimeSpan.FromDays(30),
                            DownsampleToViewSize = true,
                            RetryCount = 0,
                            RetryDelay = 250,
                            TransparencyEnabled = false,
                            Aspect = Aspect.AspectFit,
                            Source = ImageSource.FromFile("noimage.gif"),
                        };
                        relativeLayouts.Children.Add(it);
                        position += 100 + 20;
                    }
                }

                else
                {
                    var it = new CachedImage()
                    {
                        WidthRequest = 80,
                        HeightRequest = 80,
                        CacheDuration = TimeSpan.FromDays(30),
                        DownsampleToViewSize = true,
                        RetryCount = 0,
                        RetryDelay = 250,
                        TransparencyEnabled = false,
                        Aspect = Aspect.AspectFit,
                        Source = ImageSource.FromFile("noimage.gif"),
                    };
                    it = MyUILibrary.AddCachedImage(relativeLayout, it, x, position + 40, 80, 80, Aspect.Fill);
                    position += 100 + 20;
                }
                var lblHelp = new Label { Text = "Tap on image to Edit & delete Asset", HorizontalOptions = LayoutOptions.CenterAndExpand };
                var btnAdd = new Xamarin.Forms.Button
                {
                    Text = "   Add New Image   ",
                    BackgroundColor = Color.Red,
                    TextColor = Color.White,
                    FontSize = 15,
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };
                var btnimglocation = new Xamarin.Forms.Button
                {
                    Text = "   Add Image With Location  ",
                    BackgroundColor = Color.Red,
                    TextColor = Color.White,
                    FontSize = 15,
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };
                var btnlocation = new Xamarin.Forms.Button
                {
                    Text = "   Show Location  ",
                    BackgroundColor = Color.Red,
                    TextColor = Color.White,
                    FontSize = 15,
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };
                btnimglocation.Clicked += async (sender, e) =>
                {
                    var currentlocation = DependencyService.Get<ICurrentLocation>();
                    currentlocation.GetCurrentLocation();
                    TakePictureViewModel.isLocation = true;
                    await AssetViewModel.ExecuteUserImageSelectionCommand();

                };
                btnlocation.Clicked += async (sender, e) =>
                {
                    UserDialogs.Instance.ShowLoading("Loading...");
                    var currentlocation = DependencyService.Get<ICurrentLocation>();

                    currentlocation.GetCurrentLocation();
                    await Task.Delay(3000);
                    if (LocationServices.Latitude != 0 || LocationServices.Longitude != 0)
                    {
                        await Navigation.PushAsync(new MapPage(LocationServices.Latitude, LocationServices.Longitude));

                    }
                    //await Navigation.PushAsync(new MapPage(LocationServices.Latitude, LocationServices.Longitude));
                    UserDialogs.Instance.HideLoading();
                };
                var btnlayoutInternal = new StackLayout { VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand, Orientation = StackOrientation.Vertical };
                btnlayoutInternal.Children.Add(btnAdd);
                btnlayoutInternal.Children.Add(btnimglocation);
                btnlayoutInternal.Children.Add(btnlocation);
                var btnlayout = new StackLayout { VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand };

                btnlayout.Children.Add(lblHelp);
                btnlayout.Children.Add(btnlayoutInternal);

                mainlayoutGallery = new StackLayout { Padding = 5, VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand };

                mainlayoutGallery.Children.Add(relativeLayouts);
                mainlayoutGallery.Children.Add(btnlayout);
                MyUILibrary.AddLayout(relativeLayout, mainlayoutGallery, 0, position - 15, w, h / 2 - 50);
                var a = await ViewModel.GetDeviceAssets(device.device_idx);
                if (ViewModel.AssetsList != null && AssetViewModel != null)
                {
                    countAssets = ViewModel.AssetsList.Count;
                    btnAdd.BindingContext = AssetViewModel;
                }
                AssetViewModel._navigation = this.Navigation;
                AssetViewModel.Device = device;
                AssetViewModel.CountAsset = countAssets;
                btnAdd.Command = AssetViewModel.TakePicture;

                position += 100 + 10;
                position += 10;
            }
            catch (Exception ex)
            {
            }

            UserDialogs.Instance.HideLoading();
        }


        private async Task<Image> ModeltoImage(DeviceAssetsModel obj)
        {
            Image img = new Image();
            try
            {
                img = (BytesArraytoImage(obj.Deviceimage_thumbnail));

            }
            catch { }
            return img;
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

        public async void DeviceConfigration(string deviceid = null)
        {
            try
            {

                UserDialogs.Instance.ShowLoading();
                var offtimeconfigrations = await ApiService.Instance.GetDeviceOffConfig(deviceid);
                if (offtimeconfigrations != null)
                {
                    if (offtimeconfigrations.DeviceStatus == "Permanent off")
                    {
                        UserDialogs.Instance.HideLoading();
                        UserDialogs.Instance.Alert("Your cube is permanentaly off.Please switch on it manually.", "Info", "OK");
                        return;
                    }
                }

                UserDialogs.Instance.HideLoading();
                var model = new DeviceSwitchOffReq();
                model.CreatedBy = Settings.UserID;
                if (!string.IsNullOrEmpty(deviceid))
                {
                    model.DeviceId = Convert.ToInt32(deviceid);
                }
                else
                {
                    model.DeviceId = device.device_idx;
                }
                var CloseIcon = new Image { Source = "CloseIcon.png", HeightRequest = 35, WidthRequest = 35, HorizontalOptions = LayoutOptions.EndAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };
                var stkCloseIcon = new StackLayout { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
                stkCloseIcon.Children.Add(CloseIcon);

                var LblDvice = new Label { Text = "Deactivate Cube", TextColor = Color.Black, FontSize = 20, FontAttributes = FontAttributes.Bold, HorizontalOptions = LayoutOptions.StartAndExpand };
                LblDvice.LineBreakMode = LineBreakMode.WordWrap;

                var stklbl = new StackLayout { Padding = new Thickness(0, 10, 0, 0), HorizontalOptions = LayoutOptions.StartAndExpand, VerticalOptions = LayoutOptions.StartAndExpand };
                stklbl.Children.Add(LblDvice);

                StackLayout st = new StackLayout() { Margin = 5, Spacing = 0, Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.StartAndExpand };
                st.Children.Add(stklbl);
                st.Children.Add(stkCloseIcon);

                var popupLayouts = this.Content as CustomPopup;
                TapGestureRecognizer bigts = new TapGestureRecognizer();
                bigts.Tapped += async (object sender, EventArgs e) =>
                {
                    await popupLayouts.DismissPopup();
                };



                var boxview = new BoxView { WidthRequest = 10 };
                var stlhrlblentry = new StackLayout { HorizontalOptions = LayoutOptions.FillAndExpand, Orientation = StackOrientation.Horizontal };
                var lblconfigration = new Label { FontSize = 16, TextColor = Color.Black, Margin = 5, Text = "Switch off for", FontAttributes = FontAttributes.Bold, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.Start };
                stlhrlblentry.Children.Add(lblconfigration);
                var hourentryframe = new Frame { BorderColor = Color.Black, Padding = new Thickness(0, 0, 0, 0), HasShadow = false, WidthRequest = 60, HorizontalOptions = LayoutOptions.CenterAndExpand };
                var hourentry = new CustomTextView { BackgroundColor = Color.Transparent, IsFocus = true, TextColor = Color.Black, Keyboard = Keyboard.Numeric, HorizontalOptions = LayoutOptions.CenterAndExpand, FontSize = 18, MaxLength = 2, WidthRequest = 60, HeightRequest = 40 };
                hourentryframe.Content = hourentry;
                var houlbl = new Label { TextColor = Color.Black, FontAttributes = FontAttributes.Bold, Text = "Hours", FontSize = 16, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.Start };

                stlhrlblentry.Children.Add(hourentryframe);
                stlhrlblentry.Children.Add(houlbl);

                var hrstarttimestk = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.CenterAndExpand, IsVisible = true };
                var stkhours = new StackLayout { VerticalOptions = LayoutOptions.Start, Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.CenterAndExpand, IsVisible = true };

                hrstarttimestk.Children.Add(stlhrlblentry);
                hrstarttimestk.Children.Add(boxview);


                var Timestartlabel = new Label { TextColor = Color.Black, FontAttributes = FontAttributes.Bold, FontSize = 16, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.Start };
                stkhours.Children.Add(Timestartlabel);
                stkhours.Children.Add(hrstarttimestk);



                var configrationswitch = new Image { Source = "on.png", HorizontalOptions = LayoutOptions.EndAndExpand, HeightRequest = 60, WidthRequest = 60 };
                var stkconfigrationswitch = new StackLayout { HorizontalOptions = LayoutOptions.Start, WidthRequest = MyController.ScreenWidth / 3 };
                stkconfigrationswitch.Children.Add(configrationswitch);
                TapGestureRecognizer _configrationswitchswitch = new TapGestureRecognizer();
                configrationswitch.GestureRecognizers.Add(_configrationswitchswitch);
                _configrationswitchswitch.Tapped += (s, e) =>
                {

                    if ((configrationswitch.Source as FileImageSource).File == "off.png")
                    {
                        configrationswitch.Source = "on.png";
                        stkhours.IsVisible = true;
                        model.isDeviceEnable = true;
                        return;
                    }
                    if ((configrationswitch.Source as FileImageSource).File == "on.png")
                    {
                        configrationswitch.Source = "off.png";
                        stkhours.IsVisible = false;
                        model.isDeviceEnable = false;
                        return;
                    }
                };

                var boxviewsep = new BoxView { WidthRequest = 10, BackgroundColor = Color.Red };
                var stkconfigration = new StackLayout { BackgroundColor = Color.Green, Orientation = StackOrientation.Horizontal };
                // stkconfigration.Children.Add(stkconfigrationswitch);

                stkconfigration.Children.Add(boxviewsep);


                var imgremember = new Image { Source = "Checkbox.png", HeightRequest = 30, WidthRequest = 30, Aspect = Aspect.AspectFit };

                TapGestureRecognizer TRemember = new TapGestureRecognizer();
                TRemember.Tapped += (object sender, EventArgs e) =>
                {
                    if ((imgremember.Source as FileImageSource).File == "CheckBoxTick.png")
                    {
                        if (offtimeconfigrations == null)
                        {
                            imgremember.Source = ImageSource.FromFile("Checkbox.png");
                            hourentry.IsEnabled = true;
                            model.IsDevicePermanentOff = false;
                        }

                    }
                    else
                    {
                        if (offtimeconfigrations == null)
                        {
                            imgremember.Source = ImageSource.FromFile("CheckBoxTick.png");
                            hourentry.IsEnabled = false;
                            model.IsDevicePermanentOff = true;
                        }

                    }
                };
                imgremember.GestureRecognizers.Add(TRemember);
                var labelOR = new Label { FontSize = 20, TextColor = Color.Black, Text = "OR", FontAttributes = FontAttributes.Bold, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.CenterAndExpand };

                var labelManual = new Label { FontSize = 16, Margin = new Thickness(0, 0, 0, 0), TextColor = Color.Black, Text = "until next manual switch on", FontAttributes = FontAttributes.None, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.StartAndExpand };
                var orstk = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.CenterAndExpand };
                orstk.Children.Add(imgremember);
                orstk.Children.Add(labelManual);
                var RemainingTimelabel = new Label { TextColor = Color.Black, FontAttributes = FontAttributes.Bold, FontSize = 16, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand };

                var mainstkforor = new StackLayout { Orientation = StackOrientation.Vertical, VerticalOptions = LayoutOptions.Start, Spacing = 5, Margin = 3 };
                mainstkforor.Children.Add(labelOR);
                mainstkforor.Children.Add(orstk);
                mainstkforor.Children.Add(RemainingTimelabel);


                var btnok = new Xamarin.Forms.Button { Text = "Save", BackgroundColor = Color.Red, TextColor = Color.White, WidthRequest = MyController.ScreenWidth };
                btnok.Clicked += async (sender, e) =>
                {

                    var networkConnection = DependencyService.Get<INetworkConnection>();
                    networkConnection.CheckNetworkConnection();
                    var networkStatus = networkConnection.IsConnected ? "Connected" : "Not Connected";
                    if (networkStatus != "Connected")
                    {
                        UserDialogs.Instance.HideLoading();
                        UserDialogs.Instance.Alert("Please check your internet connection.", "Alert", "OK");
                        return;
                    }
                    else
                    {
                        if ((configrationswitch.Source as FileImageSource).File != "off.png")
                        {

                            if (!string.IsNullOrEmpty(hourentry.Text) || (imgremember.Source as FileImageSource).File == "CheckBoxTick.png")
                            {

                                if ((imgremember.Source as FileImageSource).File != "CheckBoxTick.png")
                                {
                                    int n;
                                    var isNumeric = int.TryParse(hourentry.Text, out n);
                                    if (isNumeric)
                                    {
                                        if (Convert.ToDouble(hourentry.Text) > 24 || Convert.ToDouble(hourentry.Text) < 1)
                                        {
                                            UserDialogs.Instance.HideLoading();
                                            UserDialogs.Instance.Alert("Please enter hours between 1 to 24!", "Info", "OK");
                                            hourentry.Text = "";
                                            return;
                                        }
                                        else
                                        {
                                            model.DeviceHours = int.Parse(hourentry.Text);
                                        }

                                    }
                                    else
                                    {
                                        UserDialogs.Instance.HideLoading();
                                        UserDialogs.Instance.Alert("Only Numbers are Allowed!", "Info", "OK");
                                        hourentry.Text = "";
                                        return;
                                    }
                                }


                                UserDialogs.Instance.ShowLoading("Loading...");
                                model.isDeviceEnable = true;
                                var result = await ApiService.Instance.DeviceOnOffConfigration(model);

                                if (result.ErrorCode == 0)
                                {
                                    await popupLayouts.DismissPopup();
                                    UserDialogs.Instance.HideLoading();
                                    UserDialogs.Instance.Alert("Save successfully", "Info", "OK");
                                }
                                else
                                {
                                    await popupLayouts.DismissPopup();
                                    UserDialogs.Instance.HideLoading();
                                    UserDialogs.Instance.Alert(result.ErrorMessage, "Info", "OK");
                                }

                            }
                            else
                            {
                                UserDialogs.Instance.HideLoading();
                                UserDialogs.Instance.Alert("Please Enter Hours to Continue!", "Info", "OK");
                                return;

                            }
                        }
                        else
                        {
                            model.DeviceHours = 0;
                            var result = await ApiService.Instance.DeviceOnOffConfigration(model);
                            if (result.ErrorCode == 0)
                            {
                                await popupLayouts.DismissPopup();
                                UserDialogs.Instance.HideLoading();
                                UserDialogs.Instance.Alert("Save successfully", "Info", "OK");
                            }
                            else
                            {
                                await popupLayouts.DismissPopup();
                                UserDialogs.Instance.HideLoading();
                                UserDialogs.Instance.Alert(result.ErrorMessage, "Info", "OK");
                            }
                        }
                    }
                };
                var lblremaininghours = new Label { IsVisible = false, TextColor = Color.Black, FontSize = 16, HorizontalOptions = LayoutOptions.CenterAndExpand, FontAttributes = FontAttributes.Bold };



                var btncancel = new Xamarin.Forms.Button { Text = "Cancel", BackgroundColor = Color.Gray, TextColor = Color.White, WidthRequest = MyController.ScreenWidth };

                btncancel.Clicked += async (sender, e) =>
                {

                    try
                    {

                        if (offtimeconfigrations != null)
                        {
                            var answerCubeConfirm = await UserDialogs.Instance.ConfirmAsync("Are you sure want to cancel this off configuration?", "Confirm", "Yes", "No");
                            UserDialogs.Instance.ShowLoading();

                            if (answerCubeConfirm)
                            {
                                var networkConnection = DependencyService.Get<INetworkConnection>();
                                networkConnection.CheckNetworkConnection();
                                var networkStatus = networkConnection.IsConnected ? "Connected" : "Not Connected";
                                if (networkStatus != "Connected")
                                {
                                    UserDialogs.Instance.HideLoading();
                                    UserDialogs.Instance.Alert("Please check your internet connection.", "Alert", "OK");
                                    return;
                                }

                                var result = await webServiceManager.GetCancelDeviceOffConfigAsync(offtimeconfigrations.AppConfig_idx);
                                if (result.ErrorCode == 0)
                                {
                                    await popupLayouts.DismissPopup();
                                    UserDialogs.Instance.HideLoading();
                                    UserDialogs.Instance.Alert("Switch off configuration cancel successfully.", "Cancel Successfully", "OK");
                                }
                                else
                                {
                                    await popupLayouts.DismissPopup();
                                    UserDialogs.Instance.HideLoading();
                                    UserDialogs.Instance.Alert(result.ErrorMessage, "Error", "OK");
                                }
                            }
                            else
                            {
                                await popupLayouts.DismissPopup();
                            }
                        }
                        else
                        {
                            await popupLayouts.DismissPopup();
                        }
                    }
                    catch (Exception ex)
                    {
                        // Debug.WriteLine(ex);
                        await popupLayouts.DismissPopup();
                        UserDialogs.Instance.HideLoading();

                    }
                    UserDialogs.Instance.HideLoading();


                };
                var btnClose = new Xamarin.Forms.Button { Text = "Close", BackgroundColor = Color.Gray, TextColor = Color.White, WidthRequest = MyController.ScreenWidth };
                btnClose.Clicked += async (sender, e) =>
                {
                    await popupLayouts.DismissPopup();
                };
                if (offtimeconfigrations != null)
                {
                    hourentry.IsEnabled = false;
                    if (offtimeconfigrations.DeviceHours != 0)
                    {
                        hourentry.Text = offtimeconfigrations.DeviceHours.ToString();
                    }
                    if (offtimeconfigrations.IsCancelable)
                    {
                        btnok.IsVisible = false;

                        if (offtimeconfigrations.cfg_changed != null)
                        {
                            Timestartlabel.Text = "Time Start From : " + offtimeconfigrations.strcfg_changed;

                        }

                        if (offtimeconfigrations.PendingTime != null)
                        {
                            var hrs = offtimeconfigrations.PendingTime / 60;
                            var min = offtimeconfigrations.PendingTime % 60;
                            if (hrs == 0)
                            {
                                RemainingTimelabel.Text = "Remaining time left is : " + min + " minutes";
                            }
                            else
                            {
                                RemainingTimelabel.Text = "Remaining time left is : " + hrs + " hours " + min + " minutes";
                            }
                        }
                        if (offtimeconfigrations.IsDevicePermanentOff == true)
                        {
                            imgremember.Source = "CheckBoxTick.png";
                            hourentry.IsEnabled = false;
                            model.IsDevicePermanentOff = true;

                        }
                        else if (offtimeconfigrations.IsDevicePermanentOff == false)
                        {
                            imgremember.Source = ImageSource.FromFile("Checkbox.png");
                            //hourentry.IsEnabled = true;
                            model.IsDevicePermanentOff = false;
                        }
                    }
                    else
                    {
                        if (offtimeconfigrations.DeviceStatus == "Permanent off")
                        {

                        }
                        else if (offtimeconfigrations.DeviceStatus == "Off")
                        {
                            btnok.IsVisible = false;
                            btncancel.IsVisible = false;
                            if (offtimeconfigrations.cfg_changed != null)
                            {
                                Timestartlabel.Text = "Time Start From : " + offtimeconfigrations.strcfg_changed;

                            }
                            if (offtimeconfigrations.PendingTime != null)
                            {
                                var hrs = offtimeconfigrations.PendingTime / 60;
                                var min = offtimeconfigrations.PendingTime % 60;
                                if (hrs == 0)
                                {
                                    RemainingTimelabel.Text = "Remaining time left is : " + min + " minutes";
                                }
                                else
                                {
                                    RemainingTimelabel.Text = "Remaining time left is : " + hrs + " hours " + min + " minutes";
                                }
                            }
                            if (offtimeconfigrations.IsDevicePermanentOff == true)
                            {
                                imgremember.Source = "CheckBoxTick.png";
                                hourentry.IsEnabled = false;
                                model.IsDevicePermanentOff = true;

                            }
                            else if (offtimeconfigrations.IsDevicePermanentOff == false)
                            {
                                imgremember.Source = ImageSource.FromFile("Checkbox.png");
                                //  hourentry.IsEnabled = true;
                                model.IsDevicePermanentOff = false;
                            }
                        }


                    }
                }
                else
                {
                    stkconfigration.IsVisible = true;
                    stkhours.IsVisible = true;
                    lblremaininghours.IsVisible = false;
                    model.isDeviceEnable = false;
                    btnClose.IsVisible = false;
                }
                var btnstk = new StackLayout { Orientation = StackOrientation.Vertical, VerticalOptions = LayoutOptions.Start, HorizontalOptions = LayoutOptions.CenterAndExpand, Padding = new Thickness(10, 10, 10, 10) };

                btnstk.Children.Add(btnok);
                btnstk.Children.Add(btncancel);
                btnstk.Children.Add(btnClose);

                StackLayout mainlayout = new StackLayout { Padding = new Thickness(10, 0, 0, 10), HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.StartAndExpand };
                mainlayout.Children.Add(st);

                //mainlayout.Children.Add(stkconfigration);
                mainlayout.Children.Add(stkhours);
                mainlayout.Children.Add(mainstkforor);

                mainlayout.Children.Add(lblremaininghours);

                StackLayout contentlayout = new StackLayout();
                contentlayout.Children.Add(mainlayout);
                contentlayout.Children.Add(btnstk);

                CloseIcon.GestureRecognizers.Add(bigts);
                if (popupLayouts.IsPopupActive)
                {
                }
                else
                {


                    var view = new Frame
                    {
                        Padding = new Thickness(0, 0, 0, 0),
                        HasShadow = false,
                        BorderColor = Color.Gray,
                        HeightRequest = MyController.ScreenHeight / 2,
                        WidthRequest = MyController.ScreenWidth - 20,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        BackgroundColor = Color.FromHex("f2f2f2"),
                        Content = contentlayout
                    };

                    await popupLayouts.ShowPopup(view);


                }
            }
            catch (System.Exception ex)
            {
                UserDialogs.Instance.HideLoading();
            }
        }



        public void GetPopupOfChangeCubeConfigAsync(int DeviceId)
        {
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

            var LiueRatingPicker = new CustomPicker { Title = "Configuration Store list", TitleTextColor = Color.Black, WidthRequest = MyController.ScreenWidth - w / 3, HeightRequest = 40, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };
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

                    var networkConnection = DependencyService.Get<INetworkConnection>();
                    networkConnection.CheckNetworkConnection();
                    var networkStatus = networkConnection.IsConnected ? "Connected" : "Not Connected";
                    if (networkStatus != "Connected")
                    {
                        UserDialogs.Instance.HideLoading();
                        UserDialogs.Instance.Alert("Please check your internet connection.", "Alert", "OK");
                        return;
                    }
                    if (!isConfigSelected)
                    {

                        await UserDialogs.Instance.AlertAsync("Please choose the configuration!! ", "Information", "OK");
                        return;
                    }
                    string cubeConfirmation = "Are you sure want to apply this configuration to cube? \nNote: All the pending changes of cube will be deleted.";
                    var answerCubeConfirm = await UserDialogs.Instance.ConfirmAsync(cubeConfirmation, "Confirm", "Yes", "No");
                    if (answerCubeConfirm)
                    {
                        UserDialogs.Instance.ShowLoading("Please wait...", MaskType.Gradient);
                        var selectedProfile = LiueRatingPicker.SelectedIndex + 1;

                        bool _ChangeCubeConfigration = await AssetViewModel.ChangeCubeConfigration(DeviceId, profileid, DateTime.Now).ConfigureAwait(true);

                        if (_ChangeCubeConfigration)
                        {
                            UserDialogs.Instance.HideLoading();
                            await UserDialogs.Instance.AlertAsync("Configuration changed Successfully", "Information", "OK");

                            popupLayouts.DismissPopup();
                        }
                        else
                        {
                            UserDialogs.Instance.HideLoading();
                            await UserDialogs.Instance.AlertAsync("Uwatch server error, try again after 5 minutes !! ", "Information", "OK");
                        }

                        UserDialogs.Instance.HideLoading();
                    }
                    else
                    {
                        popupLayouts.DismissPopup();
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




            TapGestureRecognizer bigt = new TapGestureRecognizer();
            bigt.Tapped += async (object sender, EventArgs e) =>
            {
                popupLayouts.DismissPopup();
            };

            CloseIcon.GestureRecognizers.Add(bigt);

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
                popupLayouts.ShowPopup(view);
            }

        }

        public async void GetPopupofAssetsImage(bool isopen)
        {
            try
            {
                Xamarin.Forms.StackLayout relativeLayouts = new Xamarin.Forms.StackLayout();
                relativeLayouts.VerticalOptions = LayoutOptions.StartAndExpand;
                int z = 10;
                double position = 0;
                double newx20 = MyUiUtils.getPercentual(w, 20);
                double newx40 = MyUiUtils.getPercentual(w, 40);
                double newx60 = MyUiUtils.getPercentual(w, 60);
                double newx80 = MyUiUtils.getPercentual(w, 80);

                var x = 10;
                var i = 0;
                var lblname = new Label { Text = "Device:", TextColor = Color.Black, FontSize = 18, FontAttributes = FontAttributes.Bold, WidthRequest = 75, };
                //MyUILibrary.AddLabel(relativeLayout, "Device:", x, position + 10, w, 50, Color.Black, 16);
                var LblDvice = new Label { TextColor = Color.Black, FontSize = 18, FontAttributes = FontAttributes.Bold, HorizontalOptions = LayoutOptions.StartAndExpand };
                LblDvice.LineBreakMode = LineBreakMode.WordWrap;
                if (MyController.Roll_id != 8)
                {
                    if (device != null && (!string.IsNullOrEmpty(device.FriendlyName)))
                        LblDvice.Text = "Cube: " + device.FriendlyName.ToString();
                    else
                        LblDvice.Text = "Cube: " + "Asset Images";
                }
                else
                {
                    LblDvice.Text = "Cube: " + "Asset Images";
                }
                var CloseIcon = new Image { Source = "CloseIcon.png", HeightRequest = 35, WidthRequest = 35, HorizontalOptions = LayoutOptions.EndAndExpand, VerticalOptions = LayoutOptions.StartAndExpand };

                var stklbl = new StackLayout { Padding = new Thickness(0, 10, 0, 0), HorizontalOptions = LayoutOptions.StartAndExpand, VerticalOptions = LayoutOptions.StartAndExpand };
                stklbl.Children.Add(LblDvice);

                var stkclose = new StackLayout { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
                stkclose.Children.Add(CloseIcon);

                StackLayout st = new StackLayout() { Orientation = StackOrientation.Horizontal, Spacing = 0 };
                st.Children.Add(stklbl);
                st.Children.Add(stkclose);

                relativeLayouts.Children.Add(st);

                var layoutGllry1 = new StackLayout { Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.StartAndExpand };
                var layoutGllry2 = new StackLayout { Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.StartAndExpand };
                var popupLayouts = this.Content as CustomPopup;
                if (ViewModel != null && ViewModel.AssetsList != null)
                {
                    countAssets = ViewModel.AssetsList.Count;
                    if (ViewModel.ImageList != null)
                    {
                        if (ViewModel.ImageList.Count() > 0)
                        {
                            int CountOfImage = 1;
                            foreach (var item in ViewModel.AssetsList)
                            {
                                var it = new CachedImage()
                                {
                                    WidthRequest = MyController.ScreenWidth / 5 + 5,
                                    HeightRequest = MyController.ScreenWidth / 5,
                                    CacheDuration = TimeSpan.FromDays(30),
                                    DownsampleToViewSize = true,
                                    RetryCount = 0,
                                    RetryDelay = 250,
                                    TransparencyEnabled = false,
                                    Aspect = Aspect.AspectFit,
                                    Source = ImageSource.FromFile("noimage.gif"),

                                };

                                var layout = new Xamarin.Forms.RelativeLayout();
                                layout.Padding = 2;
                                layout.Children.Add(it, Constraint.Constant(0), Constraint.Constant(0));
                                layout.BackgroundColor = Color.White;

                                var sou = await ModeltoImage(item);
                                it.Source = sou.Source as ImageSource;

                                var frm = new Frame { Padding = 1, HasShadow = false, BackgroundColor = Color.White };
                                frm.Content = it;
                                if (CountOfImage < 5)
                                {
                                    layoutGllry1.Children.Add(frm);
                                }
                                else
                                {
                                    layoutGllry2.Children.Add(frm);
                                    z += 80;
                                }
                                relativeLayouts.Children.Add(layoutGllry1);
                                relativeLayouts.Children.Add(layoutGllry2);

                                TapGestureRecognizer bigt = new TapGestureRecognizer();
                                bigt.Tapped += async (object sender, EventArgs e) =>
                                {
                                    popupLayouts.DismissPopup();
                                    var networkConnection = DependencyService.Get<INetworkConnection>();
                                    networkConnection.CheckNetworkConnection();
                                    var networkStatus = networkConnection.IsConnected ? "Connected" : "Not Connected";
                                    if (networkStatus != "Connected")
                                    {
                                        UserDialogs.Instance.Alert("Please check your internet connection.", "Alert", "OK");
                                        return;
                                    }
                                    UserDialogs.Instance.ShowLoading("Loading...");
                                    await Task.Delay(100);
                                    var AssetDetailsViewModel = new AssetDetailsViewModel();
                                    await AssetDetailsViewModel.GetActualAssetImage(item);

                                    await Navigation.PushAsync(new NewAssetsDetailPage(item.Position_no, item, AssetDetailsViewModel));
                                    UserDialogs.Instance.HideLoading();
                                };
                                it.GestureRecognizers.Add(bigt);
                                x += 80;
                                i++;
                                CountOfImage++;
                            }
                            position += 20 + 50;
                        }
                        else
                        {
                            var it = new CachedImage()
                            {
                                WidthRequest = 80,
                                HeightRequest = 80,
                                CacheDuration = TimeSpan.FromDays(30),
                                DownsampleToViewSize = true,
                                RetryCount = 0,
                                RetryDelay = 250,
                                TransparencyEnabled = false,
                                Aspect = Aspect.AspectFit,
                                Source = ImageSource.FromFile("noimage.gif"),
                            };
                            relativeLayouts.Children.Add(it);
                            //it = MyUILibrary.AddCachedImage(relativeLayouts, it, x, position + 40, 80, 80, Aspect.Fill);
                            position += 100 + 20;
                        }
                    }
                    else
                    {
                        var it = new CachedImage()
                        {
                            WidthRequest = 80,
                            HeightRequest = 80,
                            CacheDuration = TimeSpan.FromDays(30),
                            DownsampleToViewSize = true,
                            RetryCount = 0,
                            RetryDelay = 250,
                            TransparencyEnabled = false,
                            Aspect = Aspect.AspectFit,
                            Source = ImageSource.FromFile("noimage.gif"),
                        };
                        relativeLayouts.Children.Add(it);
                        position += 100 + 20;
                    }
                }
                else
                {
                    var it = new CachedImage()
                    {
                        WidthRequest = 80,
                        HeightRequest = 80,
                        CacheDuration = TimeSpan.FromDays(30),
                        DownsampleToViewSize = true,
                        RetryCount = 0,
                        RetryDelay = 250,
                        TransparencyEnabled = false,
                        Aspect = Aspect.AspectFit,
                        Source = ImageSource.FromFile("noimage.gif"),
                    };
                    relativeLayouts.Children.Add(it);
                    position += 100 + 20;
                }
                var lblHelp = new Label { Text = "Tap on image to edit & delete asset", HorizontalOptions = LayoutOptions.CenterAndExpand };
                var btnAdd = new Xamarin.Forms.Button
                {
                    Text = "   Add New Image   ",
                    BackgroundColor = Color.Red,
                    TextColor = Color.White,
                    FontSize = 15,
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };
                var btnimglocation = new Xamarin.Forms.Button
                {
                    Text = "   Add Image With Location  ",
                    BackgroundColor = Color.Red,
                    TextColor = Color.White,
                    FontSize = 15,
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };
                var btnlocation = new Xamarin.Forms.Button
                {
                    Text = "   Show Location  ",
                    BackgroundColor = Color.Red,
                    TextColor = Color.White,
                    FontSize = 15,
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };
                var btnlayout = new StackLayout { VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand };
                var btnlayoutInternal = new StackLayout { VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand, Orientation = StackOrientation.Vertical };
                btnlayoutInternal.Children.Add(btnAdd);
                btnlayoutInternal.Children.Add(btnimglocation);
                btnlayoutInternal.Children.Add(btnlocation);

                btnlayout.Children.Add(lblHelp);
                btnlayout.Children.Add(btnlayoutInternal);

                var mainlayout = new StackLayout { Padding = 5, VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand };
                mainlayout.Children.Add(relativeLayouts);
                mainlayout.Children.Add(btnlayout);

                if (ViewModel.AssetsList != null && AssetViewModel != null)
                {
                    countAssets = ViewModel.AssetsList.Count;
                    btnAdd.BindingContext = AssetViewModel;
                }
                AssetViewModel._navigation = this.Navigation;
                AssetViewModel.Device = device;
                AssetViewModel.CountAsset = countAssets;


                position += 100 + 10;
                position += 10;

                if (Xamarin.Forms.Device.Idiom == TargetIdiom.Phone)
                {
                    //PaddingMain = 8;
                    pddingH = .110;
                    paddingW = .8;
                }
                if (Xamarin.Forms.Device.Idiom == TargetIdiom.Tablet)
                {
                    //PaddingMain = 40;
                    pddingH = .40;
                    paddingW = .5;

                }
                //btnAdd.Command = AssetViewModel.TakePicture;
                btnAdd.Clicked += async (sender, e) =>
                {
                    //UserDialogs.Instance.ShowLoading("Loading...");
                    //await Task.Delay(100);
                    //if (ViewModel.ImageList.Count() < 8)
                    //{
                    //  popupLayouts.DismissPopup();
                    //}
                    //await AssetViewModel.ExecuteUserImageSelectionCommand();
                    if (ViewModel.ImageList.Count() == 8)
                    {

                    }
                    else if (ViewModel.ImageList.Count() < 8)
                    {
                        UserDialogs.Instance.ShowLoading("Loading...");
                        await Task.Delay(100);
                        popupLayouts.DismissPopup();

                    }
                    await AssetViewModel.ExecuteUserImageSelectionCommand();
                    UserDialogs.Instance.HideLoading();
                };

                btnimglocation.Clicked += async (sender, e) =>
                {
                    if (ViewModel.ImageList.Count() == 8)
                    {
                        UserDialogs.Instance.Alert("This Cube already have max quantities, Please delete one.");
                        return;
                    }
                    else if (ViewModel.ImageList.Count() < 8)
                    {

#if __ANDROID__

                        var res = await CheckLocationPermisions();
                        if (res)
                        {

                            UserDialogs.Instance.ShowLoading("Loading...");
                            var currentlocation = DependencyService.Get<ICurrentLocation>();
                            currentlocation.GetCurrentLocation();
                            await Task.Delay(3000);
                            if (LocationServices.Latitude.ToString() != "0" && LocationServices.Longitude.ToString() != "0")
                            {
                                TakePictureViewModel.isLocation = true;
                                await AssetViewModel.ExecuteUserImageSelectionCommand();

                                currentlocation.StopLocationServices();
                            }
                            else
                            {
                                MainActivity.Instance.NoLocation();
                                //UserDialogs.Instance.Alert("Location is not Enabled!", "Alert!", "Ok");
                                UserDialogs.Instance.HideLoading();
                                currentlocation.StopLocationServices();
                            }
                        }

#endif


#if __IOS__
                        var currentlocation = DependencyService.Get<ICurrentLocation>();
                        currentlocation.GetCurrentLocation();
                        UserDialogs.Instance.ShowLoading("Loading...");
                        await Task.Delay(3000);
                        TakePictureViewModel.isLocation = true;
                        //MapPage objMapPage = new MapPage();
                        await AssetViewModel.ExecuteUserImageSelectionCommand();
                        //UserDialogs.Instance.HideLoading();
                        //popupLayouts.DismissPopup();
#endif
                        popupLayouts.DismissPopup();

                    }
                };
                btnlocation.Clicked += async (sender, e) =>
                {
#if __ANDROID__
                    var res = await CheckLocationPermisions();
                    if (res)
                    {
                        UserDialogs.Instance.ShowLoading("Loading...");
                        var currentlocation = DependencyService.Get<ICurrentLocation>();

                        currentlocation.GetCurrentLocation();
                        await Task.Delay(3000);
                        if (LocationServices.Latitude.ToString() != "0" && LocationServices.Longitude.ToString() != "0")
                        {
                            await Navigation.PushAsync(new MapPage(LocationServices.Latitude, LocationServices.Longitude));
                            UserDialogs.Instance.HideLoading();
                            currentlocation.StopLocationServices();
                        }
                        else
                        {
                            MainActivity.Instance.NoLocation();
                            //UserDialogs.Instance.Alert("Location is not Enabled!", "Alert!", "Ok");
                            UserDialogs.Instance.HideLoading();
                            currentlocation.StopLocationServices();
                        }
                    }
#endif

#if __IOS__
                    UserDialogs.Instance.ShowLoading("Loading...");
                    var currentlocation = DependencyService.Get<ICurrentLocation>();

                    currentlocation.GetCurrentLocation();
                    await Task.Delay(3000);
                    if (LocationServices.Latitude != 0 || LocationServices.Longitude != 0)
                    {
                        await Navigation.PushAsync(new MapPage(LocationServices.Latitude, LocationServices.Longitude));

                    }
                    //await Navigation.PushAsync(new MapPage(LocationServices.Latitude, LocationServices.Longitude));
                    UserDialogs.Instance.HideLoading();
#endif
                };
                TapGestureRecognizer bigts = new TapGestureRecognizer();
                bigts.Tapped += async (object sender, EventArgs e) =>
                {
                    popupLayouts.DismissPopup();
                };

                CloseIcon.GestureRecognizers.Add(bigts);
                if (popupLayouts.IsPopupActive)
                {

                }
                else
                {


                    var view = new Frame
                    {
                        Padding = new Thickness(0, 0, 0, 0),
                        HasShadow = false,
                        OutlineColor = Color.Gray,
                        HeightRequest = MyController.ScreenHeight / 2 + 120,
                        WidthRequest = MyController.ScreenWidth - 20,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        BackgroundColor = Color.FromHex("f2f2f2"),
                        Content = mainlayout
                    };

                    popupLayouts.ShowPopup(view);
                }

            }
            catch { }

        }
#if __ANDROID__
        async Task<bool> CheckLocationPermisions()
        {
            var results = false;
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Plugin.Permissions.Abstractions.Permission.Location);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Plugin.Permissions.Abstractions.Permission.Location))
                    {
                        await UserDialogs.Instance.AlertAsync("Need location", "App want to use the location service for this App.", "OK");
                    }

                    var r = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Plugin.Permissions.Abstractions.Permission.Location });
                    status = r[Plugin.Permissions.Abstractions.Permission.Location];
                }
                if (status == PermissionStatus.Granted)
                {
                    results = true;
                }
                else if (status != PermissionStatus.Unknown)
                {
                    await UserDialogs.Instance.AlertAsync("Location Denied", "Can not continue, without location service. try again.", "OK");
                }
            }
            catch (Exception ex)
            {
                results = false;
            }
            return results;
        }
#endif
        public async void AddNewImagePopUp()
        {

            var popupLayoutsaddimage = this.Content as CustomPopup;

            var CloseIcon = new Image { Source = "CloseIcon.png", HeightRequest = 25, WidthRequest = 25, HorizontalOptions = LayoutOptions.EndAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };

            var lblTitle = new Label { Text = "Choose Option: ", TextColor = Color.Black, FontAttributes = FontAttributes.Bold, FontSize = 19, HorizontalOptions = LayoutOptions.StartAndExpand };

            var layoytTitle = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
            layoytTitle.Children.Add(lblTitle);
            layoytTitle.Children.Add(CloseIcon);

            var lblDetails = new Label { Text = "Choose your option :", TextColor = Color.Black, FontAttributes = FontAttributes.Bold, FontSize = 15, XAlign = TextAlignment.Start, VerticalOptions = LayoutOptions.StartAndExpand, HorizontalOptions = LayoutOptions.StartAndExpand };

            var divider = new BoxView { Color = Color.FromHex("dcdcdc"), HeightRequest = .5, HorizontalOptions = LayoutOptions.FillAndExpand };
            var layoutTitle = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand
            };
            layoutTitle.Children.Add(layoytTitle);
            layoutTitle.Children.Add(divider);

            var BtnEscalateToOther = new Xamarin.Forms.Button
            {
                Text = "   Add image   ",
                FontSize = 14,
                FontAttributes = FontAttributes.Bold,
                BackgroundColor = Color.Red,
                TextColor = Color.White,
            };

            var BtnEscalateToOtherByEmail = new Xamarin.Forms.Button
            {
                Text = "    Add Image with location   ",
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


            btnstack.Children.Add(BtnEscalateToOther);
            btnstack.Children.Add(BtnEscalateToOtherByEmail);


            var layouth = new StackLayout { Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.StartAndExpand, VerticalOptions = LayoutOptions.StartAndExpand, Spacing = 18 };

            layouth.Children.Add(lblDetails);

            var scrlMssg = new Xamarin.Forms.ScrollView();
            scrlMssg.Content = layouth;


            if (Xamarin.Forms.Device.Idiom == TargetIdiom.Phone)
            {
                //PaddingMain = 8;
                pddingH = .70;
                paddingW = .8;
            }
            if (Xamarin.Forms.Device.Idiom == TargetIdiom.Tablet)
            {
                //PaddingMain = 40;
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

            TapGestureRecognizer bigt = new TapGestureRecognizer();
            bigt.Tapped += async (object sender, EventArgs e) =>
            {
                popupLayoutsaddimage.DismissPopup();
            };

            CloseIcon.GestureRecognizers.Add(bigt);

            var view = new Frame
            {
                Padding = new Thickness(0, 0, 0, 0),
                HasShadow = true,
                HeightRequest = App.ScreenHeight / 2,
                WidthRequest = App.ScreenWidth - 100,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.FromHex("f2f2f2"),
                Content = mainlayout
            };
            popupLayoutsaddimage.ShowPopup(view);
        }
    }
}

