using System;
using System.IO;
using System.Threading.Tasks;
using uWatch.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(PhotoImplementation))]
namespace uWatch.Droid
{
	public class PhotoImplementation : Java.Lang.Object, IPhoto
	{
		public async Task<Stream> GetPhoto(string path)
		{
			// Open the photo and put it in a Stream to return       
			var memoryStream = new MemoryStream();

			using (var source = System.IO.File.OpenRead(path))
			{
				await source.CopyToAsync(memoryStream);
			}

			return memoryStream;
		}
	}
}

