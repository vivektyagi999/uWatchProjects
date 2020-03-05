using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UwatchPCL.Helpers;
using UwatchPCL.Model;
using UwatchPCL.Model.Response;
using Xamarin.Forms;

namespace UwatchPCL.WebServices
{
    public class WebServiceManager : ServiceConfigrations
    {
        IWebService webService;
        public WebServiceManager()
        {
            webService = new WebService();
        }
        public async Task<WebServiceResult<AccountModel>> SignUpForAppAsync(SignUpModel userDetail)
        {
            var data = JsonConvert.SerializeObject(userDetail, Formatting.None);
            var result = await webService.PostAsync<AccountModel>(SignUpForApp, data, false);
            return result;
        }
        public async Task<WebServiceResult<AppDetails>> CheckLatestAppVersionAsync(AppDetails appDetails)
        {
            var data = JsonConvert.SerializeObject(appDetails);
            var result = await webService.PostAsync<AppDetails>(CheckLatestAppVersionUrl, data,false);
            return result;
        }

        public async Task<WebServiceResult<AppTokenModel>> CheckUserTokenAsync(DeviceDetailsModel deviceDetails)
        {
            var data = JsonConvert.SerializeObject(deviceDetails);
            var result = await webService.PostAsync<AppTokenModel>(CheckUserTokenUrl, data,false);
            return result;
        }
        public async Task<WebServiceResult<bool>> LoginWithTagAsync(BleTransConfigModel userDetail)
        {
            var homeDefaultConfig = new HomeDefaultConfig();

            homeDefaultConfig.DefaultConfigID = userDetail.Configuration_idx;
            homeDefaultConfig.SaveTag = userDetail.SaveTag;
            homeDefaultConfig.CreatedBy = userDetail.CreatedBy;
            if (userDetail.SaveTag)
            {
                var bleTrans = new BLETransTag();
                if (!string.IsNullOrEmpty(userDetail.FriendlyName))
                {
                    bleTrans.FriendlyName = userDetail.FriendlyName;
                }
                bleTrans.MacAddress = userDetail.MacAddress;
                bleTrans.SlaveSleepTime = userDetail.SlaveSleepTime.Value;
                var lstBleTrans = new List<BLETransTag>();
                lstBleTrans.Add(bleTrans);
                homeDefaultConfig.BlueTransTags = lstBleTrans;
               
            }
            var data = JsonConvert.SerializeObject(homeDefaultConfig, Formatting.None);
            var result = await webService.PostAsync<bool>(SignUpWithTag, data, false);
            return result;
        }

        public async Task<WebServiceResult<bool>> ResetPasswordAsync(ResetPasswordModel resetDetail)
        {

            var data = JsonConvert.SerializeObject(resetDetail, Formatting.None);
            var result = await webService.PostAsync<bool>(ResetPassword, data, true);
            return result;
        }

        public async Task<WebServiceResult<BLETagLinks>> GetDeviceConfigurationLinkAsync(int DeviceId)
        {
            DeviceConfig deviceConfig = new DeviceConfig();
            deviceConfig.device_idx = DeviceId;
            var data = JsonConvert.SerializeObject(deviceConfig, Formatting.None);
            var result = await webService.PostAsync<BLETagLinks>(GetDeviceConfigurationLink, data, true);
            return result;
        }

        public async Task<WebServiceResult<objTagData>> SaveTagsToCubefromAppAsync(List<BLETransTag> selectedLinkTags, int deviceId)
        {
            objTagData saveTagsLink = new objTagData();
            saveTagsLink.DeviceID = deviceId;
            saveTagsLink.objTags = selectedLinkTags;
            var data = JsonConvert.SerializeObject(saveTagsLink, Formatting.None);
            var result = await webService.PostAsync<objTagData>(AddTagsToCubefromApp, data, false);
            return result;
        }


        public async Task<WebServiceResult<AccessCodeModel>> GetUserDetailsByAccessCodeAsync(string accessCode)
        {
            var result = await webService.GetAsync<AccessCodeModel>(GetUserDetailByAccessCode + accessCode, "", true);
            return result;
        }

        public async Task<WebServiceResult<bool>> GetCancelDeviceOffConfigAsync(int DeviceId)
        {
            var result = await webService.GetAsync<bool>(GetCancelDeviceOffConfigUrl + DeviceId, "", false);
            return result;
        }

        public async Task<WebServiceResult<int>> GetUserDetailsByProductCodeAsync(string serialNumber, string productCode)
        {
            var result = await webService.GetAsync<int>(GetUserDetailByProductCode + serialNumber + "&model_no=" + productCode, "", true);
            return result;
        }

        public async Task<WebServiceResult<AppUpdateInfo>> GetCheckAppUpdateAsync()
        {

            var result = await webService.GetAsync<AppUpdateInfo>(CheckAppUpdate + Settings.CurrentVersion + "&Platform=" + Device.OS.ToString() + "&AppName=" + MyController.AppNames.uWatch, "", false);
            return result;
        }

        public async Task<WebServiceResult<List<CubeConfigure>>> GetDefaultConfigLibAsync()
        {
            var result = await webService.GetAsync<List<CubeConfigure>>(CubeConfigLib, "", false);
            return result;
        }

        public async Task<WebServiceResult<string>> GetReceiptImageAsync(int assetId)
        {
            var result = await webService.GetAsync<string>(GetReceiptImage + assetId, "", true);
            return result;
        }

    }
    #region Classes


    public class BLETagLinks
    {
        [JsonProperty("CurrentSetting")]
        public BLESetting CurrentSetting { get; set; }

        [JsonProperty("PendingSetting")]
        public BLESetting PendingSetting { get; set; }
    }

    public class BLESetting
    {
        [JsonProperty("BlueTransTags")]
        public List<BLETransTag> BlueTransTags { get; set; }

        [JsonProperty("config_idx")]
        public long ConfigIdx { get; set; }

        [JsonProperty("chkBluetoothLowEnergy")]
        public bool ChkBluetoothLowEnergy { get; set; }
    }


    public class AppUpdateInfo
    {
        public bool UpdateRequired { get; set; }

        public bool UpdateAvailable { get; set; }
    }

    public class objTagData
    {
        public List<BLETransTag> objTags { get; set; }
        public int DeviceID { get; set; }
    }

    public class HomeDefaultConfig : BaseModel
    {
        public List<BLETransTag> BlueTransTags { get; set; }

        public int DefaultConfigID { get; set; }

        public bool SaveTag { get; set; }
    }

    #endregion
}
