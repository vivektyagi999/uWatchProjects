using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using uWatch.ViewModels;
using UwatchPCL;
using UwatchPCL.Model;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Acr.UserDialogs;

namespace uWatch
{
    public class DeviceDetailsViewModel : BaseViewModel
    {

        public ICommand SaveCommand { get; set; }
        public Command loadMoreAssetCommand;
        public int PageIndex = 0;
        int i = 0;
        public int AlertCounts { get; set; }

        public string LastSignalImage { get; set; }

        public string LastBatteryImage { get; set; }
        public int totalAssets{ get; set; }
        public List<Position> AssetsPositions { get; set; }
        public bool IsMoreExist = true;
        public DeviceStatic device { get; set; }
        public ObservableCollection<DeviceAssetsModel> AssetsList { get; set; }
        public Image[] ImageList { get; set; }
        public Image CurrentImage { get; set; }
        public ObservableCollection<ObservableCollection<DeviceAssetsModel>> _AssetsLists;
        public ObservableCollection<ObservableCollection<DeviceAssetsModel>> AssetsLists
        {
            get { return _AssetsLists; }
            set
            {
                SetProperty(ref _AssetsLists, value);
            }
        }


        public Command LoadMoreAssetCommand
        {
            get { return loadMoreAssetCommand ?? (loadMoreAssetCommand = new Command(() => { ExecuteLoadMoreAssetCommandAsync(); })); }
        }

        private async Task ExecuteLoadMoreAssetCommandAsync()
        {
            if(IsMoreExist)
            {
                UserDialogs.Instance.ShowLoading();
                var moreAsset = await GetDeviceAssets(0);
                UserDialogs.Instance.HideLoading();
            }
        }

        public DeviceDetailsViewModel()
        {

        }
        public const string ErrorMessagePropertyName = "DisplayError";
        private bool _displayError = false;
        public bool DisplayError
        {
            get { return _displayError; }
            set
            {
                if (value.Equals(_displayError)) return;

                _displayError = value;
                OnPropertyChanged();
            }
        }

        public DeviceDetailsViewModel(int deviceid)
        {
            try
            {

            }
            catch { }

        }

        public async Task GetDetails(int deviceid)
        {
            try
            {
                DeviceStatic req = new DeviceStatic();
                req.device_idx = deviceid;
                device = await ApiService.Instance.GetDeviceDetails(req).ConfigureAwait(false);
                req.RecordPerPage = 50;
                req.PageIndex = 0;

                var AlertList = await ApiService.Instance.GetAlertsList(req).ConfigureAwait(false);//What use of this line
                AlertCounts = AlertList == null ? 0 : AlertList.ToList().Count;
                if (AlertList != null)
                {
                    var lastAlert = AlertList.ToList().OrderByDescending(x => x.DeviceDate).FirstOrDefault();
                    if (lastAlert != null)
                    {
                        var Abattery = lastAlert.Battery == null ? 0 : Convert.ToInt32(lastAlert.Battery);
                        LastBatteryImage = await MyController.BatteryLevelImage(Abattery);

                        var Asignal = lastAlert.Signal == null ? 0 : Convert.ToInt32(lastAlert.Signal);
                        LastSignalImage = await MyController.SignalLevelImage(Asignal);
                    }
                }
                AssetsList = await ApiService.Instance.GetAssetList(req).ConfigureAwait(false);

                foreach (var item in AlertList)
                {
                    if ((item.lat > 0) && (item.lang > 0))
                    {

                        if (AssetsPositions == null)
                            AssetsPositions = new List<Position>();
                        AssetsPositions.Add(new Position(Convert.ToDouble(item.lat), Convert.ToDouble(item.lang)));

                    }
                }
                if (AssetsList != null && AssetsList.Count > 0)
                {
                    await ModeltoImage();
                    if (ImageList != null)
                    {
                        if (ImageList[0] != null)
                        {
                            CurrentImage = ImageList[0];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MyController.ErrorManagement(ex.Message);
            }
        }

        public async Task<ObservableCollection<DeviceAssetsModel>> GetDeviceAssets(int deviceid)
        {
            try
            {
                
                DisplayError = true;
                DeviceStatic req = new DeviceStatic();
                req.device_idx = deviceid;
                req.RecordPerPage = 8;
                req.PageIndex = PageIndex;
                AssetsList = await ApiService.Instance.GetAssetList(req);

                if (AssetsList != null)
                {
                    if (AssetsList.Count <8)
                        IsMoreExist = false;
                    foreach (var item in AssetsList)
                    {
                        item.Position_no = AssetsList.IndexOf(item);
                        totalAssets = item.TotalAssets.Value;
                    }
                    if (PageIndex==0)
                        AssetsLists = new ObservableCollection<ObservableCollection<DeviceAssetsModel>>();
                    PageIndex++;
                    SetAssetListGridImage(AssetsList);
                }
                if (AssetsList != null)
                    await ModeltoImage();

                DisplayError = false;
            }

            catch (Exception ex)
            {
                DisplayError = false;
            }
            return AssetsList;
        }

        private void SetAssetListGridImage(ObservableCollection<DeviceAssetsModel> assetsList)
        {
            
            var take = assetsList.Count;
            int skip = 0;
            while (skip < take)
            {
                AssetsLists.Add(new ObservableCollection<DeviceAssetsModel>(assetsList.Skip(skip).Take(2).ToList()));
                skip += 2;
            }
        }

        private async Task ModeltoImage()
        {
            try
            {
                ImageList = new Image[AssetsList.Count];

                foreach (var item in AssetsList)
                {
                    Image img = new Image();
                    ImageList[i] = (BytesArraytoImage(item.Deviceimage_thumbnail));
                    i++;
                }
            }
            catch (Exception ex)
            {
                MyController.ErrorManagement(ex.Message);
            }
        }

        private Image BytesArraytoImage(byte[] stream)
        {

            Image img = new Image();

            try
            {
                byte[] imagedata = stream;
                img.Source = ImageSource.FromStream(() => new MemoryStream(imagedata));
            }
            catch (Exception ex)
            {
                MyController.ErrorManagement(ex.Message);
            }
            return img;
        }


    }
}

