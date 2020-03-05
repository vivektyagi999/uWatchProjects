using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UwatchPCL;
using System.Collections.Generic;
using uWatch.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;
using Acr.UserDialogs;
using System.Threading.Tasks;
using UwatchPCL.Helpers;

namespace uWatch
{
	public class DashboardViewModel : BaseViewModel
	{
		ObservableCollection<DeviceStatic> _deviceList;
		public ObservableCollection<DeviceStatic> DeviceList 
		{
			get { return _deviceList; }

			set
			{
				_deviceList = value;
				if (PropertyChanged != null)
				{
					PropertyChanged(this,
						new PropertyChangedEventArgs("DeviceList"));// Throw!!
				}
			}
		}
        bool moreDevicesListExists = true;

		public int s = 0;

		public event PropertyChangedEventHandler PropertyChanged;

		public DeviceStatic Device { get; set; }
		public int DeviceCounts { get; set; }

		private INavigation Navigation;

		public bool isRunningLoader = true;

		private Command loadDeviceCommand;
		public ICommand LoadCharactersCommand
		{
			get { return loadDeviceCommand ?? (loadDeviceCommand = new Command(ExecuteLoadDevices)); }
		}

		public const string ErrorMessagePropertyName = "CustomLoading";
		private bool _customLoading = false;
		public bool CustomLoading
		{
			get { return _customLoading; }
			set
			{
				if (value.Equals(_customLoading)) return;

				_customLoading = value;
				OnPropertyChanged();
			}
		}

		public bool IsRunningLoader
		{ //Property that will be used to get and set the item
			get { return isRunningLoader; }

			set
			{
				isRunningLoader = value;
				if (PropertyChanged != null)
				{
					PropertyChanged(this,
						new PropertyChangedEventArgs("IsRunningLoader"));// Throw!!
				}
			}
		}

		public DashboardViewModel()
		{
			try
			{
				DeviceList = new ObservableCollection<DeviceStatic>();
			}
			catch { }
			
		}

		public DashboardViewModel(INavigation navigation)
		{
			try
			{
				Navigation = navigation;
				DeviceList = new ObservableCollection<DeviceStatic>();
			}
			catch { }
		}

		public async void ExecuteLoadDevices()
		{
			try
			{
                if (!moreDevicesListExists)
                    return;
				IsBusy = true;
				UserDialogs.Instance.ShowLoading("Loading...");
				IsRunningLoader = true;
				
				await LoadDevices();

				IsBusy = false;
				IsRunningLoader = false;
				

				UserDialogs.Instance.HideLoading();

				
			}
			catch (System.Exception ex)
			{
			}
		}

		public async Task LoadDevices()
		{
			try
			{
				DeviceStatic req = new DeviceStatic();
				req.RecordPerPage = 20;
				if (DeviceList.Count() == 0)
				{
					req.PageIndex = 0;
				}
				else
				{
					req.PageIndex = s + 1;
					s = req.PageIndex;
				}

				
				req.CreatedBy = Settings.UserID;
				
				var Items = await ApiService.Instance.GetDeviceList(req);
                if(Items.Count<20)
                {
                    moreDevicesListExists = false;
                }
				foreach (var item in Items)
				{
					var found = DeviceList.Any(c => c.FriendlyName == item.FriendlyName);
					if (!found)
					{
						DeviceList.Add(item);
					}
				}
			}
			catch (Exception ex)
			{
				MyController.ErrorManagement(ex.Message);
			}
		}

	}
}

