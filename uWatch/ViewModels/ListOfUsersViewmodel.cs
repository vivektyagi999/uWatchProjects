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
	public class ListOfUsersViewmodel : BaseViewModel
	{
		ObservableCollection<ListOfUsersResponce> userList;

		int taps = 0;
		ICommand tapCommand;

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

		

		public ObservableCollection<ListOfUsersResponce> UserList
		{ //Property that will be used to get and set the item
			get { return userList; }

			set
			{
				userList = value;
				if (PropertyChanged != null)
				{
					PropertyChanged(this,
						new PropertyChangedEventArgs("UserList"));// Throw!!
				}
			}
		}


		

		public int s = 0;

		public ListOfDeviceNotificationResponce Device { get; set; }
		public int DeviceCounts { get; set; }

		private INavigation Navigation;

		private Command loadDeviceCommand;
		public ICommand LoadCharactersCommand
		{
			get { return loadDeviceCommand ?? (loadDeviceCommand = new Command(ExecuteLoadDevices)); }
		}
		private Command loadDeviceCommandSend;
		public ICommand LoadCharactersCommandSend
		{
			get { return loadDeviceCommandSend ?? (loadDeviceCommandSend = new Command(ExecuteLoadDevicesend)); }
		}

		public ListOfUsersViewmodel()
		{
			try
			{
				
				UserList = new ObservableCollection<ListOfUsersResponce>();

			}
			catch { }
			
		}

		public ListOfUsersViewmodel(INavigation navigation)
		{
			try
			{
				Navigation = navigation;
				UserList = new ObservableCollection<ListOfUsersResponce>();

			}
			catch { }
		}

		public async void ExecuteLoadDevices()
		{
			try
			{
				IsBusy = true;
				
				await LoadDevices();
				
				IsBusy = false;
				
			}
			catch { }
		}
		public async void ExecuteLoadDevicesend()
		{
			try
			{
				IsBusy = true;
				
				IsBusy = false;
				
			}
			catch { }
		}
		public async Task LoadDevices()
		{
			try
			{
				ListOfUsersViewmodel list = new ListOfUsersViewmodel();
				var Items = await ApiService.Instance.GetuserList();

				int j = 0;
				foreach (var iteminList in Items)
				{
					ListOfUsersResponce item = new ListOfUsersResponce();
					item.SNo = j + 1;
					item.Image = "Checkbox.png";
					//item.UserId = iteminList.;
					item.Name = iteminList.Text;
                    item.Value = iteminList.Value;
					list.UserList.Add(item);
					UserList.Add(item);
					j++;
				}

			}
			catch (Exception ex)
			{
				MyController.ErrorManagement(ex.Message);
			}
		}





	}
}

