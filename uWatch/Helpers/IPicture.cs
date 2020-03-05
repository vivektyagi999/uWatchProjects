using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace uWatch
{
	public interface IPicture
	{
		Task SavePictureToDisk(ImageSource imgSrc, int Id, int position,double RotateAngle);

		 byte[] GetPictureFromDisk(int id, int position);

		string GetPictureMMSPathFromDisk(int id, int position);

		string GetPicturePath(int Id, int position);

		byte[] GetPictureFromDiskTemp(int id, int position);

         Task SaveInventoryToDisk(ImageSource imgSrc);

        byte[] GetPictureFromInventoryTemp();
		/// <summary>
		/// Downloads the image.
		/// Takes URL Of Image.
		/// </summary>
		/// <returns>Path Of Image.</returns>
		/// <param name="url">URL.</param>
		Task<string> DownloadImage(string url);

        void ReleaseMemory(string imgPath,bool isDeviceMemory);

	}
}

