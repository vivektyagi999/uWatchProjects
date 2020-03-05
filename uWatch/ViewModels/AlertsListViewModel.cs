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
using UwatchPCL.Helpers;

namespace uWatch
{
    public class AlertsListViewModel : BaseViewModel
    {
        public int AlertCounts { get; set; }
        ObservableCollection<AlertsEsclatedToAgentViewModel> alertlist;
        public bool isRunningLoader = true;
        ObservableCollection<DeviceStatic> alertLastLocStatic;
        public bool loadMoreAlertList = true;
        bool isRefresh = false;
        public override event PropertyChangedEventHandler PropertyChanged;


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
        public ObservableCollection<DeviceStatic> AlertLastLocStatic
        { //Property that will be used to get and set the item
            get { return alertLastLocStatic; }

            set
            {
                alertLastLocStatic = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this,
                        new PropertyChangedEventArgs("AlertLastLocStatic"));// Throw!!
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



        private int UserID;
        private int pageindex;
        public int s = 0;
        public AlertsEsclatedToAgentViewModel Alert { get; set; }

        private INavigation Navigation { get; set; }

        public AlertsListViewModel()
        {
            AlertLastLocStatic = new ObservableCollection<DeviceStatic>();
        }

        public AlertsListViewModel(INavigation navigation, int userid)
        {
            try
            {
#if __IOS__
                Device.BeginInvokeOnMainThread(() =>
                {
                     ICustomContextActionsManager _manager = DependencyService.Get<ICustomContextActionsManager>();
                    _manager.SetCustomView(new DeleteContextActionView(), true);
                });
#endif

                this.UserID = userid;
                this.Navigation = navigation;
                if (AlertList == null)
                    AlertLastLocStatic = new ObservableCollection<DeviceStatic>();
                AlertList = new ObservableCollection<AlertsEsclatedToAgentViewModel>();
                AlertCounts = AlertList.Count();

            }
            catch (Exception ex)
            {

            }
        }

        public async void TestLoader()
        {
            CustomLoading = true;
            await Task.Delay(1000);
            CustomLoading = false;
        }

        public async void ExecuteLoadAlerts()
        {
            try
            {
                var networkConnection = DependencyService.Get<INetworkConnection>();
                networkConnection.CheckNetworkConnection();
                var networkStatus = networkConnection.IsConnected ? "Connected" : "Not Connected";
                if (networkStatus != "Connected")
                {
                    UserDialogs.Instance.Alert("Please check your internet connection.", "Alert", "OK");
                    return;
                }

                if (!loadMoreAlertList || isRefresh)
                {
                    return;
                }
                isRefresh = true;
                Device.BeginInvokeOnMainThread(() =>
                {
                    UserDialogs.Instance.ShowLoading("Loading...");
                });
                if (Settings.RoleID == 3)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await LoadAlertListOfAgent();
                        UserDialogs.Instance.HideLoading();

                    });

                }
                else
                {

                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await LoadAlertList();
                        UserDialogs.Instance.HideLoading();

                    });
                }


                //IsRunningLoader = false;
                //IsBusy = false;
            }
            catch
            {
                CustomLoading = false;
            }
        }

        public async Task LoadAlertListOfAgents()
        {
            try
            {
                EscalatedAlertReq reqs = new EscalatedAlertReq();
                reqs.AgentID = Settings.UserID;
                reqs.DeviceID = 0;
                reqs.UserID = 0;
                reqs.PageSize = 10;

                if (AlertList.Count() == 0)
                    reqs.PageIndex = 0;
                else
                {
                    reqs.PageIndex = s + 1;
                    s = reqs.PageIndex;
                }
                var Items = await ApiService.Instance.GetEscalatedAlertOfAgent(reqs).ConfigureAwait(false);
                if (Items.Count < 10)
                {
                    loadMoreAlertList = false;
                }

                foreach (var item in Items)
                {
                    if (!AlertList.Contains(item))
                    {

                        if (item.image_id)
                        {
                            item.AlertImageIcon = await MyController.GetAlertTypelImage((int)MyController.SENSOR_TYPE.BATCH);
                        }
                        else if (item.alert_type == (int)MyController.SENSOR_TYPE.HEARTBEAT || item.alert_type == (int)MyController.SENSOR_TYPE.SWITCHON || item.alert_type == (int)MyController.SENSOR_TYPE.SWITCHOFF)
                        {

                        }
                        else if (item.ImageComing != null)
                        {
                            if (item.ImageComing.Value)
                                item.AlertImageIcon = "ImageComing.png";
                        }
                        var day = item.DeviceDate.ToString("ddd");
                        var date = DateFormat.GetDateTime(item.DeviceDate, TimeType.DateAndTime);
                        item.AlertDateTime = day + " " + date;
                        item.FullName = item.FullName;
                        item.Mobile1 = item.Mobile1;
                        item.AlertTypeName = MyController.GetAlertTypeName(item.alert_type);
                        item.AlertTypeImage = await MyController.GetAlertTypelImage(item.alert_type);
                        item.TemperatureImage = await MyController.TempratureLevelImage(item.DegC);
                        item.SignalImage = await MyController.SignalLevelImage(item.Signal);
                        item.BatteryImage = await MyController.BatteryLevelImage(item.Battery);
                        if (item.EscalateToAreaManager == null && item.EscalateToAgentID != null)
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
        public async Task LoadAlertListOfAgent()
        {
            try
            {
                EscalatedAlertReq reqs = new EscalatedAlertReq();
                reqs.AgentID = Settings.UserID;
                reqs.DeviceID = 0;
                reqs.UserID = 0;
                reqs.PageSize = 10;


                if (AlertList.Count() == 0)
                    reqs.PageIndex = 0;
                else
                {
                    reqs.PageIndex = s + 1;
                    s = reqs.PageIndex;
                }
                var Items = await ApiService.Instance.GetEscalatedAlertOfAgent(reqs).ConfigureAwait(false);
                if (Items.Count < 10)
                {
                    loadMoreAlertList = false;
                }

                foreach (var item in Items)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        if (!AlertList.Contains(item))
                        {

                            if (item.image_id)
                            {
                                item.AlertImageIcon = await MyController.GetAlertTypelImage((int)MyController.SENSOR_TYPE.BATCH);
                            }
                            else if (item.alert_type == (int)MyController.SENSOR_TYPE.HEARTBEAT || item.alert_type == (int)MyController.SENSOR_TYPE.SWITCHON || item.alert_type == (int)MyController.SENSOR_TYPE.SWITCHOFF)
                            {

                            }
                            else if (item.ImageComing != null)
                            {
                                if (item.ImageComing.Value)
                                    item.AlertImageIcon = "ImageComing.png";
                            }
                            var day = item.DeviceDate.ToString("ddd");
                            var date = DateFormat.GetDateTime(item.DeviceDate, TimeType.DateAndTime);
                            item.AlertDateTime = day + " " + date;
                            item.FullName = item.FullName;
                            item.Mobile1 = item.Mobile1;
                            item.AlertTypeName = MyController.GetAlertTypeName(item.alert_type);
                            item.AlertTypeImage = await MyController.GetAlertTypelImage(item.alert_type);
                            item.TemperatureImage = await MyController.TempratureLevelImage(item.DegC);
                            item.SignalImage = await MyController.SignalLevelImage(item.Signal);
                            item.BatteryImage = await MyController.BatteryLevelImage(item.Battery);
                            if (item.EscalateToAreaManager == null && item.EscalateToAgentID != null)
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
                    });
                }

            }
            catch (Exception ex)
            {
                MyController.ErrorManagement(ex.Message);
                Device.BeginInvokeOnMainThread(() =>
                {
                    UserDialogs.Instance.HideLoading();

                });
            }
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
                Device.BeginInvokeOnMainThread(() =>
                {

                    UserDialogs.Instance.HideLoading();

                });
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
            config.datetime = CreatedDate.ToString("dd-MMM-yyyy hh:mm tt");
            bool AssetList = false;
            try
            {
                AssetList = await ApiService.Instance.ChangeCubeConfigration(config).ConfigureAwait(false);
            }
            catch
            {

            }
            return AssetList;
        }
        public async Task LoadAlertLists()
        {
            try
            {
                //get the last location of Alert

                DeviceStatic reqs = new DeviceStatic();
                reqs.RecordPerPage = 30;

                reqs.PageIndex = 0;

                reqs.CreatedBy = Settings.UserID;

                var AlertLOcationItems = await ApiService.Instance.GetDeviceList(reqs);


                if (AlertLOcationItems != null)
                {
                    foreach (var item in AlertLOcationItems)
                    {

                        AlertLastLocStatic.Add(item);

                    }
                }

                // get the list of alert
                DeviceStatic req = new DeviceStatic();
                req.OwnerUserID = UserID;
                req.RecordPerPage = 30;
                if (AlertList.Count() == 0)
                    req.PageIndex = 0;
                else
                {
                    req.PageIndex = s + 1;
                    s = req.PageIndex;
                }
                var Items = await ApiService.Instance.GetAlertsListByUser(req).ConfigureAwait(false);
                if (Items.Count < 30)
                {
                    loadMoreAlertList = false;
                }
                foreach (var item in Items)
                {

                    if (!AlertList.Contains(item))
                    {

                        if (item.image_id)
                        {
                            item.AlertImageIcon = await MyController.GetAlertTypelImage((int)MyController.SENSOR_TYPE.BATCH);
                        }
                        else if (item.alert_type == (int)MyController.SENSOR_TYPE.HEARTBEAT || item.alert_type == (int)MyController.SENSOR_TYPE.SWITCHON || item.alert_type == (int)MyController.SENSOR_TYPE.SWITCHOFF)
                        {

                        }
                        else if (item.ImageComing != null)
                        {
                            if (item.ImageComing.Value)
                                item.AlertImageIcon = "ImageComing.png";
                        }
                        var day = item.DeviceDate.ToString("ddd");
                        var date = DateFormat.GetDateTime(item.DeviceDate, TimeType.DateAndTime);
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

        public async Task LoadAlertList()
        {
            try
            {
                //get the last location of Alert

                DeviceStatic reqs = new DeviceStatic
                {
                    RecordPerPage = 30,

                    PageIndex = 0,


                    CreatedBy = Settings.UserID
                };

                var AlertLOcationItems = await ApiService.Instance.GetDeviceList(reqs);


                if (AlertLOcationItems != null)
                {
                    foreach (var item in AlertLOcationItems)
                    {

                        AlertLastLocStatic.Add(item);

                    }

                }

                // get the list of alert
                DeviceStatic req = new DeviceStatic
                {
                    OwnerUserID = UserID,
                    RecordPerPage = 30
                };
                if (!AlertList.Any())
                    req.PageIndex = 0;
                else
                {
                    req.PageIndex = s + 1;
                    s = req.PageIndex;
                }
                var Items = await ApiService.Instance.GetAlertsListByUser(req).ConfigureAwait(false);
                isRefresh = false;
                if (Items.Count < 30)
                {
                    loadMoreAlertList = false;
                }
                if (s == 0)
                {
                    AlertList = new ObservableCollection<AlertsEsclatedToAgentViewModel>();
                }
                foreach (var item in Items)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {

                        if (!AlertList.Contains(item))
                        {

                            if (item.image_id)
                            {
                                item.AlertImageIcon = await MyController.GetAlertTypelImage((int)MyController.SENSOR_TYPE.BATCH);
                            }
                            else if (item.alert_type == (int)MyController.SENSOR_TYPE.HEARTBEAT || item.alert_type == (int)MyController.SENSOR_TYPE.SWITCHON || item.alert_type == (int)MyController.SENSOR_TYPE.SWITCHOFF)
                            {

                            }
                            else if (item.ImageComing != null)
                            {
                                if (item.ImageComing.Value)
                                    item.AlertImageIcon = "ImageComing.png";
                            }



                            var day = item.DeviceDate.ToString("ddd");
                            var date = DateFormat.GetDateTime(item.DeviceDate, TimeType.DateAndTime);
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
                    });
                }
            }
            catch (Exception ex)
            {
                MyController.ErrorManagement(ex.Message);
                Device.BeginInvokeOnMainThread(() =>
                {
                    UserDialogs.Instance.HideLoading();
                });
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
                    await this.Navigation.PushAsync(new AlertDetail(Alert));
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

