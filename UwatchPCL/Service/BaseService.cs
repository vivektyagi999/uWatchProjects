using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UwatchPCL.Helpers;

namespace UwatchPCL
{
	public class BaseService
	{
		protected HttpClient CreateClient ()
		{
			var httpClient = new HttpClient 
			{ 
				BaseAddress = new Uri(Constants.BaseURL)
			};

			httpClient.DefaultRequestHeaders.Accept.Clear();
			httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			httpClient.DefaultRequestHeaders.Add("Token", Settings.AccessToken);
			httpClient.DefaultRequestHeaders.Add("UserName", Settings.UserName);
			return httpClient;
		}

	}
}

