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
using uWatch;

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
    public partial class AssetsPage : ContentPage
    {
        public static bool IsList;
        public INavigation _navigation { get; set; }
        double pddingH, paddingW;
        private ImageSource picture;
        Label lblHelp;
        Xamarin.Forms.Button btnAdd;
        StackLayout mainlayoutGallery;
        InfiniteListView assetListView;
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

        double w = MyController.VirtualWidth;
        double h = MyController.VirtualHeight;
        public InfiniteListView listView;

        public bool MoreThanOneDevices { get; set; }

        public DeviceStatic device { get; set; }
        public DataTemplate Cell { get; private set; }

        public DashboardViewModel ViewModelDevices { get; set; }

        public TakePictureViewModel AssetViewModel { get; set; }
        public DeviceDetailsViewModel ViewModel { get; set; }

        public AssetsPage(DashboardViewModel assetsViewModel = null)
        {
            try
            {
                MyController.fromAssetsToGallery = false;
                this.ViewModelDevices = assetsViewModel;
                InitializeComponent();
                AssetViewModel = new TakePictureViewModel();

                //if()
                //if (ViewModelDevices.DeviceList.Count >= 1)
                //{
                    IsList = true;
                    SetLayout(true);
               // }

                //else
                //{
                //    relativeLayout = new Xamarin.Forms.RelativeLayout();
                //    var lblError = MyUILibrary.AddLabel(relativeLayout, "No Assets to Display", 20, 150, w - 40, 55, Color.Blue, 20);
                //    lblError.HorizontalOptions = LayoutOptions.CenterAndExpand;
                //    scrollview = new Xamarin.Forms.ScrollView();
                //    scrollview.Content = relativeLayout;


                //    var custom = new CustomPopup();
                //    var popupLayouts = this.Content as CustomPopup;
                //    Content = custom;
                //    custom.Content = scrollview;

                //}
            }
            catch (Exception ex)
            {
                MyController.ErrorManagement(ex.Message);
            }
        }

        public AssetsPage(DeviceDetailsViewModel assetViewModel)
        {
            try
            {
                this.ViewModel = assetViewModel;
                if (ViewModel.device != null)
                    this.device = ViewModel.device;
                AssetViewModel = new TakePictureViewModel();
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
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Plugin.Permissions.Abstractions.Permission.Camera);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Plugin.Permissions.Abstractions.Permission.Camera))
                    {
                        await DisplayAlert("Need Camera", "uWatch need to access your Camera", "OK");
                    }

                    var r = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Plugin.Permissions.Abstractions.Permission.Camera });
                    status = r[Plugin.Permissions.Abstractions.Permission.Camera];
                }
                if (status == PermissionStatus.Granted)
                {
                    results = true;
                }
                else if (status != PermissionStatus.Unknown)
                {
                    await DisplayAlert("Location Denied", "Can not continue, try again.", "OK");
                }
            }
            catch (Exception ex)
            {
                results = false;
                MyController.ErrorManagement(ex.Message);
            }
            return results;
        }

        async Task<bool> CheckStoragePermisions()
        {
            var results = false;
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Plugin.Permissions.Abstractions.Permission.Storage);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Plugin.Permissions.Abstractions.Permission.Storage))
                    {
                        await DisplayAlert("Need Camera", "uWatch need to access your Storage", "OK");
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
                    await DisplayAlert("Location Denied", "Can not continue, try again.", "OK");
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
            var s = await CheckStoragePermisions();
            var c = await CheckCameraPermisions();
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

                        //ViewModel = new DeviceDetailsViewModel(this.device.device_idx);
                        //var Assets = await ViewModel.GetDeviceAssets(device.device_idx);
                        //if (this.device != null)

                        if (!IsList)
                        {
                            SetLayout();
                        }
                        else
                        {
                            var networkConnection1 = DependencyService.Get<INetworkConnection>();
                            networkConnection1.CheckNetworkConnection();
                            var networkStatus1 = networkConnection1.IsConnected ? "Connected" : "Not Connected";
                            if (networkStatus1 != "Connected")
                            {
                                UserDialogs.Instance.Alert("Please check your internet connection.", "Alert", "OK");
                                return;
                            }
                           
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
                var networkConnection = DependencyService.Get<INetworkConnection>();
                networkConnection.CheckNetworkConnection();
                var networkStatus = networkConnection.IsConnected ? "Connected" : "Not Connected";
                if (networkStatus != "Connected")
                {
                    UserDialogs.Instance.Alert("Please check your internet connection.", "Alert", "OK");
                    return;
                }
                UserDialogs.Instance.ShowLoading();
                ViewModel = new DeviceDetailsViewModel(0);
                if (ViewModel.AssetsList == null)
                    ViewModel.AssetsList = new System.Collections.ObjectModel.ObservableCollection<DeviceAssetsModel>();
                ViewModel.AssetsList = await ViewModel.GetDeviceAssets(0);
                ViewModel.device = this.device;
                var baseLayout = new StackLayout() { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
                var btnAdd = new Xamarin.Forms.Button
                {
                    Text = "Add New Image",
                    BackgroundColor = Color.Red,
                    TextColor = Color.White,
                    FontSize = 15,
                    WidthRequest = 250,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.EndAndExpand
                };

                var btngetlocation = new Xamarin.Forms.Button
                {
                    Text = "Show Location",
                    BackgroundColor = Color.Red,
                    TextColor = Color.White,
                    FontSize = 15,
                    WidthRequest = 250,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.EndAndExpand
                };

                btnAdd.Clicked += async (sender, e) =>
                {
                    if (ViewModel.ImageList != null)
                    {
                        if (ViewModel.totalAssets >= Constants.ImageMaxLimit)
                        {
                            UserDialogs.Instance.Alert("Maximum quantity of 20 asset images  reached, delete to make room.");
                            return;
                        }
                        //else if (ViewModel.ImageList.Count() < Constants.ImageMaxLimit)
                        //{
                        //    UserDialogs.Instance.ShowLoading("Loading...");
                        //    await Task.Delay(100);

                        //}
                        UserDialogs.Instance.ShowLoading("Loading...");
                        await Task.Delay(100);
                        await AssetViewModel.ExecuteUserImageSelectionCommand();

                        UserDialogs.Instance.HideLoading();
                    }
                   
                    //popupLayouts.DismissPopup();


                };

                btngetlocation.Clicked += async (sender, e) =>
                {
                    try
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
                    }
                    catch (System.Exception ex)
                    {
                        UserDialogs.Instance.HideLoading();
                    }
                };
                var stkBottomLayout = new StackLayout()
                {
                    Padding=new Thickness(0,10,0,0),
                    HeightRequest = 100,
                    VerticalOptions = LayoutOptions.EndAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };
                // ViewModel.device = this.device;
                if (ViewModel.ImageList != null)
                {

                    if (ViewModel.ImageList.Count() > 0)
                    {
                         assetListView = new InfiniteListView()
                        {
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            VerticalOptions = LayoutOptions.FillAndExpand,
                            //BackgroundColor = Color.FromRgb(247, 247, 247),
                            BindingContext = ViewModel,
                            RowHeight = 180,
                            SeparatorVisibility = SeparatorVisibility.None,
                            HasUnevenRows = false
                        };
                        assetListView.LoadMoreCommand = ViewModel.LoadMoreAssetCommand;
                        assetListView.ItemsSource = ViewModel.AssetsLists;
                        Cell = new DataTemplate(typeof(AssetImageListCell));
                        assetListView.ItemTemplate = Cell;
                        assetListView.ItemTapped += (s, e) => {

                            (s as InfiniteListView).SelectedItem = null;
                        };


                        var lblHelp = new Label { Text = "Tap on image to edit & delete asset", TextColor=Color.Black, FontSize=16, FontAttributes=FontAttributes.Bold, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };

                        stkBottomLayout.Children.Add(lblHelp);
                        stkBottomLayout.Children.Add(btnAdd);
                       // stkBottomLayout.Children.Add(btngetlocation);
                        baseLayout.Children.Add(assetListView);
                      //  baseLayout.Children.Add(stkBottomLayout);
                    }
                    else
                    {
                        stkBottomLayout.Children.Add(btnAdd);
                       // stkBottomLayout.Children.Add(btngetlocation);
                        stkBottomLayout.VerticalOptions = LayoutOptions.CenterAndExpand;

                    }

                 }
                else
                {
                    stkBottomLayout.Children.Add(btnAdd);
                   // stkBottomLayout.Children.Add(btngetlocation);
                    stkBottomLayout.VerticalOptions = LayoutOptions.CenterAndExpand;
                }
                baseLayout.Children.Add(stkBottomLayout);

                MyUILibrary.AddLayout(relativeLayout, baseLayout, 0, 0, w, h - 90);
                if (ViewModel.AssetsList != null && AssetViewModel != null)
                {
                    countAssets = ViewModel.AssetsList.Count;
                    btnAdd.BindingContext = AssetViewModel;
                }
                AssetViewModel._navigation = this.Navigation;
                AssetViewModel.Device = device;
                AssetViewModel.CountAsset = ViewModel.totalAssets;

                UserDialogs.Instance.HideLoading();


            }
            catch (Exception ex)
            {

                UserDialogs.Instance.HideLoading();


            }
        }


        private async void SetLayout(bool moredevices = false)
        {
            try
            {

                Title = "Assets";
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
                var LblDvice = new Label { TextColor = Color.Black, FontSize = 15 };
                LblDvice.LineBreakMode = LineBreakMode.WordWrap;
                if (MyController.Roll_id != 8)
                {
                    if (device != null && (!string.IsNullOrEmpty(device.FriendlyName)))
                        LblDvice.Text = "Cube: " + device.FriendlyName.ToString();
                    else
                    {
                        //LblDvice.Text = "Cube: " + "Asset Images";
                        LblDvice.Text = " Asset Images";
                    }


                }
                else
                {
                    //LblDvice.Text = "Cube: " + "Asset Images"; 
                    LblDvice.Text = " Asset Images";
                }
                var layoutGllry1 = new StackLayout { Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.StartAndExpand };
                var layoutGllry2 = new StackLayout { Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.StartAndExpand };

                StackLayout st = new StackLayout() { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.StartAndExpand };
                st.Children.Add(LblDvice);

                MyUILibrary.AddLayout(relativeLayout, st, x + 20, position + 10, w - 20, 50);

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
                                    if (AssetDetailsViewModel.DeviceAsset != null)
                                    {
                                        await Navigation.PushAsync(new NewAssetsDetailPage(item.Position_no, item, AssetDetailsViewModel));
                                    }
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
                var lblHelp = new Label { Text = "Tap on image to edit & delete asset", HorizontalOptions = LayoutOptions.CenterAndExpand };
                var btnAdd = new Xamarin.Forms.Button
                {
                    Text = "Add New Image",
                    BackgroundColor = Color.Red,
                    TextColor = Color.White,
                    FontSize = 15,
                    WidthRequest = 250,
                    HorizontalOptions = LayoutOptions.CenterAndExpand
                };
                var btnAddimagewithlocation = new Xamarin.Forms.Button
                {
                    Text = "Add Image With Location",
                    BackgroundColor = Color.Red,
                    TextColor = Color.White,
                    FontSize = 15,
                    WidthRequest = 250,
                    HorizontalOptions = LayoutOptions.CenterAndExpand
                };
                var btngetlocation = new Xamarin.Forms.Button
                {
                    Text = "Show Location",
                    BackgroundColor = Color.Red,
                    TextColor = Color.White,
                    FontSize = 15,
                    WidthRequest = 250,
                    HorizontalOptions = LayoutOptions.CenterAndExpand
                };

                btnAddimagewithlocation.Clicked += async (sender, e) =>
                  {
                      try
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
                                  //MapPage objMapPage = new MapPage();
                                  await AssetViewModel.ExecuteUserImageSelectionCommand();
                                  UserDialogs.Instance.HideLoading();
                                  currentlocation.StopLocationServices();
                              }
                              else
                              {
                                  MainActivity.Instance.NoLocation();
                                  // UserDialogs.Instance.Alert("Location is not Enabled!", "Alert!", "Ok");
                                  UserDialogs.Instance.HideLoading();
                                  //popupLayouts.DismissPopup();
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
                          await AssetViewModel.ExecuteUserImageSelectionCommand();
                          UserDialogs.Instance.HideLoading();

#endif
                      }
                      catch (System.Exception ex)
                      {
                          UserDialogs.Instance.HideLoading();
                      }
                  };
                btngetlocation.Clicked += async (sender, e) =>
                {
                    try
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
                        UserDialogs.Instance.HideLoading();
#endif
                    }
                    catch (System.Exception ex)
                    {

                    }
                };

                var btnlayout = new StackLayout { VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand };

                btnlayout.Children.Add(lblHelp);
                btnlayout.Children.Add(btnAdd);

                mainlayoutGallery = new StackLayout { Padding = 5, VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand };

                mainlayoutGallery.Children.Add(relativeLayouts);
                mainlayoutGallery.Children.Add(btnlayout);
                MyUILibrary.AddLayout(relativeLayout, mainlayoutGallery, 0, position - 25, w, h / 2 - 50);

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
                        LblDvice.Text = "Cubes: " + device.FriendlyName.ToString();
                    else
                        LblDvice.Text = "Cubes: " + "Asset Images";
                }
                else
                {
                    LblDvice.Text = "Cubes: " + "Asset Images";
                }
                var CloseIcon = new Image { Source = "CloseIcon.png", HeightRequest = 35, WidthRequest = 35, HorizontalOptions = LayoutOptions.EndAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };

                var stklbl = new StackLayout { Padding = new Thickness(0, 10, 0, 0), HorizontalOptions = LayoutOptions.StartAndExpand, VerticalOptions = LayoutOptions.StartAndExpand };
                stklbl.Children.Add(LblDvice);

                var stkclose = new StackLayout { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
                stkclose.Children.Add(CloseIcon);

                StackLayout st = new StackLayout() { Padding = new Thickness(0, 0, 0, 0), Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.StartAndExpand };
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
                                    if (AssetDetailsViewModel.DeviceAsset != null)
                                    {
                                        await Navigation.PushAsync(new NewAssetsDetailPage(item.Position_no, item, AssetDetailsViewModel));

                                    }
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
                var btnlayoutInternal = new StackLayout { VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand, Orientation = StackOrientation.Vertical };
                btnlayoutInternal.Children.Add(btnAdd);
                //btnlayoutInternal.Children.Add(btnimglocation);
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

                    pddingH = .110;
                    paddingW = .8;
                }
                if (Xamarin.Forms.Device.Idiom == TargetIdiom.Tablet)
                {

                    pddingH = .40;
                    paddingW = .5;

                }


                btnAdd.Clicked += async (sender, e) =>
              {
                  if (ViewModel.ImageList.Count() == 8)
                  {
                        UserDialogs.Instance.Alert("Maximum quantity of 20 asset images  reached, delete to make room.");
                      return;
                  }
                  else if (ViewModel.ImageList.Count() < 8)
                  {
                      UserDialogs.Instance.ShowLoading("Loading...");
                      await Task.Delay(100);
                      popupLayouts.DismissPopup();

                  }


                  await AssetViewModel.ExecuteUserImageSelectionCommand();

                  UserDialogs.Instance.HideLoading();
                  //popupLayouts.DismissPopup();


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
                                //MapPage objMapPage = new MapPage();
                                await AssetViewModel.ExecuteUserImageSelectionCommand();

                                currentlocation.StopLocationServices();
                                popupLayouts.DismissPopup();
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

                        await AssetViewModel.ExecuteUserImageSelectionCommand();
                        UserDialogs.Instance.HideLoading();
                        popupLayouts.DismissPopup();
#endif


                    }
                };
                btnlocation.Clicked += async (sender, e) =>
                {
                    try
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
                    }
                    catch (System.Exception ex)
                    {
                        UserDialogs.Instance.HideLoading();
                    }
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



                    this.ToolbarItems.Clear();
                    NavigationPage.SetHasBackButton(this, false);
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

    }
}

