using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Linq;
using UwatchPCL.Helpers;

namespace MessageSender
{
	public static class MessageSender
	{

		

		/// <summary>
		/// Sends the push notification.
		/// [first of all add PushSharp package from nuget to functioning of this method]
		/// [need Certificate,device token]
		/// </summary>
		public static void SendPushNotification()
		{
			var token = Settings.DeviceToken;
			for (int i = 0; i <= 10; i++)
			{
				PushNotificationAndroid(token, "AIzaSyD_XVr-i4iCcZAaurwkCoR8C321Mm4zfbs", "Hi you got New Json notification"+i.ToString());

			}
			
		}
		/// <summary>
		/// Pushs the notification IO.
		/// </summary>
		/// <param name="strDeviceToken">String device token.</param>
		/// <param name="strCertPath">String[certificate with path ex {~/App_Data/partyplanner_dev.p12}]</param>
		/// <param name="isProd">If set to <c>true</c> is production notification or for development/testing.</param>
		/// <param name="strMessage">String [notification message].</param>

		public static void PushNotificationAndroid(string DeviceToken, string apiKey, string strMsg)
		{
			//
			var jGcmData = new JObject();
			var jData = new JObject();

			jData.Add("message", strMsg);
			jData.Add("AlertId", "597");
			jGcmData.Add("to", DeviceToken);
			jGcmData.Add("data", jData);


			var url = new Uri("https://gcm-http.googleapis.com/gcm/send");
			try
			{
				using (var client = new HttpClient())
				{
					client.DefaultRequestHeaders.Accept.Add(
						new MediaTypeWithQualityHeaderValue("application/json"));

					client.DefaultRequestHeaders.TryAddWithoutValidation(
						"Authorization", "key=" + apiKey);

					Task.WaitAll(client.PostAsync(url,
                          new StringContent(jGcmData.ToString(), Encoding.UTF8, "application/json"))
						.ContinueWith(response =>
							{
								//Console.WriteLine(response);
								//Console.WriteLine("Message sent: check the client device notification tray.");
							}));
				}
			}
			catch (Exception e)
			{
				//Console.WriteLine("Unable to send GCM message:");
				//Console.Error.WriteLine(e.StackTrace);
			}
		}
	}
}