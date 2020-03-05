using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Acr.UserDialogs;
using UwatchPCL;
using System.Windows.Input;
using System.IO;
using UwatchPCL.Helpers;
using System.Collections.ObjectModel;

#if __IOS__
using Xamarin.Media;
#endif
#if __ANDROID__
using Xamarin.Media;
using Android.Graphics;
using Plugin.Settings;
using Android.Provider;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
#endif

namespace uWatch.ViewModels
{
    public class TakePictureViewModel : BaseViewModel
    {

        public bool FromCameraCap { get; set; }
        public string ImageDirecotryPath;
        public INavigation _navigation { get; set; }
        public static bool isLocation = false;
        public DeviceStatic Device { get; set; }
        public int CountAsset { get; set; }
        public string Assettype { get; set; }

        public static int RotationAngle { get; set; }

        public ImageSource picture;
        public ImageSource Picture
        {
            get { return picture; }
            set
            {
                if (value != null)
                {
                    picture = value;
                    OnPropertyChanged("Picture");
                };
            }
        }
        //private ImageSource picture;
        //public ImageSource Picture
        //{
        //    get
        //    {
        //        return this.picture;
        //    }
        //    set
        //    {
        //        if (Equals(value, this.picture))
        //        {
        //            return;
        //        }
        //        this.picture = value;
        //        OnPropertyChanged();
        //    }
        //}
        public Command FromGallery { get; private set; }
        public Command FromCamera { get; private set; }

        public ICommand TakePicture { get; set; }
        public ICommand TakePictureWithLocation { get; set; }
        public TakePictureViewModel()
        {
            TakePicture = new Command(async () => await ExecuteUserImageSelectionCommand());
            TakePictureWithLocation = new Command(async () => await ExecuteUserImageSelectionCommandWithLocation());
        }

        public async Task<ObservableCollection<DeviceConfig>> FetchConfigurationProfileDetails()
        {
            ObservableCollection<DeviceConfig> AssetList = null;
            try
            {
                DeviceStatic req = new DeviceStatic();
                req.OwnerUserID = UwatchPCL.Helpers.Settings.UserID;
                AssetList = await ApiService.Instance.FetchConfigurationProfileDetails(req).ConfigureAwait(false);
            }
            catch { }
            return AssetList;
        }

        public async Task<bool> ChangeCubeConfigration(int DeviceId, int ProfileId, DateTime CreatedDate)
        {
            SaveConfigsetting config = new SaveConfigsetting();
            config.deviceid = DeviceId;
            config.profileid = ProfileId;
            config.datetime = CreatedDate.ToString("dd-MMM-yyyy hh:mm tt");
            bool AssetList = false;
            try
            {
                AssetList = await ApiService.Instance.ChangeCubeConfigration(config).ConfigureAwait(false);
            }
            catch { }
            return AssetList;
        }

        private async Task TakePictureAsync()
        {

        }

        public static byte[] ReadFully(System.IO.Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }

        }

        public async void OnPhotoReceived(ImageSource file, string imgPath)
        {

            try
            {

                UserDialogs.Instance.ShowLoading("Loading...");
                Picture = file;
                int countAssetPosition;
                if(!string.IsNullOrEmpty(Assettype))
                {
                    countAssetPosition = CountAsset + 1;
                }
                else
                {
                    countAssetPosition = CountAsset;
                }
                if (file != null)
                {
                    await DependencyService.Get<IPicture>().SavePictureToDisk(file, 0, countAssetPosition, RotationAngle);
                }
#if __ANDROID__
                ImageDirecotryPath = imgPath.Substring(0, imgPath.LastIndexOf('/'));
                if (!string.IsNullOrEmpty(ImageDirecotryPath))
                {

                    DependencyService.Get<IPicture>().ReleaseMemory(ImageDirecotryPath, false);

                }
#endif
                if (string.IsNullOrEmpty(Assettype))
                {
                    await _navigation.PushAsync(new NewTakePicturePage(this, imgPath));
                }
                UserDialogs.Instance.HideLoading();


            }
            catch (Exception ex)
            {

                UserDialogs.Instance.HideLoading();

                var a = ex.Message;
            }
        }


        async public Task ExecuteUserImageSelectionCommand()
        {
            try
            {
                if (CountAsset >= Constants.ImageMaxLimit)
                {

                    UserDialogs.Instance.Alert("Maximum quantity of 20 asset images  reached, delete to make room.");

                    return;
                }
                else
                {
                    try
                    {
                        cameraFunc(string.Empty);


                    }
                    catch { }

                }

            }
            catch
            {
            }
        }
        async public Task ExecuteUserImageSelectionCommandWithLocation()
        {
            try
            {
                if (CountAsset >= Constants.ImageMaxLimit)
                {
                    UserDialogs.Instance.Alert("Maximum quantity of 20 asset images  reached, delete to make room.");
                    return;
                }
                else
                {
                    try
                    {
                        cameraFunc(string.Empty);
                    }
                    catch { }

                }
            }
            catch
            {
            }
        }


        // on select of camera
        public async Task cameraFunc(string assettype )
        {

            Assettype = assettype;
#if __IOS__

            FromCameraCap = true;

            var picker = new MediaPicker();
            if (!picker.IsCameraAvailable)
                UserDialogs.Instance.Alert("Camera is not available");
            else
            {
                try
                {

                    var file = await picker.TakePhotoAsync(new StoreCameraMediaOptions
                    {
                        Name = "Photo.jpg ",
                        Directory = "uWatch"
                    });
                    Console.WriteLine(file.Path);
                    var p = ImageSource.FromStream(() => file.GetStream());
                    OnPhotoReceived(p, "");

                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Canceled");
                }

            }
#endif
#if __ANDROID__

            FromCameraCap = false;
            var s = await CheckCameraPermisions();
            if (s)
            {

                var picker = new MediaPicker(Xamarin.Forms.Forms.Context);
                if (!picker.IsCameraAvailable)
                {

                    UserDialogs.Instance.Alert("Camera is not available");
                }
                else
                {
                    //UserDialogs.Instance.ShowLoading("Please Wait...");
                    //await Task.Delay(500);
                    try
                    {


                        var file = await picker.TakePhotoAsync(new StoreCameraMediaOptions { Directory = "uWatch", Name = "Photo.jpg" });

                        var p = ImageSource.FromStream(() => file.GetStream());

                        BitmapFactory.Options options = new BitmapFactory.Options { InJustDecodeBounds = true };
                        BitmapFactory.DecodeFile(file.Path, options);


                        // Now we will load the image and have BitmapFactory resize it for us.
                        //options.InSampleSize = inSampleSize;
                        // Next we calculate the ratio that we need to resize the image by
                        // in order to fit the requested dimensions.
                        int outHeight = options.OutHeight;
                        int outWidth = options.OutWidth;
                        int inSampleSize = 1;



                        options.InJustDecodeBounds = false;
                        Bitmap resizedBitmap = BitmapFactory.DecodeFile(file.Path, options);

                        // Images are being saved in landscape, so rotate them back to portrait if they were taken in portrait
                        Matrix mtx = new Matrix();
                        global::Android.Media.ExifInterface exif = new global::Android.Media.ExifInterface(file.Path);
                        string orientation = exif.GetAttribute(global::Android.Media.ExifInterface.TagOrientation);

                        switch (orientation)
                        {

                            case "6":
                                // portrait


                                mtx.PreRotate(90);
                                mtx.SetRotate(90, resizedBitmap.Width, resizedBitmap.Height);
                                resizedBitmap = Bitmap.CreateBitmap(resizedBitmap, 0, 0, resizedBitmap.Width, resizedBitmap.Height, mtx, true);
                                RotationAngle = Convert.ToInt32(orientation);
                                mtx.Dispose();
                                mtx = null;
                                break;

                            case "8": // portrait


                                mtx.PreRotate(270);
                                mtx.SetRotate(270, resizedBitmap.Width, resizedBitmap.Height);
                                resizedBitmap = Bitmap.CreateBitmap(resizedBitmap, 0, 0, resizedBitmap.Width, resizedBitmap.Height, mtx, true);
                                RotationAngle = Convert.ToInt32(orientation);
                                mtx.Dispose();
                                mtx = null;
                                break;

                            case "3": // portrait


                                mtx.PreRotate(180);
                                mtx.SetRotate(180, resizedBitmap.Width, resizedBitmap.Height);
                                resizedBitmap = Bitmap.CreateBitmap(resizedBitmap, 0, 0, resizedBitmap.Width, resizedBitmap.Height, mtx, true);
                                RotationAngle = Convert.ToInt32(orientation);
                                mtx.Dispose();
                                mtx = null;
                                break;

                            case "1": // landscape

                                break;

                            default:


                                mtx.PreRotate(90);
                                mtx.SetRotate(90, resizedBitmap.Width, resizedBitmap.Height);
                                resizedBitmap = Bitmap.CreateBitmap(resizedBitmap, 0, 0, resizedBitmap.Width, resizedBitmap.Height, mtx, true);
                                RotationAngle = Convert.ToInt32(orientation);
                                mtx.Dispose();
                                mtx = null;
                                break;
                        }
                        UserDialogs.Instance.ShowLoading("Loading...");
                        await Task.Delay(100);

                        OnPhotoReceived(p, file.Path);
                        await Task.Delay(100);
                        UserDialogs.Instance.HideLoading();


                    }
                    catch (Exception ex)
                    {
                        UserDialogs.Instance.HideLoading();
                    }
                }
            }

#endif



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
        // on select of Gallery
        public async void galleryFunc()
        {

            FromCameraCap = false;
#if __IOS__
            var picker = new MediaPicker();
            try
            {

                if (!picker.PhotosSupported)
                {
                    UserDialogs.Instance.HideLoading();
                    UserDialogs.Instance.Alert("Photo Gallery is unavailable");
                }
                else
                {
                    var result = await picker.PickPhotoAsync();
                    var p = ImageSource.FromStream(() => result.GetStream());
                    this.OnPhotoReceived(p, "");
                }

            }
            catch (Exception ex)
            {
                string x = ex.Message;
            }
#endif
#if __ANDROID__
            var picker = new MediaPicker(Forms.Context);
            try
            {
                var s = await CheckCameraPermisions();
                if (s)
                {
                    using (UserDialogs.Instance.Loading("Please Wait..."))
                    {
                        if (!picker.PhotosSupported)
                        {
                            UserDialogs.Instance.HideLoading();
                            UserDialogs.Instance.Alert("Photo Gallery is unavailable");
                        }
                        else
                        {
                            var result = await picker.PickPhotoAsync();
                            var p = ImageSource.FromStream(() => result.GetStream());
                            this.OnPhotoReceived(p, "");

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                string x = ex.Message;
            }
#endif
        }

    }
}