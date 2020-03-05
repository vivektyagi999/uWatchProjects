using Xamarin.Forms;
using UwatchPCL;
using UwatchPCL.Helpers;
using System.Threading.Tasks;
using Acr.UserDialogs;
using UwatchPCL.Model.Request;
using System;
using UwatchPCL.WebServices;
using Xamarin.Essentials;
using static UwatchPCL.MyController;

#if __ANDROID__
using static Android.Provider.Settings;
using Android.OS;
#endif

namespace uWatch
{
    public class LoadingScreenPage : ContentPage
    {
        static INavigation nav;
        ContentView contentView;
        ILoginService _LoginService;
        WebServiceManager webServiceManager;
        public LoadingScreenPage()
        {

            try
            {
                var grid = new Grid();
                //UI of loading screen 
                var stack = new StackLayout { BackgroundColor = Color.FromRgb(237, 27, 38), HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
                grid.Children.Add(stack, 0, 0);
                var layoutActivitymain = new StackLayout { Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };

                var layoutActivity = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };
                var lbl = new Label { Text = "Loading...", TextColor = Color.White, VerticalOptions = LayoutOptions.CenterAndExpand };
                var loader = new ActivityIndicator { Color = Color.White, HeightRequest = 25, WidthRequest = 25, IsRunning = true, VerticalOptions = LayoutOptions.CenterAndExpand };
                layoutActivity.Children.Add(loader);
                layoutActivity.Children.Add(lbl);

                var img = new Image { Source = "spy_icon.png", HeightRequest = MyController.ScreenHeight / 7, WidthRequest = MyController.ScreenHeight / 7 };
                var stkCopyright = new StackLayout { VerticalOptions = LayoutOptions.End };
                var lblCopyright = new Label { Text = "© uWatch 2016/2017/2018/2019", TextColor = Color.White, HorizontalOptions = LayoutOptions.CenterAndExpand };

                layoutActivitymain.Children.Add(img);
                layoutActivitymain.Children.Add(layoutActivity);

                stack.Children.Add(layoutActivitymain);
                stkCopyright.Children.Add(lblCopyright);
                stack.Children.Add(stkCopyright);
                contentView = new ContentView
                {
                    BackgroundColor = Color.FromHex("#A6000000"),
                    IsVisible = false,

                };
                var gridContent = new Grid();

                gridContent.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                gridContent.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                gridContent.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                contentView.Content = gridContent;
                var frame = new Frame
                {
                    BackgroundColor = Color.White,
                    Margin = 10,

                };
                gridContent.Children.Add(frame, 0, 1);
                var stackcontent = new StackLayout
                {

                };
                frame.Content = stackcontent;
                var label = new Label
                {
                    FontAttributes = FontAttributes.Bold,
                    Text = "New Update is Available.",
                    FontSize = 18,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand
                };

                var button = new Button
                {
                    BackgroundColor = Color.LightGray,
                    CornerRadius = 12,
                    HorizontalOptions = LayoutOptions.EndAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    Text = "Update Now"
                };
                button.Clicked += Button_Clicked;

                stackcontent.Children.Add(label);
                stackcontent.Children.Add(button);
                grid.Children.Add(contentView, 0, 0);
                Content = grid;
            }
            catch (System.Exception ex)
            {

            }

        }

        private void Button_Clicked(object sender, EventArgs e)
        {
#if __IOS__
								Uri uri = new Uri("https://itunes.apple.com/us/app/uwatch/id1200756802?ls=1&mt=8");
								Device.OpenUri(uri);
#else
#if __ANDROID__
            Uri uri = new Uri("https://play.google.com/store/apps/details?id=com.uwatch.uwatchapp&hl=en");
            Device.OpenUri(uri);
#else
#endif
#endif
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                webServiceManager = new WebServiceManager();
                // it can run withouit an issue then call direct method
                if (Device.RuntimePlatform == Device.iOS)
                {
                    await getData();
                }
                else
                {
                    // issue on main thread so call the method on background
                    if (Device.RuntimePlatform == Device.Android)
                    {
                        await Task.Run(async () => await getData());
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        public async Task getData()
        {
            try
            {
                var data = await DependencyService.Get<IPushNotificationClear>().clearNotification();
                var model = new AppDetails
                {
                    AppName = AppNames.uWatch.ToString(),
                    Platform = Device.RuntimePlatform
                };
#if __IOS__
                model.CurrentVersion = Foundation.NSBundle.MainBundle.InfoDictionary["CFBundleVersion"].ToString();
#else   
                model.CurrentVersion = AppInfo.VersionString + "." + AppInfo.BuildString;
#endif

                var result = await webServiceManager.CheckLatestAppVersionAsync(model);
                if (result.Data != null)
                {
                    if (result.Data.UpdateAvailable == true)
                    {
                        if (result.Data.UpdateRequired == true)
                        {
                            UserDialogs.Instance.HideLoading();
                            contentView.IsVisible = true;
                            return;
                        }
                        else
                        {
                            UserDialogs.Instance.HideLoading();
                            string address = "Are you sure you want to update uWatch?";
                            var answer = await UserDialogs.Instance.ConfirmAsync(address, "Update uWatch", "Ok", "Cancel");
                            if (answer)
                            {
                                if (!UwatchPCL.Helpers.Constants.fortest)
                                {
#if __IOS__
								Uri uri = new Uri("https://itunes.apple.com/us/app/uwatch/id1200756802?ls=1&mt=8");
								Device.OpenUri(uri);
#else
#if __ANDROID__
                                    Uri uri = new Uri("https://play.google.com/store/apps/details?id=com.uwatch.uwatchapp&hl=en");
                                    Device.OpenUri(uri);
#else
#endif
#endif
                                    Device.BeginInvokeOnMainThread(() =>
        Xamarin.Forms.Application.Current.MainPage = new Login()
                                       );
                                    return;
                                }
                                else
                                {
                                    await UserDialogs.Instance.AlertAsync("Update path is not available for test application");

                                }
                            }


                        }

                    }


                }
                else
                {
                    UserDialogs.Instance.HideLoading();
                    await UserDialogs.Instance.AlertAsync(result.ErrorMessage);
                    return;
                }


                var deviceDetails = new DeviceDetailsModel
                {
                    AppName = AppNames.uWatch.ToString(),
                    DeviceToken = Settings.DeviceToken,
                    Manufacturer = DeviceInfo.Manufacturer,
                    PhoneModel = DeviceInfo.Model,

                };
#if __ANDROID__
                deviceDetails.SerialNo = Build.Serial;

#endif
#if __IOS__
                deviceDetails.SerialNo = UIKit.UIDevice.CurrentDevice.IdentifierForVendor.ToString(); ;
#endif

                var res = await webServiceManager.CheckUserTokenAsync(deviceDetails);

                if (string.IsNullOrEmpty(res.Data.DeviceToken))
                {
                    Settings.UserName = "";
                    Settings.Password = "";
                    Settings.IsLogout = true;
                    Application.Current.MainPage = new Login();
                }
                else
                {
                    clsAccessToken _clsAccessToken = new clsAccessToken();
                    // pass the credentials and again check the login internally 
                    _LoginService = new LoginService();
                    if (Settings.IsLogout == false)
                    {
                        if ((!String.IsNullOrEmpty(Settings.UserName)) && (!string.IsNullOrEmpty(Settings.Password)))
                        {
                            AccountModelReq loginreq = new AccountModelReq();
                            loginreq.Username = Settings.UserName;
                            loginreq.Password = Settings.Password;
                            loginreq.AppName = AppNames.uWatch;
#if __IOS__
				loginreq.CurrentVersion  = Foundation.NSBundle.MainBundle.InfoDictionary["CFBundleVersion"].ToString();
						loginreq.Platform = "iOS";
#endif
#if __ANDROID__
                            global::Android.Content.Context context = Forms.Context;
                            var build = context.PackageManager.GetPackageInfo(context.PackageName, 0).VersionName;
                            var code = context.PackageManager.GetPackageInfo(context.PackageName, 0).VersionCode.ToString();
                            loginreq.CurrentVersion = build + "." + code;
                            loginreq.Platform = "Android";
#endif

                            _clsAccessToken = await Task.Run(async () => await _LoginService.UserLoginAsync(loginreq));

                            if (!_LoginService.IsSuccess)
                            {
                            }

                        }
                        else
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                Navigation.PushModalAsync(new Login());
                            });
                        }

                        if (!_LoginService.IsAuthenticated)
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                Navigation.PushModalAsync(new Login());
                            });
                        }
                        else
                        {

                            //Send Push Notification token to API Server.
                            AppTokenModel req = new AppTokenModel();
                            req.PhoneName = Xamarin.Essentials.DeviceInfo.Name;
                            req.PhoneModel = Xamarin.Essentials.DeviceInfo.Model;
                            req.VersionString = Xamarin.Essentials.DeviceInfo.VersionString;
                            req.Idiom = Xamarin.Essentials.DeviceInfo.Idiom.ToString();
                            req.Manufacturer = Xamarin.Essentials.DeviceInfo.Manufacturer;

#if __IOS__
				req.AppVersion  = Foundation.NSBundle.MainBundle.InfoDictionary["CFBundleVersion"].ToString();

#endif
#if __ANDROID__
                            global::Android.Content.Context context = Forms.Context;
                            var build = context.PackageManager.GetPackageInfo(context.PackageName, 0).VersionName;
                            var code = context.PackageManager.GetPackageInfo(context.PackageName, 0).VersionCode.ToString();
                            req.AppVersion = build + "." + code;

#endif
                            req.DeviceToken = Settings.DeviceToken;
                            req.DeviceOS = Device.RuntimePlatform;
                            req.UserID = Settings.UserID;
                            req.strCreatedDate = DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss");
                            string Serial = "n/a";
#if __ANDROID__

                            Serial = global::Android.OS.Build.Serial;
                            if (string.IsNullOrWhiteSpace(Serial) || Serial == Build.Unknown || Serial == "0" || Serial == "unknown")
                            {
                                try
                                {
                                    var context1 = global::Android.App.Application.Context;
                                    Serial = Secure.GetString(context1.ContentResolver, Secure.AndroidId);
                                }
                                catch (Exception ex)
                                {
                                    System.Diagnostics.Debug.WriteLine(ex);
                                }
                            }

                            req.IsOnProduction = true;

#endif
#if __IOS__
						Serial = UIKit.UIDevice.CurrentDevice.IdentifierForVendor.ToString();

						//for production
						req.IsOnProduction = true;

					
#endif
                            req.SerialNo = Serial;

                            await ApiService.Instance.SaveTokenToServer(req);
                            var count = await ApiService.Instance.GetUnreadMessageCount(Settings.UserID);

                            GoToRoot();
                        }
                    }
                }

            }
            catch (System.Exception ex)
            {
                var x = ex.StackTrace;
            }
        }
        public async void GoToRoot()
        {
            try
            {
                if (MyController.AlertId != "")
                {
                    if (Settings.IsLogout == false)
                    {
                        await GetAlertPage();
                    }

                }

                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                     {
                         Xamarin.Forms.Application.Current.MainPage = new MainPage();
                     });
                }
            }
            catch (System.Exception)
            {
            }
        }



        public static async Task GetAlertPage()
        {
            try
            {
                var mainPage = new MainPage();
                var navPage = mainPage;
                mainPage.IsPresented = false;
                if (MyController.isMessage)
                {
                    UserDialogs.Instance.ShowLoading("Opening Messages!");
                    Device.BeginInvokeOnMainThread(async () =>
                       {
                           var MessageviewModel = new MessageViewModel();
                           await MessageviewModel.LoadDevices();
                           await MessageviewModel.LoadDevicesSendBox();
                           mainPage.nav = new NavigationPage(new MessegeListPage(MessageviewModel));
                           mainPage.Detail = mainPage.nav;
                           System.GC.Collect();
                           Application.Current.MainPage = navPage;
                       });
                    UserDialogs.Instance.HideLoading();

                }
                else
                {
                    AlertsEsclatedToAgentViewModel req = new AlertsEsclatedToAgentViewModel();
                    req.alertlog_idx = Convert.ToInt32(MyController.AlertId);
                    var EscaltingAlert = ApiService.Instance.GetAlert(req).Result;
                    if (EscaltingAlert == null)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                    {
                        mainPage.nav = new NavigationPage(new AlertDetail("Your Cube has been removed!")); // { BarBackgroundColor = Color.Red, BarTextColor = Color.White };
                        mainPage.Detail = mainPage.nav;
                        System.GC.Collect();
                        Application.Current.MainPage = new MainPage();
                    });
                    }
                    else
                    {
                        if (MyController.multiple == 1)
                        {
                            //------In case of multiple alerts 
                            Device.BeginInvokeOnMainThread(() =>
                        {

                            Application.Current.MainPage = navPage;
                        });

                        }
                        else
                        {
                            Device.BeginInvokeOnMainThread(() =>
                        {

                            mainPage.nav = new NavigationPage(new AlertDetail(EscaltingAlert));
                            mainPage.Detail = mainPage.nav;
                            System.GC.Collect();
                            Application.Current.MainPage = navPage;
                        });
                        }
                    }
                }
            }
            catch (System.Exception)
            {
            }
        }

        private async Task GetPage()
        {
            try
            {
                await GetMainPage();
            }
            catch (System.Exception)
            {
            }
        }

        public async Task<Page> GetMainPage()
        {
            Page _page = null;
            try
            {
                //if crosssettings data expires
                if (Settings.IsLogout)
                {
                    _page = new Login();

                    Device.BeginInvokeOnMainThread(() =>
                {
                    Navigation.PushModalAsync(new Login());
                });
                }
                else
                {
                    if (Settings.UserID <= 0)
                    {
                        _page = new Login();
                        Device.BeginInvokeOnMainThread(() =>
                {
                    Navigation.PushModalAsync(new Login());
                });
                    }
                    else if (MyController.AlertId == "")
                    {
                        var reqLogin = new UwatchPCL.Model.Request.AccountModelReq();
                        reqLogin.Username = Settings.UserName;
                        reqLogin.Password = Settings.Password;
                        var resLogin = await ApiService.Instance.UserLogin(reqLogin);
                        if (resLogin != null && resLogin.error == null)
                        {
                            Settings.AccessToken = resLogin.access_token;
                            var req = new UwatchPCL.Model.Response.AccountModel();
                            req.User_Idx = Settings.UserID;
                            var res = await ApiService.Instance.UserDetails(req);
                            if (res != null)
                            {
                                MyController.user_id = Settings.UserID;
                                MyController.Roll_id = Settings.RoleID;
                            }
                            var mainPage = new MainPage();
                            mainPage.IsPresented = true;
                            _page = mainPage;
                            Device.BeginInvokeOnMainThread(() =>
                {
                    Navigation.PushModalAsync(mainPage);
                });
                        }
                        else
                        {
                            UserDialogs.Instance.ShowError(resLogin.error, 2000); 
                            _page = new Login();
                            Device.BeginInvokeOnMainThread(() =>
                {
                    Navigation.PushModalAsync(new Login());
                });
                        }
                    }
                    else if (MyController.AlertId != "" && Settings.UserID > 0 && MyController.lstNs.Count == 1)
                    {
                        var reqLogin = new UwatchPCL.Model.Request.AccountModelReq();
                        reqLogin.Username = Settings.UserName;
                        reqLogin.Password = Settings.Password;
                        var resLogin = ApiService.Instance.UserLogin(reqLogin).Result;
                        if (resLogin != null)
                        {
                            Settings.AccessToken = resLogin.access_token;
                            var req = new UwatchPCL.Model.Response.AccountModel();
                            req.User_Idx = Settings.UserID;
                            var res = ApiService.Instance.UserDetails(req).Result;
                            if (res != null)
                            {
                                MyController.user_id = Settings.UserID;
                                MyController.Roll_id = Settings.RoleID;
                            }
                            var mainPage = new MainPage();
                            var navPage = mainPage;
                            mainPage.IsPresented = false;
                            mainPage.nav = new NavigationPage(new AlertDetail()); // { BarBackgroundColor = Color.Red, BarTextColor = Color.White };
                            mainPage.Detail = mainPage.nav;
                            _page = navPage;
                            Device.BeginInvokeOnMainThread(() =>
                {
                    Navigation.PushModalAsync(navPage);
                });
                        }
                    }
                    else
                    {
                        var reqLogin = new UwatchPCL.Model.Request.AccountModelReq();
                        reqLogin.Username = Settings.UserName;
                        reqLogin.Password = Settings.Password;
                        var resLogin = ApiService.Instance.UserLogin(reqLogin).Result;
                        if (resLogin != null)
                        {
                            Settings.AccessToken = resLogin.access_token;
                            var req = new UwatchPCL.Model.Response.AccountModel();
                            req.User_Idx = Settings.UserID;
                            var res = ApiService.Instance.UserDetails(req).Result;
                            if (res != null)
                            {
                                MyController.user_id = Settings.UserID;
                                MyController.Roll_id = Settings.RoleID;
                            }
                            var mainPage = new MainPage();
                            var navPage = mainPage;
                            mainPage.IsPresented = false;
                            mainPage.nav = new NavigationPage(new AlertDetail());
                            mainPage.Detail = mainPage.nav;
                            _page = navPage;
                            Device.BeginInvokeOnMainThread(() =>
                {
                    Navigation.PushModalAsync(navPage);
                });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return _page;
        }

    }
}

