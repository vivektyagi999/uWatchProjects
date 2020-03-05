using System;
using Android.App;
using Android.Content;
using Android.Provider;
using uWatch.Droid;
using Java.IO;
using Xamarin.Forms;
using Uri = Android.Net.Uri;

[assembly: Dependency(typeof(CameraGallery))]
namespace uWatch.Droid
{
    public class CameraGallery : Activity, ICameraGallery
    {
        public void CameraMedia()
        {
            AppClass._dir = new Java.IO.File(global::Android.OS.Environment.ExternalStorageDirectory, "uWatch");
            //AppClass._dir = new File(global::Android.OS.Environment.GetExternalStoragePublicDirectory(global::Android.OS.Environment.DirectoryPictures), "beeWatch");

            if (!AppClass._dir.Exists())
            {
                AppClass._dir.Mkdirs();
            }

            Intent intent = new Intent(MediaStore.ActionImageCapture);

            //AppClass._file = new Java.IO.File(AppClass._dir, String.Format("beeWatch.jpg", Guid.NewGuid()));
            AppClass._file = new Java.IO.File(AppClass._dir, GetUniqueFileName(".jpeg"));
            intent.PutExtra(MediaStore.ExtraOutput, Uri.FromFile(AppClass._file));

            Activity activity = (Activity)Forms.Context;
            activity.StartActivityForResult(intent, 0);
        }
        public string GetUniqueFileName(string ext)
        {
            return System.Guid.NewGuid().ToString().Replace("-", "") + (ext ?? "");
        }
        public void GalleryMedia()
        {
            var imageIntent = new Intent();
            imageIntent.SetType("image/*");
            imageIntent.SetAction(Intent.ActionGetContent);
            ((Activity)Forms.Context).StartActivityForResult(Intent.CreateChooser(imageIntent, "Select photo"), 1);
        }
    }
}
