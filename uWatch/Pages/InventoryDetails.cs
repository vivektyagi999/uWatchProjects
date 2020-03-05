using System;
using System.Threading.Tasks;
using Acr.UserDialogs;
using FFImageLoading.Forms;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using UwatchPCL;
using Xamarin.Forms;
using UwatchPCL.Helpers;
using System.Collections.Generic;
using System.IO;

#if __ANDROID__
using Xamarin.Media;
using Android.Graphics;
using Android.Media;
#endif
namespace uWatch
{
    public class InventoryDetails : ContentPage
    {
        public static bool IsCamera;
        public static byte[] CameraPreview;
        public static int RoomId = 0;
        public bool FromCameraCap { get; set; }
        // List<RoomDetails> _roomdetails = new List<RoomDetails>();
        public InventoryDetails(List<RoomDetails> __roomdetails, List<RoomImageModel> _imagelist)
        {
            try
            {
                Title = "Inventory Details";
                InitializeView(__roomdetails, _imagelist);
            }
            catch (Exception ex)
            {

            }
        }
        public void InitializeView(List<RoomDetails> __roomdetails, List<RoomImageModel> _imagelist)
        {
            try
            {
                Xamarin.Forms.StackLayout relativeLayouts = new Xamarin.Forms.StackLayout();
                relativeLayouts.VerticalOptions = LayoutOptions.StartAndExpand;

                //var lblname = new Label { Text = "Inventory Details", TextColor = Color.Black, FontSize = 16, WidthRequest = 75, };
                // var LblDvice = new Label { TextColor = Color.Black, FontSize = 15, VerticalOptions = LayoutOptions.CenterAndExpand };
                var lst = new InfiniteListView();
                lst.HasUnevenRows = true;
                if (__roomdetails.Count > 0)
                {
                    lst.ItemsSource = __roomdetails;
                    lst.ItemTemplate = new DataTemplate(typeof(RoomDetailsCell));
                }

                var devicelayout = new StackLayout { Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand };
                if (__roomdetails.Count > 0)
                {
                    //devicelayout.HeightRequest = App.ScreenHeight / 2 ;
                    devicelayout.VerticalOptions = LayoutOptions.FillAndExpand;
                    devicelayout.Children.Add(lst);
                }
                else
                {
                    var lblnodesc = new Label { Text = "Description not available", TextColor = Xamarin.Forms.Color.Black, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.CenterAndExpand, FontSize = 18 };
                    devicelayout.Children.Add(lblnodesc);
                }

                StackLayout st = new StackLayout() { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };
                st.Children.Add(devicelayout);
                var layoutGllry1 = new StackLayout { Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.StartAndExpand };
                var layoutGllry2 = new StackLayout { Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.StartAndExpand };
                //if (ViewModel != null && ViewModel.AssetsList != null)

                //countAssets = ViewModel.AssetsList.Count;
                //if (ViewModel.ImageList != null)

                if (_imagelist.Count > 0)

                {
                    int CountOfImage = 1;
                    foreach (var item in _imagelist)
                    {
                        var it = new CachedImage()
                        {
                            WidthRequest = App.ScreenWidth / 2 - 10,
                            HeightRequest = App.ScreenWidth / 2 - 10,
                            CacheDuration = TimeSpan.FromDays(30),
                            DownsampleToViewSize = true,
                            RetryCount = 0,
                            RetryDelay = 250,
                            TransparencyEnabled = false,
                            Aspect = Aspect.AspectFit,

                        };
                        var layout = new Xamarin.Forms.RelativeLayout();
                        layout.Padding = 2;
                        layout.Children.Add(it, Constraint.Constant(0), Constraint.Constant(0));
                        layout.BackgroundColor = Xamarin.Forms.Color.White;

                        //var sou = await ModeltoImage(item);
                        //it.Source = sou.Source as ImageSource;
                        //it.Source = "https://tse2.mm.bing.net/th?id=OIP.5y3uHQnzbQVydA-B5YKkVwEsDh&pid=15.1&P=0&w=241&h=182";
                        it.Source = item.RoomImagePath;
                        var frm = new Frame { Padding = 1, HasShadow = false, BackgroundColor = Xamarin.Forms.Color.Silver };
                        frm.Content = it;
                        if (CountOfImage < 5)
                        {
                            layoutGllry1.Children.Add(frm);
                        }
                        //else
                        //{
                        //    layoutGllry2.Children.Add(frm);

                        //}
                        relativeLayouts.Children.Add(st);
                        relativeLayouts.Children.Add(layoutGllry1);
                        //relativeLayouts.Children.Add(layoutGllry2);

                        TapGestureRecognizer bigt = new TapGestureRecognizer();
                        bigt.Tapped += async (object sender, EventArgs e) =>
                            {
                                UserDialogs.Instance.ShowLoading("Loading image");
                                await Task.Delay(500);
                                try
                                {

                                    var image = sender as CachedImage;
                                    var img = image.Source as UriImageSource;

                                    ContentPage p = new ContentPage();
                                    var browser = new CustomWebView();

                                    browser.BackgroundColor = Xamarin.Forms.Color.Black;

                                    string htmlsource = "<style>img{display: inline; height: auto; margin: 0 auto; max-width: 100%;}img-container {position: relative;top: 20%; width:100%; height:300px; overflow:hidden; text-align:center;}img{ max-width:100%;width:100%;vertical-align: middle;}\n</style><html>\n<head>\n</head>\n<body style =\"background-color:black\">\n< div class=\"img-container\"> </div>\n<img src = " + img.Uri + " />\n</body>\n</html>";

                                    var htmlSource = new HtmlWebViewSource();

                                    htmlSource.Html = @htmlsource;

                                    if (Xamarin.Forms.Device.OS != TargetPlatform.iOS)
                                    {
                                        htmlSource.BaseUrl = DependencyService.Get<IBaseUrl>().Get();
                                    }

                                    browser.Source = htmlSource;

                                    Grid grid = new Grid
                                    {
                                        VerticalOptions = LayoutOptions.FillAndExpand,
                                        RowDefinitions = {
                                new RowDefinition { Height = new GridLength (40, GridUnitType.Absolute) },
                                new RowDefinition { Height = new GridLength (1, GridUnitType.Star) }
                               },
                                        ColumnDefinitions = {
                                new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },
                                        }
                                    };


                                    grid.Children.Add(browser, 0, 3, 0, 3);
                                    p.Title = "Inventory Image";
                                    p.Content = grid;
                                    await Navigation.PushAsync(p);
                                    await Task.Delay(500);
                                    UserDialogs.Instance.HideLoading();
                                }
                                catch (Exception ex)
                                {
                                    UserDialogs.Instance.HideLoading();
                                }
                            };
                        it.GestureRecognizers.Add(bigt);

                        CountOfImage++;
                    }

                }
                else
                {
                    var it = new CachedImage()
                    {
                        WidthRequest = App.ScreenWidth / 2 - 10,
                        HeightRequest = App.ScreenWidth / 2 - 10,
                        CacheDuration = TimeSpan.FromDays(30),
                        DownsampleToViewSize = true,
                        RetryCount = 0,
                        RetryDelay = 250,
                        TransparencyEnabled = false,
                        Aspect = Aspect.AspectFit,
                        Source = ImageSource.FromFile("noimage.gif"),
                    };
                    layoutGllry1.Children.Add(it);
                    relativeLayouts.Children.Add(st);
                    relativeLayouts.Children.Add(layoutGllry1);

                }



                var btnAdd = new Xamarin.Forms.Button
                {
                    Text = "   Add New Image   ",
                    BackgroundColor = Xamarin.Forms.Color.Red,
                    TextColor = Xamarin.Forms.Color.White,
                    FontSize = 15,
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };
                btnAdd.Clicked += (object sender, EventArgs e) =>
                  {
                      try
                      {
#if __ANDROID__
                          Device.BeginInvokeOnMainThread(async () =>
                          {

                              var s = await CheckCameraPermisions();
                              if (s)
                              {
                                  cameraFunc();
                              }
                          });
                          
                          
#endif
#if __IOS__
                        cameraFunc();
#endif

                      }
                      catch (Exception ex)
                      {

                      }
                  };
                var btnlayoutInternal = new StackLayout { VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand, Orientation = StackOrientation.Vertical };
                btnlayoutInternal.Children.Add(btnAdd);
                var btnlayout = new StackLayout { VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand };

                btnlayout.Children.Add(btnlayoutInternal);
                var mainlayoutGallery = new StackLayout { Padding = 5, VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand };

                mainlayoutGallery.Children.Add(relativeLayouts);
                mainlayoutGallery.Children.Add(btnlayout);

                Content = mainlayoutGallery;
            }
            catch (Exception ex)
            {

            }
        }



        private ImageSource picture;
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
        public async void Loadaftersucess()
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Redirecting!");
                var result = await ApiService.Instance.GetRoomDetailsList(RoomId.ToString()).ConfigureAwait(true);
                var resultimagelist = await ApiService.Instance.RoomImagesList(RoomId.ToString()).ConfigureAwait(true);
                InitializeView(result, resultimagelist);
                UserDialogs.Instance.HideLoading();
                await Task.Delay(50);
                UserDialogs.Instance.Alert("Inventory image added", "", "Ok");
            }
            catch (Exception ex)
            {

            }
        }
        public async Task OnPhotoReceived()
        {


            try
            {

                Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
                {
                    UserDialogs.Instance.ShowLoading("Loading...");
                    await Task.Delay(500);
                });
                RoomImageModel _RoomImageModel = new RoomImageModel();
                _RoomImageModel.RoomID = RoomId;
                _RoomImageModel.CreatedBy = Settings.UserID;

                if (CameraPreview != null && CameraPreview.Length > 0)
                {
                    _RoomImageModel.RoomImage = CameraPreview;



                    var result = await ApiService.Instance.AddRoomImages(_RoomImageModel);

                    Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                    {
                        UserDialogs.Instance.HideLoading();
                        if (result.ErrorCode == 0)
                        {
                            DependencyService.Get<IPicture>().ReleaseMemory("", true);
                            Loadaftersucess();

                        }
                        else if (result.ErrorCode < 0)
                        {
                            UserDialogs.Instance.Alert(result.ErrorMessage, "", "Ok");
                        }
                        else if (result.ErrorCode > 0)
                        {
                            UserDialogs.Instance.Alert(result.ErrorMessage, "", "Ok");
                        }
                        else
                        {
                            UserDialogs.Instance.Alert("Some error occured please try after some time", "", "Ok");
                        }

                    });
                }
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    UserDialogs.Instance.HideLoading();
                });
            }
            catch (Exception ex)
            {
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    UserDialogs.Instance.HideLoading();
                });
                var a = ex.Message;
            }
        }

        public async void cameraFunc()
        {

            try
            {
                UserDialogs.Instance.ShowLoading("Fetching Image...");
                IsCamera = true;
#if __IOS__

                Device.BeginInvokeOnMainThread(() => DependencyService.Get<ICameraGallery>().CameraMedia());
                UserDialogs.Instance.HideLoading();
                MessagingCenter.Subscribe<string>("CameraImage", "CameraPreview", async (senders) =>
                {
                if (CameraPreview != null&& CameraPreview.Length>0)
                    {
                        //CameraCaptureImage = ImageSource.FromStream(() => new MemoryStream(CameraPreview));

                        //IsCameraCapture = true;
                        await OnPhotoReceived();
                    }
                    else
                    {
                        CameraPreview = null;
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    UserDialogs.Instance.HideLoading();
                });
                       // IsCameraCapture = false;
                    }
                MessagingCenter.Unsubscribe<string>("CameraImage", "CameraPreview");
                });


#endif
#if __ANDROID__
                Device.BeginInvokeOnMainThread(() => DependencyService.Get<ICameraGallery>().CameraMedia());
                MessagingCenter.Subscribe<string>("CameraImage", "CameraPreview", async (senders) =>
                {

                if (CameraPreview != null&& CameraPreview.Length>0)
                    {
                        //CameraCaptureImage = ImageSource.FromStream(() => new MemoryStream(CameraPreview));

                        //IsCameraCapture = true;
                        await OnPhotoReceived();
                    }
                    else
                    {
                        CameraPreview = null;
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    UserDialogs.Instance.HideLoading();
                });
                       // IsCameraCapture = false;
                    }
                MessagingCenter.Unsubscribe<string>("CameraImage", "CameraPreview");
                    
                });
#endif

            }
            catch (Exception ex)
            {

            }




            //#if __IOS__
            ////FromCameraCap = true;

            ////var picker = new Xamarin.Media.MediaPicker();
            ////if (!picker.IsCameraAvailable)
            ////    UserDialogs.Instance.Alert("Camera is not available");
            ////else
            ////{
            ////    try
            ////    {

            ////        var file = await picker.TakePhotoAsync(new Xamarin.Media.StoreCameraMediaOptions
            ////        {
            ////            Name = "Photo.jpg ",
            ////            Directory = "uWatch"
            ////        });
            ////        Console.WriteLine(file.Path);
            ////        var p = ImageSource.FromStream(() => file.GetStream());
            ////        await OnPhotoReceived(p,"");

            ////    }
            ////    catch (OperationCanceledException)
            ////    {
            ////        Console.WriteLine("Canceled");
            ////    }

            ////}
            //#endif
            //            #if __ANDROID__

            //            FromCameraCap = false;
            //            var s = await CheckCameraPermisions();
            //            if (s)
            //            {

            //                var picker = new MediaPicker(Xamarin.Forms.Forms.Context);
            //                if (!picker.IsCameraAvailable)
            //                {

            //                    UserDialogs.Instance.Alert("Camera is not available");
            //                }
            //                else
            //                {
            //                    //UserDialogs.Instance.ShowLoading("Please Wait...");
            //                    //await Task.Delay(500);
            //                    try
            //                    {


            //                        var file = await picker.TakePhotoAsync(new StoreCameraMediaOptions { Directory = "uWatch", Name = "Photo.jpg" });



            //                        var p = ImageSource.FromStream(() => file.GetStream());

            //                        BitmapFactory.Options options = new BitmapFactory.Options { InJustDecodeBounds = true };
            //                        BitmapFactory.DecodeFile(file.Path, options);


            //                        // Now we will load the image and have BitmapFactory resize it for us.
            //                        //options.InSampleSize = inSampleSize;
            //                        // Next we calculate the ratio that we need to resize the image by
            //                        // in order to fit the requested dimensions.
            //                        int outHeight = options.OutHeight;
            //                        int outWidth = options.OutWidth;
            //                        int inSampleSize = 1;
            //                        if (outHeight > 300 || outWidth > 300)
            //                        {
            //                            inSampleSize = outWidth > outHeight
            //                                ? outHeight / 300
            //                                    : outWidth / 300;
            //                        }

            //                        options.InSampleSize = inSampleSize;
            //                        options.InJustDecodeBounds = false;
            //                        Bitmap resizedBitmap = BitmapFactory.DecodeFile(file.Path, options);

            //                        // Images are being saved in landscape, so rotate them back to portrait if they were taken in portrait
            //                        Matrix mtx = new Matrix();
            //                        global::Android.Media.ExifInterface exif = new global::Android.Media.ExifInterface(file.Path);
            //                        string orientation = exif.GetAttribute(global::Android.Media.ExifInterface.TagOrientation);

            //                        switch (orientation)
            //                        {

            //                            case "6":
            //                                // portrait


            //                                mtx.PreRotate(90);
            //                                mtx.SetRotate(90, resizedBitmap.Width, resizedBitmap.Height);
            //                                resizedBitmap = Bitmap.CreateBitmap(resizedBitmap, 0, 0, resizedBitmap.Width, resizedBitmap.Height, mtx, true);
            //                                mtx.Dispose();
            //                                mtx = null;
            //                                break;

            //                            case "8": // portrait


            //                                mtx.PreRotate(270);
            //                                mtx.SetRotate(270, resizedBitmap.Width, resizedBitmap.Height);
            //                                resizedBitmap = Bitmap.CreateBitmap(resizedBitmap, 0, 0, resizedBitmap.Width, resizedBitmap.Height, mtx, true);
            //                                mtx.Dispose();
            //                                mtx = null;
            //                                break;

            //                            case "3": // portrait


            //                                mtx.PreRotate(180);
            //                                mtx.SetRotate(180, resizedBitmap.Width, resizedBitmap.Height);
            //                                resizedBitmap = Bitmap.CreateBitmap(resizedBitmap, 0, 0, resizedBitmap.Width, resizedBitmap.Height, mtx, true);
            //                                mtx.Dispose();
            //                                mtx = null;
            //                                break;

            //                            case "1": // landscape

            //                                break;

            //                            default:


            //                                mtx.PreRotate(90);
            //                                mtx.SetRotate(90, resizedBitmap.Width, resizedBitmap.Height);
            //                                resizedBitmap = Bitmap.CreateBitmap(resizedBitmap, 0, 0, resizedBitmap.Width, resizedBitmap.Height, mtx, true);
            //                                mtx.Dispose();
            //                                mtx = null;
            //                                break;
            //                        }
            //                        // UserDialogs.Instance.HideLoading();
            //                        UserDialogs.Instance.ShowLoading("Loading...");
            //                        await Task.Delay(500);


            //                        OnPhotoReceived(p, file.Path);
            //                        await Task.Delay(1500);
            //                        UserDialogs.Instance.HideLoading();


            //                    }
            //                    catch
            //                    {
            //                        UserDialogs.Instance.HideLoading();
            //                    }
            //                }
            //            }

            //#endif




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
    }
}
