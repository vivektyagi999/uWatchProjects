using System;
using System.Threading.Tasks;
using Acr.UserDialogs;
using System.Net;
using UwatchPCL;
using Xamarin.Forms;
using System.IO;
#if __ANDROID__
using uWatch.Droid;
using Xamarin.Media;
using Android.Graphics;
using Plugin.Settings;
using Android.Provider;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
#endif
#if __IOS__
using Xamarin.Media;
using UIKit;
#endif



namespace uWatch
{
    public class PlatformHelper
    {
#if __ANDROID__
        async static Task<bool> CheckCameraPermisions()
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

        #region Device Camera
        public static async Task<byte[]> CameraFuncAsync()
        {
            Byte[] byteArray = null;

#if __IOS__
            var picker = new MediaPicker();
            if (!picker.IsCameraAvailable)
                UserDialogs.Instance.Alert("Camera is not available");
            else
            {
                try
                {

                    var mediaFile = await picker.TakePhotoAsync(new StoreCameraMediaOptions
                    {
                        Name = "Photo.jpg ",
                        Directory = "uWatch"
                    });
                    if (mediaFile != null)
                    {
                        byteArray = ReadImageAndCopyToPath(mediaFile.Path);
                    }

                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Canceled");
                }

            }
#endif
#if __ANDROID__
            var picker = new MediaPicker(Forms.Context);
            try
            {

                var s = await CheckCameraPermisions();
                if (s)
                {
                    if (!picker.IsCameraAvailable)
                        UserDialogs.Instance.Alert("Camera is not available");
                    else
                    {
                        var mediaFile =await picker.TakePhotoAsync(new StoreCameraMediaOptions
                        {
                            Name = "Photo.jpg ",
                            Directory = "uWatch"
                        });

                        Java.IO.File _destFile = new Java.IO.File(mediaFile.Path);
                        ReadImageAndCopyToPath(Xamarin.Forms.Forms.Context, global::Android.Net.Uri.Parse(_destFile.ToURI().ToString()), _destFile.Path, 800, 800);
                        if (mediaFile != null)
                        {
                            byteArray = File.ReadAllBytes(mediaFile.Path);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                string x = ex.Message;
            }
           
#endif

            return byteArray;

        }
#endregion

#region Resize Image

#if __ANDROID__

        public static void ReadImageAndCopyToPath(global::Android.Content.Context context, global::Android.Net.Uri sourceUri, string destinationPath, int maxScaleWidth, int maxScaleHeight)
        {
            using (var stream = context.ContentResolver.OpenInputStream(sourceUri))
            {

                var oimage = BitmapFactory.DecodeStream(stream);
                if (oimage == null)
                {
                    throw new System.Exception("File not found.");
                }
                //oimage = OrientationHelper.CheckAndModifyOrientation(oimage, sourceUri.Path);

                var ratioX = (double)maxScaleWidth / oimage.Width;
                var ratioY = (double)maxScaleHeight / oimage.Height;
                var ratio = System.Math.Min(ratioX, ratioY);
                var newWidth = (int)(oimage.Width * ratio);
                var newHeight = (int)(oimage.Height * ratio);

                if (newWidth == 0 || oimage.Width < newWidth)
                    newWidth = oimage.Width;
                if (newHeight == 0 || oimage.Height < newHeight)
                    newHeight = oimage.Height;

                Bitmap resizedImage = Bitmap.CreateScaledBitmap(oimage, newWidth, newHeight, false);

                try
                {
                    Matrix mtx = new Matrix();
                    global::Android.Media.ExifInterface exif = new global::Android.Media.ExifInterface(destinationPath);
                    string orientation = exif.GetAttribute(global::Android.Media.ExifInterface.TagOrientation);

                    switch (orientation)
                    {
                        case "6": // portrait
                            mtx.PreRotate(90);
                            resizedImage = Bitmap.CreateBitmap(resizedImage, 0, 0, resizedImage.Width, resizedImage.Height, mtx, false);
                            mtx.Dispose();
                            mtx = null;
                            break;
                        case "1": // landscape
                            break;
                        default:
                            mtx.PreRotate(90);
                            resizedImage = Bitmap.CreateBitmap(resizedImage, 0, 0, resizedImage.Width, resizedImage.Height, mtx, false);
                            mtx.Dispose();
                            mtx = null;
                            break;
                    }
                }
                catch (Exception ex)
                {

                }

                using (var os = new System.IO.FileStream(destinationPath, System.IO.FileMode.Create))
                {
                    resizedImage.Compress(Bitmap.CompressFormat.Jpeg, 35, os);
                }
            }
        }


#endif

#if __IOS__
        double radians(double degrees) { return degrees * Math.PI / 180; }
        public static byte[] ReadImageAndCopyToPath(string destinationPath)
        {

            UIImage photo = null;
            photo = UIImage.FromFile(destinationPath);
            UIImage highres = photo;
            highres = highres.Scale(new CoreGraphics.CGSize(1100, 900));
            Foundation.NSData d = highres.AsJPEG(0.3f);
            UIImage lowres = new UIKit.UIImage(d);
            byte[] myByteArray = new Byte[d.Length];
            System.Runtime.InteropServices.Marshal.Copy(d.Bytes, myByteArray, 0, Convert.ToInt32(d.Length));
            return myByteArray;

        }
        private UIImage RotateImage(UIImage src, UIImageOrientation orientation)
        {
            UIGraphics.BeginImageContext(src.Size);

            if (orientation == UIImageOrientation.Right)
            {
                CoreGraphics.CGAffineTransform.MakeRotation((nfloat)radians(90));
            }
            else if (orientation == UIImageOrientation.Left)
            {
                CoreGraphics.CGAffineTransform.MakeRotation((nfloat)radians(-90));
            }
            else if (orientation == UIImageOrientation.Down)
            {

            }
            else if (orientation == UIImageOrientation.Up)
            {
                CoreGraphics.CGAffineTransform.MakeRotation((nfloat)radians(90));
            }

            src.Draw(new CoreGraphics.CGPoint(0, 0));
            UIImage image = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return image;
        }
#endif

#endregion

    }
}
