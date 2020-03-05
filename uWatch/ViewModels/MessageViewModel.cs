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
	public class MessageViewModel : BaseViewModel
	{
		ObservableCollection<InMailModel> messageListInbox;
		ObservableCollection<InMailModel> messageListSendbox;
		ObservableCollection<InMailModel> messageListDrafts;
		ObservableCollection<DraftMailModel> draftMessageList;
        bool morePageExistsInbox=true;
        bool morePageExistsSendbox = true;

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

		

		public ObservableCollection<InMailModel> MessageListInbox
		{ //Property that will be used to get and set the item
			get { return messageListInbox; }

			set
			{
				messageListInbox = value;
				if (PropertyChanged != null)
				{
					PropertyChanged(this,
						new PropertyChangedEventArgs("MessageListInbox"));// Throw!!
				}
			}
		}

		public ObservableCollection<DraftMailModel> DraftMessageList
		{ //Property that will be used to get and set the item
			get { return draftMessageList; }

			set
			{
				draftMessageList = value;
				if (PropertyChanged != null)
				{
					PropertyChanged(this,
						new PropertyChangedEventArgs("DraftMessageList"));// Throw!!
				}
			}
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

		public ObservableCollection<InMailModel> MessageListSendbox
		{ //Property that will be used to get and set the item
			get { return messageListSendbox; }

			set
			{
				messageListSendbox = value;
				if (PropertyChanged != null)
				{
					PropertyChanged(this,
						new PropertyChangedEventArgs("MessageListSendbox"));// Throw!!
				}
			}
		}

		public ObservableCollection<InMailModel> MessageListDrafts
		{ //Property that will be used to get and set the item
			get { return messageListDrafts; }

			set
			{
				messageListDrafts = value;
				if (PropertyChanged != null)
				{
					PropertyChanged(this,
						new PropertyChangedEventArgs("MessageListDrafts"));// Throw!!
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

		public MessageViewModel()
		{
			try
			{

				MessageListInbox = new ObservableCollection<InMailModel>();
				MessageListSendbox = new ObservableCollection<InMailModel>();
				DraftMessageList = new ObservableCollection<DraftMailModel>();
				

			}
			catch { }
			
		}

		public MessageViewModel(INavigation navigation)
		{
			try
			{
				Navigation = navigation;

				MessageListInbox = new ObservableCollection<InMailModel>();
				MessageListSendbox = new ObservableCollection<InMailModel>();
				DraftMessageList = new ObservableCollection<DraftMailModel>();

			}
			catch { }
		}
		public void GetMessageCount()
		{
			var MCount = ApiService.Instance.GetUnreadMessageCount(Settings.UserID).Result;
			if (MCount == "0")
			{
				MessageCount = "";
			}
			else
			{
				MessageCount = MCount;
			}

		}
		public async void ExecuteLoadDevices()
		{
			try
			{
                if (!morePageExistsInbox)
                    return;
				UserDialogs.Instance.ShowLoading("Loading...");
				
				await LoadDevices();
				
				UserDialogs.Instance.HideLoading();

			}
			catch (System.Exception ex)
			{
			}
		}
		public async void ExecuteLoadDevicesend()
		{
			try
			{
                if (!morePageExistsSendbox)
                    return;
				IsBusy = true;
				
				UserDialogs.Instance.ShowLoading("Loading...");
				
				await LoadDevicesSendBox();
				
				UserDialogs.Instance.HideLoading();
				IsBusy = false;
				
			}
			catch { }
		}
		public async Task LoadDevices(string onTap = null)
		{
			try
			{
                
				if (onTap == "True")
				{
					MessageListInbox = new ObservableCollection<InMailModel>();
				}
				DeviceNotificationRequest req = new DeviceNotificationRequest();
				req.RecordPerPage = 20;
				if (MessageListInbox.Count() == 0)
				{
					req.PageIndex = 0;
				}
				else
				{
					req.PageIndex = s + 1;
					s = req.PageIndex;
				}
				req.type = MessageType.INBOX;
				
				var Items = await ApiService.Instance.GetsendInboxmessageList(req);
                if (Items.Count < 20)
                {
                    morePageExistsInbox = false;
                }
				foreach (var item in Items)
				{

					var found = MessageListInbox.Any(c => c.MailID == item.MailID);

					if (!found)
					{
						MessageListInbox.Add(item);
					}


				}
			}
			catch (Exception ex)
			{
				MyController.ErrorManagement(ex.Message);
			}
		}

		public async Task LoadDevicesDrafts()
		{
			try
			{
                
				var result =await ApiService.Instance.GetDraftList(Settings.UserID);
				foreach (var item in result)
				{

					var found = DraftMessageList.Any(c => c.DraftMailID == item.DraftMailID);

					if (!found)
					{
						DraftMessageList.Add(item);
					}


				}


			}
			catch (Exception ex)
			{
				MyController.ErrorManagement(ex.Message);
			}
		}
		public async Task LoadDevicesSendBox(string onTap = null)
		{
			try
			{
               
				if (onTap == "True")
				{
					MessageListSendbox = null;
				}

				DeviceNotificationRequest req = new DeviceNotificationRequest();
				req.RecordPerPage = 20;
				if (MessageListSendbox == null)
				{
					MessageListSendbox = new ObservableCollection<InMailModel>();

				}
				if (MessageListSendbox.Count() == 0)
				{
					req.PageIndex = 0;
				}
				else
				{
					req.PageIndex = s + 1;
					s = req.PageIndex;
				}
				req.type = MessageType.SENT;

				var Items = await ApiService.Instance.GetsendInboxmessageList(req);
                if(Items.Count<20)
                {
                    morePageExistsSendbox = false;
                }
				foreach (var item in Items)
				{

					var found = MessageListSendbox.Any(c => c.MailID == item.MailID);

					if (!found)
					{
						MessageListSendbox.Add(item);
					}

					
				}
			}
			catch (Exception ex)
			{
				MyController.ErrorManagement(ex.Message);
			}
		}

		public async Task LoadListOfUsers()
		{
			try
			{
				DeviceNotificationRequest req = new DeviceNotificationRequest();
				req.RecordPerPage = 10;
				if (MessageListSendbox == null)
				{
					MessageListSendbox = new ObservableCollection<InMailModel>();

				}
				if (MessageListSendbox.Count() == 0)
				{
					req.PageIndex = 0;
				}
				else
				{
					req.PageIndex = s + 1;
					s = req.PageIndex;
				}
				req.type = MessageType.SENT;

				var Items = await ApiService.Instance.GetsendInboxmessageList(req);
				foreach (var item in Items)
				{

					var found = MessageListSendbox.Any(c => c.MailID == item.MailID);

					if (!found)
					{
						MessageListSendbox.Add(item);
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

