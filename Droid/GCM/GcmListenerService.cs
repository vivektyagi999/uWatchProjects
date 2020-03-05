using Android.App;
using Android.Content;
using Android.OS;
using Android.Gms.Gcm;
using Android.Util;
using System.Linq;
using Android.Graphics;
using UwatchPCL;
using Android.Provider;
using Acr.UserDialogs;
using Xamarin.Forms;

namespace uWatch.Droid
{
	[Service (Exported = false), IntentFilter (new [] { "com.google.android.c2dm.intent.RECEIVE" })]
	public class MyGcmListenerService : GcmListenerService
	{
		string alertid, message;
		public override void OnMessageReceived (string from, Bundle data)
 		{
			// Extract the message received from GCM:

			if (data.ContainsKey("AlertID"))
			{
				message = data.GetString("message");
				alertid = data.GetString("AlertID");
				MyController.lstNs.Add(message);
				MyController.isMessage = false;
				SendNotification(message, alertid);
			}
			if (data.ContainsKey("MailID"))
			{
				message = data.GetString("Title");
				alertid = data.GetString("MailID");
				MyController.lstNs.Add(message);
				MyController.isMessage = true;
				SendMessage(message, alertid);
			}
			Log.Debug("MyGcmListenerService", "From:    " + from);
			Log.Debug("MyGcmListenerService", "Message: " + message);

			// Forward the received message in a local notification:

		}
		/// <summary>
		/// Sends the message notification.
		/// </summary>
		/// <param name="message">Message.</param>
		/// <param name="alertid">MailID.</param>
		void SendMessage(string message, string alertid)
 		{
			try
			{
				PendingIntent pendingIntent = null;


				string pathToPushSound = "android.resource://" + this.PackageName + "/raw/" + Resource.Drawable.uWatchTone;
				global::Android.Net.Uri soundUri = global::Android.Net.Uri.Parse(pathToPushSound);
				MyController.AlertId = alertid;
				if (MyController.isAppClosed)
				{
					

					var intent = new Intent(this, typeof(MainActivity));
					intent.AddFlags(ActivityFlags.ClearTop);
					pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot);

				}
				else
				{
					
					var intent = new Intent(this, typeof(MainActivity));
					pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.UpdateCurrent);
				}

				Bitmap largeIcon = BitmapFactory.DecodeResource(Resources, Resource.Drawable.micon48);

				var nsStyle = new Notification.InboxStyle();
				foreach (var item in MyController.lstNs.Take(3))
				{
					nsStyle.AddLine(item);
				}
				nsStyle.SetBigContentTitle("uWatch Messages");
				if (MyController.lstNs.Count == 1)
				{
					MyController.multiple = 0;
					nsStyle.SetSummaryText("");
				}
				else
				{
					MyController.multiple = 1;
					nsStyle.SetSummaryText("+ " + (MyController.lstNs.Count - 3).ToString() + "more");
				}

				var notificationBuilder = new Notification.Builder(this)
					  .SetSmallIcon(Resource.Drawable.icon)
					.SetLargeIcon(largeIcon)
					.SetContentTitle("Messages")
					.SetContentText(message)
					.SetAutoCancel(true)
					//.SetGroup("uwatch")
					.SetTicker("1")
					  .SetNumber(MyController.lstNs.Count)
					  .SetStyle(nsStyle)
					.SetContentIntent(pendingIntent)
					//.SetSound(Settings.System.DefaultNotificationUri)
					.SetSound(soundUri)
					.SetPriority((int)NotificationPriority.Max);

				var notificationManager = (NotificationManager)GetSystemService(Context.NotificationService);
				notificationManager.Notify(0, notificationBuilder.Build());
			}
			catch(System.Exception ex)
			{
				
			}
		}
		// Use Notification Builder to create and launch the notification:
		void SendNotification (string message, string alertid)
		{
			try
			{
				//UserDialogs.Instance.ShowLoading("Please Wait....");

				PendingIntent pendingIntent = null;

				//string pathToPushSound = "android.resource://" + this.ApplicationContext.PackageName + "/raw/push_sound";
				//string pathToPushSound = "announcement.mp3";
				string pathToPushSound ="android.resource://" + this.PackageName + "/raw/" + Resource.Drawable.uWatchTone;
				global::Android.Net.Uri soundUri = global::Android.Net.Uri.Parse(pathToPushSound);
				MyController.AlertId = alertid;
				if (MyController.isAppClosed)
				{
					//string[] values = { "abc", message };
					//Xamarin.Forms.MessagingCenter.Send<GcmListenerService, string[]>(this, "DisplayAlert", values);


					var intent = new Intent(this, typeof(MainActivity));
					intent.AddFlags(ActivityFlags.ClearTop);
					pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot);




					//Intent launchIntent = PackageManager.GetLaunchIntentForPackage("com.uwatch.uwatchapp");
					//pendingIntent = PendingIntent.GetActivity(this, 0, launchIntent, PendingIntentFlags.OneShot);
				}
				else
				{
					
					var intent = new Intent(this, typeof(MainActivity));
					//intent.AddFlags(ActivityFlags.ClearTop);
					pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.UpdateCurrent);
				}

				Bitmap largeIcon = BitmapFactory.DecodeResource(Resources, Resource.Drawable.micon48);

				var nsStyle = new Notification.InboxStyle();
				foreach (var item in MyController.lstNs.Take(3))
				{
					nsStyle.AddLine(item);
				}
				nsStyle.SetBigContentTitle("uWatch Alerts");
				if (MyController.lstNs.Count == 1)
				{
					MyController.multiple = 0;
					nsStyle.SetSummaryText("");
				}
				else
				{
					MyController.multiple = 1;
					nsStyle.SetSummaryText("+ " + (MyController.lstNs.Count - 3).ToString() + "more");
				}

				var notificationBuilder = new Notification.Builder(this)
					  .SetSmallIcon(Resource.Drawable.icon)
					.SetLargeIcon(largeIcon)
					.SetContentTitle("uWatch Notification")
					.SetContentText(message)
					.SetAutoCancel(true)
					//.SetGroup("uwatch")
					.SetTicker("1")
					  .SetNumber(MyController.lstNs.Count)
					  .SetStyle(nsStyle)
					.SetContentIntent(pendingIntent)
					 //.SetSound(Settings.System.DefaultNotificationUri)
					.SetSound(soundUri)
					.SetPriority((int)NotificationPriority.Max);

				var notificationManager = (NotificationManager)GetSystemService(Context.NotificationService);
				//Random rnd = new Random();
				//int random = rnd.Next(1, 123456);
				//notificationManager.SetInterruptionFilter(InterruptionFilter.Priority);
				notificationManager.Notify(0, notificationBuilder.Build());
				//UserDialogs.Instance.HideLoading();
			}
			catch(System.Exception ex)
			{ 
			}
		}
	}
}
