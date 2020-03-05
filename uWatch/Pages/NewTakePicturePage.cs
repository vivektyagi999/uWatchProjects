using System;
using System.IO;
using System.Threading.Tasks;
using Acr.UserDialogs;
using FFImageLoading.Forms;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using uWatch.Controls;
using uWatch.ViewModels;
using UwatchPCL;
using UwatchPCL.Helpers;
using UwatchPCL.Model;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;
#if __ANDROID__
using uWatch.Droid;
#endif
//using Android.Graphics;

namespace uWatch
{
    public class NewTakePicturePage : ContentPage
    {
        byte[] data;

        public string imgFromCam;
        public string BarCodeFormat = "";
        public TakePictureViewModel ViewModel { get; set; }
        private DeviceAssetsModel Asset = new DeviceAssetsModel();
        public bool IsRotate, IsExpiryDateSelected = false, IsPurchaseDateSelected = false;
        public int RotationAngle;
        public NewTakePicturePage(TakePictureViewModel model, string ImageFromCamInAndroid)
        {
            try
            {
                Title = "Asset Details";
                MyController.fromAssetsToGallery = true;
                this.ViewModel = model;
                imgFromCam = ImageFromCamInAndroid;
                BindingContext = ViewModel;
                SetLayout();
            }
            catch (Exception ex)
            {
            }
        }
        private async void SetLayout()
        {
            var PurchageDateFrame = new Frame { HasShadow = false, Padding = new Thickness(5, 0, 0, 0), OutlineColor = Color.FromRgb(187, 187, 187) };
            var LayoutDatePickerPurchageDate = new StackLayout { Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.StartAndExpand, HorizontalOptions = LayoutOptions.StartAndExpand };
            var purchasedateimage = new Image { Source = "calendarimage.png", HeightRequest = 20, WidthRequest = 20 };
            var DatePickerPurchageDate = new NullableDatePicker { PlaceHolder = "Purchase Date", WidthRequest = MyController.ScreenWidth / 2 - 50, HeightRequest = 40, Format = "dd-MMM-yyy" };
            LayoutDatePickerPurchageDate.Children.Add(purchasedateimage);
            LayoutDatePickerPurchageDate.Children.Add(DatePickerPurchageDate);
            var layoutPurchage = new StackLayout { HorizontalOptions = LayoutOptions.StartAndExpand };
            PurchageDateFrame.Content = LayoutDatePickerPurchageDate;
            layoutPurchage.Children.Add(PurchageDateFrame);

            var ExpiryDateFrame = new Frame { HasShadow = false, Padding = new Thickness(5, 0, 0, 0), OutlineColor = Color.FromRgb(187, 187, 187) };
            var LayoutDatePickerExpiryDate = new StackLayout { Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.StartAndExpand, HorizontalOptions = LayoutOptions.StartAndExpand };
            var expirydateimage = new Image { Source = "calendarimage.png", HeightRequest = 20, WidthRequest = 20 };
            var DateExpiryDate = new NullableDatePicker { PlaceHolder = "Warranty Expiry", WidthRequest = MyController.ScreenWidth / 2 - 50, HeightRequest = 40, Format = "dd-MMM-yyy" };
            var layoutExpiry = new StackLayout { HorizontalOptions = LayoutOptions.EndAndExpand };
            LayoutDatePickerExpiryDate.Children.Add(expirydateimage);
            LayoutDatePickerExpiryDate.Children.Add(DateExpiryDate);
            ExpiryDateFrame.Content = LayoutDatePickerExpiryDate;
            layoutExpiry.Children.Add(ExpiryDateFrame);

            var layoutMainDate = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand };
            layoutMainDate.Children.Add(layoutPurchage);
            layoutMainDate.Children.Add(layoutExpiry);

            var layoutkeyword = new StackLayout { Spacing = 2 };
            var lblkeyword = new Label { Text = "Keyword", FontSize = 18, TextColor = Color.Gray };
            var lblkeywordHide = new Label { Text = "Tap me to hide the keyboard", IsVisible = false, FontSize = 16, TextColor = Color.Red, FontAttributes = FontAttributes.Italic };
            var Editorkeyword = new ExtendedEditor { HeightRequest = 40 };
            Editorkeyword.Behaviors.Add(new KeywordValidator());
            Editorkeyword.SetBinding(ExtendedEditor.TextProperty, new Binding("Keyword", BindingMode.TwoWay));

            layoutkeyword.Children.Add(lblkeyword);
            layoutkeyword.Children.Add(lblkeywordHide);
            layoutkeyword.Children.Add(Editorkeyword);

            var layoutBarCode = new StackLayout { Spacing = 2, Padding = new Thickness(0, 0, 4, 0) };
            var lblBarCode = new Label { Text = "Scan or enter serial number", FontSize = 18, TextColor = Color.Black };
            var layoutBarCodeEditor = new StackLayout { Spacing = 5, Orientation = StackOrientation.Horizontal };
            var EditorBarCode = new ExtendedEditor { HeightRequest = 50, HorizontalOptions = LayoutOptions.FillAndExpand };
            EditorBarCode.SetBinding(ExtendedEditor.TextProperty, new Binding("BarCode", BindingMode.TwoWay));
            EditorBarCode.Behaviors.Add(new KeywordValidator());
            var btnScan = new Button { Image = "barcode.png", BorderColor = Color.Gray, BorderWidth = 1, WidthRequest = 50, HeightRequest = 50, BackgroundColor = Color.White };
            var imgScan = new Image { Source = "barcode.png", Aspect = Aspect.AspectFit };
            var tapGestureRecognizer = new TapGestureRecognizer();
            btnScan.Clicked += (s, e) =>
            {
                var scanner = new ZXingScannerPage();
                scanner.AutoFocus();
                Navigation.PushAsync(scanner);
                scanner.OnScanResult += (ZXing.Result result) =>
                {
                    Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
                    {
                        await Navigation.PopAsync();
                        EditorBarCode.Text = result.Text;
                        BarCodeFormat = result.BarcodeFormat.ToString();
                    });
                };
            };
            imgScan.GestureRecognizers.Add(tapGestureRecognizer);
            layoutBarCodeEditor.Children.Add(EditorBarCode);
            layoutBarCodeEditor.Children.Add(btnScan);
            layoutBarCode.Children.Add(lblBarCode);
            layoutBarCode.Children.Add(layoutBarCodeEditor);

            var layoutLocation = new StackLayout { Orientation = StackOrientation.Horizontal, Spacing = 2, Padding = new Thickness(0, 10, 0, 0) };
            var locationCheckBox = new Image { Source = "Checkbox.png", WidthRequest = 25, HeightRequest = 25 };
            var locationText = new Label { Text = "Include GPS location ", FontSize = 18, TextColor = Color.Black };
            layoutLocation.Children.Add(locationText);
            layoutLocation.Children.Add(locationCheckBox);


            var layoutIsnewItem = new StackLayout { Orientation = StackOrientation.Horizontal, Spacing = 2 };
            var IsnewCheckBox = new Image { Source = "Checkbox.png", WidthRequest = 20, HeightRequest = 20 };
            Settings.IsNewItem = false;
            var IsnewlalText = new Label { Text = "New asset ?", FontSize = 18, TextColor = Color.Gray };
            layoutIsnewItem.Children.Add(IsnewlalText);
            layoutIsnewItem.Children.Add(IsnewCheckBox);

            TapGestureRecognizer locationTap = new TapGestureRecognizer();
            locationTap.Tapped += async (object sender, EventArgs e) =>
            {
                if ((locationCheckBox.Source as FileImageSource).File == "CheckBoxTick.png")
                {
                    TakePictureViewModel.isLocation = false;
                    locationCheckBox.Source = ImageSource.FromFile("Checkbox.png");
                    //Settings.IsNewItem = false;
                }
                else
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
                            locationCheckBox.Source = ImageSource.FromFile("CheckBoxTick.png");
                            TakePictureViewModel.isLocation = true;
                            UserDialogs.Instance.HideLoading();
                            currentlocation.StopLocationServices();
                        }
                        else
                        {
                            MainActivity.Instance.NoLocation();
                            // UserDialogs.Instance.Alert("Location is not Enabled!", "Alert!", "Ok");
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
                    locationCheckBox.Source = ImageSource.FromFile("CheckBoxTick.png");
                    TakePictureViewModel.isLocation = true;
                    UserDialogs.Instance.HideLoading();

                    //MessagingCenter.Subscribe<uWatch.iOS.CurrentLocation>(this, "LocationServices", (senders) =>
                    //{
                    //    MessagingCenter.Unsubscribe<uWatch.iOS.CurrentLocation>(this, "LocationServices");
                    //    UserDialogs.Instance.HideLoading();
                    //    locationCheckBox.Source = ImageSource.FromFile("CheckBoxTick.png");
                    //    TakePictureViewModel.isLocation = true;
                    //});
#endif
                    // Settings.IsNewItem = true;
                }
            };
            locationCheckBox.GestureRecognizers.Add(locationTap);


            TapGestureRecognizer TNewItem = new TapGestureRecognizer();
            TNewItem.Tapped += (object sender, EventArgs e) =>
            {
                if ((IsnewCheckBox.Source as FileImageSource).File == "CheckBoxTick.png")
                {
                    IsnewCheckBox.Source = ImageSource.FromFile("Checkbox.png");
                    Settings.IsNewItem = false;
                }
                else
                {
                    IsnewCheckBox.Source = ImageSource.FromFile("CheckBoxTick.png");
                    Settings.IsNewItem = true;
                }
            };
            IsnewCheckBox.GestureRecognizers.Add(TNewItem);
            DatePickerPurchageDate.DateSelected += (object sender, DateChangedEventArgs e) =>
            {
                IsPurchaseDateSelected = true;
                DateExpiryDate.MinimumDate = DatePickerPurchageDate.Date;
                Asset.strPurchaseDate = DateFormat.GetDateTime(DatePickerPurchageDate.NullableDate, TimeType.OnlyDate);
            };
            DateExpiryDate.DateSelected += (object sender, DateChangedEventArgs e) =>
            {
                IsExpiryDateSelected = true;
                DateExpiryDate.Date = DateExpiryDate.Date;
                Asset.strExpireDate = DateFormat.GetDateTime(DateExpiryDate.NullableDate, TimeType.OnlyDate);
            };
            var layoutDescription = new StackLayout { Spacing = 2, VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand };
            var lblDesc = new Label { Text = "Description", FontSize = 18, TextColor = Color.Black };
            var lblHidekeyboard = new Label { Text = "Tap outside to hide the keyboard", FontSize = 16, IsVisible = false, FontAttributes = FontAttributes.Italic, TextColor = Color.Red };
            var EditorDesc = new ExtendedEditor { HeightRequest = 90 };
            layoutDescription.Children.Add(lblDesc);
            layoutDescription.Children.Add(lblHidekeyboard);
            layoutDescription.Children.Add(EditorDesc);


            EditorDesc.Focused += (sender, e) =>
            {
                lblHidekeyboard.IsVisible = true;

            };
            EditorDesc.Unfocused += (sender, e) =>
            {
                lblHidekeyboard.IsVisible = false;

            };
            Editorkeyword.Focused += (sender, e) =>
            {
                lblkeywordHide.IsVisible = true;

            };
            Editorkeyword.Unfocused += (sender, e) =>
            {
                lblkeywordHide.IsVisible = false;

            };
            TapGestureRecognizer HideKeyboard = new TapGestureRecognizer();
            HideKeyboard.Tapped += async (object sender, EventArgs e) =>
            {
                EditorDesc.Unfocus();
            };
            lblHidekeyboard.GestureRecognizers.Add(HideKeyboard);
            TapGestureRecognizer HideKeywordKeyboard = new TapGestureRecognizer();
            HideKeywordKeyboard.Tapped += async (object sender, EventArgs e) =>
            {
                Editorkeyword.Unfocus();
            };
            lblkeywordHide.GestureRecognizers.Add(HideKeywordKeyboard);
            var layoutImageContent = new Grid { ColumnSpacing = 5, Padding = new Thickness(0, 40, 0, 0), HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
            layoutImageContent.ColumnDefinitions = new ColumnDefinitionCollection
            {
                    new ColumnDefinition { Width=30  },
                    new ColumnDefinition { Width=GridLength.Star },
                    new ColumnDefinition { Width=30 },

            };


            var imgLeft = new Image { Source = "rotate_left.png", HeightRequest = 25, WidthRequest = 25, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };

            var imgAssets = new CachedImage { VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand, Margin = new Thickness(0, 30, 0, 10), Aspect = Aspect.AspectFit };
            var imgRight = new Image { Source = "rotate_right.png", HeightRequest = 25, WidthRequest = 25, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };

            if (ViewModel.Picture != null)
            {
                try
                {

                    var imageOfAssets = DependencyService.Get<IPicture>().GetPictureFromDiskTemp(0, ViewModel.CountAsset);
#if __ANDROID__
                    imgAssets.Source = ImageSource.FromStream(() => new MemoryStream(imageOfAssets));
                    if (TakePictureViewModel.RotationAngle == 6)
                    {
                        imgAssets.Rotation += 90;
                        Asset.RotationAngle += 1;
                    }
                    else if (TakePictureViewModel.RotationAngle == 3)
                    {
                        imgAssets.Rotation += 180;
                        Asset.RotationAngle += 2;
                    }
                    else if (TakePictureViewModel.RotationAngle == 8)
                    {
                        imgAssets.Rotation += 270;
                        Asset.RotationAngle += 3;
                    }

#endif


#if __IOS__
                    imgAssets.SetBinding(CachedImage.SourceProperty, "Picture");
                   // imgAssets.SetBinding(Image.SourceProperty, new Binding("Picture", BindingMode.TwoWay));
#endif

                    layoutImageContent.Children.Add(imgLeft, 0, 0);
                    layoutImageContent.Children.Add(imgAssets, 1, 0);
                    layoutImageContent.Children.Add(imgRight, 2, 0);


                    TapGestureRecognizer Tapleft = new TapGestureRecognizer();
                    Tapleft.Tapped += (object sender, EventArgs e) =>
                    {
                        try
                        {
                            IsRotate = true;
                            imgAssets.Rotation -= 90;
                            RotationAngle -= 90;
                        }
                        catch { }
                    };
                    imgLeft.GestureRecognizers.Add(Tapleft);


                    TapGestureRecognizer TapRight = new TapGestureRecognizer();
                    TapRight.Tapped += (object sender, EventArgs e) =>
                    {
                        try
                        {
                            IsRotate = true;
                            imgAssets.Rotation += 90;
                            RotationAngle += 90;
                        }
                        catch (System.Exception ex)
                        {
                        }

                    };
                    imgRight.GestureRecognizers.Add(TapRight);


                    var layoutButton = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.EndAndExpand };
                    var btnSave = new Button { Text = "   Save   ", FontSize = 18, WidthRequest = MyController.ScreenWidth / 2 - 40, BackgroundColor = Color.Red, TextColor = Color.White };
                    var btnCancel = new Button { Text = "Cancel", FontSize = 18, WidthRequest = MyController.ScreenWidth / 2 - 40, BackgroundColor = Color.FromRgb(123, 123, 123), TextColor = Color.White };

                    layoutButton.Children.Add(btnSave);
                    layoutButton.Children.Add(btnCancel);
                    btnCancel.Clicked += (sender, e) =>
                    {

                        Navigation.PopAsync();
                    };

                    btnSave.Clicked += async (object sender, EventArgs e) =>
                    {
                        if (Editorkeyword.TextColor == Color.Red)
                        {
                            UserDialogs.Instance.Alert("KeyWord accepts single word only.", "Alert", "OK");
                            return;
                        }
                        int saveimg = 0;
                        var networkConnection = DependencyService.Get<INetworkConnection>();
                        networkConnection.CheckNetworkConnection();
                        var networkStatus = networkConnection.IsConnected ? "Connected" : "Not Connected";
                        if (networkStatus != "Connected")
                        {
                            UserDialogs.Instance.Alert("Please check your internet connection.", "Alert", "OK");
                            return;
                        }

                        if (true)
                        {

                            if (Asset.Deviceimage == null)
                                Asset.Deviceimage = DependencyService.Get<IPicture>().GetPictureFromDisk(0, ViewModel.CountAsset);

                            //string img = Convert.ToBase64String(Asset.Receiptimage);

                            Asset.Device_id = 0;
                            Asset.Deviceasset_idx = 0;
                            Asset.geo_coords = " ";
                            if (IsRotate)
                            {

                                Asset.RotationAngle += (RotationAngle / 90);
                            }
                            else
                            {
                                if (ViewModel.FromCameraCap)
                                {
                                    Asset.RotationAngle += 1;
                                }
                                else
                                {

                                    Asset.RotationAngle += (RotationAngle / 90);
                                }

                            }
#if __ANDROID__
                            Asset.ViewRotation = TakePictureViewModel.RotationAngle;
#endif
                            if (TakePictureViewModel.isLocation)
                            {
                                Asset.Latitude = Convert.ToString(LocationServices.Latitude);
                                Asset.Longitude = Convert.ToString(LocationServices.Longitude);
                            }
                            else
                            {
                                Asset.Latitude = "";
                                Asset.Longitude = "";
                            }

                            Asset.OwnerUserID = Settings.UserID;
                            // Asset.friendly_name = ViewModel.Device.FriendlyName;
                            Asset.description = EditorDesc.Text;
                            Asset.strCreatedDate = System.DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss");
                            Asset.strModifyDate = System.DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss");
                            Asset.Keyword = Editorkeyword.Text;
                            Asset.SerialNumber = EditorBarCode.Text;
                            Asset.BarCodeFormat = BarCodeFormat;
                            Asset.IsNew = Settings.IsNewItem;

                            IsRotate = false;

                            var confirmDialog = await UserDialogs.Instance.ConfirmAsync("Add receipt image?", "Receipt", "Yes", "No");
                            if (confirmDialog)
                            {
                                var resultFile = await PlatformHelper.CameraFuncAsync();
                                if (resultFile != null)
                                {
                                    Asset.Receiptimage = resultFile;
                                }
                            }

                            UserDialogs.Instance.HideLoading();
                            UploadAssetsAsync(Asset);
                        }

                    };
                    var stacklayoutscroll = new StackLayout { };
                    var scrl = new ScrollView { Orientation = ScrollOrientation.Vertical, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
                    var stacklayoutMain = new StackLayout { Spacing = 8, Padding = 10, VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand };
                    // stacklayoutscroll.Children.Add(layoutMainDate);
                    // stacklayoutscroll.Children.Add(layoutkeyword);
                    stacklayoutscroll.Children.Add(layoutBarCode);
                    stacklayoutscroll.Children.Add(layoutLocation);
                    // stacklayoutscroll.Children.Add(layoutIsnewItem);
                    stacklayoutscroll.Children.Add(layoutDescription);
                    stacklayoutscroll.Children.Add(layoutImageContent);
                    stacklayoutMain.Children.Add(stacklayoutscroll);
                    stacklayoutMain.Children.Add(layoutButton);
                    Content = stacklayoutMain;
                }
                catch (System.Exception ex)
                {
                    UserDialogs.Instance.HideLoading();
                }
            }
        }

        private async void UploadAssetsAsync(DeviceAssetsModel Asset)
        {

            UserDialogs.Instance.ShowLoading("Uploading Asset...");

            var saveimg = await ApiService.Instance.SaveAsset(Asset);
#if __IOS__
            UserDialogs.Instance.HideLoading();
#endif
            if (saveimg > 0)
            {
                LocationServices.Latitude = 0;
                LocationServices.Longitude = 0;
                DependencyService.Get<IPicture>().ReleaseMemory("", true);
                await UserDialogs.Instance.AlertAsync("Asset Uploaded Successfully", "Asset upload", "OK");
            }
            else
            {
                // UserDialogs.Instance.HideLoading();
                UserDialogs.Instance.Alert("There was an error communicating with the server. Please try again!", "Server Error", "Ok");
            }
            System.GC.Collect();
            MyController.fromAssetsToGallery = true;
            (App.Current.MainPage as MasterDetailPage).Detail = new NavigationPage(new AssetsPage());

        }

        protected override bool OnBackButtonPressed()
        {
            try
            {
                System.GC.Collect();
                MyController.fromAssetsToGallery = true;

            }
            catch { }
            return base.OnBackButtonPressed();
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
        protected override void OnDisappearing()
        {
            TakePictureViewModel.isLocation = false;
            base.OnDisappearing();
        }
    }


}


