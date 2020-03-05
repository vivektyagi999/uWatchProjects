using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Acr.UserDialogs;
using uWatch.ViewModels;
using UwatchPCL;
using UwatchPCL.Model;

namespace uWatch
{
	public class AssetDetailsViewModel : BaseViewModel
	{
		public DeviceAssetsModel DeviceAsset { get; set;}

		public int RotationAngle { get; set; }

		public AssetDetailsViewModel()
		{
            DeviceAsset = new DeviceAssetsModel();
		}

		public async Task GetActualAssetImage(DeviceAssetsModel obj)
		{
			try
			{
				DeviceAsset = await ApiService.Instance.GetAssetbyUrl(obj);
			}
			catch {
            }
		}

     
    }
}

