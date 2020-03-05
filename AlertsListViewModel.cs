using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using uWatch.ViewModels;
using UwatchPCL;
using Xamarin.Forms;
using System.Collections.Generic;
using System.ComponentModel;

namespace uWatch
{
	public class AlertsListViewModel : BaseViewModel
	{
		public int AlertCounts { get; set; }
		ObservableCollection<AlertsEsclatedToAgentViewModel> alertlist;
		public event PropertyChangedEventHandler PropertyChanged;

	//	private ObservableCollection<AlertsEsclatedToAgentViewModel> alertlist;
		//public ObservableCollection<AlertsEsclatedToAgentViewModel> AlertList
		//{
		//	get
		//	{
		//		return alertlist;
		//	}
		//	set
		//	{
		//		alertlist = value;
		//		OnPropertyChanged("AlertList");
		//	}
		//}


		public ObservableCollection<AlertsEsclatedToAgentViewModel> AlertList
		{ //Property that will be used to get and set the item
			get { return alertlist; }

			set
			{
				alertlist = value;
				if (PropertyChanged != null)
				{
					PropertyChanged(this,
						new PropertyChangedEventArgs("AlertList"));// Throw!!
				}
			}
		}


		private Command loadMoreCommand;
		public ICommand LoadCharactersCommand
		{
			get { return loadMoreCommand ?? (loadMoreCommand = new Command(ExecuteLoadAlerts)); }
		}

		private Command loadAlertCommand;
		public ICommand LoadAlertCommand
		{
			get { return loadAlertCommand ?? (loadAlertCommand = new Command(ExecuteLoadAlertDetails)); }
		}

		//public ICommand LoadAlertCommand { get; set; }

		private int UserID;
		private int pageindex;
		public int s = 0;
		public AlertsEsclatedToAgentViewModel Alert { get; set; }

		private INavigation Navigation { get; set;}

		public AlertsListViewModel()
		{

		}

		public AlertsListViewModel(INavigation navigation, int userid)
		{
			try
			{
				this.UserID = userid;
				this.Navigation = navigation;
				if (AlertList == null)
					AlertList = new ObservableCollection<AlertsEsclatedToAgentViewModel>();
				AlertCounts = AlertList.Count();
				//LoadCharactersCommand = new Command(LoadCharacters);
				//LoadAlertCommand = new Command(LaodAlertDetails);
			}
			catch { }
		}


		public async void ExecuteLoadAlerts()
		{
			try
			{
				IsBusy = true;
				UserDialogs.Instance.ShowLoading("Loading...");

				await LoadAlertList();

				UserDialogs.Instance.HideLoading();
				IsBusy = false;
			}
			catch { }
		}


		public async Task LoadAlertInExistingList(int ExistingAlertId)
		{
			try
			{
				AlertsEsclatedToAgentViewModel req = new AlertsEsclatedToAgentViewModel();
				req.alertlog_idx = ExistingAlertId;
				var updatedAlert = await ApiService.Instance.GetAlert(req);
				var itemOfList = AlertList.Where(x => x.alertlog_idx == ExistingAlertId).FirstOrDefault();
				if (updatedAlert.EscalateTo == null && updatedAlert.EscalateToAgentID != null)
				{
					itemOfList.EscalateImage = "Escalate.png";
				}
				else if (updatedAlert.EscalateTo != null)
				{
					if (updatedAlert.EscalateMethod.ToLower() == "au")
					{
						itemOfList.EscalateImage = "Greenarrow.png";
					}
					else
					{
						itemOfList.EscalateImage = "Redarrow.png";
					}
				}
				itemOfList = updatedAlert;
			}
			catch (Exception ex)
			{
				MyController.ErrorManagement(ex.Message);
			}
		}

		public async Task<ObservableCollection<DeviceConfig>> FetchConfigurationProfileDetails()
		{
			ObservableCollection<DeviceConfig> AssetList = null;
			try
			{
				DeviceStatic req = new DeviceStatic();
				req.OwnerUserID = UserID;
				AssetList = await ApiService.Instance.FetchConfigurationProfileDetails(req).ConfigureAwait(false);
				}
			catch { }
			return AssetList;
		}


		public async Task<bool> ChangeCubeConfigration(int DeviceId, int ProfileId, DateTime CreatedDate)
		{
			SaveConfigsetting config = new SaveConfigsetting();
			config.deviceid = DeviceId;
			config.profileid = ProfileId;
			config.datetime = CreatedDate.ToString();
			bool AssetList = false;
			try
			{
				AssetList = await ApiService.Instance.ChangeCubeConfigration(config).ConfigureAwait(false);
			}
			catch { }
			return AssetList;
		}


		public async Task LoadAlertList()
		{
			try
			{
				DeviceStatic req = new DeviceStatic();
				req.OwnerUserID = UserID;
				req.RecordPerPage = 50;
				if (AlertList.Count() == 0)
					req.PageIndex = 0;
				else
				{
					req.PageIndex = s + 1;
					s = req.PageIndex;
				}
				var Items =  await ApiService.Instance.GetAlertsListByUser(req).ConfigureAwait(false);

				var Items1 = await ApiService.Instance.FetchConfigurationProfileDetails(req).ConfigureAwait(false);

				foreach (var item in Items)
				{
					if (!AlertList.Contains(item))
					{

						if (item.image_id == "1")
						{
							item.AlertImageIcon = await MyController.GetAlertTypelImage((int)MyController.SENSOR_TYPE.Image);
						}
						var day = item.DeviceDate.Date.DayOfWeek;
						var date = item.DeviceDate.ToString("dd/MM/yyyy hh:mm tt");
						item.AlertDateTime = day + " " + date;
						item.AlertTypeName = MyController.GetAlertTypeName(item.alert_type);
						item.AlertTypeImage = await MyController.GetAlertTypelImage(item.alert_type);
						item.TemperatureImage = await MyController.TempratureLevelImage(item.DegC);
						item.SignalImage = await MyController.SignalLevelImage(item.Signal);
						item.BatteryImage = await MyController.BatteryLevelImage(item.Battery);
						if (item.EscalateTo == null && item.EscalateToAgentID != null)
						{
							item.EscalateImage = "Escalate.png";
						}
						else if (item.EscalateTo != null)
						{
							if (item.EscalateMethod.ToLower() == "au")
							{
								item.EscalateImage = "Greenarrow.png";
							}
							else
							{
								item.EscalateImage = "Redarrow.png";
							}
						}
						AlertList.Add(item);
					}
				}
			}
			catch (Exception ex)
			{
				MyController.ErrorManagement(ex.Message);
			}
		}

		public async void ExecuteLoadAlertDetails()
		{
			try
			{
				var networkConnection = DependencyService.Get<INetworkConnection>();
				networkConnection.CheckNetworkConnection();
				var networkStatus = networkConnection.IsConnected ? "Connected" : "Not Connected";
				if (networkStatus == "Connected")
				{
					UserDialogs.Instance.ShowLoading("Loading...");
					if (Alert == null)
						return;
					await this.Navigation.PushAsync(new AlertDetails(Alert));
					UserDialogs.Instance.HideLoading();
				}
			}
			catch (Exception ex)
			{
				MyController.ErrorManagement(ex.Message);
			}
		}

	}
}

