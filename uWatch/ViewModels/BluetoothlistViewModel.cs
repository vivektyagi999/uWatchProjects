using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Plugin.BLE;
using uWatch.ViewModels;
using UwatchPCL;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Acr.UserDialogs;
using System.Collections.Generic;
#if __ANDROID__
using Android.OS;
#endif

namespace uWatch
{
	public class BluetoothlistViewModel: BaseViewModel
	{
		
		public BluetoothlistViewModel()
		{
			
		}
		public event PropertyChangedEventHandler PropertyChanged;
		
		bool nodevicefound = false;
		public  ObservableCollection<BluetoohModel> Bluetootdevicelist
		{ //Property that will be used to get and set the item
			get;set;
		}
		public async Task GetDeviceList()
		{
			Bluetootdevicelist = new ObservableCollection<BluetoohModel>();
			var ble = CrossBluetoothLE.Current;
			var adapter = CrossBluetoothLE.Current.Adapter;
			
			var d=adapter.ConnectedDevices;



			adapter.DeviceDiscovered +=  (s, a) =>
			{
				
				var devicelist = adapter.DiscoveredDevices.ToList();


				
				var uWatchdivicelist = devicelist.ToList();
			
				   
						foreach ( var device in uWatchdivicelist )
						{
					if (Bluetootdevicelist.Where(x => x.DeviceId.Contains(device.Id.ToString())).Count()==0)
							{
								var _BluetoohModel = new BluetoohModel
								{
									DeviceName = device.Name,
						 			DeviceId=device.Id.ToString(),
									DeviceRSSI = device.Rssi.ToString() + "  " + "dbm"

								};
								Bluetootdevicelist.Add(_BluetoohModel);
							}
							
						}



			};
			await adapter.StartScanningForDevicesAsync();

		}
	}
}
