using System;
using Android.App;
using Android.Content;
using Android.Util;
using Android.Gms.Gcm;
using Android.Gms.Gcm.Iid;
using UwatchPCL.Helpers;

namespace uWatch.Droid
{
	// This intent service receives the registration token from GCM:
	[Service (Exported = false)]
	class RegistrationIntentService : IntentService
	{
		// Notification topics that I subscribe to:
		static readonly string[] Topics = { "global" };

		// Create the IntentService, name the worker thread for debugging purposes:
		public RegistrationIntentService() : base ("RegistrationIntentService")
		{ }

		// OnHandleIntent is invoked on a worker thread:
		protected override void OnHandleIntent (Intent intent)
		{
			try
			{
				Log.Info("RegistrationIntentService", "Calling InstanceID.GetToken");

				// Ensure that the request is atomic:
				lock (this)
				{
					// Request a registration token:
					//var test = GoogleCloudMessaging.GetInstance(this);

					var instanceID = InstanceID.GetInstance(this);
					//var a = InstanceID.ErrorMissingInstanceidService;
					//var b = InstanceID.ErrorServiceNotAvailable;
					//var c = InstanceID.ErrorMainThread;
					//var d = InstanceID.ErrorTimeout;
					var token = instanceID.GetToken("472066817321   ",GoogleCloudMessaging.InstanceIdScope, null);
					// Log the registration token that was returned from GCM:
					Log.Info("RegistrationIntentService", "GCM Registration Token: " + token);

					// Send to the app server (if it requires it):
					SendRegistrationToAppServer(token);

					// Subscribe to receive notifications:
					SubscribeToTopics(token, Topics);
				}
			}
			catch (Exception e)
			{
				Log.Debug("RegistrationIntentService", "Failed to get a registration token");
			}
		}

		void SendRegistrationToAppServer(string token)		{
			// Add custom implementation here as needed.
		}

		// Subscribe to topics to receive notifications from the app server:
		void SubscribeToTopics (string token, string[] topics)
		{
			try
			{
					Settings.DeviceToken = token;
			}
			catch 
			{
			}
			foreach (var topic in topics)
			{
				var pubSub = GcmPubSub.GetInstance(this);
				pubSub.Subscribe(token, "/topics/" + topic, null);
				var pub = pubSub.ToString();
			}
		}
	}
}
