using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.IO;
using UwatchPCL.Model;
using UwatchPCL.Model.Request;
using UwatchPCL.Model.Response;
using System.Collections.ObjectModel;
using UwatchPCL.Helpers;
using Xamarin.Forms;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Xml;
using uWatch.ViewModels;
using Acr.UserDialogs;
using System.Diagnostics;
using UwatchPCL.WebServices;

namespace UwatchPCL
{
    public class ApiService
    {



        private static ApiService instance;
        bool IsSuccess;

        /// <summary>
        /// The API URL For Production.
        /// </summary>
        public const string ApiUrl = "http://api.uwatch.co.uk/";
        public const string ImageUrl = "https://login.uwatch.co.uk/";


        /// <summary>
        /// The API URL For Test.
        /// </summary>
        //public const string ApiUrl = "http://apitest.uwatch.co.uk/";
        //public const string ImageUrl = "http://uat.uwatch.co.uk/";



        public int TIMEOUT = 50;

        public static ApiService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ApiService();
                }
                return instance;
            }
        }

        /// <summary>
        /// Fetch User Login List
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<bool> SaveTokenToServer(AppTokenModel req)
        {
            bool TokenSaved = false;
            try 
            {
                req.AppName = MyController.AppNames.uWatch.ToString();
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Token", Settings.AccessToken);
                httpClient.DefaultRequestHeaders.Add("UserName", Settings.UserName);

                HttpContent Content = new StringContent(JsonConvert.SerializeObject(req, Formatting.None), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("api/device/SavaAppToken", Content).ConfigureAwait(false);
                //HttpRequestMessage req1 = new HttpRequestMessage(HttpMethod.Post, "api/device/SavaAppToken");
                //req1.Content = new StringContent(JsonConvert.SerializeObject(req, Formatting.None), Encoding.UTF8, "application/json");
                //var response = await httpClient.SendAsync(req1).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        TokenSaved = await Task.Run(() => Convert.ToBoolean(json)).ConfigureAwait(false);
                    }
                    IsSuccess = true;
                }
                else
                {

                    IsSuccess = false;

                }
            }
            catch (Exception ex)
            {
                IsSuccess = false;
            }
            return IsSuccess;
        }

        /// <summary>
        /// Fetch User Login List
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<bool> DeleteTokenFromServer(AppTokenModel req)
        {
            bool TokenSaved = false;
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Token", Settings.AccessToken);
                httpClient.DefaultRequestHeaders.Add("UserName", Settings.UserName);
                HttpRequestMessage req1 = new HttpRequestMessage(HttpMethod.Post, "api/device/DeleteAppToken");
                var data = JsonConvert.SerializeObject(req, Formatting.None);
                req1.Content = new StringContent(data, Encoding.UTF8, "application/json");
                var response = await httpClient.SendAsync(req1).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        TokenSaved = await Task.Run(() => Convert.ToBoolean(json)).ConfigureAwait(false);
                    }
                    IsSuccess = true;
                }
                else
                {
                    IsSuccess = false;
                }
            }
            catch (Exception ex)
            {

            }
            return TokenSaved;
        }

        //for forgot password
        public async Task<AddUserModel> SendMailForForgotPassword(String username)
        {
            AddUserModel UserModel = null;
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("UserName", Settings.UserName);
                HttpRequestMessage req1 = new HttpRequestMessage(HttpMethod.Post, "api/Account/Forgotpassword?UserName=" + username);
                req1.Content = new StringContent(JsonConvert.SerializeObject(username, Formatting.None), Encoding.UTF8, "application/json");
                var response = await httpClient.SendAsync(req1).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        UserModel = await Task.Run(() =>
                                                    JsonConvert.DeserializeObject<AddUserModel>(json)
                                                   ).ConfigureAwait(false);


                    }
                    IsSuccess = true;
                }
                else
                {
                    IsSuccess = false;

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return UserModel;
        }

        /// <summary>
        /// Gets the device list.
        /// </summary>
        /// <returns>Task<ObservableCollection<DeviceStatic>></returns>
        /// <param name="req">DeviceStatic</param>
        public async Task<ObservableCollection<DeviceStatic>> GetDeviceList(DeviceStatic req)
        {
            ObservableCollection<DeviceStatic> DeviceList = null;
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Token", Settings.AccessToken);
                httpClient.DefaultRequestHeaders.Add("UserName", Settings.UserName);
                HttpRequestMessage req1 = new HttpRequestMessage(HttpMethod.Post, "api/device/FetchAllDevice");
                req1.Content = new StringContent(JsonConvert.SerializeObject(req, Formatting.None), Encoding.UTF8, "application/json");
                var response = await httpClient.SendAsync(req1);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        DeviceList = await Task.Run(() =>
                        JsonConvert.DeserializeObject<ObservableCollection<DeviceStatic>>(json)
                                   );

                    }
                    IsSuccess = true;
                }
                else
                {
                    IsSuccess = false;

                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message == "Error: NameResolutionFailure")
                {
                    UserDialogs.Instance.Alert("Please check your internet connection.", "Alert", "OK");
                }
                Debug.WriteLine(ex);

            }
            return DeviceList;
        }

        /// <summary>
        /// Gets the device details.
        /// </summary>
        /// <returns>Task<DeviceStatic></returns>
        /// <param name="req">DeviceStatic</param>
        public async Task<DeviceStatic> GetDeviceDetails(DeviceStatic req)
        {
            DeviceStatic DeviceList = null;
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Token", Settings.AccessToken);
                httpClient.DefaultRequestHeaders.Add("UserName", Settings.UserName);
                HttpRequestMessage req1 = new HttpRequestMessage(HttpMethod.Get, "api/device/FetchDevice?id=" + req.device_idx.ToString())
                {
                    Content = new StringContent(JsonConvert.SerializeObject(req, Formatting.None), Encoding.UTF8, "application/json")
                };
                var response = await httpClient.GetAsync(req1.RequestUri).ConfigureAwait(true);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        DeviceList = await Task.Run(() =>
                                                    JsonConvert.DeserializeObject<DeviceStatic>(json)
                                                   ).ConfigureAwait(false);
                    }
                }
                else
                {
                }


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return DeviceList;
        }

        /// <summary>
        /// Gets the alerts list.
        /// </summary>
        /// <returns>ObservableCollection<AlertsEsclatedToAgentViewModel></returns>
        /// <param name="req">DeviceStatic</param>
        public async Task<ObservableCollection<AlertsEsclatedToAgentViewModel>> GetAlertsList(DeviceStatic req)
        {
            ObservableCollection<AlertsEsclatedToAgentViewModel> AlertList = null;
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Token", Settings.AccessToken);
                httpClient.DefaultRequestHeaders.Add("UserName", Settings.UserName);
                HttpRequestMessage req1 = new HttpRequestMessage(HttpMethod.Get, "api/device/DeviceAlert?DeviceID=" + req.device_idx.ToString() + "&PageIndex=" + req.PageIndex.ToString() + "&RecordPerPage=" + req.RecordPerPage.ToString());
                req1.Content = new StringContent(JsonConvert.SerializeObject(req, Formatting.None), Encoding.UTF8, "application/json");
                var response = await httpClient.GetAsync(req1.RequestUri).ConfigureAwait(true);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        AlertList =
                            await Task.Run(() =>
                            JsonConvert.DeserializeObject<ObservableCollection<AlertsEsclatedToAgentViewModel>>(json)
                                          ).ConfigureAwait(false);
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return AlertList;
        }

        /// <summary>
        /// Gets the alerts list by user.
        /// </summary>
        /// <returns>Task<ObservableCollection<AlertsEsclatedToAgentViewModel>></returns>
        /// <param name="req">DeviceStatic</param>
        public async Task<ObservableCollection<AlertsEsclatedToAgentViewModel>> GetAlertsListByUser(DeviceStatic req)
        {
            ObservableCollection<AlertsEsclatedToAgentViewModel> AlertList = null;
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(ApiUrl);
                httpClient.Timeout = TimeSpan.FromMilliseconds(1000 * TIMEOUT);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Token", Settings.AccessToken);
                httpClient.DefaultRequestHeaders.Add("UserName", Settings.UserName);
                HttpRequestMessage req1 = new HttpRequestMessage(HttpMethod.Get, "api/device/MyAlert?User_idx=" + req.OwnerUserID.ToString() + "&DeviceId=0" + "&PageIndex=" + req.PageIndex.ToString() + "&RecordPerPage=" + req.RecordPerPage.ToString());
                req1.Content = new StringContent(JsonConvert.SerializeObject(req, Formatting.None), Encoding.UTF8, "application/json");
                var response = await httpClient.GetAsync(req1.RequestUri).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        AlertList =
                            await Task.Run(() =>
                            JsonConvert.DeserializeObject<ObservableCollection<AlertsEsclatedToAgentViewModel>>(json)
                                          ).ConfigureAwait(true);
                    }
                }
                else
                {
                    var item = "aa";
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message == "Error: NameResolutionFailure")
                {
                    UserDialogs.Instance.Alert("Please check your internet connection.", "Alert", "OK");
                }
                if (ex.Message == "A task was canceled.")
                {
                    UserDialogs.Instance.Alert("Something went wrong. please try again.", "Alert", "OK");
                }
                else if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException.Message == "Network is unreachable")
                    {
                        UserDialogs.Instance.Alert("Please check your internet connection.", "Alert", "OK");
                    }
                }

            }
            return AlertList;
        }

        /// <summary>
        /// Gets the alert.
        /// </summary>
        /// <returns>Task<AlertsEsclatedToAgentViewModel></returns>
        /// <param name="req">AlertsEsclatedToAgentViewModel</param>
        public async Task<AlertsEsclatedToAgentViewModel> GetAlert(AlertsEsclatedToAgentViewModel req)
        {
            AlertsEsclatedToAgentViewModel Alert = null;
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Token", Settings.AccessToken);
                httpClient.DefaultRequestHeaders.Add("UserName", Settings.UserName);
                HttpRequestMessage req1 = new HttpRequestMessage(HttpMethod.Get, "api/device/AlertDetails?alertlog_idx=" + req.alertlog_idx.ToString() + "&UserID=" + Settings.UserID.ToString());
                req1.Content = new StringContent(JsonConvert.SerializeObject(req, Formatting.None), Encoding.UTF8, "application/json");
                var response = await httpClient.GetAsync(req1.RequestUri).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if ((!string.IsNullOrWhiteSpace(json)) && json != "null")
                    {
                        Alert =
                            await Task.Run(() =>
                            JsonConvert.DeserializeObject<AlertsEsclatedToAgentViewModel>(json)
                                          ).ConfigureAwait(false);
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return Alert;
        }


        /// <summary>
        /// Fetch User Login List
        /// </summary>
        /// <param name="req">AccountModel</param>
        /// <returns>Task<UsersDetail></returns>
        public async Task<UsersDetail> UserDetails(AccountModel req)
        {
            UsersDetail userLoginDetail = null;
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Token", Settings.AccessToken);
                httpClient.DefaultRequestHeaders.Add("UserName", Settings.UserName);
                HttpRequestMessage req1 = new HttpRequestMessage(HttpMethod.Get, "api/account/FetchUserDetail?User_idx=" + req.User_Idx.ToString());
                req1.Content = new StringContent(JsonConvert.SerializeObject(req, Formatting.None), Encoding.UTF8, "application/json");
                var response = await httpClient.GetAsync(req1.RequestUri).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        userLoginDetail = await Task.Run(() =>
                        JsonConvert.DeserializeObject<UsersDetail>(json)
                                   ).ConfigureAwait(false);

                    }
                    IsSuccess = true;
                }
                else
                {
                    IsSuccess = false;

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return userLoginDetail;
        }

        /// <summary>
        /// Fetch User Login List
        /// </summary>
        /// <param name="req">AccountModelReq</param>
        /// <returns>Task<clsAccessToken></returns>
        public async Task<clsAccessToken> UserLogin(AccountModelReq req)
        {
            clsAccessToken userLoginDetail = null;
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Token", Settings.AccessToken);
                httpClient.DefaultRequestHeaders.Add("UserName", Settings.UserName);
                HttpRequestMessage req1 = new HttpRequestMessage(HttpMethod.Post, "api/account/checklogin");
                req1.Content = new StringContent(JsonConvert.SerializeObject(req, Formatting.None), Encoding.UTF8, "application/json");
                var response = await httpClient.SendAsync(req1).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        userLoginDetail = await Task.Run(() =>
                        JsonConvert.DeserializeObject<clsAccessToken>(json)
                                   ).ConfigureAwait(false);

                    }
                    IsSuccess = true;
                }
                else
                {
                    IsSuccess = false;

                }
            }
            catch (Exception ex)
            {
                userLoginDetail = new clsAccessToken
                {
                    error = "There was an error communicating with the server. Please try again! (" + ex.Message.ToString() + ")"
                };

            }
            return userLoginDetail;
        }


        /// <summary>
        /// Fetch User Login List
        /// </summary>
        /// <param name="req">DeviceAssetsModel</param>
        /// <returns>Task<int></returns>
        public async Task<int> SaveAsset(DeviceAssetsModel req)
        {
            int userLoginDetail = -1;
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Token", Settings.AccessToken);
                httpClient.DefaultRequestHeaders.Add("UserName", Settings.UserName);
                HttpRequestMessage req1 = new HttpRequestMessage(HttpMethod.Post, "api/device/SaveAssetsImage");
                req1.Content = new StringContent(JsonConvert.SerializeObject(req, Formatting.None), Encoding.UTF8, "application/json");
                var response = await httpClient.SendAsync(req1).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {


                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        userLoginDetail = await Task.Run(() =>
                                        Convert.ToInt32(json)
                                   ).ConfigureAwait(false);

                    }
                    IsSuccess = true;
                }
                else
                {
                    IsSuccess = false;

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return userLoginDetail;
        }

        /// <summary>
        /// Fetch User Login List
        /// </summary>
        /// <param name="req">DeviceAssetsModel</param>
        /// <returns>Task<bool></returns>
        public async Task<bool> DeleteAsset(DeviceAssetsModel req)
        {
            bool result = false;
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Token", Settings.AccessToken);
                httpClient.DefaultRequestHeaders.Add("UserName", Settings.UserName);
                HttpRequestMessage req1 = new HttpRequestMessage(HttpMethod.Post, "api/device/DeleteAssetsImage?deviceAssetsID=" + req.Deviceasset_idx.ToString());
                req1.Content = new StringContent(JsonConvert.SerializeObject(req, Formatting.None), Encoding.UTF8, "application/json");
                var response = await httpClient.SendAsync(req1).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        result = await Task.Run(() => Convert.ToBoolean(json)).ConfigureAwait(false);
                    }
                    IsSuccess = true;
                }
                else
                {
                    IsSuccess = false;

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return result;
        }

        /// <summary>
        /// Gets the asset.
        /// </summary>
        /// <returns>Task<DeviceAssetsModel></returns>
        /// <param name="req">DeviceAssetsModel</param>
        public async Task<DeviceAssetsModel> GetAsset(DeviceAssetsModel req)
        {
            DeviceAssetsModel Asset = null;
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Token", Settings.AccessToken);
                httpClient.DefaultRequestHeaders.Add("UserName", Settings.UserName);
                HttpRequestMessage req1 = new HttpRequestMessage(HttpMethod.Get, "api/Device/FetchAssetsImage?AssetsID=" + req.Deviceasset_idx);
                req1.Content = new StringContent(JsonConvert.SerializeObject(req, Formatting.None), Encoding.UTF8, "application/json");
                var response = await httpClient.GetAsync(req1.RequestUri).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        Asset = await Task.Run(() =>
                                                    JsonConvert.DeserializeObject<DeviceAssetsModel>(json)
                                                   ).ConfigureAwait(false);
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return Asset;
        }

        /// <summary>
        /// Gets the assetby URL.
        /// </summary>
        /// <returns>Task<DeviceAssetsModel></returns>
        /// <param name="req">DeviceAssetsModel</param>
        public async Task<DeviceAssetsModel> GetAssetbyUrl(DeviceAssetsModel req)
        {
            DeviceAssetsModel Asset = null;
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Token", Settings.AccessToken);
                httpClient.DefaultRequestHeaders.Add("UserName", Settings.UserName);
                HttpRequestMessage req1 = new HttpRequestMessage(HttpMethod.Get, "api/Device/FetchAssetsImage?AssetsID=" + req.Deviceasset_idx);
                req1.Content = new StringContent(JsonConvert.SerializeObject(req, Formatting.None), Encoding.UTF8, "application/json");
                var response = await httpClient.GetAsync(req1.RequestUri).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        Asset = await Task.Run(() =>
                                                    JsonConvert.DeserializeObject<DeviceAssetsModel>(json)
                                                   ).ConfigureAwait(false);
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return Asset;
        }

        /// <summary>
        /// Gets the asset list.
        /// </summary>
        /// <returns>Task<ObservableCollection<DeviceAssetsModel>></returns>
        /// <param name="req">DeviceStatic</param>
        public async Task<ObservableCollection<DeviceAssetsModel>> GetAssetList(DeviceStatic req)
        {
            ObservableCollection<DeviceAssetsModel> AssetList = null;
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Token", Settings.AccessToken);
                httpClient.DefaultRequestHeaders.Add("UserName", Settings.UserName);
                HttpRequestMessage req1 = new HttpRequestMessage(HttpMethod.Get, "api/device/FetchDetailDevice?UserId=" + Settings.UserID.ToString() + "&PageIndex=" + req.PageIndex + "&RecordPerPage=" + req.RecordPerPage);

                // req1.Content = new StringContent(JsonConvert.SerializeObject(req, Formatting.None), Encoding.UTF8, "application/json");
                var response = await httpClient.GetAsync(req1.RequestUri).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        AssetList = await Task.Run(() =>
                                                    JsonConvert.DeserializeObject<ObservableCollection<DeviceAssetsModel>>(json)
                                                   ).ConfigureAwait(false);
                    }
                }
            }
            catch (Exception ex)
            {
                MyController.ErrorManagement(ex.Message);
            }
            return AssetList;
        }

        /// <summary>
        /// Gets the alert image.
        /// </summary>
        /// <returns>Task<byte[]></returns>
        /// <param name="req">AlertImage</param>
        public async Task<byte[]> GetAlertImage(AlertImage req)
        {
            byte[] AlertImage = null;
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Token", Settings.AccessToken);
                httpClient.DefaultRequestHeaders.Add("UserName", Settings.UserName);
                HttpRequestMessage req1 = new HttpRequestMessage(HttpMethod.Get, "api/device/GetAlertImage?alertlog_id=" + req.Alertlog_id.ToString());
                req1.Content = new StringContent(JsonConvert.SerializeObject(req, Formatting.None), Encoding.UTF8, "application/json");
                var response = httpClient.GetAsync(req1.RequestUri).Result;
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        AlertImage = await Task.Run(() =>
                                                    JsonConvert.DeserializeObject<byte[]>(json)
                                                   ).ConfigureAwait(false);
                    }
                }
            }
            catch (Exception ex)
            {
                MyController.ErrorManagement(ex.Message);
            }
            return AlertImage;
        }

        /// <summary>
        /// Gets the agent details.
        /// </summary>
        /// <returns>Task<UserDetailWithRoleModel></returns>
        /// <param name="req">string</param>
        public async Task<UserDetailWithRoleModel> GetAgentDetails(string req)
        {
            UserDetailWithRoleModel Agent = null;

            List<UserDetailWithRoleModel> AgentList = null;
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Token", Settings.AccessToken);
                httpClient.DefaultRequestHeaders.Add("UserName", Settings.UserName);
                HttpRequestMessage req1 = new HttpRequestMessage(HttpMethod.Get, "api/Account/FetchAllOwnerNameList?RoleName=" + "Agent".ToString());
                req1.Content = new StringContent(JsonConvert.SerializeObject(req, Formatting.None), Encoding.UTF8, "application/json");
                var response = httpClient.GetAsync(req1.RequestUri).Result;
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        AgentList = await Task.Run(() =>
                                                    JsonConvert.DeserializeObject<List<UserDetailWithRoleModel>>(json)
                                                   ).ConfigureAwait(false);
                        if (AgentList != null && AgentList.Count > 0)
                        {
                            Agent = AgentList.Find(a => a.UserName.ToLower().ToString() == req.ToLower().ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return Agent;
        }

        /// <summary>
        /// Fetch User Login List
        /// </summary>
        /// <param name="req">AlertsEsclatedToAgentViewModel</param>
        /// <returns>Task<int></returns>
        public async Task<int> EsclateAlert(AlertsEsclatedToAgentViewModel req)
        {
            int result = 0;
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Token", Settings.AccessToken);
                httpClient.DefaultRequestHeaders.Add("UserName", Settings.UserName);
                HttpRequestMessage req1 = new HttpRequestMessage(HttpMethod.Post, "api/Device/EscalateAlert");
                req1.Content = new StringContent(JsonConvert.SerializeObject(req, Formatting.None), Encoding.UTF8, "application/json");
                var response = await httpClient.SendAsync(req1).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        result = await Task.Run(() => Convert.ToInt32(json)).ConfigureAwait(false);
                    }
                    IsSuccess = true;
                }
                else
                {
                    IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return result;
        }

        /// <summary>
        /// Esclates the alert to area manager.
        /// </summary>
        /// <returns>Task<int></returns>
        /// <param name="req">AlertsEsclatedToAgentViewModel</param>
        public async Task<int> EsclateAlertToAreaManager(AlertsEsclatedToAgentViewModel req)
        {
            int result = 0;
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Token", Settings.AccessToken);
                httpClient.DefaultRequestHeaders.Add("UserName", Settings.UserName);
                HttpRequestMessage req1 = new HttpRequestMessage(HttpMethod.Post, "api/Agent/EscalateAlertToAreaManager");
                req1.Content = new StringContent(JsonConvert.SerializeObject(req, Formatting.None), Encoding.UTF8, "application/json");
                var response = await httpClient.SendAsync(req1).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        result = await Task.Run(() => Convert.ToInt32(json)).ConfigureAwait(false);
                    }
                    IsSuccess = true;
                }
                else
                {
                    IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return result;
        }
        //get message list

        /// <summary>
        /// Gets the message list.
        /// </summary>
        /// <returns>Task<ObservableCollection<MessageStatics>></returns>
        /// <param name="req">MessageStatics</param>
        public async Task<ObservableCollection<MessageStatics>> GetMessageList(MessageStatics req)
        {
            ObservableCollection<MessageStatics> MessageList = null;


            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Token", Settings.AccessToken);
                httpClient.DefaultRequestHeaders.Add("UserName", Settings.UserName);
                HttpRequestMessage req1 = new HttpRequestMessage(HttpMethod.Get, "api/device/GetDeviceNotification?UserId=" + Settings.UserID.ToString());
                req1.Content = new StringContent(JsonConvert.SerializeObject(req, Formatting.None), Encoding.UTF8, "application/json");
                var response = httpClient.GetAsync(req1.RequestUri).Result;
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        MessageList = await Task.Run(() =>
                        JsonConvert.DeserializeObject<ObservableCollection<MessageStatics>>(json)
                                   );

                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return MessageList;
        }
        public async Task<int> IgnoreNotification(IgnoreNotificationReq req)
        {
            int TokenSaved = 0;
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Token", Settings.AccessToken);
                httpClient.DefaultRequestHeaders.Add("UserName", Settings.UserName);
                HttpRequestMessage req1 = new HttpRequestMessage(HttpMethod.Post, "api/Device/IgnoreNotification?NotificationID=" + req.NotificationID.ToString() + "&AllNotification=" + req.AllNotification);
                req1.Content = new StringContent(JsonConvert.SerializeObject(req, Formatting.None), Encoding.UTF8, "application/json");
                var response = await httpClient.SendAsync(req1).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        TokenSaved = await Task.Run(() => Convert.ToInt32(json)).ConfigureAwait(false);
                    }
                    IsSuccess = true;
                }
                else
                {
                    IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return TokenSaved;
        }
        public async Task<bool> DeleteMessage(string MailId)
        {
            bool result = false;
            try
            {
                ListOfDeviceNotificationResponce model = new ListOfDeviceNotificationResponce();
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Token", Settings.AccessToken);
                httpClient.DefaultRequestHeaders.Add("UserName", Settings.UserName);
                HttpRequestMessage req1 = new HttpRequestMessage(HttpMethod.Post, "api/Device/DeleteMessage?id=" + MailId);
                req1.Content = new StringContent(JsonConvert.SerializeObject(model, Formatting.None), Encoding.UTF8, "application/json");
                var response = await httpClient.SendAsync(req1).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        result = await Task.Run(() => Convert.ToBoolean(json)).ConfigureAwait(false);
                    }
                    IsSuccess = true;
                }
                else
                {
                    IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return result;
        }
        public async Task<int> AlwaysIgnoreNotification(int DeviceId)
        {
            int TokenSaved = 0;
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Token", Settings.AccessToken);
                httpClient.DefaultRequestHeaders.Add("UserName", Settings.UserName);
                HttpRequestMessage req1 = new HttpRequestMessage(HttpMethod.Post, "api/device/AlwaysIgnore?DeviceID=" + DeviceId.ToString());
                var response = await httpClient.SendAsync(req1).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        TokenSaved = await Task.Run(() => Convert.ToInt32(json)).ConfigureAwait(false);
                    }
                    IsSuccess = true;
                }
                else
                {
                    IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return TokenSaved;
        }
        public async Task<int> ReadMessage(MessageStatics req)
        {
            int TokenSaved = 0;
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Token", Settings.AccessToken);
                httpClient.DefaultRequestHeaders.Add("UserName", Settings.UserName);
                HttpRequestMessage req1 = new HttpRequestMessage(HttpMethod.Post, "api/Device/NotificationStatusRead?NotificationID=" + req.NotificationID.ToString());
                req1.Content = new StringContent(JsonConvert.SerializeObject(req, Formatting.None), Encoding.UTF8, "application/json");
                var response = await httpClient.SendAsync(req1).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        TokenSaved = await Task.Run(() => Convert.ToInt32(json)).ConfigureAwait(false);
                    }
                    IsSuccess = true;
                }
                else
                {
                    IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return TokenSaved;
        }


        public async Task<ObservableCollection<DeviceConfig>> FetchConfigurationProfileDetails(DeviceStatic req)
        {
            int TokenSaved = 0;
            ObservableCollection<DeviceConfig> AssetList = null;
            try
            {

                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Token", Settings.AccessToken);
                httpClient.DefaultRequestHeaders.Add("UserName", Settings.UserName);
                HttpRequestMessage req1 = new HttpRequestMessage(HttpMethod.Get, "api/Device/FetchConfigurationProfileDetails?User_Idx=" + req.OwnerUserID.ToString());
                req1.Content = new StringContent(JsonConvert.SerializeObject(req, Formatting.None), Encoding.UTF8, "application/json");


                var response = await httpClient.GetAsync(req1.RequestUri).ConfigureAwait(true);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        AssetList = await Task.Run(() =>
                                                    JsonConvert.DeserializeObject<ObservableCollection<DeviceConfig>>(json)
                                                   ).ConfigureAwait(false);
                    }
                }
                else
                {
                    IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return AssetList;
        }


        public async Task<bool> ChangeCubeConfigration(SaveConfigsetting config)
        {
            bool TokenSaved = false;
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Token", Settings.AccessToken);
                httpClient.DefaultRequestHeaders.Add("UserName", Settings.UserName);
                HttpRequestMessage req1 = new HttpRequestMessage(HttpMethod.Post, "api/device/UpdateSettingByProfileID");
                req1.Content = new StringContent(JsonConvert.SerializeObject(config, Formatting.None), Encoding.UTF8, "application/json");
                var response = await httpClient.SendAsync(req1).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        TokenSaved = await Task.Run(() => Convert.ToBoolean(json)).ConfigureAwait(false);
                    }
                    IsSuccess = true;
                }
                else
                {
                    IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return TokenSaved;



        }

        public async Task<ObservableCollection<MemberStatics>> GetMemberList(MemberStatics req)
        {
            ObservableCollection<MemberStatics> MessageList = null;


            try
            {
                HttpClient httpClient = new HttpClient
                {
                    BaseAddress = new Uri(ApiUrl)
                };
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Token", Settings.AccessToken);
                httpClient.DefaultRequestHeaders.Add("UserName", Settings.UserName);
                HttpRequestMessage req1 = new HttpRequestMessage(HttpMethod.Get, "api/agent/FetchAgentsMemmberList")
                {
                    Content = new StringContent(JsonConvert.SerializeObject(req, Formatting.None), Encoding.UTF8, "application/json")
                };
                var response = httpClient.GetAsync(req1.RequestUri).Result;
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        MessageList = await Task.Run(() =>
                        JsonConvert.DeserializeObject<ObservableCollection<MemberStatics>>(json));

                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return MessageList;
        }
        public async Task<ObservableCollection<AddUserModel>> GetMemberLists(MemberStatics req)
        {
            req.SearchText = "";
            ObservableCollection<AddUserModel> MessageList = null;
            try
            {
                HttpClient httpClient = new HttpClient
                {
                    BaseAddress = new Uri(ApiUrl)
                };
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Token", Settings.AccessToken);
                httpClient.DefaultRequestHeaders.Add("UserName", Settings.UserName);
                HttpRequestMessage req1 = new HttpRequestMessage(HttpMethod.Post, "api/agent/FetchAgentsMemmberList")
                {
                    Content = new StringContent(JsonConvert.SerializeObject(req, Formatting.None), Encoding.UTF8, "application/json")
                };
                var response = await httpClient.SendAsync(req1).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        MessageList = await Task.Run(() =>
                        JsonConvert.DeserializeObject<ObservableCollection<AddUserModel>>(json)
                                   );

                    }
                }
                else
                {
                    IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                IsSuccess = false;
                Debug.WriteLine(ex);
            }
            return MessageList;
        }
        public async Task<Example> GetLatLongs(string Zip)
        {
            Example res = null;
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpRequestMessage req1 = new HttpRequestMessage(HttpMethod.Get, "http://maps.googleapis.com/maps/api/geocode/json?components=postal_code:" + Zip.Trim() + "&sensor=false");
                var response = await httpClient.GetAsync(req1.RequestUri).ConfigureAwait(true);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    res = await Task.Run(() =>
                            JsonConvert.DeserializeObject<Example>(json)
                                       );

                }


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return res;
        }
        public async Task<ObservableCollection<AlertsEsclatedToAgentViewModel>> GetEscalatedAlertOfAgent(EscalatedAlertReq req)
        {

            ObservableCollection<AlertsEsclatedToAgentViewModel> MessageList = null;
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Token", Settings.AccessToken);
                httpClient.DefaultRequestHeaders.Add("UserName", Settings.UserName);
                HttpRequestMessage req1 = new HttpRequestMessage(HttpMethod.Post, "api/agent/FetchAgentAlertList");
                req1.Content = new StringContent(JsonConvert.SerializeObject(req, Formatting.None), Encoding.UTF8, "application/json");
                var response = await httpClient.SendAsync(req1).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        MessageList = await Task.Run(() =>
                        JsonConvert.DeserializeObject<ObservableCollection<AlertsEsclatedToAgentViewModel>>(json)
                                   );

                    }
                }
                else
                {
                    IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                IsSuccess = false;
                Debug.WriteLine(ex);
            }
            return MessageList;
        }
        public async Task<ObservableCollection<InMailModel>> GetsendInboxmessageList(DeviceNotificationRequest req)
        {
            ObservableCollection<InMailModel> MessageList = null;
            req.UserID = Settings.UserID;

            int MessageTypeId = 0;
            if (req.type == MessageType.INBOX)
            {
                MessageTypeId = 0;

            }
            else
            {
                MessageTypeId = 1;
            }


            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Token", Settings.AccessToken);
                httpClient.DefaultRequestHeaders.Add("UserName", Settings.UserName);
                HttpRequestMessage req1 = new HttpRequestMessage(HttpMethod.Get, "api/device/GetDeviceMessagesForApp?UserId=" + Settings.UserID + "&type=" + MessageTypeId + "&PageIndex=" + req.PageIndex + "&RecordPerPage=" + req.RecordPerPage);
                req1.Content = new StringContent(JsonConvert.SerializeObject(req, Formatting.None), Encoding.UTF8, "application/json");
                var response = httpClient.GetAsync(req1.RequestUri).Result;
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        MessageList = await Task.Run(() =>
                        JsonConvert.DeserializeObject<ObservableCollection<InMailModel>>(json)
                                   );

                    }

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return MessageList;
        }

        //find the list of users

        public async Task<ObservableCollection<SelectedListItemModel>> GetuserList()
        {
            ObservableCollection<SelectedListItemModel> MessageList = null;


            DeviceNotificationRequest req = new DeviceNotificationRequest();

            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Token", Settings.AccessToken);
                httpClient.DefaultRequestHeaders.Add("UserName", Settings.UserName);
                //  HttpRequestMessage req1 = new HttpRequestMessage(HttpMethod.Get, "api/Account/FetchAllOwnerNameList?RoleName=Agent&UserID=" + 0);
                HttpRequestMessage req1 = new HttpRequestMessage(HttpMethod.Get, "api/Account/FetchMessageUserList?RoleID=" + Settings.RoleID.ToString() + "&UserID=" + Settings.UserID.ToString());
                req1.Content = new StringContent(JsonConvert.SerializeObject(req, Formatting.None), Encoding.UTF8, "application/json");
                var response = httpClient.GetAsync(req1.RequestUri).Result;
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        MessageList = await Task.Run(() =>
                                                     JsonConvert.DeserializeObject<ObservableCollection<SelectedListItemModel>>(json)
                                   );

                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return MessageList;
        }
        public async Task<bool> SendMessage(InMailModel req)
        {
            bool TokenSaved = false;
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Token", Settings.AccessToken);
                httpClient.DefaultRequestHeaders.Add("UserName", Settings.UserName);
                HttpRequestMessage req1 = new HttpRequestMessage(HttpMethod.Post, "api/admin/SendInMail");
                req1.Content = new StringContent(JsonConvert.SerializeObject(req, Formatting.None), Encoding.UTF8, "application/json");
                var response = await httpClient.SendAsync(req1).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        TokenSaved = await Task.Run(() => Convert.ToBoolean(json)).ConfigureAwait(false);
                    }
                    IsSuccess = true;
                }
                else
                {
                    IsSuccess = false;

                }
            }
            catch (Exception ex)
            {
                IsSuccess = false;
                Debug.WriteLine(ex);
            }
            return TokenSaved;
        }

        ///Change Message Read Status
        /// 
        /// 
        /// 
        public async Task<bool> ReadMessage(string MessageID)
        {
            bool result = false;
            InMailModel req = new InMailModel();
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Token", Settings.AccessToken);
                httpClient.DefaultRequestHeaders.Add("UserName", Settings.UserName);

                HttpRequestMessage req1 = new HttpRequestMessage(HttpMethod.Get, "api/Admin/UpdateMessageReadStatus?MailID=" + MessageID);
                req1.Content = new StringContent(JsonConvert.SerializeObject(req, Formatting.None), Encoding.UTF8, "application/json");
                var response = httpClient.GetAsync(req1.RequestUri).Result;
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        result = await Task.Run(() => Convert.ToBoolean(json)).ConfigureAwait(false);
                    }
                    IsSuccess = true;
                }
                else
                {
                    IsSuccess = false;
                }
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex);
                result = false;
            }


            return result;
        }
        public async Task<string> GetUnreadMessageCount(int UserID)
        {
            string result = "0";
            InMailModel req = new InMailModel();
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Token", Settings.AccessToken);
                httpClient.DefaultRequestHeaders.Add("UserName", Settings.UserName);
                HttpRequestMessage req1 = new HttpRequestMessage(HttpMethod.Get, "api/device/GetUnreadMessageCount?UserID=" + UserID);
                req1.Content = new StringContent(JsonConvert.SerializeObject(req, Formatting.None), Encoding.UTF8, "application/json");
                var response = httpClient.GetAsync(req1.RequestUri).Result;
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        result = await Task.Run(() => Convert.ToString(json)).ConfigureAwait(false);
                    }
                    IsSuccess = true;
                }
                else
                {
                    IsSuccess = false;
                }
            }
            catch (System.Exception ex)
            {
                result = "0";
                Debug.WriteLine(ex);
            }
            return result;
        }
        public async Task<WebServiceResult<bool>> DeviceOnOffConfigration(DeviceSwitchOffReq req)
        {
            WebServiceResult<bool> ret = null;
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Token", Settings.AccessToken);
                httpClient.DefaultRequestHeaders.Add("UserName", Settings.UserName);
                HttpRequestMessage req1 = new HttpRequestMessage(HttpMethod.Post, "api/device/DeviceOnOffConfigrationNew");
                req1.Content = new StringContent(JsonConvert.SerializeObject(req, Formatting.None), Encoding.UTF8, "application/json");
                var response = await httpClient.SendAsync(req1).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        ret = await Task.Run(() =>
                        JsonConvert.DeserializeObject<WebServiceResult<bool>>(json)
                                   ).ConfigureAwait(false);

                    }
                    IsSuccess = true;
                }
                else
                {
                    IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                ret = null;
            }
            return ret;
        }
        public async Task<ReturnClass> DeleteAlertLogs(int[] req)
        {
            ReturnClass result = new ReturnClass();
            try
            {
                int[] SelectedAlertLogIdsList = req;
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Token", Settings.AccessToken);
                httpClient.DefaultRequestHeaders.Add("UserName", Settings.UserName);
                HttpRequestMessage req1 = new HttpRequestMessage(HttpMethod.Post, "api/Device/DeleteAlertLogs");
                req1.Content = new StringContent(JsonConvert.SerializeObject(SelectedAlertLogIdsList, Formatting.None), Encoding.UTF8, "application/json");
                var response = await httpClient.SendAsync(req1).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        result = await Task.Run(() => JsonConvert.DeserializeObject<ReturnClass>(json)).ConfigureAwait(false);
                    }
                    IsSuccess = true;
                }
                else
                {
                    result.ErrorCode = -1;
                    result.ErrorMessage = response.ReasonPhrase;
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                result.ErrorCode = 1;
                result.ErrorMessage = "Some error has occured pleasse try after sometime";
            }
            return result;
        }
        public async Task<List<BatchImages>> GetBatchAlertImages(string batchImageIdentifier)
        {
            List<BatchImages> result = new List<BatchImages>();
            InMailModel req = new InMailModel();
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Token", Settings.AccessToken);
                httpClient.DefaultRequestHeaders.Add("UserName", Settings.UserName);
                HttpRequestMessage req1 = new HttpRequestMessage(HttpMethod.Get, "api/Device/BatchAlertImageList?identifier=" + batchImageIdentifier);
                req1.Content = new StringContent(JsonConvert.SerializeObject(req, Formatting.None), Encoding.UTF8, "application/json");
                var response = httpClient.GetAsync(req1.RequestUri).Result;
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        result = await Task.Run(() => JsonConvert.DeserializeObject<List<BatchImages>>(json)).ConfigureAwait(false);
                    }
                    IsSuccess = true;
                }
                else
                {
                    IsSuccess = false;
                }
            }
            catch (System.Exception ex)
            {
                result = null;
                Debug.WriteLine(ex);
            }
            return result;
        }
        public async Task<Setting> FetchDeviceConfigurationDetails(string deviceid)
        {
            Setting result = new Setting();
            var model = new DeviceConfig
            {
                device_idx = Convert.ToInt32(deviceid)
            };
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Token", Settings.AccessToken);
                httpClient.DefaultRequestHeaders.Add("UserName", Settings.UserName);
                HttpRequestMessage req1 = new HttpRequestMessage(HttpMethod.Post, "api/Device/FetchDeviceConfigurationDetails");
                req1.Content = new StringContent(JsonConvert.SerializeObject(model, Formatting.None), Encoding.UTF8, "application/json");
                var response = await httpClient.SendAsync(req1).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        result = await Task.Run(() =>
                        JsonConvert.DeserializeObject<Setting>(json)
                                   ).ConfigureAwait(false);

                    }
                    IsSuccess = true;
                }
                else
                {
                    IsSuccess = false;
                }
            }
            catch (System.Exception ex)
            {
                result = null;
                Debug.WriteLine(ex);
            }
            return result;
        }

        public async Task<DeviceSwitchOffReq> GetDeviceOffConfig(string DeviceID)
        {
            DeviceSwitchOffReq result = new DeviceSwitchOffReq();
            InMailModel req = new InMailModel();
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Token", Settings.AccessToken);
                httpClient.DefaultRequestHeaders.Add("UserName", Settings.UserName);
                HttpRequestMessage req1 = new HttpRequestMessage(HttpMethod.Get, "api/Device/GetDeviceOffConfig?DeviceID=" + DeviceID);
                req1.Content = new StringContent(JsonConvert.SerializeObject(req, Formatting.None), Encoding.UTF8, "application/json");
                var response = httpClient.GetAsync(req1.RequestUri).Result;
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        result = await Task.Run(() => JsonConvert.DeserializeObject<DeviceSwitchOffReq>(json)).ConfigureAwait(false);
                    }
                    IsSuccess = true;
                }
                else
                {
                    IsSuccess = false;
                }
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex);
                result = null;

            }
            return result;
        }


        /// <summary>
        /// Save as draft Api
        /// </summary>
        public async Task<bool> DraftSendInMail(InMailModel model)
        {
            bool flag = false;
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Token", Settings.AccessToken);
                httpClient.DefaultRequestHeaders.Add("UserName", Settings.UserName);
                HttpRequestMessage req1 = new HttpRequestMessage(HttpMethod.Post, "api/admin/DraftSendInMail");
                req1.Content = new StringContent(JsonConvert.SerializeObject(model, Formatting.None), Encoding.UTF8, "application/json");
                var response = await httpClient.SendAsync(req1).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        flag = true;


                    }
                    IsSuccess = true;
                }
                else
                {
                    flag = false;
                    IsSuccess = false;
                }
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex);
                flag = false;
            }
            return flag;
        }

        public async Task<List<DraftMailModel>> GetDraftList(int UserID)
        {
            List<DraftMailModel> result = new List<DraftMailModel>();
            InMailModel req = new InMailModel();
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Token", Settings.AccessToken);
                httpClient.DefaultRequestHeaders.Add("UserName", Settings.UserName);
                HttpRequestMessage req1 = new HttpRequestMessage(HttpMethod.Get, "api/admin/DraftMail?UserID=" + UserID);
                req1.Content = new StringContent(JsonConvert.SerializeObject(req, Formatting.None), Encoding.UTF8, "application/json");
                var response = httpClient.GetAsync(req1.RequestUri).Result;
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        result = await Task.Run(() => JsonConvert.DeserializeObject<List<DraftMailModel>>(json)).ConfigureAwait(false);
                    }
                    IsSuccess = true;
                }
                else
                {
                    IsSuccess = false;
                }
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex);
                result = null;
            }
            return result;
        }
        public async Task<ReturnClass> GetInventoryList()
        {
            ReturnClass result = new ReturnClass();
            InMailModel req = new InMailModel();
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Token", Settings.AccessToken);
                httpClient.DefaultRequestHeaders.Add("UserName", Settings.UserName);
                HttpRequestMessage req1 = new HttpRequestMessage(HttpMethod.Get, "api/Device/GetRoomList?OwnerID=" + Settings.UserID);
                req1.Content = new StringContent(JsonConvert.SerializeObject(req, Formatting.None), Encoding.UTF8, "application/json");
                var response = await httpClient.GetAsync(req1.RequestUri).ConfigureAwait(false); ;
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        result = await Task.Run(() => JsonConvert.DeserializeObject<ReturnClass>(json)).ConfigureAwait(false);

                    }
                    IsSuccess = true;
                }
                else
                {
                    IsSuccess = false;
                }
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex);
                result = null;
            }
            return result;
        }

        /// <summary>
        /// Add inventory in list.
        /// </summary>
        /// <returns>The inventory list.</returns>
        public async Task<ReturnClass> AddInventory(string roomName, int RoomId = 0)
        {
            Room objRoom = new Room();
            objRoom.RoomName = roomName;
            objRoom.RoomID = RoomId;
            objRoom.OwnerId = Settings.UserID;
            ReturnClass result = new ReturnClass();
            InMailModel req = new InMailModel();
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Token", Settings.AccessToken);
                httpClient.DefaultRequestHeaders.Add("UserName", Settings.UserName);
                 HttpRequestMessage req1 = new HttpRequestMessage(HttpMethod.Post, "api/device/SaveRoom")
                {
                    Content = new StringContent(JsonConvert.SerializeObject(objRoom, Formatting.None), Encoding.UTF8, "application/json")
                };
                var response = await httpClient.SendAsync(req1).ConfigureAwait(false); ;

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        result = await Task.Run(() => JsonConvert.DeserializeObject<ReturnClass>(json)).ConfigureAwait(false);
                    }
                    IsSuccess = true;
                }
                else
                {
                    IsSuccess = false;
                }
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex);
                IsSuccess = false;
            }
            return result;
        }

        public async Task<ReturnClass> AddRoomImages(RoomImageModel objRoomImage)
        {
            var result = new ReturnClass();
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Token", Settings.AccessToken);
                httpClient.DefaultRequestHeaders.Add("UserName", Settings.UserName);
                HttpRequestMessage req1 = new HttpRequestMessage(HttpMethod.Post, "api/Device/AddRoomImages");
                req1.Content = new StringContent(JsonConvert.SerializeObject(objRoomImage, Formatting.None), Encoding.UTF8, "application/json");
                var response = await httpClient.SendAsync(req1).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        result = await Task.Run(() => JsonConvert.DeserializeObject<ReturnClass>(json)).ConfigureAwait(false);
                    }
                    IsSuccess = true;
                }
                else
                {
                    IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return result;
        }
        public async Task<List<RoomDetails>> GetRoomDetailsList(string RoomId)
        {
            List<RoomDetails> result = new List<RoomDetails>();
            InMailModel req = new InMailModel();
            try
            {
                HttpClient httpClient = new HttpClient
                {
                    BaseAddress = new Uri(ApiUrl)
                };
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Token", Settings.AccessToken);
                httpClient.DefaultRequestHeaders.Add("UserName", Settings.UserName);
                HttpRequestMessage req1 = new HttpRequestMessage(HttpMethod.Get, "api/Device/RoomDetailList?RoomID=" + RoomId + "&UserId=" + Settings.UserID)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(req, Formatting.None), Encoding.UTF8, "application/json")
                };
                var response = await httpClient.GetAsync(req1.RequestUri).ConfigureAwait(false); ;
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        var resultjson = await Task.Run(() => JsonConvert.DeserializeObject<ReturnClass>(json)).ConfigureAwait(false);
                        result = JsonConvert.DeserializeObject<List<RoomDetails>>(resultjson.data);

                    }
                    IsSuccess = true;
                }
                else
                {
                    IsSuccess = false;
                }
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex);
                result = null;
            }
            return result;
        }
        public async Task<List<RoomImageModel>> RoomImagesList(string RoomId)
        {
            List<RoomImageModel> result = new List<RoomImageModel>();
            InMailModel req = new InMailModel();
            try
            {
                HttpClient httpClient = new HttpClient
                {
                    BaseAddress = new Uri(ApiUrl)
                };
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Token", Settings.AccessToken);
                httpClient.DefaultRequestHeaders.Add("UserName", Settings.UserName);
                HttpRequestMessage req1 = new HttpRequestMessage(HttpMethod.Get, "api/Device/RoomImagesList?RoomID=" + RoomId + "&UserId=" + Settings.UserID)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(req, Formatting.None), Encoding.UTF8, "application/json")
                };
                var response = await httpClient.GetAsync(req1.RequestUri).ConfigureAwait(false); ;
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        var resultjson = await Task.Run(() => JsonConvert.DeserializeObject<ReturnClass>(json)).ConfigureAwait(false);
                        result = JsonConvert.DeserializeObject<List<RoomImageModel>>(resultjson.data);
                    }
                    IsSuccess = true;
                }
                else
                {
                    IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);

                result = null;
            }
            return result;
        }
    }
}