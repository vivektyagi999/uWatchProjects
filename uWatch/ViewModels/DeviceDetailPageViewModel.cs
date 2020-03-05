using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Acr.UserDialogs;
using uWatch.ViewModels;
using UwatchPCL;
using Xamarin.Forms;
using Rg.Plugins.Popup.Extensions;
//using Xamarin.Media;
using UwatchPCL.Animations;

namespace uWatch
{
	public class DeviceDetailPageViewModel:BaseViewModel
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(
			[CallerMemberName] string propertyName = null)
		{
			var handler = PropertyChanged;
			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		public string DeviceId
		{
			get;
			set;
		}
		public string DeviceName
		{
			get;
			set;
		}

		Setting currentConfigrationsettings;
		public Setting CurrentConfigrationsettings
		{ //Property that will be used to get and set the item
			get { return currentConfigrationsettings; }

			set
			{
				currentConfigrationsettings = value;
				if (PropertyChanged != null)
				{
					PropertyChanged(this,
						new PropertyChangedEventArgs("CurrentConfigrationsettings"));// Throw!!
				}
			}
		}
		public DeviceDetailPageViewModel()
		{
			CurrentConfigrationsettings = new Setting();

		}
		public async  void GetDeviceConfigurationDetails()
		{
			var networkConnection = DependencyService.Get<INetworkConnection>();
			networkConnection.CheckNetworkConnection();
			var networkStatus = networkConnection.IsConnected ? "Connected" : "Not Connected";
			if (networkStatus != "Connected")
			{
				
				UserDialogs.Instance.Alert("Please check your internet connection.", "Alert", "OK");
				return;
			}

			CurrentConfigrationsettings = await ApiService.Instance.FetchDeviceConfigurationDetails(DeviceId).ConfigureAwait(true);
			  	
		}
	}
}
