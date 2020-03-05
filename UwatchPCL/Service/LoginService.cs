using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using System.IO;
using Xamarin.Forms;
using System.Net.Http;
using System.Net.Http.Headers;
using UwatchPCL.Model.Request;
using UwatchPCL.Helpers;
using Acr.UserDialogs;

namespace UwatchPCL
{
	public class LoginService : BaseService, ILoginService
	{
		

		public LoginService()
		{
			
		}
		private bool _isAuthenticated;
	

		public bool IsSuccess { get; set;}

		public bool IsAuthenticated
		{
			get
			{
				return _isAuthenticated;
			}
		}

		public async Task<clsAccessToken> UserLoginAsync(AccountModelReq req)
		{
			clsAccessToken userLoginDetail = null;
			using (var httpClient = CreateClient())
			{
				try
				{
					
					HttpContent Content = new StringContent(JsonConvert.SerializeObject(req, Formatting.None), Encoding.UTF8, "application/json");
					var response = await httpClient.PostAsync("api/account/checklogin",Content).ConfigureAwait(false);
					if (response.IsSuccessStatusCode)
					{
						var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
						if (!string.IsNullOrWhiteSpace(json))
						{
							userLoginDetail = await Task.Run(() => JsonConvert.DeserializeObject<clsAccessToken>(json)).ConfigureAwait(false);
							Settings.AccessToken = userLoginDetail.access_token;

							if (userLoginDetail.access_token == null)
							{
								IsSuccess = false;
								
							}
						}
						_isAuthenticated = true;
					}
					else 
					{
						IsSuccess = false;
					}
				}
				catch(System.Exception ex)
				{
                    if(ex.InnerException.Message== "Error: NameResolutionFailure")
                    {
                        UserDialogs.Instance.Alert("Please check your internet connection.", "Alert", "OK");
                    }
                    var x = ex.StackTrace;
					var x1 = ex.InnerException;
					IsSuccess = false;
				}
			}
			return userLoginDetail;
		}

	}
}

