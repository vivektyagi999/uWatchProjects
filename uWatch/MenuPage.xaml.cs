using System;
using System.Collections.Generic;

using Xamarin.Forms;
using UwatchPCL;
using Acr.UserDialogs;
using System.Threading.Tasks;
using UwatchPCL.Helpers;
using UwatchPCL.Model;
using System.Linq;
using System.Collections.ObjectModel;

#if __IOS__
using Foundation;
#endif

#if __ANDROID__
using Android.Content.PM;
using Android.Content;


#endif
using UwatchPCL.Model;
namespace uWatch
{
    public class MenuPages : ContentPage
    {
        Label Condition, count;
        RoundedButton Condition2;
        public static bool IsTimerOn;
        public Action<Page> OnMenuTap;
        public ListView ListView { get { return listView; } }
        RelativeLayout _countrelativelayout;
        Button btnCount;
        InfiniteListView listView;
        string uWatchIcon = "";
        public RelativeLayout relativelayout;
        public MasterPageViewModel viewModel;

        public double w = MyController.VirtualWidth;
        public double h = MyController.VirtualHeight;
        double FontSize;

        public MenuPages()
        {
            if (Device.Idiom == TargetIdiom.Tablet)
            {
                FontSize = 19;
            }
            else
            {
                FontSize = 18;
            }
            try
            {
                Icon = "menu.png";
                Title = "Menu";

                relativelayout = new RelativeLayout();
                viewModel = new MasterPageViewModel();
                BindingContext = viewModel;

                listView = new InfiniteListView();

                listView.HorizontalOptions = LayoutOptions.StartAndExpand;
                listView.VerticalOptions = LayoutOptions.FillAndExpand;
                listView.SeparatorColor = Color.White;
                listView.BackgroundColor = Color.Transparent;
                listView.HeightRequest = MyController.ScreenHeight / 2;
                listView.SetBinding(ListView.ItemsSourceProperty, new Binding("MenuItems", BindingMode.TwoWay));

                int _countofListviewItems = 0;
                listView.ItemTemplate = new DataTemplate(() =>
          {

              _countofListviewItems += 1;

              var cell = new ViewCell();
              if (Device.Idiom == TargetIdiom.Tablet)
              {
                  FontSize = 19;
              }
              else
              {
                  FontSize = 18;
              }
              btnCount = new Button
              {
                  Text = "1",
                  HeightRequest = 30,
                  WidthRequest = 30,
                  BorderRadius = 18,
                  TextColor = Color.Red,
                  BackgroundColor = Color.White,
              };


              Condition = new Label();

              Condition.HorizontalOptions = LayoutOptions.Start;
              Condition.TextColor = Color.White;
              Condition.XAlign = TextAlignment.Start;


              if (Settings.RoleID == 6)
              {
                  if (_countofListviewItems == 3)
                  {
                      var image = new Image();
                      count = new Label();
                      image.Source = "white.png";


                      Condition2 = new RoundedButton();
                      Condition2.HeightRequest = 30;
                      Condition2.WidthRequest = 30;
                      Condition2.BorderRadius = Device.RuntimePlatform == Device.Android ? 20 : 14;
                      Condition2.HorizontalOptions = LayoutOptions.End;
                      Condition2.TextColor = Color.Red;
                      Condition2.BackgroundColor = Color.White;

                  }
                  else
                  {
                  }
              }
              else if (Settings.RoleID == 8)
              {
                  if (_countofListviewItems == 2)
                  {
                      var image = new Image();
                      count = new Label();
                      image.Source = "white.png";

                      Condition2 = new RoundedButton();
                      Condition2.HeightRequest = 30;
                      Condition2.WidthRequest = 30;
                      Condition2.BorderRadius = 18;
                      Condition2.HorizontalOptions = LayoutOptions.End;
                      Condition2.TextColor = Color.Red;
                      Condition2.BackgroundColor = Color.White;

                  }
                  else
                  {
                  }
              }
              else if (Settings.RoleID == 3)
              {
                  if (_countofListviewItems == 3)
                  {
                      var image = new Image();
                      count = new Label();
                      image.Source = "white.png";


                      Condition2 = new RoundedButton();
                      Condition2.IsVisible = false;
                      Condition2.HeightRequest = 30;
                      Condition2.WidthRequest = 30;
                      Condition2.BorderRadius = 18;
                      Condition2.HorizontalOptions = LayoutOptions.End;
                      Condition2.TextColor = Color.Red;
                      Condition2.BackgroundColor = Color.White;
                  }
                  else
                  {
                  }
              }

              var Images = new Image();
              Images.VerticalOptions = LayoutOptions.Start;
              Images.HorizontalOptions = LayoutOptions.Start;
              if (Device.Idiom == TargetIdiom.Phone)
              {
                  Images.HeightRequest = 30;
                  Images.WidthRequest = 30;
                  Condition.VerticalOptions = LayoutOptions.CenterAndExpand;
                  Condition.FontSize = FontSize;
              }
              else
              {
                  if (Device.Idiom == TargetIdiom.Tablet)
                  {
                      Images.HeightRequest = 40;
                      Images.WidthRequest = 40;
                      Condition.VerticalOptions = LayoutOptions.CenterAndExpand;
                      Condition.FontSize = FontSize;

                  }
              }

              Condition.SetBinding(Label.TextProperty, new Binding("Title"));
              Images.SetBinding(Image.SourceProperty, new Binding("IconSource"));

              var mainStack1 = new StackLayout()
              {
                  Orientation = StackOrientation.Horizontal,
                  HorizontalOptions = LayoutOptions.Start,

                  Children = { Images, Condition }
              };
              var mainStack = new StackLayout()
              {
                  Spacing = 20,
                  Orientation = StackOrientation.Horizontal,
                  HorizontalOptions = LayoutOptions.FillAndExpand,

                  Children = { mainStack1 }
              };
              if (Settings.RoleID == 6)
              {
                  if (_countofListviewItems == 3)
                  {
                      mainStack.Children.Add(Condition2);
                  }
                  else
                  {
                  }
              }
              else if (Settings.RoleID == 8)
              {
                  if (_countofListviewItems == 2)
                  {
                      mainStack.Children.Add(Condition2);
                  }
                  else
                  {

                  }
              }
              else if (Settings.RoleID == 3)
              {
                  if (_countofListviewItems == 3)
                  {
                      mainStack.Children.Add(Condition2);
                  }
                  else
                  {
                  }
              }
              var stacklayoutJobType = new StackLayout()
              {

                  Orientation = StackOrientation.Vertical,
                  VerticalOptions = LayoutOptions.CenterAndExpand,
                  HorizontalOptions = LayoutOptions.FillAndExpand,
                  Children = { mainStack }

              };

              cell.View = stacklayoutJobType;

              cell.BindingContextChanged += async (sender, e) =>
                {
                    base.OnBindingContextChanged();


                    ViewCell theViewCell = ((ViewCell)sender);

                    var item = (MenuItem)theViewCell.BindingContext;
                    if (item != null)
                    {
                        if (item.Title.ToString() == "Messages")
                        {
                            var mcount = await ApiService.Instance.GetUnreadMessageCount(Settings.UserID);
                            if (mcount == "0")
                            {
                                Condition2.IsVisible = false;
                            }
                            else
                            {
                                Condition2.IsVisible = true;
                                Condition2.Text = mcount;
                            }
                            IsTimerOn = true;

                            await Timer();
                        }
                    }
                };


              return cell;

          });

                listView.ItemTapped += async (sender, e) =>
                {
                    var networkConnection = DependencyService.Get<INetworkConnection>();
                    networkConnection.CheckNetworkConnection();
                    var networkStatus = networkConnection.IsConnected ? "Connected" : "Not Connected";

                    var r = (MainPage)this.Parent;

                    var item = (MenuItem)e.Item;
                    if (item != null && item.Title != "Logout")
                    {
                        UserDialogs.Instance.ShowLoading("Please Wait...", MaskType.Gradient);
                        await Task.Delay(500);
                    }
                    try
                    {
                        if (item != null)
                        {
                            if (item.Title == "Members")
                            {
                                if (networkStatus != "Connected")
                                {
                                    listView.SelectedItem = null;
                                    UserDialogs.Instance.HideLoading();
                                    UserDialogs.Instance.Alert("Please check your internet connection.", "Alert", "OK");
                                    return;
                                }
                                var memberviewModel = new MemberViewModel();
                                await memberviewModel.LoadDevices();
                                OnMenuTap(new MembersListPage(memberviewModel));


                            }
                            else if (item.Title == "Cubes")
                            {
                                if (networkStatus != "Connected")
                                {
                                    listView.SelectedItem = null;
                                    UserDialogs.Instance.HideLoading();
                                    UserDialogs.Instance.Alert("Please check your internet connection.", "Alert", "OK");
                                    return;
                                }
                                var assetsViewModel = new DashboardViewModel(this.Navigation);
                                await assetsViewModel.LoadDevices();
                                if (assetsViewModel.DeviceList.Count > 1)
                                {

                                    OnMenuTap(new DevicesPage(assetsViewModel));
                                    r.IsPresented = false;

                                }
                                else if (assetsViewModel.DeviceList.Count == 1)
                                {

                                    OnMenuTap(new DevicesPage(assetsViewModel));
                                    r.IsPresented = false;
                                }
                                else if (assetsViewModel.DeviceList.Count == 0)
                                {

                                    assetsViewModel.Device = new DeviceStatic() { device_idx = 0 };
                                    var ViewModel = new DeviceDetailsViewModel();
                                    if (ViewModel.AssetsList == null)
                                        ViewModel.AssetsList = new System.Collections.ObjectModel.ObservableCollection<DeviceAssetsModel>();
                                    ViewModel.AssetsList = await ViewModel.GetDeviceAssets(assetsViewModel.Device.device_idx);
                                    ViewModel.device = assetsViewModel.Device;
                                    OnMenuTap(new DevicesPage(ViewModel));
                                }

                            }
                            else if (item.Title == "Alerts" || item.Title == "Escalated Alerts")
                            {
                                if (networkStatus != "Connected")
                                {
                                    listView.SelectedItem = null;
                                    UserDialogs.Instance.HideLoading();
                                    UserDialogs.Instance.Alert("Please check your internet connection.", "Alert", "OK");
                                    return;
                                }
                                if (Settings.RoleID == 3)
                                {
                                    var alertListViewModel = new AlertsListViewModel(Navigation, Settings.UserID);
                                    await alertListViewModel.LoadAlertListOfAgent();
                                    OnMenuTap(new AlertListPage(0, alertListViewModel));

                                }
                                else
                                {
                                    var alertListViewModel = new AlertsListViewModel(Navigation, Settings.UserID);
                                    await alertListViewModel.LoadAlertList();
                                    OnMenuTap(new AlertListPage(0, alertListViewModel));

                                }

                            }
                            else if (item.Title == "Assets")
                            {

                                if (networkStatus != "Connected")
                                {
                                    listView.SelectedItem = null;
                                    UserDialogs.Instance.HideLoading();
                                    UserDialogs.Instance.Alert("Please check your internet connection.", "Alert", "OK");
                                    return;
                                }
                                var assetsViewModel = new DashboardViewModel(this.Navigation);
                                await assetsViewModel.LoadDevices();
                                if (assetsViewModel.DeviceList.Count >= 1)
                                {

                                    OnMenuTap(new AssetsPage(assetsViewModel));
                                    r.IsPresented = false;

                                }

                                else if (assetsViewModel.DeviceList.Count == 0)
                                {

                                    assetsViewModel.Device = new DeviceStatic() { device_idx = 0 };
                                    var ViewModel = new DeviceDetailsViewModel();
                                    if (ViewModel.AssetsList == null)
                                        ViewModel.AssetsList = new System.Collections.ObjectModel.ObservableCollection<DeviceAssetsModel>();
                                    ViewModel.AssetsList = await ViewModel.GetDeviceAssets(assetsViewModel.Device.device_idx);
                                    ViewModel.device = assetsViewModel.Device;
                                    OnMenuTap(new AssetsPage(ViewModel));
                                }

                            }
                            else if (item.Title == "Messages")
                            {
                                if (networkStatus != "Connected")
                                {
                                    listView.SelectedItem = null;
                                    UserDialogs.Instance.HideLoading();
                                    UserDialogs.Instance.Alert("Please check your internet connection.", "Alert", "OK");
                                    return;
                                }
                                var MessageviewModel = new MessageViewModel();
                                await MessageviewModel.LoadDevices();
                                await MessageviewModel.LoadDevicesSendBox();
                                OnMenuTap(new MessegeListPage(MessageviewModel));


                            }
                            //Inventory
                            else if (item.Title == "Inventory")
                            {
                                if (networkStatus != "Connected")
                                {
                                    listView.SelectedItem = null;
                                    UserDialogs.Instance.HideLoading();
                                    UserDialogs.Instance.Alert("Please check your internet connection.", "Alert", "OK");
                                    return;
                                }
                                UserDialogs.Instance.HideLoading();
                                OnMenuTap(new Inventory());
                            }
                            else if (item.Title == "Logout")
                            {
                                IsTimerOn = false;
                                if (networkStatus != "Connected")
                                {
                                    listView.SelectedItem = null;
                                    UserDialogs.Instance.HideLoading();

                                    UserDialogs.Instance.Alert("Please check your internet connection.", "Alert", "OK");
                                    return;

                                }
                                else
                                {
                                    await Logout();
                                }
                            }
                            else
                            {
                                IsTimerOn = false;
                                OnMenuTap(new HelpAndSupportPage());

                            }
                        }
                        listView.SelectedItem = null;

                    }
                    catch (Exception ex)
                    {
                        UserDialogs.Instance.HideLoading();
                    }
                    await Task.Delay(500);
                    listView.SelectedItem = null;
                    UserDialogs.Instance.HideLoading();
                };

                Padding = new Thickness(0, 0, 0, 0);
                Icon = "menu.png";
                Title = "Menu";

                double position = 0;
                double newx20 = MyUiUtils.getPercentual(w, 20);
                double newx40 = MyUiUtils.getPercentual(w, 40);
                double newx60 = MyUiUtils.getPercentual(w, 60);
                double newx80 = MyUiUtils.getPercentual(w, 80);
                if (Constants.fortest)
                {
                    uWatchIcon = "test_site.png";
                }
                else
                {
                    uWatchIcon = "spy_icon.png";
                }
                if (Device.Idiom == TargetIdiom.Tablet)
                {
                    MyUILibrary.AddImage(relativelayout, "side_menu_bg.png", 0, 0, w, h, Aspect.Fill);
                    var imguser = MyUILibrary.AddImage(relativelayout, uWatchIcon, (((w - newx20) / 2) - 100 - 20) + 2, position + 50, 90, 90);
                    imguser.Aspect = Aspect.AspectFit;
                    int x = 0;
                    var fullname = viewModel.FullName;
                    if (fullname != null)
                    {
                        if (fullname.Length > 20)
                        {
                            x = 30;
                        }
                        else
                        {
                            x = 50;
                        }
                    }
                    else
                    {
                        x = 50;
                    }
                    var lblFullName = MyUILibrary.AddLabel(relativelayout, "User Full Name", x - 10, position + 140, newx80, 55, Color.White, FontSize);
                    lblFullName.HorizontalOptions = LayoutOptions.StartAndExpand;
                    lblFullName.SetBinding(Label.TextProperty, new Binding("FullName", BindingMode.TwoWay));

                    var lblUserId = MyUILibrary.AddLabel(relativelayout, "User Id", 50, position + 140 + 25, w, 55, Color.White, FontSize);
                    lblUserId.HorizontalOptions = LayoutOptions.StartAndExpand;
                    lblUserId.SetBinding(Label.TextProperty, new Binding("UserId", BindingMode.TwoWay));
                }
                else
                {
                    MyUILibrary.AddImage(relativelayout, "side_menu_bg.png", 0, 0, w, h, Aspect.Fill);
                    var imguser = MyUILibrary.AddImage(relativelayout, uWatchIcon, ((w - newx20) / 2) - 50, position + 50, 90, 90);
                    imguser.Aspect = Aspect.AspectFit;
                    var lblFullName = MyUILibrary.AddLabel(relativelayout, "User Full Name", 0, position + 140, newx80, 55, Color.White, FontSize);
                    lblFullName.HorizontalOptions = LayoutOptions.CenterAndExpand;
                    lblFullName.SetBinding(Label.TextProperty, new Binding("FullName", BindingMode.TwoWay));
                    var fullname = viewModel.FullName;
                    if (fullname != null)
                    {
                        if (fullname.Length > 25)
                        {
                            position = +140 + 50;
                        }
                        else
                        {
                            position = +140 + 25;
                        }
                    }
                    else
                    {
                        position = +140 + 25;
                    }
                    var lblUserId = MyUILibrary.AddLabel(relativelayout, "User Id", (newx20 / 2) - 80, position, w, 55, Color.White, FontSize);

                    lblUserId.HorizontalOptions = LayoutOptions.CenterAndExpand;
                    lblUserId.SetBinding(Label.TextProperty, new Binding("UserId", BindingMode.TwoWay));
                }
                position = +140 + 25;

                MyUILibrary.AddListView(relativelayout, listView, 0, position + 25, w, h / 2, Color.Gray, 50);
                if (Constants.fortest)
                {
                    MyUILibrary.AddLabel(relativelayout, "uWatch Test Version", 20, h - 100, w, 50, Color.White, FontSize);

                }
                else
                {
                    MyUILibrary.AddLabel(relativelayout, "uWatch Version", 20, h - 100, w, 50, Color.White, FontSize);
                }
                var lblversion = MyUILibrary.AddLabel(relativelayout, "", 32, h - 80, w, 50, Color.White, FontSize);
                string version = "";
#if __IOS__
				version = NSBundle.MainBundle.InfoDictionary["CFBundleVersion"].ToString();
#endif
#if __ANDROID__
                Context context = Forms.Context;
                var build = context.PackageManager.GetPackageInfo(context.PackageName, 0).VersionName;
                var code = context.PackageManager.GetPackageInfo(context.PackageName, 0).VersionCode.ToString();
                version = build + "." + code;

#endif

                Settings.Version = version;
                lblversion.Text = version;
                MyUILibrary.AddLabel(relativelayout, "<<Slide to Hide/Show Main Menu>>", 5, h - 50, w, 50, Color.White, FontSize);


                Content = relativelayout;
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.ShowError(ex.Message);
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

        }

        public async System.Threading.Tasks.Task Logout()
        {

            UserDialogs.Instance.ShowLoading("Loading...");


            AppTokenModel req = new AppTokenModel();
            req.AppVersion = Settings.Version;
            req.DeviceToken = Settings.DeviceToken;
            req.DeviceOS = Device.RuntimePlatform;
            req.UserID = Settings.UserID;
            req.AppVersion = Settings.Version;
            var r = await ApiService.Instance.DeleteTokenFromServer(req);
            Settings.IsLogout = true;

            Application.Current.MainPage = new Login();
            UserDialogs.Instance.HideLoading();

        }

        async Task Timer()
        {
            try
            {
                int i = 1;
                string mcount = "";

                Xamarin.Forms.Device.StartTimer(TimeSpan.FromSeconds(2), () =>
                   {
                       MenuItem item = new MenuItem();
                       i = i + 2;
                       Task.Run(async () =>
                       {
                           mcount = await ApiService.Instance.GetUnreadMessageCount(Settings.UserID);

                       });
                       if (!string.IsNullOrEmpty(mcount))
                       {
                           if (mcount == "0")
                           {
                               if (Condition2.IsVisible)
                               {
                                   Condition2.IsVisible = false;
                               }
                           }
                           else
                           {
                               Condition2.IsVisible = true;
                               Condition2.Text = mcount;
                           }
                       }

                       return IsTimerOn;
                   });
            }
            catch (Exception ex)
            {


            }
        }

    }

}


