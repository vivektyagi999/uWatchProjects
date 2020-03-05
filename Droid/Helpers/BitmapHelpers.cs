using System;
using System.IO;
using System.Linq;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.OS;


namespace uWatch.Droid
{
    public static class BitmapHelpers
    {
       
        public static Bitmap LoadAndResizeBitmap(this string fileName, int width, int height)
        {

            // First we get the the dimensions of the file on disk
            BitmapFactory.Options options = new BitmapFactory.Options { InJustDecodeBounds = true };
            BitmapFactory.DecodeFile(fileName, options);

            // Next we calculate the ratio that we need to resize the image by
            // in order to fit the requested dimensions.
            int outHeight = options.OutHeight;
            int outWidth = options.OutWidth;
            int inSampleSize = 1;

            if (outHeight > height || outWidth > width)
            {
                inSampleSize = outWidth > outHeight
                    ? outHeight / height
                        : outWidth / width;
            }

            // Now we will load the image and have BitmapFactory resize it for us.
            options.InSampleSize = inSampleSize;
            options.InJustDecodeBounds = false;
            Bitmap resizedBitmap = BitmapFactory.DecodeFile(fileName, options);
            try
            {
                Matrix mtx = new Matrix();
                ExifInterface exif = new ExifInterface(fileName);
                string orientation = exif.GetAttribute(ExifInterface.TagOrientation);

                switch (orientation)
                {
                    case "6": // portrait
                        mtx.PreRotate(90);
                        resizedBitmap = Bitmap.CreateBitmap(resizedBitmap, 0, 0, resizedBitmap.Width, resizedBitmap.Height, mtx, false);
                        mtx.Dispose();
                        mtx = null;
                        break;
                    case "1": // landscape
                        break;
                    default:
                        mtx.PreRotate(90);
                        resizedBitmap = Bitmap.CreateBitmap(resizedBitmap, 0, 0, resizedBitmap.Width, resizedBitmap.Height, mtx, false);
                        mtx.Dispose();
                        mtx = null;
                        break;
                }
            }
            catch (Exception ex)
            {

            }
            // Images are being saved in landscape, so rotate them back to portrait if they were taken in portrait




            return resizedBitmap;
        }
    }
    public class BitmapWorkerTask : AsyncTask<int, int, Bitmap>
    {
        private global::Android.Net.Uri uriReference;
        private int data = 0;
        private ContentResolver resolver;

        public BitmapWorkerTask(ContentResolver cr, global::Android.Net.Uri uri)
        {
            uriReference = uri;
            resolver = cr;
        }

        protected override Bitmap RunInBackground(params int[] p)
        {
            data = p[0];

            Bitmap mBitmap =global::Android.Provider.MediaStore.Images.Media.GetBitmap(resolver, uriReference);
            Bitmap myBitmap = null;

            if (mBitmap != null)
            {
                Matrix matrix = new Matrix();
                if (data != 0)
                {
                    matrix.PreRotate(data);
                }

                myBitmap = Bitmap.CreateBitmap(mBitmap, 0, 0, mBitmap.Width, mBitmap.Height, matrix, true);
                return myBitmap;
            }

            return null;
        }


        protected override void OnPostExecute(Bitmap bitmap)
        {
            if (bitmap != null)
            {
                MemoryStream stream = new MemoryStream();
                bitmap.Compress(Bitmap.CompressFormat.Jpeg, 50, stream);
                byte[] bitmapData = stream.ToArray();

                // SamplePage.Galleryimage(bitmapData);

                bitmap.Recycle();
                GC.Collect();
            }
        }
    }
}
