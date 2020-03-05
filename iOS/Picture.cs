using System;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using UIKit;
using uWatch.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: Dependency(typeof(Picture))]

namespace uWatch.iOS
{
	public class Picture : IPicture
	{

		public String GetPictureMMSPathFromDisk(int Id, int position)
		{
			var documentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			string jpgFilename = System.IO.Path.Combine(documentsDirectory, Id.ToString() + ".jpg");

			//var b = System.IO.File.ReadAllBytes(jpgFilename);
			return jpgFilename;
		}

		public byte[] GetPictureFromDisk(int Id, int position)
		{
			var documentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			string jpgFilename = System.IO.Path.Combine(documentsDirectory, Id.ToString() + ".jpg");
			///
			UIKit.UIImage image = new UIKit.UIImage(jpgFilename);
			UIKit.UIImage highres = image;
			Foundation.NSData d = highres.AsJPEG(0.7f);
			UIKit.UIImage lowres = new UIKit.UIImage(d);
			Byte[] myByteArray = new Byte[d.Length];
			System.Runtime.InteropServices.Marshal.Copy(d.Bytes, myByteArray, 0, Convert.ToInt32(d.Length));
			return myByteArray;
			//var b = System.IO.File.ReadAllBytes(jpgFilename);
			//return b;
			//return finalimage;
		}

		public byte[] GetPictureFromDiskTemp(int Id, int position)
		{
			var documentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			string jpgFilename = System.IO.Path.Combine(documentsDirectory, Id.ToString() + ".jpg");
			//NSData data = image.AsJPEG(0.3);
			var b = System.IO.File.ReadAllBytes(jpgFilename);
			return b;
		}
        public byte[] GetPictureFromInventoryTemp()
        {
            var documentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string jpgFilename = System.IO.Path.Combine(documentsDirectory , "Photo.jpg");
            //NSData data = image.AsJPEG(0.3);
            var b = System.IO.File.ReadAllBytes(jpgFilename);
            return b;
        }

		public string GetPicturePath(int Id, int position)
		{
			var documentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			string jpgFilename = System.IO.Path.Combine(documentsDirectory, Id.ToString() + ".jpg");
			return jpgFilename;
		}

		public async Task SavePictureToDisk(ImageSource imgSrc, int Id, int position, double rotationangle = 0)
		{
			UIImage photo = null;

			try
			{
				if (imgSrc is StreamImageSource)
				{
					var renderer = new StreamImagesourceHandler();
					photo = await renderer.LoadImageAsync(imgSrc);
				}
				else if (imgSrc is FileImageSource)
				{
					var renderer = new FileImageSourceHandler();
					photo = await renderer.LoadImageAsync(imgSrc);
				}
				var documentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
				string jpgFilename = System.IO.Path.Combine(documentsDirectory, Id.ToString() + ".jpg");
				NSData imgData = photo.AsJPEG();
				NSError err = null;
				//byte[] dataBytes = new byte[imgData.Length];
				//System.Runtime.InteropServices.Marshal.Copy(imgData.Bytes, dataBytes, 0, Convert.ToInt32(imgData.Length));
				//var ResizeBytes = ResizeImageIOS(dataBytes, 800, 600);
				//UIImage originalImage = ImageFromByteArray(ResizeBytes);
				//NSData ResizeData = originalImage.AsJPEG(0.7f);
				if (imgData.Save(jpgFilename, false, out err))
				{
					Console.WriteLine("saved as " + jpgFilename);
				}
				else
				{
					Console.WriteLine("NOT saved as " + jpgFilename + " because" + err.LocalizedDescription);
				}
			}
			catch (Exception ex)
			{

			}
		}
        public async Task SaveInventoryToDisk(ImageSource imgSrc)
        {
            UIImage photo = null;

            try
            {
                if (imgSrc is StreamImageSource)
                {
                    var renderer = new StreamImagesourceHandler();
                    photo = await renderer.LoadImageAsync(imgSrc);
                }
                else if (imgSrc is FileImageSource)
                {
                    var renderer = new FileImageSourceHandler();
                    photo = await renderer.LoadImageAsync(imgSrc);
                }
                var documentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                string jpgFilename = System.IO.Path.Combine(documentsDirectory,"Photo.jpg");
                NSData imgData = photo.AsJPEG();
                NSError err = null;
                //byte[] dataBytes = new byte[imgData.Length];
                //System.Runtime.InteropServices.Marshal.Copy(imgData.Bytes, dataBytes, 0, Convert.ToInt32(imgData.Length));
                //var ResizeBytes = ResizeImageIOS(dataBytes, 800, 600);
                //UIImage originalImage = ImageFromByteArray(ResizeBytes);
                //NSData ResizeData = originalImage.AsJPEG(0.7f);
                if (imgData.Save(jpgFilename, false, out err))
                {
                    Console.WriteLine("saved as " + jpgFilename);
                }
                else
                {
                    Console.WriteLine("NOT saved as " + jpgFilename + " because" + err.LocalizedDescription);
                }


            //UIImage photo = null;

            //try
            //{
                //if (imgSrc is StreamImageSource)
                //{
                //    var renderer = new StreamImagesourceHandler();
                //    photo = await renderer.LoadImageAsync(imgSrc);
                //}
                //else if (imgSrc is FileImageSource)
                //{
                //    var renderer = new FileImageSourceHandler();
                //    photo = await renderer.LoadImageAsync(imgSrc);
                //}
                //var documentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                //string jpgFilename = System.IO.Path.Combine(documentsDirectory, "Photo.jpg");
                //NSData imgData = photo.AsJPEG();
                //NSError err = null;
                ////byte[] dataBytes = new byte[imgData.Length];
                ////System.Runtime.InteropServices.Marshal.Copy(imgData.Bytes, dataBytes, 0, Convert.ToInt32(imgData.Length));
                ////var ResizeBytes = ResizeImageIOS(dataBytes, 800, 600);
                ////UIImage originalImage = ImageFromByteArray(ResizeBytes);
                ////NSData ResizeData = originalImage.AsJPEG(0.7f);
                //if (imgData.Save(jpgFilename, false, out err))
                //{
                //    Console.WriteLine("saved as " + jpgFilename);
                //}
                //else
                //{
                //    Console.WriteLine("NOT saved as " + jpgFilename + " because" + err.LocalizedDescription);
                //}
            }
            catch (Exception ex)
            {

            }
        }
		public static byte[] ResizeImageIOS(byte[] imageData, float width, float height)
		{
			UIImage originalImage = ImageFromByteArray(imageData);
			UIImageOrientation orientation = originalImage.Orientation;

			//create a 24bit RGB image
			using (CGBitmapContext context = new CGBitmapContext(IntPtr.Zero,
												 (int)width, (int)height, 8,
												 (int)(4 * width), CGColorSpace.CreateDeviceRGB(),
												 CGImageAlphaInfo.PremultipliedFirst))
			{

				RectangleF imageRect = new RectangleF(0, 0, width, height);

				// draw the image
				context.DrawImage(imageRect, originalImage.CGImage);

				UIKit.UIImage resizedImage = UIKit.UIImage.FromImage(context.ToImage(), 0, orientation);

				// save the image as a jpeg
				return resizedImage.AsJPEG().ToArray();
			}
		}

		public static UIKit.UIImage ImageFromByteArray(byte[] data)
		{
			if (data == null)
			{
				return null;
			}

			UIKit.UIImage image;
			try
			{
				image = new UIKit.UIImage(Foundation.NSData.FromArray(data));
			}
			catch (Exception e)
			{
				Console.WriteLine("Image load failed: " + e.Message);
				return null;
			}
			return image;
		}

		Task<string> IPicture.DownloadImage(string url)
		{
			string path = "";
			string filename = Guid.NewGuid().ToString();

			try
			{

				using (var webclient = new WebClient())
				{
					var imageBytes = webclient.DownloadData(url);

					var imageBitmap = UIImage.LoadFromData(NSData.FromArray(imageBytes));

					string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
					string localPath = Path.Combine(documentsPath, filename + ".jpg");
					NSData imgData = imageBitmap.AsJPEG();
					NSError err = null;
					if (imgData.Save(localPath, false, out err))// writes to local storage
					{
						path = localPath;
						Console.WriteLine("saved as " + localPath);
					}
					else
					{
						Console.WriteLine("Couldn't save at " + localPath + " because of " + err.LocalizedDescription);
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Image downloading failed: " + ex.Message);
			}
			return Task.FromResult<string>(path);
		}

        public void ReleaseMemory(string imgPath, bool isDeviceMemory)
        {
            //throw new NotImplementedException();
        }
    }


}
