using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Android.Graphics;
using Android.Media;
using Android.Provider;
using Java.IO;
using uWatch.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: Dependency(typeof(uWatch.Droid.Picture))]

namespace uWatch.Droid
{
    public class Picture : IPicture
    {

        public string GetPictureMMSPathFromDisk(int Id, int position)
        {
            var _dir = new Java.IO.File(global::Android.OS.Environment.ExternalStorageDirectory, "uWatch");
            //_dir = new Java.IO.File(_dir, "Assets");
            _dir = new Java.IO.File(_dir, "Device" + Id.ToString());

            var documentsDirectory = _dir.AbsolutePath;
            string jpgFilename = System.IO.Path.Combine(_dir.Path, (position + 1).ToString() + ".jpg");

            //var b = System.IO.File.ReadAllBytes(jpgFilename);

            return jpgFilename;
        }

        public byte[] GetPictureFromDisk(int Id, int position)
        {
            var _dir = new Java.IO.File(global::Android.OS.Environment.ExternalStorageDirectory, "uWatch");
            //_dir = new Java.IO.File(_dir, "Assets");
            _dir = new Java.IO.File(_dir, "Device" + Id.ToString());

            var documentsDirectory = _dir.AbsolutePath;
            string jpgFilename = System.IO.Path.Combine(_dir.Path, (position + 1).ToString() + ".jpg");

            var b = System.IO.File.ReadAllBytes(jpgFilename);

            return b;
        }


        public byte[] GetPictureFromDiskTemp(int Id, int position)
        {

            var _dir = new Java.IO.File(global::Android.OS.Environment.ExternalStorageDirectory, "uWatch");
            //_dir = new Java.IO.File(_dir, "Assets");
            _dir = new Java.IO.File(_dir, "Device" + Id.ToString());

            var documentsDirectory = _dir.AbsolutePath;
            string jpgFilename = System.IO.Path.Combine(_dir.Path, (position + 1).ToString() + ".jpg");

            var b = System.IO.File.ReadAllBytes(jpgFilename);
            /*
			if (b.Length > 700)
			{
				


				var bt = BitmapFactory.DecodeByteArray(b, 0, b.Length);
				
				bt = ThumbnailUtils.ExtractThumbnail(bt, bt.Width, bt.Height);



				var streams = new MemoryStream();
				bt.Compress(Bitmap.CompressFormat.Jpeg, 30, streams);


				b = streams.ToArray();
               


		} */

            return b;


        }
        public byte[] GetPictureFromInventoryTemp()
        {

            var _dir = new Java.IO.File(global::Android.OS.Environment.ExternalStorageDirectory, "uWatch");
            //_dir = new Java.IO.File(_dir, "Assets");
            _dir = new Java.IO.File(_dir, "Inventory" );

            var documentsDirectory = _dir.AbsolutePath;
            string jpgFilename = System.IO.Path.Combine(_dir.Path + ".jpg");

            var b = System.IO.File.ReadAllBytes(jpgFilename);
            /*
            if (b.Length > 700)
            {
                


                var bt = BitmapFactory.DecodeByteArray(b, 0, b.Length);
                
                bt = ThumbnailUtils.ExtractThumbnail(bt, bt.Width, bt.Height);



                var streams = new MemoryStream();
                bt.Compress(Bitmap.CompressFormat.Jpeg, 30, streams);


                b = streams.ToArray();
               


        } */

            return b;


        }
        public string GetPicturePath(int Id, int position)
        {
            var _dir = new Java.IO.File(global::Android.OS.Environment.ExternalStorageDirectory, "uWatch");
            //_dir = new Java.IO.File(_dir, "Assets");
            _dir = new Java.IO.File(_dir, "Device" + Id.ToString());

            var documentsDirectory = _dir.AbsolutePath;
            string jpgFilename = System.IO.Path.Combine(_dir.Path, (position + 1).ToString() + ".jpg");

            return jpgFilename;
        }

        public async Task SavePictureToDisk(ImageSource imgSrc, int Id, int position, double rotationangle = 0)
        {
            try
            {
                Bitmap photo = null;
                //await Task.Delay(5000);
                if (imgSrc is StreamImageSource)
                {

                    var renderer = new StreamImagesourceHandler();

                    photo = await renderer.LoadImageAsync(imgSrc, Forms.Context);

                }
                else if (imgSrc is FileImageSource)
                {

                    var renderer = new FileImageSourceHandler();

                    photo = await renderer.LoadImageAsync(imgSrc, Forms.Context);

                }
                var _dir = new Java.IO.File(global::Android.OS.Environment.ExternalStorageDirectory, "uWatch");
                //_dir = new Java.IO.File(_dir, "Assets");
                _dir = new Java.IO.File(_dir, "Device" + Id.ToString());
                if (!_dir.Exists())
                {
                    var v = _dir.Mkdirs();
                }
                var documentsDirectory = _dir.AbsolutePath;

                string jpgFilename = System.IO.Path.Combine(documentsDirectory, (position + 1).ToString() + ".jpg");
                var stream = new FileStream(jpgFilename, FileMode.Create);
                var resizedimage = ScaleDownBitmap(photo,800,true);
				//var res = photo.Compress(Bitmap.CompressFormat.Jpeg, 42, stream);
				var res = resizedimage.Compress(Bitmap.CompressFormat.Jpeg, 30, stream);
               
                stream.Close();
            }
            catch (Exception ex)
            {
                string exMessageString = ex.Message;
            }
        }
        public async Task SaveInventoryToDisk(ImageSource imgSrc)
        {
            try
            {
                Bitmap photo = null;
                //await Task.Delay(5000);
                if (imgSrc is StreamImageSource)
                {

                    var renderer = new StreamImagesourceHandler();

                    photo = await renderer.LoadImageAsync(imgSrc, Forms.Context);

                }
                else if (imgSrc is FileImageSource)
                {

                    var renderer = new FileImageSourceHandler();

                    photo = await renderer.LoadImageAsync(imgSrc, Forms.Context);

                }
                var _dir = new Java.IO.File(global::Android.OS.Environment.ExternalStorageDirectory, "uWatch");
                //_dir = new Java.IO.File(_dir, "Assets");
                _dir = new Java.IO.File(_dir, "Inventory" );
                if (!_dir.Exists())
                {
                    var v = _dir.Mkdirs();
                }
                var documentsDirectory = _dir.AbsolutePath;

                string jpgFilename = System.IO.Path.Combine(documentsDirectory + ".jpg");
                var stream = new FileStream(jpgFilename, FileMode.Create);
                var resizedimage = ScaleDownBitmap(photo, 800, true);
                //var res = photo.Compress(Bitmap.CompressFormat.Jpeg, 42, stream);
                var res = resizedimage.Compress(Bitmap.CompressFormat.Jpeg, 30, stream);

                stream.Close();
            }
            catch (Exception ex)
            {
                string exMessageString = ex.Message;
            }
        }
		public static Bitmap ScaleDownBitmap(Bitmap originalImage, float maxImageSize, bool filter)
		{
			float ratio = Math.Min((float)maxImageSize / originalImage.Width, (float)maxImageSize / originalImage.Height);
			int width = (int)Math.Round(ratio * (float)originalImage.Width);
			int height = (int)Math.Round(ratio * (float)originalImage.Height);

			Bitmap newBitmap = Bitmap.CreateScaledBitmap(originalImage, width, height, filter);
			return newBitmap;
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

        public static byte[] ResizeImageAndroid(byte[] imageData, float width, float height)
        {
            MemoryStream ms = new MemoryStream();
            try
            {
                // Load the bitmap
                Bitmap originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);
                Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, (int)width, (int)height, false);


                resizedImage.Compress(Bitmap.CompressFormat.Png, 40, ms);

            }
            catch (System.Exception ex)
            {

            }

			return ms.ToArray();



        }

        public static byte[] ToByteArray(System.IO.Stream stream)
        {

			using (stream)
			{
			    using (MemoryStream memStream = new MemoryStream())
			    {
			        stream.CopyTo(memStream);
			        return memStream.ToArray();
			    }
			}
			
			
        }

        Task<string> IPicture.DownloadImage(string url)
        {
            string path = null;
            string filename = Guid.NewGuid().ToString();

            try
            {
                Bitmap imageBitmap = null;
                using (var webClient = new WebClient())
                {
                    var imageBytes = webClient.DownloadData(url);
                    if (imageBytes != null && imageBytes.Length > 0)
                    {
                        //imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                        var dir = new Java.IO.File(global::Android.OS.Environment.GetExternalStoragePublicDirectory(global::Android.OS.Environment.DirectoryDownloads), filename + ".jpg");
                        System.IO.File.WriteAllBytes(dir.AbsolutePath, imageBytes);
                        path = dir.AbsolutePath;

                    }
                }

                //var _dir = new Java.IO.File(global::Android.OS.Environment.GetExternalStoragePublicDirectory(global::Android.OS.Environment.DirectoryDownloads), filename+".jpg");

                //if (!_dir.Exists())
                //{
                //	var v = _dir.Mkdirs();
                //}

                //File.WriteAllBytes(_dir.AbsolutePath, imageBytes);
                //var stream = new FileStream(_dir.AbsolutePath, FileMode.Create);

                //var res = imageBitmap.Compress(Bitmap.CompressFormat.Jpeg, 42, stream);

                //stream.Close();

                //path = _dir.AbsolutePath;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Image downloading failed: " + ex.Message);
            }

            return Task.FromResult<string>(path);
        }

        public void ReleaseMemory(string imgPath, bool isDeviceMemory)
        {
            try
            {
                if(!isDeviceMemory)
                {
                    var list = System.IO.Directory.GetFiles(imgPath, "*");

                    if (list.Length > 0)
                    {
                        for (int i = 0; i < list.Length; i++)
                        {
                            System.IO.File.Delete(list[i]);
                        }
                    }
                }
                else
                {
                    var _dir = new Java.IO.File(global::Android.OS.Environment.ExternalStorageDirectory, "uWatch");
                    var list = System.IO.Directory.GetFiles(_dir.Path, "*");
                    var list1 = System.IO.Directory.GetDirectories(_dir.Path, "*");

                    if (list1.Length > 0)
                    {
                        for (int i = 0; i < list1.Length; i++)
                        {
                            System.IO.Directory.Delete(list1[i],true);
                        }
                    }

                    if (list.Length > 0)
                    {
                        for (int i = 0; i < list.Length; i++)
                        {
                            System.IO.File.Delete(list[i]);
                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }
           
        }
    }
}

