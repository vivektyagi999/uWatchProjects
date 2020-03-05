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

namespace UwatchPCL
{
	public class ApiService
	{

		//		http://uwatch-mcs.co.uk//nguiver@btinternet.com/pa55w0rd.auth
		//		http://uwatch-mcs.co.uk//nguiver@btinternet.com/6.getdevices
		//		http://uwatch-mcs.co.uk//6/nguiver@btinternet.com/359 785020243005.getalertlist

		private static ApiService instance;
		bool IsSuccess;

		private string oldApiUrl = "http://uwatch-mcs.co.uk//";
		public static string LocalApiUrl = "http://192.168.2.177:81/";
		//public static string ApiUrl = "http://109.228.10.71/apiTest/";
		public static string ApiUrl = "http://109.228.10.71/api/";

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
				HttpClient httpClient = new HttpClient();
				httpClient.BaseAddress = new Uri(ApiUrl);
				httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				httpClient.DefaultRequestHeaders.Add("Token", Settings.AccessToken);
				httpClient.DefaultRequestHeaders.Add("UserName", Settings.UserName);
				HttpRequestMessage req1 = new HttpRequestMessage(HttpMethod.Post, "api/device/SavaAppToken");
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
				//userLoginDetail = new DeviceAssetRes();
				//userLoginDetail.error = "There was an error communicating with the server. Please try again! (" + ex.Message.ToString() + ")";
			}
			return TokenSaved;
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
				//userLoginDetail = new DeviceAssetRes();
				//userLoginDetail.error = "There was an error communicating with the server. Please try again! (" + ex.Message.ToString() + ")";
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
				//userLoginDetail = new DeviceAssetRes();
				//userLoginDetail.error = "There was an error communicating with the server. Please try again! (" + ex.Message.ToString() + ")";
			}
			return UserModel;
		}

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
				//				DeviceList = new UsersDetail();
				//				DeviceList.error = "There was an error communicating with the server. Please try again! (" + ex.Message.ToString() + ")";
			}
			return DeviceList;
		}

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
				HttpRequestMessage req1 = new HttpRequestMessage(HttpMethod.Get, "api/device/FetchDevice?id=" + req.device_idx.ToString());
				req1.Content = new StringContent(JsonConvert.SerializeObject(req, Formatting.None), Encoding.UTF8, "application/json");
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
				//var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
				//DeviceList = await Task.Run(() => JsonConvert.DeserializeObject<DeviceStatic>(content)).ConfigureAwait(false);
			}
			catch (Exception ex)
			{
				//				DeviceList = new UsersDetail();
				//				DeviceList.error = "There was an error communicating with the server. Please try again! (" + ex.Message.ToString() + ")";
			}
			return DeviceList;
		}

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
			}
			catch (Exception ex)
			{
				//				DeviceList = new UsersDetail();
				//				DeviceList.error = "There was an error communicating with the server. Please try again! (" + ex.Message.ToString() + ")";
			}
			return AlertList;
		}


		public async Task<ObservableCollection<AlertsEsclatedToAgentViewModel>> GetAlertsListByUser(DeviceStatic req)
		{
			ObservableCollection<AlertsEsclatedToAgentViewModel> AlertList = null;
			try
			{
				HttpClient httpClient = new HttpClient();
				httpClient.BaseAddress = new Uri(ApiUrl);
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
			}
			catch (Exception ex)
			{
				//				DeviceList = new UsersDetail();
				//				DeviceList.error = "There was an error communicating with the server. Please try again! (" + ex.Message.ToString() + ")";
			}
			return AlertList;
		}


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
			}
			catch (Exception ex)
			{
				//				DeviceList = new UsersDetail();
				//				DeviceList.error = "There was an error communicating with the server. Please try again! (" + ex.Message.ToString() + ")";
			}
			return Alert;
		}


		/// <summary>
		/// Fetch User Login List
		/// </summary>
		/// <param name="req"></param>
		/// <returns></returns>
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
				//userLoginDetail = new UsersDetail();
				//userLoginDetail.error = "There was an error communicating with the server. Please try again! (" + ex.Message.ToString() + ")";
			}
			return userLoginDetail;
		}

		/// <summary>
		/// Fetch User Login List
		/// </summary>
		/// <param name="req"></param>
		/// <returns></returns>
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
				userLoginDetail = new clsAccessToken();
				userLoginDetail.error = "There was an error communicating with the server. Please try again! (" + ex.Message.ToString() + ")";
			}
			return userLoginDetail;
		}


		/// <summary>
		/// Fetch User Login List
		/// </summary>
		/// <param name="req"></param>
		/// <returns></returns>
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
				//userLoginDetail = new DeviceAssetRes();
				//userLoginDetail.error = "There was an error communicating with the server. Please try again! (" + ex.Message.ToString() + ")";
			}
			return userLoginDetail;
		}

		/// <summary>
		/// Fetch User Login List
		/// </summary>
		/// <param name="req"></param>
		/// <returns></returns>
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
			}
			return result;
		}

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
			}
			catch (Exception ex)
			{
				//				DeviceList = new UsersDetail();
				//				DeviceList.error = "There was an error communicating with the server. Please try again! (" + ex.Message.ToString() + ")";
			}
			return Asset;
		}

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
				HttpRequestMessage req1 = new HttpRequestMessage(HttpMethod.Get, "api/Device/FetchAssetsImagePath?AssetsID=" + req.Deviceasset_idx);
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
			}
			catch (Exception ex)
			{
				//				DeviceList = new UsersDetail();
				//				DeviceList.error = "There was an error communicating with the server. Please try again! (" + ex.Message.ToString() + ")";
			}
			return Asset;
		}


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
				HttpRequestMessage req1 = new HttpRequestMessage(HttpMethod.Get, "api/device/FetchDetailDevice?deviceid=" + req.device_idx.ToString() + "&UserId=" + Settings.UserID.ToString());
				req1.Content = new StringContent(JsonConvert.SerializeObject(req, Formatting.None), Encoding.UTF8, "application/json");
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
				//				DeviceList = new UsersDetail();
				//				DeviceList.error = "There was an error communicating with the server. Please try again! (" + ex.Message.ToString() + ")";
			}
			return Agent;
		}

		/// <summary>
		/// Fetch User Login List
		/// </summary>
		/// <param name="req"></param>
		/// <returns></returns>
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
			}
			return result;
		}
		//get message list

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
				//				DeviceList = new UsersDetail();
				//				DeviceList.error = "There was an error communicating with the server. Please try again! (" + ex.Message.ToString() + ")";
			}
			return MessageList;
		}
		public async Task<int> IgnoreNotification (IgnoreNotificationReq req)
		{
			int TokenSaved = 0;
			try
			{
				HttpClient httpClient = new HttpClient();
				httpClient.BaseAddress = new Uri(ApiUrl);
				httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				httpClient.DefaultRequestHeaders.Add("Token", Settings.AccessToken);
				httpClient.DefaultRequestHeaders.Add("UserName", Settings.UserName);
				HttpRequestMessage req1 = new HttpRequestMessage(HttpMethod.Post, "api/Device/IgnoreNotification?NotificationID="+ req.NotificationID.ToString() +"&AllNotification="+ req.AllNotification);
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
				//userLoginDetail = new DeviceAssetRes();
				//userLoginDetail.error = "There was an error communicating with the server. Please try again! (" + ex.Message.ToString() + ")";
			}
			return TokenSaved;
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
				//req1.Content = new StringContent(JsonConvert.SerializeObject(req, Formatting.None), Encoding.UTF8, "application/json");
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
				//userLoginDetail = new DeviceAssetRes();
				//userLoginDetail.error = "There was an error communicating with the server. Please try again! (" + ex.Message.ToString() + ")";
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
				//userLoginDetail = new DeviceAssetRes();
				//userLoginDetail.error = "There was an error communicating with the server. Please try again! (" + ex.Message.ToString() + ")";
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
				//userLoginDetail = new DeviceAssetRes();
				//userLoginDetail.error = "There was an error communicating with the server. Please try again! (" + ex.Message.ToString() + ")";
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
				//userLoginDetail = new DeviceAssetRes();
				//userLoginDetail.error = "There was an error communicating with the server. Please try again! (" + ex.Message.ToString() + ")";
			}
			return TokenSaved;



		}




	}
}




