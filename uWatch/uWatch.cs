
using Xamarin.Forms;
using UwatchPCL;
using UwatchPCL.Helpers;
using System.Threading.Tasks;
using Acr.UserDialogs;
using UwatchPCL.Model.Request;
using System;
#if __ANDROID__
using Android.OS;
#endif
namespace uWatch
{
    public class App : Application
    {
        static Application app;
        public static int ScreenWidth;
        public static int ScreenHeight;
        public static bool isFirstPage = true;

        public static bool UseIos13FullScreenModal { get; set; }
        public static Application CurrentApp
        {
            get { return app; }
        }
        readonly ILoginService _LoginService;
        public App()
        {
            try
            {
#if __ANDROID__
                global::Android.Content.Context context = Forms.Context;
                var build = context.PackageManager.GetPackageInfo(context.PackageName, 0).VersionName;
                var code = context.PackageManager.GetPackageInfo(context.PackageName, 0).VersionCode.ToString();
                Settings.CurrentVersion = build + "." + code;
#endif
#if __IOS__
                Settings.CurrentVersion = Foundation.NSBundle.MainBundle.InfoDictionary["CFBundleVersion"].ToString();
#endif
                //if (Settings.IsLogout == false)
                //{
                //    isFirstPage = false;
                MainPage = new LoadingScreenPage();
                //}
                //else {
                //    isFirstPage = true;
                //    MainPage = new Login();


                //}

            }
            catch (Exception ex)
            {
                
            }

        }
    }
}

