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
using UwatchPCL.Model.Response;

namespace uWatch
{
	public class MemberViewModel : BaseViewModel
	{
		ObservableCollection<AddUserModel> memberList;

		int taps = 0;
		ICommand tapCommand;
        bool morepageexists = true;
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

		

		public ObservableCollection<AddUserModel> MemberList
		{ //Property that will be used to get and set the item
			get { return memberList; }

			set
			{
				memberList = value;
				if (PropertyChanged != null)
				{
					PropertyChanged(this,
						new PropertyChangedEventArgs("MemberList"));// Throw!!
				}
			}
		}
		
		public int s = 0;

		public MemberStatics Device { get; set; }
		public int DeviceCounts { get; set; }

		private INavigation Navigation;

		private Command loadDeviceCommand;
		public ICommand LoadCharactersCommand
		{
			get { return loadDeviceCommand ?? (loadDeviceCommand = new Command(ExecuteLoadDevices)); }
		}

		public MemberViewModel()
		{
			try
			{
				
				MemberList = new ObservableCollection<AddUserModel>();
			}
			catch { }
			
		}

		public MemberViewModel(INavigation navigation)
		{
			try
			{
				Navigation = navigation;
				MemberList = new ObservableCollection<AddUserModel>();
			}
			catch { }
		}

		public async void ExecuteLoadDevices()
		{
			try
			{
                if (morepageexists)
                {
                    await LoadDevices();
                }
				
			}
			catch { }
		}

		public async Task LoadDevices()
		{
			try
			{
				MemberStatics req = new MemberStatics();
				req.RecordPerPage = 25;
				if (MemberList.Count() == 0)
				{
					req.PageIndex = 0;
				}
				else
				{
					req.PageIndex = s + 1;
					s = req.PageIndex;
				}
				req.User_Idx = Settings.UserID;
				
				req.CreatedBy = Settings.UserID;
				
				var Items = await ApiService.Instance.GetMemberLists(req);
                if (Items.Count() < 25)
                {
                    morepageexists = false;
                }
  				foreach (var item in Items)
				{
					
                    var found = MemberList.Any(c => c.User_Idx == item.User_Idx);

					if (!found)
					{
						MemberList.Add(item);
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

