using uWatch.Droid;
using Xamarin.Forms;

using System;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.Provider;

using Java.IO;


[assembly: Dependency(typeof(CameraProvider))]
namespace uWatch.Droid
{
	using Uri = Android.Net.Uri;

	public class CameraProvider :  global::Xamarin.Forms.Platform.Android.FormsApplicationActivity, ICameraProvider
	{
		private static File file;
		private static File pictureDirectory;

		private static TaskCompletionSource<CameraResult> tcs;

		//public void openGallery()
		//{
		//	KeyAgentApp.Droid.MainActivity androidContext = (KeyAgentApp.Droid.MainActivity)Forms.Context;

		//	//Android.Widget.Toast.MakeText (Xamarin.Forms.Forms.Context, "Select max 20 images", Android.Widget.ToastLength.Long).Show ();
		//	var imageIntent = new Android.Content.Intent(Android.Content.Intent.ActionPick);
		//	imageIntent.SetType("image/*");
		//	imageIntent.PutExtra(Android.Content.Intent.ExtraAllowMultiple, true);
		//	imageIntent.SetAction(Android.Content.Intent.ActionGetContent);
		//	androidContext.ConfigureActivityResultCallback(ImageChooserCallback);
		//	((Android.App.Activity)Forms.Context).StartActivityForResult(
		//		Android.Content.Intent.CreateChooser(imageIntent, "Select photo"), 0);

		//}




		public async Task<CameraResult> TakePictureAsync()
		{

			uWatch.Droid.MainActivity androidContext = (uWatch.Droid.MainActivity)Forms.Context;

			Intent intent = new Intent(MediaStore.ActionImageCapture);
			pictureDirectory = new File(Android.OS.Environment.ExternalStorageDirectory, "uWatch");
			if (!pictureDirectory.Exists())
			{
				pictureDirectory.Mkdirs();
			}
			file = new File(pictureDirectory, String.Format("photo_{0}.jpg", Guid.NewGuid()));
			intent.PutExtra(MediaStore.ExtraOutput, Uri.FromFile(file));

			//intent.SetAction(Android.Content.Intent.ActionGetContent);

			//androidContext.ConfigureActivityResultCallback(ImageChooserCallback);
			((Android.App.Activity)Forms.Context).StartActivityForResult(intent, 0);


			tcs = new TaskCompletionSource<CameraResult>();
			return tcs.Task.Result;
		}

		private void ImageChooserCallback(int requestCode, Android.App.Result resultCode, Android.Content.Intent data)
		{
			
		}

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			if (resultCode == Result.Canceled)
			{
				tcs.TrySetResult(null);
				return;
			}

			if (resultCode != Result.Ok)
			{
				tcs.TrySetException(new Exception("Unexpected error"));
				return;
			}

			CameraResult res = new CameraResult();
			res.Picture = ImageSource.FromFile(file.Path);
			res.FilePath = file.Path;

			tcs.TrySetResult(res);
		}


		public static void OnResult(Result resultCode)
		{
			if (resultCode == Result.Canceled)
			{
				tcs.TrySetResult(null);
				return;
			}

			if (resultCode != Result.Ok)
			{
				tcs.TrySetException(new Exception("Unexpected error"));
				return;
			}

			CameraResult res = new CameraResult();
			res.Picture = ImageSource.FromFile(file.Path);
			res.FilePath = file.Path;

			tcs.TrySetResult(res);
		}
	}
}