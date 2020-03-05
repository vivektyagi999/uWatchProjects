using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UwatchPCL;
using Plugin.BLE;
using Acr.UserDialogs;
using Xamarin.Forms;
using System.Linq;
using System.Threading.Tasks;

namespace uWatch
{
	public partial class BluetoothDeviceListPage : ContentPage
	{
		public static  IAdapter _bluetoothAdapter;
		public static IAdapter BluetoothAdapter { get { return _bluetoothAdapter; } }
		public ObservableCollection<IDevice> DiscoveredDevices { get; private set; }
		bool isScanning = false;
		public BluetoothDeviceListPage()
		{
			Title = "Bluetooth Devices";

			ToolbarItem ReScan = new ToolbarItem
			{
                Icon = "reload.png"

			};
			ReScan.Clicked += async (s, e) =>
			  {
				UserDialogs.Instance.ShowLoading("Searching for uWatch Bluetooth Devices!");
				await Task.Delay(_bluetoothAdapter.ScanTimeout);
				   var _ble = CrossBluetoothLE.Current;

				  if (!_ble.IsOn)
				  {
					  UserDialogs.Instance.HideLoading();
					  deviceListView.IsVisible = false;
					  lblNodevice.Text = "This app needs access to bluetooth Please Turn on!";
					  lblNodevice.IsVisible = true;

				  }
                 else
				  {
					  try
					  {
						
						InitialteBluetoohSearching();

					  }
					  catch (Exception ex)
					  {
						UserDialogs.Instance.HideLoading();
					  }
				}
			  };
			this.ToolbarItems.Add(ReScan);
			InitializeComponent();
			InitialteBluetoohSearching();
		}
		public void InitialteBluetoohSearching()
		{
			_bluetoothAdapter = DependencyService.Get<IAdapter>();
			_bluetoothAdapter.ScanTimeout = TimeSpan.FromSeconds(10);
			_bluetoothAdapter.ConnectionTimeout = TimeSpan.FromSeconds(10);
			DiscoveredDevices = new ObservableCollection<IDevice>();
			BluetoothAdapter.DeviceDiscovered += DeviceDiscovered;
			BluetoothAdapter.StartScanningForDevices();
			BluetoothAdapter.ScanTimeoutElapsed += async(sender, e) =>
			  {
				  if (DiscoveredDevices.Count == 0)
				  {
					  deviceListView.IsVisible = false;
					  lblNodevice.Text = "No uWatch BLE Device Found!";
					  lblNodevice.IsVisible = true;
					  await Task.Delay(60);
					  UserDialogs.Instance.HideLoading();
				  }
				  else { 
					  UserDialogs.Instance.HideLoading();
				}
			  };
			deviceListView.IsVisible = true;
			lblNodevice.IsVisible = false;
			deviceListView.ItemsSource = DiscoveredDevices;

		}
		public void NoAvilabledevice()
		{
			deviceListView.IsVisible = false;
			lblNodevice.Text = "No BLE Device Found!";
			lblNodevice.IsVisible = true;
		}
		#region BluetoothAdapter callbacks

		void DeviceDiscovered(object sender, DeviceDiscoveredEventArgs e)
		{
			if (!string.IsNullOrEmpty(e.Device.Name))
			{
				if (e.Device.Name.Contains("uWatch"))
				{
					if (!(DiscoveredDevices.Any(X => X.Id == e.Device.Id)))
					{
						DiscoveredDevices.Add(e.Device);
					}
				}
			}

		}

		#endregion
	}
}
