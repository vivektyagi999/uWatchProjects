using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UwatchPCL.Helpers;
using Xamarin.Forms;

namespace UwatchPCL.WebServices
{
    public class WebService : ServiceConfigrations, IWebService
    {
        private async Task<WebServiceResult<T>> SendRequest<T>(string action, string parameters, string method, bool returntype)
        {

            try
            {

                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(Constants.BaseURL);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Token", Settings.AccessToken);
                httpClient.DefaultRequestHeaders.Add("UserName", Settings.UserName);
                HttpResponseMessage response = null;
                WebServiceResult<T> result = new WebServiceResult<T>();
                if (method == "POST")
                {
                    HttpContent Content = new StringContent(parameters, Encoding.UTF8, "application/json");
                    response = await httpClient.PostAsync(action, Content);
                }
                else if (method == "GET")
                {
                    HttpRequestMessage req1 = new HttpRequestMessage(HttpMethod.Get, action);
                    req1.Content = new StringContent(parameters, Encoding.UTF8, "application/json");
                    response = await httpClient.GetAsync(req1.RequestUri);
                }
               
                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(responseString))
                    {
                        if (returntype)
                        {
                            var dataResult = JsonConvert.DeserializeObject<T>(responseString);
                            result.ErrorCode = 0;
                            result.Data = dataResult;
                            result.ErrorMessage = response.ReasonPhrase;
                        }
                        else
                        {
                            var dataResult = JsonConvert.DeserializeObject<WebServiceResult<string>>(responseString);
                            result.ErrorCode = dataResult.ErrorCode;
                            result.ErrorMessage = dataResult.ErrorMessage;
                            if (result.ErrorCode == 0)
                            {
                                var datavalue = JsonConvert.DeserializeObject<T>(dataResult.Data);
                                result.Data = datavalue;
                            }
                        }
                    }
                    else
                    {
                        result.ErrorCode = 1;
                        result.ErrorMessage = response.ReasonPhrase;
                    }
                }
                else
                {
                    result.ErrorCode = 1;
                    result.ErrorMessage = response.ReasonPhrase;
                }
            
                return result;
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message == "Error: ConnectFailure (Network is unreachable)")
                {
                    return new WebServiceResult<T>() { ErrorCode = 1, ErrorMessage = "Please check your internet connection." };
                }
                else
                {
                    return new WebServiceResult<T>() { ErrorCode = 1, ErrorMessage = ex.Message };
                }

            }

        }

        public Task<WebServiceResult<T>> SendRequestAsync<T>(string action, string parameters, string method, bool returntype)
        {
            return Task.Run(() =>
            {
                return SendRequest<T>(action, parameters, method, returntype);
            });
        }


        public Task<WebServiceResult<T>> GetAsync<T>(string action, string objectData, bool returntype)
        {
            return SendRequestAsync<T>(action, objectData, "GET", returntype);
        }

        public Task<WebServiceResult<T>> PostAsync<T>(string action, string objectData, bool returntype)
        {
            return SendRequestAsync<T>(action, objectData, "POST", returntype);
        }

        public WebService()
        {

        }

    }
}
