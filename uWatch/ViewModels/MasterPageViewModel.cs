using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using uWatch.ViewModels;
using UwatchPCL;
using UwatchPCL.Helpers;

namespace uWatch
{
	public class MasterPageViewModel : BaseViewModel
	{
		private ObservableCollection<MenuItem> menuItems;
		public ObservableCollection<MenuItem> MenuItems
		{
			get { return menuItems; }

			set
			{
				menuItems = value;
				if (PropertyChanged != null)
				{
					OnPropertyChanged("MenuItems");
				}
			}
		}
		private static MasterPageViewModel instance;
		public static MasterPageViewModel Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new MasterPageViewModel();
				}
				return instance;
			}
		}
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

		public string FullName { get; set; }

		public string UserId { get; set; }

		
		public  MasterPageViewModel()
		{
			try
			{
				MenuItems = new ObservableCollection<MenuItem>();
				FullName = Settings.FullName;
				UserId = Settings.UserName.ToString();
				if (Settings.RoleID == 3)
				{
					MenuItems.Add(new MenuItem { Title = "Escalated Alerts", IconSource = "left_alert.png", TargetType = typeof(AlertListPage) });
					MenuItems.Add(new MenuItem { Title = "Members", IconSource = "member.png", TargetType = typeof(MembersListPage) });

				}

				else
				{
					if (Settings.RoleID != 8)
					{
						MenuItems.Add(new MenuItem { Title = "Cubes", IconSource = "left_moniter.png", TargetType = typeof(DevicesPage) });

						MenuItems.Add(new MenuItem { Title = "Alerts", IconSource = "left_alert.png", TargetType = typeof(AlertListPage) });

					}
				}
				//if (Settings.RoleID != 3 && Settings.RoleID != 9)
				//{
				//	MenuItems.Add(new MenuItem { Title = "Assets", IconSource = "left_setting.png", TargetType = typeof(AssetsPage) });
				//}
                //Inventory
                if (Settings.RoleID == 8)
                {
                    MenuItems.Add(new MenuItem { Title = "Inventory", IconSource = "invent.png", TargetType = typeof(Inventory) });
                }
				MenuItems.Add(new MenuItem { Title = "Messages", IconSource = "messagesNewIcon.png", TargetType = typeof(MessegeListPage)});
				MenuItems.Add(new MenuItem { Title = "Help & Support", IconSource = "left_help.png", TargetType = typeof(HelpAndSupportPage) });
				MenuItems.Add(new MenuItem { Title = "Logout", IconSource = "logoutNewIcon.png", TargetType = typeof(Login) });

			}
			catch { }
		}


	}
}

