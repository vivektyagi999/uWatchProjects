using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
//using Xamarin.Media;
using Acr.UserDialogs;
using System.IO;
using System.Collections.ObjectModel;
using System.Linq;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using uWatch.ViewModels;



#if __ANDROID__
using Android.Content;
using Android.Media;
using Android.Graphics;
#elif __IOS__
using ELCImagePicker;
using Xamarin.Media;
#endif

namespace uWatch
{
    public class GalleryViewModel : BaseViewModel
    {
        byte[] data;
        public Command FromGallery { get; private set; }
#if __ANDROID__
        Android.Graphics.Bitmap bt;
#endif
#if __IOS__
        private List<AssetResult> mResults = new List<AssetResult>();
#endif

        public Command FromCamera { get; private set; }

        int OrderId;
        OrderImage orderImage;
     //   private readonly IOrderImageRepository orderImageRepository;

        public GalleryViewModel(int orderId)
        {
            try
            {
                List<OrderImage> ordersImageRepository = new List<OrderImage>();
                ObservableCollection<OrderImage> OrdersCollection = new ObservableCollection<OrderImage>(ordersImageRepository.ToList());

               // this.orderImageRepository = new OrderImageRepository();
                OrderId = orderId;

               // var ordersImageRepository = new List<OrderImage>();
          //     this.OrdersCollection = new ObservableCollection<OrderImage>(ordersImageRepository.ToList());
               // OrdersImage = OrdersCollection;
            }
            catch
            {
            }
        }

        // get order imaged by order id
        public void GetOrdersImageByOrderId()
        {
            try
            {
                //var ordersImageRepository = orderImageRepository.GetOrdersImgeByOrderId(OrderId);
                //this.OrdersCollection = new ObservableCollection<OrderImage>(ordersImageRepository.ToList());
                //OrdersImage = OrdersCollection;
            }
            catch
            {
            }
        }

        public ObservableCollection<OrderImage> listOffloorimage { get; private set; }


        public const string PhotoPropertyName = "Photo";
        private string photo;

        public string Photo
        {
            get
            {
                return photo;
            }
            set { SetProperty(ref photo, value, PhotoPropertyName); }
        }

        //for popup to select item on click of photo

        async public void ExecuteUserImageSelectionCommand()
        {
            try
            {
                UserDialogs.Instance.ActionSheet(new ActionSheetConfig()
                    .SetTitle("Add Images")
                    .Add("Camera", () => cameraFunc())
                    .Add("Gallery", () => galleryFunc())
                    .SetCancel("Cancel")
                );
            }
            catch
            {
            }
        }
        // on select of camera
        public async void cameraFunc()
        {
            await Task.Delay(1000);
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
            if (status != PermissionStatus.Granted)
            {
                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Camera))
                {

                }

                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera });
                status = results[Permission.Camera];
            }

            if (status == PermissionStatus.Granted)
            {
#if __IOS__
                var picker = new MediaPicker();
                if (!picker.IsCameraAvailable)
                    UserDialogs.Instance.Alert("Camera is not available");
                else
                {
                    //File folder = new File(Environment.GetFolderPath(Environment.SpecialFolder) + "/OrderImageDirectory");
                    try
                    {
                        var file = await picker.TakePhotoAsync(new StoreCameraMediaOptions
                        {
                            Name = "orderImage.jpg ",
                            Directory = "OrderImageDirectory"
                        });
                        Console.WriteLine(file.Path);
                        OnPhotoReceived(file.Path, null, true);
                    }
                    catch (OperationCanceledException)
                    {
                        Console.WriteLine("Canceled");
                    }
                }
#elif __ANDROID__
                var picker = new MediaPicker(Xamarin.Forms.Forms.Context);
                if (!picker.IsCameraAvailable)
                    UserDialogs.Instance.Alert("Camera is not available");
                else
                {

                    try
                    {
                        var file = await picker.TakePhotoAsync(new StoreCameraMediaOptions
                        {
                            Name = "orderImage.jpg ",
                            Directory = "OrderImageDirectory"

                        });

                        Console.WriteLine(file.Path);
                        OnPhotoReceived(file.Path, null, true);
                    }
                    catch (OperationCanceledException)
                    {
                        Console.WriteLine("Canceled");
                    }
                }
#endif

            }
            else if (status != PermissionStatus.Unknown)
            {

            }


        }
        // on select of Gallery
        public async void galleryFunc()
        {
            await Task.Delay(1000);
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
            if (status != PermissionStatus.Granted)
            {
                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Storage))
                {
                    //await DisplayAlert("Need location", "Gunna need that location", "OK");
                }

                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Storage });
                status = results[Permission.Storage];
            }

            if (status == PermissionStatus.Granted)
            {
#if __IOS__

                var picker = ELCImagePickerViewController.Instance;
                picker.MaximumImagesCount = 15;

                picker.Completion.ContinueWith(t =>
                {
                    picker.BeginInvokeOnMainThread(() =>
                    {
                //dismiss the picker
                picker.DismissViewController(true, null);

                        if (t.IsCanceled || t.Exception != null)
                        {
                        }
                        else
                        {
                    //Util.File.Path = new List<string>();

                    var items = t.Result as List<AssetResult>;
                            foreach (AssetResult item in items)
                            {
                                mResults.Add(item);

                        //Util.File.Path.Add (item.Path);
                    }

                    //MessagingCenter.Send<IMessanger> (this, "PostProject");
                }
                        alert(mResults);

                    });
                });
                var topController = UIKit.UIApplication.SharedApplication.KeyWindow.RootViewController;
                while (topController.PresentedViewController != null)
                {
                    topController = topController.PresentedViewController;
                }


                topController.PresentViewController(picker, true, null);

#elif __ANDROID__
                openGallery();
#endif

            }
            else if (status != PermissionStatus.Unknown)
            {

            }
        }
#if __IOS__
        public void alert(List<AssetResult> data)
        {

            foreach (var item in data)
            {
                using (Foundation.NSData imageData = item.Image.AsJPEG(0.7f))
                {
                    Byte[] myByteArray = new Byte[imageData.Length];
                    System.Runtime.InteropServices.Marshal.Copy(imageData.Bytes, myByteArray, 0, Convert.ToInt32(imageData.Length));
                    OnPhotoReceived("", myByteArray, false);
                }

            }

        }
#endif
        public ObservableCollection<OrderImage> OrdersCollection { get; private set; }

        public static ObservableCollection<OrderImage> OrdersImage;

        private ImageSource imageSource;

        public ImageSource ImageSource
        {
            get
            {
                return imageSource;
            }
            set
            {
                imageSource = value;
            }
        }

        //convert image to binary
        public static byte[] ConvertImageToBinary(string _path)
        {

            //FileStream fS = new FileStream(_path, FileMode.Open, FileAccess.Read);
            byte[] b = new byte[120];
            //fS.Read(b, 0, (int)fS.Length);
            //fS.Close();
            return b;

        }

        //on select of photo
        async private void OnPhotoReceived(string imgPath, Byte[] imgdata, bool fromCamera)
        {
            try
            {
                if(OrdersCollection == null)
                {
                    OrdersCollection = new ObservableCollection<OrderImage>();
                }
#if __IOS__
                if (!string.IsNullOrEmpty(imgPath))
                {
                    UIKit.UIImage image = new UIKit.UIImage(imgPath);

                    UIKit.UIImage highres = image;
                    Foundation.NSData d = highres.AsJPEG(0.7f);
                    UIKit.UIImage lowres = new UIKit.UIImage(d);
                    Byte[] myByteArray = new Byte[d.Length];
                    System.Runtime.InteropServices.Marshal.Copy(d.Bytes, myByteArray, 0, Convert.ToInt32(d.Length));
                    imgdata = myByteArray;
                }
#endif
               if (imgdata == null)
                {
                    data = ConvertImageToBinary(imgPath);
#if __ANDROID__
                    bt = Android.Graphics.BitmapFactory.DecodeByteArray(data, 0, data.Length);
                    //bt = MaxResizeImage (bt, bt.Width, bt.Height);
                    bt = ThumbnailUtils.ExtractThumbnail(bt, bt.Width, bt.Height);

                    var streams = new MemoryStream();
                    bt.Compress(Android.Graphics.Bitmap.CompressFormat.Jpeg, 42, streams);
                    //bt.Recycle();
                    await GetGCCollect();

                    data = streams.ToArray();

#endif

                    //data = imgByte;
                }
                else
                {
                    data = imgdata;

                }
                //byte[] data = ConvertImageToBinary (imgPath);
                listOffloorimage = OrdersCollection;
                orderImage = new OrderImage();
                orderImage.OrderId = OrderId;
                orderImage.FileName = imgPath;
                orderImage.orderImage = data;
                listOffloorimage.Add(orderImage);
               // orderImageRepository.SaveOrderImage(orderImage);

                if (fromCamera)
                {
                   // await DependencyService.Get<IPicture>().SavePictureToDisk(imgPath, this.OrderId, orderImage.Id);
                }

                OrdersImage = listOffloorimage;
                listOffloorimage = null;
                DisplayAlert("", "");
            }
            catch (Exception ex)
            {
            }
        }

        // delete order mages on click of cross image
        public void DeletePhotoByOrderImageId(int OrderimgId)
        {
            try
            {
               // orderImageRepository.DeleteOrderimgByOrderId(OrderimgId);
                GetOrdersImageByOrderId();

              //  var RsultinBytes = DependencyService.Get<IPicture>().GetPictureFromDisk(this.OrderId, OrderimgId);

                DisplayAlert("", "");
            }
            catch
            {
            }
        }

        //----------------------------------------------------------
#if __ANDROID__
        public void openGallery()
        {
            try
            {
                uWatch.Droid.MainActivity androidContext = (uWatch.Droid.MainActivity)Forms.Context;

                var imageIntent = new Intent(Intent.ActionPick);
                imageIntent.SetType("image/*");
                imageIntent.PutExtra(Intent.ExtraAllowMultiple, true);
                imageIntent.SetAction(Intent.ActionGetContent);
                var act = Forms.Context as Droid.MainActivity;
                act.ConfigureActivityResultCallback(ImageChooserCallback);
                act.StartActivityForResult(Intent.CreateChooser(imageIntent, "Select photo"), 0);
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error occured during openGallery: "+ ex.Message);
            }

        }


        private async void ImageChooserCallback(int requestCode, Android.App.Result resultCode, Android.Content.Intent data)
        {
            try
            {

                if (resultCode == Android.App.Result.Ok)
                {
                    Android.Content.ClipData.Item item;
                    if (data.ClipData != null)
                    {
                        for (int i = 0; i < data.ClipData.ItemCount; i++)
                        {
                            item = data.ClipData.GetItemAt(i);
                            Android.Net.Uri uri = item.Uri;
                            System.IO.Stream stream = Forms.Context.ContentResolver.OpenInputStream(Android.Net.Uri.Parse(uri.ToString()));
                            byte[] imageBytes = ReadFully(stream);
                            var RsultinBytes = imageBytes;
                            bt = Android.Graphics.BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                            bt = ThumbnailUtils.ExtractThumbnail(bt, bt.Width, bt.Height);
                            await GetGCCollect();
                            var streams = new MemoryStream();
                            bt.Compress(Android.Graphics.Bitmap.CompressFormat.Jpeg, 42, streams);
                            imageBytes = streams.ToArray();
                            OnPhotoReceived("", imageBytes, false);

                        }
                    }
                    else
                    {
                        System.IO.Stream stream = Forms.Context.ContentResolver.OpenInputStream(Android.Net.Uri.Parse(data.DataString));
                        byte[] imageBytes = ReadFully(stream);
                        bt = Android.Graphics.BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                        bt = ThumbnailUtils.ExtractThumbnail(bt, bt.Width, bt.Height);
                        await GetGCCollect();
                        var streams = new MemoryStream();
                        bt.Compress(Android.Graphics.Bitmap.CompressFormat.Jpeg, 42, streams);
                        imageBytes = streams.ToArray();
                        OnPhotoReceived("", imageBytes, false);
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error occured during ImageChooserCallback: " + ex.Message);
            }
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

        public async Task GetGCCollect()
        {
            Xamarin.Forms.Forms.Context.CacheDir.Delete();
            GC.Collect();
        }

#endif

    }
    //public interface IPicture
    //{
    //    Task SavePictureToDisk(ImageSource imgSrc, int Id, int position);
    //    //  Task SavePictureToDisk(ImageSource imgSrc);
    //   // byte[] GetPictureFromDisk(int id, int position);
    //}
}

