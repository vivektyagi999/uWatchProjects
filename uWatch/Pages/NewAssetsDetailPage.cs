using System;
using System.Threading.Tasks;
using Acr.UserDialogs; 
using FFImageLoading.Forms;
using uWatch.Controls;
using uWatch.ViewModels;
using UwatchPCL;
using UwatchPCL.Helpers;
using UwatchPCL.Model;
using UwatchPCL.WebServices;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace uWatch
{
    public class NewAssetsDetailPage : ContentPage
    {
        public AssetDetailsViewModel ViewModel { get; set; }
        public DeviceAssetsModel DeviceAsset; bool IsExpiryDateSelected, IsPurchaseDateSelected = false;
        public DeviceAssetsModel TempDeviceAsset;
        public string ReceiptImage;
        WebServiceManager webserviceManager;
        public string BarCodeFormat
        {
            get;
            set;
        }
        public NewAssetsDetailPage(int position, DeviceAssetsModel selectedImageModel, AssetDetailsViewModel viewmodel)
        {
            try
            {
                DeviceAsset = selectedImageModel;
                TempDeviceAsset = selectedImageModel;
                Title = "Asset Details";
                this.ViewModel = viewmodel;
                BindingContext = ViewModel.DeviceAsset;
                BarCodeFormat = ViewModel.DeviceAsset.BarCodeFormat;
                webserviceManager = new WebServiceManager();
                SetLayout();
            }
            catch { }

        }
        private async void SetLayout()
        {

            try
            {
                var PurchageDateFrame = new Frame { HasShadow = false, Padding = new Thickness(5, 0, 0, 0), OutlineColor = Color.FromRgb(187, 187, 187) };
                var LayoutDatePickerPurchageDate = new StackLayout { Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.StartAndExpand, HorizontalOptions = LayoutOptions.StartAndExpand };
                var purchasedateimage = new Image { Source = "calendarimage.png", HeightRequest = 20, WidthRequest = 20 };
                var DatePickerPurchageDate = new NullableDatePicker { PlaceHolder = "Purchase Date", WidthRequest = MyController.ScreenWidth / 2 - 50, HeightRequest = 40, Format = "dd-MMM-yyy" };

                if (ViewModel.DeviceAsset.PurchaseDate != null)
                {
                    var date = ((DateTime)ViewModel.DeviceAsset.PurchaseDate).ToString("D");
                    DatePickerPurchageDate.NullableDate = Convert.ToDateTime(DateFormat.GetDateTime(ViewModel.DeviceAsset.PurchaseDate, TimeType.OnlyDate));
                }

                LayoutDatePickerPurchageDate.Children.Add(purchasedateimage);
                LayoutDatePickerPurchageDate.Children.Add(DatePickerPurchageDate);
                var layoutPurchage = new StackLayout { HorizontalOptions = LayoutOptions.StartAndExpand };
                PurchageDateFrame.Content = LayoutDatePickerPurchageDate;
                layoutPurchage.Children.Add(PurchageDateFrame);


                var ExpiryDateFrame = new Frame { HasShadow = false, Padding = new Thickness(5, 0, 0, 0), OutlineColor = Color.FromRgb(187, 187, 187) };
                var LayoutDatePickerExpiryDate = new StackLayout { Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.StartAndExpand, HorizontalOptions = LayoutOptions.StartAndExpand };
                var expirydateimage = new Image { Source = "calendarimage.png", HeightRequest = 20, WidthRequest = 20 };
                var DateExpiryDate = new NullableDatePicker { PlaceHolder = "Warranty Expiry", WidthRequest = MyController.ScreenWidth / 2 - 50, HeightRequest = 40, Format = "dd-MMM-yyy" };
                if (ViewModel.DeviceAsset.ExpireDate != null)
                {
                    IsExpiryDateSelected = true;

                    DateExpiryDate.NullableDate = Convert.ToDateTime(ViewModel.DeviceAsset.ExpireDate);
                    DateExpiryDate.MinimumDate = Convert.ToDateTime(ViewModel.DeviceAsset.PurchaseDate);
                }
                var layoutExpiry = new StackLayout { HorizontalOptions = LayoutOptions.EndAndExpand };
                LayoutDatePickerExpiryDate.Children.Add(expirydateimage);
                LayoutDatePickerExpiryDate.Children.Add(DateExpiryDate);
                ExpiryDateFrame.Content = LayoutDatePickerExpiryDate;
                layoutExpiry.Children.Add(ExpiryDateFrame);

                DatePickerPurchageDate.DateSelected += (object sender, DateChangedEventArgs e) =>
                {
                    DateExpiryDate.MinimumDate = DatePickerPurchageDate.Date;
                    ViewModel.DeviceAsset.strPurchaseDate = DateFormat.GetDateTime(DatePickerPurchageDate.NullableDate, TimeType.OnlyDate);
                };
                DateExpiryDate.DateSelected += (object sender, DateChangedEventArgs e) =>
                {
                    IsExpiryDateSelected = true;
                    ViewModel.DeviceAsset.strExpireDate = DateFormat.GetDateTime(DateExpiryDate.NullableDate, TimeType.OnlyDate);
                };
                var layoutMainDate = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand };
                layoutMainDate.Children.Add(layoutPurchage);
                layoutMainDate.Children.Add(layoutExpiry);

                var layoutkeyword = new StackLayout { Spacing = 2 };
                var lblkeyword = new Label { Text = "Keyword", FontSize = 18, TextColor = Color.Gray };
                var lblkeywordHide = new Label { Text = "Tap outside to hide the keyboard", IsVisible = false, FontSize = 16, TextColor = Color.Red, FontAttributes = FontAttributes.Italic };
                var Editorkeyword = new ExtendedEditor { HeightRequest = 40 };
                Editorkeyword.SetBinding(ExtendedEditor.TextProperty, new Binding("Keyword", BindingMode.TwoWay));
                Editorkeyword.Behaviors.Add(new KeywordValidator());
                layoutkeyword.Children.Add(lblkeyword);
                layoutkeyword.Children.Add(lblkeywordHide);
                layoutkeyword.Children.Add(Editorkeyword);

                var layoutBarCode = new StackLayout { Spacing = 6 };
                var lblBarCode = new Label { Text = "Scan or enter serial number", FontSize = 18, TextColor = Color.Black };
                var layoutBarCodeEditor = new StackLayout { Spacing = 10, Orientation = StackOrientation.Horizontal };
                var EditorBarCode = new ExtendedEditor { HeightRequest = 50, HorizontalOptions = LayoutOptions.FillAndExpand };
                EditorBarCode.SetBinding(ExtendedEditor.TextProperty, new Binding("SerialNumber", BindingMode.TwoWay));
                var btnScan = new Button { Image = "barcode.png", BorderColor = Color.Gray, BorderWidth = 0.6f, WidthRequest = 50, HeightRequest = 50, BackgroundColor = Color.White };
                btnScan.Clicked += (s, e) =>
                {
                    var scanner = new ZXingScannerPage();
                    scanner.AutoFocus();
                    Navigation.PushAsync(scanner);
                    scanner.OnScanResult += (ZXing.Result resultBarCode) =>
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await Navigation.PopAsync();
                            EditorBarCode.Text = resultBarCode.Text;
                            BarCodeFormat = resultBarCode.BarcodeFormat.ToString();
                        });
                    };

                };
                EditorBarCode.Behaviors.Add(new KeywordValidator());
                var imgScan = new Image { Source = "barcode.png", Aspect = Aspect.AspectFit };
                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += (s, e) =>
                {
                    var scanner = new ZXingScannerPage();
                    scanner.AutoFocus();
                    Navigation.PushAsync(scanner);
                    scanner.OnScanResult += (ZXing.Result resultBarCode) =>
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await Navigation.PopAsync();
                            EditorBarCode.Text = resultBarCode.Text;
                            BarCodeFormat = resultBarCode.BarcodeFormat.ToString();
                        });
                    };
                };
                imgScan.GestureRecognizers.Add(tapGestureRecognizer);
                layoutBarCodeEditor.Children.Add(EditorBarCode);
                layoutBarCodeEditor.Children.Add(btnScan);
                layoutBarCode.Children.Add(lblBarCode);
                layoutBarCode.Children.Add(layoutBarCodeEditor);

                var layoutIsnewItem = new StackLayout { Orientation = StackOrientation.Horizontal, Spacing = 2 };
                var IsnewCheckBox = new Image { Source = "CheckBoxTick.png", WidthRequest = 35, HeightRequest = 35 };
                //var IsnewlalText = new Label { Text = "Is new asset", FontSize = 18, TextColor = Color.Gray };
                var IsnewlalText = new Label { Text = "New asset?", FontSize = 18, TextColor = Color.Gray };
                layoutIsnewItem.Children.Add(IsnewlalText);
                layoutIsnewItem.Children.Add(IsnewCheckBox);

                if (ViewModel.DeviceAsset.IsNew == true)
                {
                    IsnewCheckBox.Source = ImageSource.FromFile("CheckBoxTick.png");
                    Settings.IsNewItem = true;
                }
                else
                {
                    IsnewCheckBox.Source = ImageSource.FromFile("Checkbox.png");
                    Settings.IsNewItem = false;
                }
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

                var layoutDescription = new StackLayout { Spacing = 2, HeightRequest = 100 };
                var lblDesc = new Label { Text = "Description", FontSize = 18, TextColor = Color.Black };
                var lblHide = new Label { Text = "Tap outside to hide the keyboard", IsVisible = false, FontSize = 16, TextColor = Color.Red, FontAttributes = FontAttributes.Italic };
                var EditorDesc = new ExtendedEditor { HeightRequest = 100 };
                EditorDesc.SetBinding(ExtendedEditor.TextProperty, new Binding("description", BindingMode.TwoWay));
                layoutDescription.Children.Add(lblDesc);

                layoutDescription.Children.Add(lblHide);
                layoutDescription.Children.Add(EditorDesc);

                Editorkeyword.Focused += (sender, e) =>
                {
                    lblkeywordHide.IsVisible = true;

                };
                Editorkeyword.Unfocused += (sender, e) =>
                {
                    lblkeywordHide.IsVisible = false;

                };

                EditorDesc.Focused += (sender, e) =>
                {
                    lblHide.IsVisible = true;

                };
                EditorDesc.Unfocused += (sender, e) =>
                {
                    lblHide.IsVisible = false;

                };

                TapGestureRecognizer HideKeyboardkeyword = new TapGestureRecognizer();
                HideKeyboardkeyword.Tapped += async (object sender, EventArgs e) =>
                {
                    Editorkeyword.Unfocus();
                };
                lblkeywordHide.GestureRecognizers.Add(HideKeyboardkeyword);

                TapGestureRecognizer HideKeyboard = new TapGestureRecognizer();
                HideKeyboard.Tapped += async (object sender, EventArgs e) =>
                {
                    EditorDesc.Unfocus();
                };
                lblHide.GestureRecognizers.Add(HideKeyboard);

                var layoutImageContent = new StackLayout { Spacing = 8, Padding = new Thickness(0, 0, 0, 0), Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
                var imgLeft = new Image { Source = "rotate_left.png", HeightRequest = 25, WidthRequest = 25, HorizontalOptions = LayoutOptions.StartAndExpand };
                var imgAssets = new CachedImage { HeightRequest = MyController.ScreenWidth / 2, WidthRequest = MyController.ScreenWidth / 2, HorizontalOptions = LayoutOptions.FillAndExpand };
                var imgRight = new Image { Source = "rotate_right.png", HeightRequest = 25, WidthRequest = 25, HorizontalOptions = LayoutOptions.EndAndExpand };
                var indicator = new ActivityIndicator { Color = Xamarin.Forms.Color.Red, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };

                System.Uri uri;
                System.Uri.TryCreate(ViewModel.DeviceAsset.UploadedFilePath, UriKind.Absolute, out uri);
                Task<ImageSource> result = Task<ImageSource>.Factory.StartNew(() => ImageSource.FromUri(uri));
                imgAssets.Source = await result;

                indicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsLoading");

                var relativelayout = new RelativeLayout { };
                relativelayout.Children.Add(imgAssets,
                 Constraint.RelativeToParent((parent) =>
                 {
                     return 10;
                 }),
                    Constraint.RelativeToParent((parent) =>
                    {
                        return 50;
                    }),
                    Constraint.RelativeToParent((parent) =>
                    {
                        return MyController.ScreenWidth - 100;
                    }),
                    Constraint.RelativeToParent((parent) =>
                    {
                        return MyController.ScreenWidth - 130;
                    }));

                relativelayout.Children.Add(indicator,
                    Constraint.RelativeToParent((parent) =>
                    {
                        return parent.Width / 2;
                    }),
                    Constraint.RelativeToParent((parent) =>
                    {
                        return parent.Height / 2;
                    }));


                indicator.BindingContext = imgAssets;

                layoutImageContent.Children.Add(imgLeft);
                layoutImageContent.Children.Add(relativelayout);
                layoutImageContent.Children.Add(imgRight);

                TapGestureRecognizer bigt = new TapGestureRecognizer();
                bigt.Tapped += (object sender, EventArgs e) =>
                {
                    Navigation.PushAsync(new ZoomPage(imgAssets.Source, "Asset Image", ReceiptImage));

                };

                indicator.GestureRecognizers.Add(bigt);
                imgAssets.GestureRecognizers.Add(bigt);


                TapGestureRecognizer Tapleft = new TapGestureRecognizer();
                Tapleft.Tapped += (object sender, EventArgs e) =>
                {
                    imgAssets.Rotation -= 90;
                    ViewModel.DeviceAsset.RotationAngle -= 90;

                };
                imgLeft.GestureRecognizers.Add(Tapleft);


                TapGestureRecognizer TapRight = new TapGestureRecognizer();
                TapRight.Tapped += (object sender, EventArgs e) =>
                {
                    imgAssets.Rotation += 90;
                    ViewModel.DeviceAsset.RotationAngle += 90;

                };
                imgRight.GestureRecognizers.Add(TapRight);


                var layoutButton = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.EndAndExpand };
                var btnSave = new Button { Text = "Save", FontSize = 18, WidthRequest = MyController.ScreenWidth / 3 - 10, BackgroundColor = Color.Red, TextColor = Color.White };
                var btnDelete = new Button { Text = "Delete", FontSize = 18, WidthRequest = MyController.ScreenWidth / 3 - 10, BackgroundColor = Color.Red, TextColor = Color.White };
                var btnCancel = new Button { Text = "Cancel", FontSize = 18, WidthRequest = MyController.ScreenWidth / 3 - 10, BackgroundColor = Color.FromRgb(123, 123, 123), TextColor = Color.White };

                layoutButton.Children.Add(btnSave);
                layoutButton.Children.Add(btnDelete);
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
                    var networkConnection = DependencyService.Get<INetworkConnection>();
                    networkConnection.CheckNetworkConnection();
                    var networkStatus = networkConnection.IsConnected ? "Connected" : "Not Connected";
                    if (networkStatus != "Connected")
                    {
                        UserDialogs.Instance.Alert("Please check your internet connection.", "Alert", "OK");
                        return;
                    }

                    if (DateExpiryDate.Date < DatePickerPurchageDate.Date)
                    {
                        UserDialogs.Instance.Alert("Your expire date should be greater than purchage date..!", "Alert", "OK");
                        return;
                    }
                    DatePickerPurchageDate.IsEnabled = true;
                    DateExpiryDate.IsEnabled = true;
                    EditorDesc.IsEnabled = true;
                    if ((IsnewCheckBox.Source as FileImageSource).File == "CheckBoxTick.png")
                    {
                        if (IsExpiryDateSelected)
                        {
                            ViewModel.DeviceAsset.strExpireDate = DateFormat.GetDateTime(DateExpiryDate.Date, TimeType.OnlyDate);
                        }
                        else
                        {
                            await UserDialogs.Instance.AlertAsync("Please select Warranty Expiry", "Information", "Ok");
                            return;
                        }
                    }

                    ViewModel.DeviceAsset.RotationAngle = ViewModel.DeviceAsset.RotationAngle / 90;
                    ViewModel.DeviceAsset.Device_id = ViewModel.DeviceAsset.Device_id;
                    ViewModel.DeviceAsset.Deviceimage = ViewModel.DeviceAsset.Deviceimage;
                    ViewModel.DeviceAsset.Deviceasset_idx = ViewModel.DeviceAsset.Deviceasset_idx;
                    ViewModel.DeviceAsset.geo_coords = ViewModel.DeviceAsset.geo_coords;
                    ViewModel.DeviceAsset.friendly_name = ViewModel.DeviceAsset.friendly_name;
                    ViewModel.DeviceAsset.description = EditorDesc.Text;
                    ViewModel.DeviceAsset.strCreatedDate = System.DateTime.Now.ToString("dd-MMM-yyyy");
                    ViewModel.DeviceAsset.strModifyDate = System.DateTime.Now.ToString("dd-MMM-yyyy");
                    ViewModel.DeviceAsset.Keyword = Editorkeyword.Text;
                    ViewModel.DeviceAsset.SerialNumber = EditorBarCode.Text;
                    ViewModel.DeviceAsset.BarCodeFormat = BarCodeFormat;
                    ViewModel.DeviceAsset.IsNew = Settings.IsNewItem;
                    var confirmDialog = await UserDialogs.Instance.ConfirmAsync("Add receipt image?", "Receipt", "Yes", "No");
                    if (confirmDialog)
                    {

                        var resultFile = await PlatformHelper.CameraFuncAsync();
                        if (resultFile != null)
                        {
                            ViewModel.DeviceAsset.Receiptimage = resultFile;
                        }

                    }
                    UserDialogs.Instance.HideLoading();

                    UserDialogs.Instance.ShowLoading("Updating Asset...");
                  await  Task.Delay(10000);

                    var saveimg = await ApiService.Instance.SaveAsset(ViewModel.DeviceAsset);

#if __IOS__
                 UserDialogs.Instance.HideLoading();
#endif
                    if (saveimg > 0)
                    {
                       // UserDialogs.Instance.HideLoading();
                        await UserDialogs.Instance.AlertAsync("Asset Updated Successfully", "Asset update", "OK");
                    }
                    else
                    {
                        //UserDialogs.Instance.HideLoading();
                        UserDialogs.Instance.Alert("There was an error communicating with the server. Please try again!", "Server Error", "Ok");
                    }
                    System.GC.Collect();
                    MyController.fromAssetsToGallery = true;
                    (App.Current.MainPage as MasterDetailPage).Detail = new NavigationPage(new AssetsPage());

                };
                btnDelete.Clicked += async (object sender, EventArgs e) =>
                {

                    var networkConnection = DependencyService.Get<INetworkConnection>();
                    networkConnection.CheckNetworkConnection();
                    var networkStatus = networkConnection.IsConnected ? "Connected" : "Not Connected";
                    if (networkStatus != "Connected")
                    {
                        UserDialogs.Instance.Alert("Please check your internet connection.", "Alert", "OK");
                        return;
                    }
                    string address;
                    if (string.IsNullOrEmpty(ReceiptImage))
                    {
                        address = "Are you sure you want to delete this Asset?";
                    }
                    else
                    {
                        address = "Both images will be deleted.Are you sure you want to delete this Asset?";

                    }
                    var answer = await DisplayAlert("Delete Asset", address, "Yes", "No");
                    if (answer == true)
                    {
                        UserDialogs.Instance.ShowLoading("Deleting Asset...");

                        var resu = await ApiService.Instance.DeleteAsset(ViewModel.DeviceAsset);
                        if (resu)
                        {
                            UserDialogs.Instance.HideLoading();
                        }
                        else
                        {
                            UserDialogs.Instance.HideLoading();
                            UserDialogs.Instance.Alert("There was an error communicating with the server. Please try again!", "Server Error", "Ok");
                        }

                        MyController.fromAssetsToGallery = true;
                        (App.Current.MainPage as MasterDetailPage).Detail = new NavigationPage(new AssetsPage());
                    }
                };

                var stacklayoutscroll = new StackLayout { };
                var scrl = new ScrollView { Orientation = ScrollOrientation.Vertical };
                var stacklayoutMain = new StackLayout { Spacing = 8, Padding = 8, VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand };
                // stacklayoutscroll.Children.Add(layoutMainDate);
                // stacklayoutscroll.Children.Add(layoutkeyword);
                stacklayoutscroll.Children.Add(layoutBarCode);
                // stacklayoutscroll.Children.Add(layoutIsnewItem);
                stacklayoutscroll.Children.Add(layoutDescription);
                stacklayoutscroll.Children.Add(layoutImageContent);
                scrl.Content = stacklayoutscroll;
                stacklayoutMain.Children.Add(scrl);
                stacklayoutMain.Children.Add(layoutButton);


                TapGestureRecognizer bigts = new TapGestureRecognizer();
                bigts.Tapped += async (object sender, EventArgs e) =>
                {
                    EditorDesc.Unfocus();
                };
                stacklayoutMain.GestureRecognizers.Add(bigts);

                Content = stacklayoutMain;

                Device.BeginInvokeOnMainThread(async () =>
                {
                    UserDialogs.Instance.ShowLoading();
                    var resultReceipt = await webserviceManager.GetReceiptImageAsync(ViewModel.DeviceAsset.Deviceasset_idx);
                    if (resultReceipt.ErrorCode == 0)
                    {
                        ReceiptImage = resultReceipt.Data;
                        UserDialogs.Instance.HideLoading();
                    }
                    else if (resultReceipt.ErrorCode > 0)
                    {
                        UserDialogs.Instance.Alert(resultReceipt.ErrorMessage, "Alert", "Ok");
                        UserDialogs.Instance.HideLoading();
                    }
                    else if (resultReceipt.ErrorCode < 0)
                    {
                        UserDialogs.Instance.Alert(resultReceipt.ErrorMessage, "Alert", "Ok");
                        UserDialogs.Instance.HideLoading();
                    }
                    else
                    {
                        UserDialogs.Instance.Alert(resultReceipt.ErrorMessage, "Alert", "Ok");
                        UserDialogs.Instance.HideLoading();
                    }
                });
            }
            catch (Exception ex)
            {

            }

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
    }

}


