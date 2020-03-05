using System;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;


#if __ANDROID__
using Java.Lang;
using Android.OS;
using Android.Telephony;
using Android.Util;
using static Android.Provider.Settings;

#endif
#if __IOS__
using UIKit;
#endif
using uWatch.ViewModels;
using UwatchPCL;
using UwatchPCL.Helpers;
using UwatchPCL.Model.Request;
using Xamarin.Forms;
using Plugin.Settings;
using Rg.Plugins.Popup.Services;
using UwatchPCL.WebServices;

namespace uWatch
{
    public class LoginViewModel : BaseViewModel
    {
        private INavigation navigation;
        private readonly ILoginService _loginService;
        WebServiceManager webServiceManager;
        private Command loginCommand;

        public static bool Clicked = true;
        public string RememberImage { get; set; }

        public const string UsernamePropertyName = "Username";
        private string username = string.Empty;
        public static bool b = false;
        public string Username
        {
            get { return username; }
            set { SetProperty(ref username, value, UsernamePropertyName); }
        }

        public const string PasswordPropertyName = "Password";
        private string password = string.Empty;

        public string Password
        {
            get { return password; }
            set { SetProperty(ref password, value, PasswordPropertyName); }
        }


        private string strpassword;


        public LoginViewModel(INavigation navigation)
        {
            this.navigation = navigation;
            _loginService = new LoginService();


            if (Settings.IsRememberMe)
            {
                this.Username = Xamarin.Essentials.Preferences.Get("UserName", "");
                this.Password = Xamarin.Essentials.Preferences.Get("PassWord", ""); 
            }
            else
            {
                this.Username = "";
                this.Password = "";
            }
            //Username = "080785ns0u";
            //Password = "12345*";
            webServiceManager = new WebServiceManager();


        }

        public async void CheckUpdateAsync()
        {

            var result = await webServiceManager.GetCheckAppUpdateAsync();
            if (result.IsSuccess)
            {
                if (result.Data.UpdateRequired)
                {
                    await UserDialogs.Instance.AlertAsync("New Update is Available. Update Now");
                    if (!UwatchPCL.Helpers.Constants.fortest)
                    {
#if __IOS__
                            Uri uri = new Uri("https://itunes.apple.com/us/app/uwatch/id1200756802?ls=1&mt=8");
                            Device.OpenUri(uri);
#endif
#if __ANDROID__
                        Uri uri = new Uri("https://play.google.com/store/apps/details?id=com.uwatch.uwatchapp&hl=en");
                        Device.OpenUri(uri);
#endif
                    }
                    else
                    {
                        await UserDialogs.Instance.AlertAsync("Update path is not available for test application");
                    }
                    //#if __IOS__
                    //                                    Uri uri = new Uri("https://itunes.apple.com/us/app/uwatch/id1200756802?ls=1&mt=8");
                    //                                    Device.OpenUri(uri);
                    //#else
                    //#if __ANDROID__
                    //                                    Uri uri = new Uri("https://play.google.com/store/apps/details?id=com.uwatch.uwatchapp&hl=en");
                    //                                    Device.OpenUri(uri);
                    //#else
                }
                else if (result.Data.UpdateAvailable)
                {
                    string address = "Are you sure you want to update uWatch?";
                    var answer = await UserDialogs.Instance.ConfirmAsync(address, "Update uWatch", "Ok", "Cancel");
                    if (answer)
                    {

                        if (!UwatchPCL.Helpers.Constants.fortest)
                        {


#if __IOS__
                            Uri uri = new Uri("https://itunes.apple.com/us/app/uwatch/id1200756802?ls=1&mt=8");
                            Device.OpenUri(uri);
#endif
#if __ANDROID__
                            Uri uri = new Uri("https://play.google.com/store/apps/details?id=com.uwatch.uwatchapp&hl=en");
                            Device.OpenUri(uri);
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


        public bool CanLogin()
        {
            if ((string.IsNullOrEmpty(Username)) && (string.IsNullOrEmpty(Password)))
            {

                UserDialogs.Instance.Alert("Please try to enter your username and password again.", "Login");//, "Input Error");
                return false;
            }
            else if (string.IsNullOrEmpty(Username))
            {

                UserDialogs.Instance.Alert("Username Should not be Blank", "Login");//, "Input Error");
                return false;
            }
            else if (string.IsNullOrEmpty(Password))
            {

                UserDialogs.Instance.Alert("Password Should not be Blank", "Input Error");//, "Input Error");
                return false;
            }

            return true;
        }

        public const string LoginCommandPropertyName = "LoginCommand";
        public Command LoginCommand
        {
            get
            {
                return loginCommand ?? (loginCommand = new Command(async () => await ExecuteLoginCommand()));
            }
        }
        // on click of login btn
        protected async Task ExecuteLoginCommand()
        {
            try
            {


                var networkConnection = DependencyService.Get<INetworkConnection>();
                networkConnection.CheckNetworkConnection();
                var networkStatus = networkConnection.IsConnected ? "Connected" : "Not Connected";
                if (networkStatus == "Connected")
                {

                    await Login();

                }
                else
                {

                    UserDialogs.Instance.Alert("Please check your internet connection.", "Login", "OK");
                }
                Clicked = true;
            }
            catch (System.Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                MyController.ErrorManagement(ex.Message);
            }
        }

        public async Task Login()
        {


            UserDialogs.Instance.ShowLoading("Loading...");
            await Task.Delay(1000);
            try
            {

                var objReq = new AccountModelReq();
                objReq.Username = this.Username;
                objReq.Password = this.Password;

                objReq.AppName = MyController.AppNames.uWatch;
#if __IOS__
                objReq.CurrentVersion = Foundation.NSBundle.MainBundle.InfoDictionary["CFBundleVersion"].ToString();
                objReq.Platform = "iOS";
#endif
#if __ANDROID__
                global::Android.Content.Context context = Forms.Context;
                var build = context.PackageManager.GetPackageInfo(context.PackageName, 0).VersionName;
                var code = context.PackageManager.GetPackageInfo(context.PackageName, 0).VersionCode.ToString();
                objReq.CurrentVersion = build + "." + code;
                objReq.Platform = "Android";
#endif
                strpassword = this.Password;
                if (objReq.Username.Length > 0 && objReq.Password.Length > 0)
                {

                    var un = await _loginService.UserLoginAsync(objReq);

                    if (un != null && un.error == null)
                    {

                        if (un.IsPasswordChanged)
                        {
                            CrossSettings.Current.AddOrUpdateValue("UserRoleId", un.Roleid.ToString());
                            if (un.User_Idx <= 0)
                            {

                                UserDialogs.Instance.HideLoading();
                                UserDialogs.Instance.Alert("incorrect user name / password");

                            }
                            else if (!(new int[] { 2, 3, 6, 8 }.Contains(un.Roleid)))
                            {

                                UserDialogs.Instance.HideLoading();
                                UserDialogs.Instance.Alert("User is not authorised for Login,\n You can only log in if you are an owner, uARM, Demonstrator or Agent.");

                            }
                            else
                            {


                                MyController.user_id = un.User_Idx;
                                Settings.UserID = un.User_Idx;
                                Settings.UserName = un.UserName;
                                Settings.AccessToken = un.access_token;
                                Settings.Password = this.Password;
                                Xamarin.Essentials.Preferences.Set("UserName", un.UserName);
                                Xamarin.Essentials.Preferences.Set("PassWord", Password);
                                Settings.RoleID = un.Roleid;
                                Settings.FullName = un.FullName;

                                //Send Push Notification token to API Server.
                                AppTokenModel req = new AppTokenModel();
#if __IOS__
                                req.AppVersion = Foundation.NSBundle.MainBundle.InfoDictionary["CFBundleVersion"].ToString();

#endif
#if __ANDROID__
                                global::Android.Content.Context contextr = Forms.Context;
                                var buildr = context.PackageManager.GetPackageInfo(contextr.PackageName, 0).VersionName;
                                var coder = context.PackageManager.GetPackageInfo(contextr.PackageName, 0).VersionCode.ToString();
                                req.AppVersion = buildr + "." + coder;

#endif
                                //req.AppVersion = Settings.Version;
                                req.DeviceToken = Settings.DeviceToken;
                                req.DeviceOS = MyController.Device_OS;
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
                                    catch (System.Exception ex)
                                    {
                                        System.Diagnostics.Debug.WriteLine(ex);
                                    }
                                }
                                req.IsOnProduction = true;
#endif
#if __IOS__
                                Serial = UIDevice.CurrentDevice.IdentifierForVendor.ToString();

                                //for production
                                req.IsOnProduction = true;

                                //for development
                                //req.IsOnProduction = false;
#endif
                                req.SerialNo = Serial;
                                req.PhoneName = Xamarin.Essentials.DeviceInfo.Name;
                                req.PhoneModel = Xamarin.Essentials.DeviceInfo.Model;
                                req.VersionString = Xamarin.Essentials.DeviceInfo.VersionString;
                                req.Idiom = Xamarin.Essentials.DeviceInfo.Idiom.ToString();
                                req.Manufacturer = Xamarin.Essentials.DeviceInfo.Manufacturer;
                                var res = await ApiService.Instance.SaveTokenToServer(req);
                                Settings.IsLogout = false;
                                Device.BeginInvokeOnMainThread(new Action(() =>
                                {
                                    Application.Current.MainPage = new MainPage(un);
                                }));
                            }

                        }
                        else
                        {
                            Settings.UserName = un.UserName;
                            Settings.Password = "";
                            UserDialogs.Instance.HideLoading();
                            await navigation.PushModalAsync(new ResetPasswordPage(un.UserName, objReq.Password), true);
                        }

                    }
                    else if (un.error != "")
                    {

                        UserDialogs.Instance.HideLoading();
                        UserDialogs.Instance.Alert("incorrect user name / password");

                    }
                    else
                    {

                        UserDialogs.Instance.HideLoading();
                        UserDialogs.Instance.Alert("User does not exist in database !");

                    }
                }



            }
            catch (System.Exception ex)
            {


            }
            await Task.Delay(1000);
            UserDialogs.Instance.HideLoading();


        }


    }
}

