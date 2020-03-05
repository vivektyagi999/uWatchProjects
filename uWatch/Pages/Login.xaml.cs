using System;
using Xamarin.Forms;
using UwatchPCL;
using uWatch;
using UwatchPCL.Helpers;
using System.Threading.Tasks;
using Acr.UserDialogs;
#if __ANDROID__
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;


#endif
#if __ANDROID__
using Android.OS;
#else
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
#endif

using Rg.Plugins.Popup.Extensions;


namespace uWatch
{
    public partial class Login : ContentPage
    {
        const int rowHeight = 190;
        RelativeLayout relativeLayout;
        Xamarin.Forms.ScrollView scrollview;

        int w = MyController.VirtualWidth;
        int H = MyController.VirtualHeight;

        public LoginViewModel ViewModel;
        public Button btnLogin, btnRegister;
        public Image Img_TopRedLine = null;
        public Image imgIcon = null;

        public Xamarin.Forms.Entry txtUsername;
        public Xamarin.Forms.Entry txtPassword;

        public double newx20 = 0;
        public bool UseIos13FullScreenModal { get; set; }
        public Login()
        {
            try
            {
                InitializeComponent();

                BindingContext = ViewModel = new LoginViewModel(Navigation);

#if __ANDROID__
                MyController.Device_OS = Device.OS.ToString();
#endif
#if __IOS__
                MyController.Device_OS = Device.OS.ToString();
                On<Xamarin.Forms.PlatformConfiguration.iOS>().SetModalPresentationStyle(UIModalPresentationStyle.FormSheet);

#endif
                //if (App.isFirstPage)
                //{
                //    ViewModel.CheckUpdateAsync();
                //}

            }
            catch (Exception ex)
            {
                MyController.ErrorManagement(ex.Message);
            }
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
                        await DisplayAlert("Need location", "App want to use the location service for this App.", "OK");
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
                    await DisplayAlert("Location Denied", "Can not continue, without location service. try again.", "OK");
                }
            }
            catch (Exception ex)
            {
                results = false;
            }
            return results;
        }
#endif

        protected override async void OnAppearing()
        {
            try
            {


#if __ANDROID__
                //var s = await CheckLocationPermisions();
                //if (s)
                //{
                    ViewModel = new LoginViewModel(this.Navigation);
                    this.BackgroundColor = Color.White;
                    BindingContext = ViewModel;
                    SetLayout();
                //}
                //else
                //{
                //    Process.KillProcess(Process.MyPid());
                //}
                base.OnAppearing();
#endif
#if __IOS__
                ViewModel = new LoginViewModel(this.Navigation);
                this.BackgroundColor = Color.White;
                BindingContext = ViewModel;
                SetLayout();
                base.OnAppearing();
#endif
            }
            catch (Exception ex)
            {
                MyController.ErrorManagement(ex.Message);
            }

        }

        protected async override void OnDisappearing()
        {
            base.OnDisappearing();
            
        }

        private void SetLayout()
        {
            relativeLayout = new RelativeLayout();
            relativeLayout.BackgroundColor = Color.White;
            AddLayout();
            scrollview = new Xamarin.Forms.ScrollView();
            scrollview.Content = relativeLayout;
            Content = scrollview;
        }

        private async void AddLayout()
        {
            double position = 0;
            newx20 = MyUiUtils.getPercentual(w, 20);
            double newx40 = MyUiUtils.getPercentual(w, 40);
            double newx60 = MyUiUtils.getPercentual(w, 60);
            double newx80 = MyUiUtils.getPercentual(w, 80);

            var imgTopRedLine = MyUILibrary.AddImage(relativeLayout, "red_line.png", 0, position + 12, w, 10, Aspect.Fill);
            var imgBottomRedLine = MyUILibrary.AddImage(relativeLayout, "red_line.png", 0, H - 36, w, 10, Aspect.Fill);
            Device.OnPlatform
            (
                iOS: () =>
                {
                    imgTopRedLine.TranslationY = position + 12;

                }
            );

            position += 3;

            MyUILibrary.AddImage(relativeLayout, "uwatch_logo.png", (w - newx40) / 2, position + 40, newx40, newx40, Aspect.AspectFit);
            position += 10 + newx40;

            var lblLogin = MyUILibrary.AddLabel(relativeLayout, "Enter your login details", 0, position, w, 55, Color.Black, 18);
            position += 25;
            lblLogin.HorizontalOptions = LayoutOptions.CenterAndExpand;

            txtUsername = MyUILibrary.AddEntry(relativeLayout, "", "Username", (w - newx80) / 2, position + 20, newx80, 35, Color.Black, Keyboard.Default, TextAlignment.Start, fontsize: 18);
            position += 25 + 25;
            txtUsername.SetBinding(Xamarin.Forms.Entry.TextProperty, new Binding("Username"));
            if (ViewModel.Username.Length == 0)
            {
                ViewModel.Username = txtUsername.Text;
            }

            txtPassword = MyUILibrary.AddEntry(relativeLayout, "", "Password", (w - newx80) / 2, position + 20, newx80, 35, Color.Black, Keyboard.Default, TextAlignment.Start, true, fontsize: 18);
            position += 25 + 25;
            txtPassword.SetBinding(Xamarin.Forms.Entry.TextProperty, new Binding("Password"));
            if (ViewModel.Password.Length == 0)
            {
                ViewModel.Password = txtPassword.Text;
            }

            var imgremember = MyUILibrary.AddImage(relativeLayout, "Checkbox.png", (w - newx80) / 2 + 10, position + 30, 20, 20, Aspect.AspectFit);
            if (!Settings.IsRememberMe)
            {
                imgremember.Source = ImageSource.FromFile("Checkbox.png");
                Settings.IsRememberMe = false;
            }
            else
            {
                imgremember.Source = ImageSource.FromFile("CheckBoxTick.png");
                Settings.IsRememberMe = true;
                
            }


            TapGestureRecognizer TRemember = new TapGestureRecognizer();
            TRemember.Tapped += (object sender, EventArgs e) =>
            {
                if ((imgremember.Source as FileImageSource).File == "CheckBoxTick.png")
                {
                    imgremember.Source = ImageSource.FromFile("Checkbox.png");
                    Settings.IsRememberMe = false;
                }
                else
                {
                    imgremember.Source = ImageSource.FromFile("CheckBoxTick.png");
                    Settings.IsRememberMe = true;
                }
            };
            imgremember.GestureRecognizers.Add(TRemember);
            MyUILibrary.AddLabel(relativeLayout, "Remember me", ((w - newx80) / 2) + 40, position + 30, newx80, 55, Color.Black, 15);


            btnLogin = MyUILibrary.AddButton(relativeLayout, "Login", (w - newx60) / 2, position + 80, newx60, 40, Color.Red, Color.Red, Color.White, 15);
            position += 60;

            var lblforgot = new Label() { Text = "Forgot password?", FontSize = 16, TextColor = Color.Blue };
            var lblSignUp = new Label() { Text = " Sign up", FontSize = 16, TextColor = Color.Blue };
            var stkLayoutSignup = new StackLayout()
            {
                Children = { lblforgot, lblSignUp },
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            MyUILibrary.AddLayout(relativeLayout, stkLayoutSignup, 0, position + 90, w, 55);
            string appversion = "";
#if __IOS__
            if (Constants.fortest)
            {
                appversion = "uWatch Test Version: " + Foundation.NSBundle.MainBundle.InfoDictionary["CFBundleVersion"].ToString();
            }
            else
            {
                appversion = "uWatch Version: " + Foundation.NSBundle.MainBundle.InfoDictionary["CFBundleVersion"].ToString();
            }
            //objReq.Platform = "iOS";
#endif
#if __ANDROID__
            global::Android.Content.Context context = Forms.Context;
            var build = context.PackageManager.GetPackageInfo(context.PackageName, 0).VersionName;
            var code = context.PackageManager.GetPackageInfo(context.PackageName, 0).VersionCode.ToString();
            if (Constants.fortest)
            {
                appversion = "uWatch Test Version: " + build + "." + code;
            }
            else
            {
                appversion = "uWatch Version: " + build + "." + code;
            }

            //objReq.Platform = "Android";
#endif

            var lblappversion = MyUILibrary.AddLabel(relativeLayout, appversion, 0, position + 130, w, 55, Color.Black, 15);
            lblappversion.HorizontalOptions = LayoutOptions.CenterAndExpand;
            TapGestureRecognizer TForgot = new TapGestureRecognizer();
            TForgot.Tapped += async (object sender, EventArgs e) =>
            {
                App.UseIos13FullScreenModal = UseIos13FullScreenModal;
                UserDialogs.Instance.ShowLoading("Loading...", MaskType.Gradient);
                await Navigation.PushModalAsync(new Xamarin.Forms.NavigationPage(new ForgotPage()));
                UserDialogs.Instance.HideLoading();
            };
            lblforgot.GestureRecognizers.Add(TForgot);

            TapGestureRecognizer TSignUp = new TapGestureRecognizer();
            TSignUp.Tapped += async (object sender, EventArgs e) =>
            {
                App.UseIos13FullScreenModal = UseIos13FullScreenModal;
                UserDialogs.Instance.ShowLoading("Loading...", MaskType.Gradient);
                await Navigation.PushModalAsync(new Xamarin.Forms.NavigationPage(new RegistrationPage()));
                UserDialogs.Instance.HideLoading();
            };
            lblSignUp.GestureRecognizers.Add(TSignUp);

            position += 20 + 55;
        

        btnLogin.Clicked += async (sender, e) =>
            {
                try
                {
                    var networkConnection = DependencyService.Get<INetworkConnection>();
                    networkConnection.CheckNetworkConnection();
                    var networkStatus = networkConnection.IsConnected ? "Connected" : "Not Connected";
                    if (networkStatus == "Connected")
                    {
                        if (ViewModel.CanLogin())
                        {
                            await ViewModel.Login();
                        }

                    }
                    else
                    {
                        UserDialogs.Instance.Alert("Please check your internet connection.", "Login", "OK");
                    }
                }
                catch (System.Exception ex)
                {
                    MyController.ErrorManagement(ex.Message);
                }
            };

        }


       
    }
}

