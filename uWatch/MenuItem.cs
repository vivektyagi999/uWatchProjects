using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using uWatch.ViewModels;
using UwatchPCL;
using UwatchPCL.Helpers;
using UwatchPCL.Model;

namespace uWatch
{
	public class MenuItem:BaseModel
	{
		private static MenuItem instance;
		public static MenuItem Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new MenuItem();
				}
				return instance;
			}
		}
		public MenuItem()
		{
			
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
		public string Title { get; set; }

		public string IconSource { get; set; }

		public Type TargetType { get; set; }
		private  string messagecount;
		public  string MessageCount 
		{ 
			get { return messagecount; }

			set
			{
				messagecount = value;
				if (PropertyChanged != null)
				{
					OnPropertyChanged("MessageCount");
				}
			}
		}

		public  async Task GetUnreadMessage()
		{
			try
			{
				var MCount =await ApiService.Instance.GetUnreadMessageCount(Settings.UserID);
				if (MCount == "0")
				{
					MessageCount = "";
				}
				else
				{
					MessageCount = MCount;
				}

				
			}
			catch (System.Exception ex)
			{
			}
		}
		async void Timer()
		{
			try
			{
				int i = 1;
				Xamarin.Forms.Device.StartTimer(TimeSpan.FromSeconds(2), () =>
				{
					
					return true;
				});
			}
			catch { }
		}
	}

	public enum MenuType
	{
		Devices,
		Assets,
		AlertList,
		Logout
	}
}

