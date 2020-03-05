using System;
using Android.App;
using Android.Content;
using Firebase.Messaging;
using Android.Support.V4.App;
using System.Linq;
using Android.Graphics;
using UwatchPCL;
using Android.Media;
using Android.OS;
using static Android.Gms.Common.Apis.GoogleApi;

namespace uWatch.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class MyFirebaseMessagingService : FirebaseMessagingService
    {
        const string TAG = "MyFirebaseMsgService";

        /**
         * Called when message is received.
         */

        // [START receive_message]
        public override void OnMessageReceived(RemoteMessage message)
        {
            // TODO(developer): Handle FCM messages here.
            // If the application is in the foreground handle both data and notification messages here.
            // Also if you intend on generating your own notifications as a result of a received FCM
            // message, here is where that should be initiated. See sendNotification method below.
            try
            {
                var NotificationBody = message.Data.Values.ToList().FirstOrDefault();
                if (message.Data.ContainsKey("AlertID"))
                {
                    MyController.AlertId = message.Data["AlertID"].ToString();
                    MyController.isMessage = false;
                }
                else if (message.Data.ContainsKey("MailID"))
                {
                    MyController.AlertId = message.Data["MailID"].ToString();
                    MyController.isMessage = true;
                }
                if (NotificationBody != null)
                {
                    global::Android.Util.Log.Debug(TAG, "From: " + message.From);
                    global::Android.Util.Log.Debug(TAG, "Notification Message Body: " + NotificationBody);
                    if (MyController.isMessage)
                    {
                        SendMessages(message.Data["body"].ToString());
                    }
                    else
                    {
                        SendNotification(NotificationBody);
                    }
                }
                else if (message.GetNotification().Body != null)
                {
                    global::Android.Util.Log.Debug(TAG, "From: " + message.From);
                    global::Android.Util.Log.Debug(TAG, "Notification Message Body: " + message.GetNotification().Body);
                    if (MyController.isMessage)
                    {
                        SendMessages(message.GetNotification().Body);
                    }
                    else
                    {
                        SendNotification(message.GetNotification().Body);
                    }
                }
            }
            catch (System.Exception ex)
            {

            }


        }
        // [END receive_message]
        /**
	* Create and show a simple notification containing the received FCM message.
	*/

        static readonly string CHANNEL_ID = "location_notification";
        void SendNotification(string messageBody)
        {
            Intent intent;
            //if (MyController.isAppClosed)
            //{
            intent = new Intent(this, typeof(SplashScreen));
            //}
            //else
            //{
            //	intent = new Intent(this, typeof(MainActivity));
            //}
            intent.AddFlags(ActivityFlags.ClearTop);
            var pendingIntent = PendingIntent.GetActivity(this, 0 /* Request code */, intent, PendingIntentFlags.OneShot);

            string pathToPushSound = "android.resource://" + this.PackageName + "/raw/" + Resource.Drawable.uWatchTone;
            global::Android.Net.Uri soundUri = global::Android.Net.Uri.Parse(pathToPushSound);
            
            var name = Resources.GetString(Resource.String.common_google_play_services_notification_channel_name);
            NotificationManager mNotificationManager = (NotificationManager)Application.Context.GetSystemService(Context.NotificationService);
            NotificationChannel mChannel;
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                mChannel = new NotificationChannel(CHANNEL_ID, name, NotificationImportance.High);
                AudioAttributes audioAttributes = new AudioAttributes.Builder()
                        .SetContentType(AudioContentType.Sonification)
                        .SetUsage(AudioUsageKind.Notification)
                        .Build();
                mChannel.SetSound(soundUri, audioAttributes);

                if (mNotificationManager != null)
                {
                    mNotificationManager.CreateNotificationChannel(mChannel);
                }

                var notificationBuilder1 = new NotificationCompat.Builder(this, CHANNEL_ID)
               .SetSmallIcon(Resource.Drawable.micon48)
               .SetContentTitle("uWatch Alert")
               .SetContentText(messageBody)
               .SetAutoCancel(true)
               .SetStyle(new NotificationCompat.BigTextStyle().BigText(messageBody))
               .SetLargeIcon(BitmapFactory.DecodeResource(this.Resources, Resource.Drawable.icon))
               // .SetSound(soundUri)
               .SetContentIntent(pendingIntent);
                var notificationManager1 = NotificationManager.FromContext(this);
                Random random1 = new Random();
                int m1 = random1.Next(9999 - 1000) + 1000;
                //notificationManager.Notify(0 /* ID of notification */, notificationBuilder.Build());
                notificationManager1.Notify(m1 /* ID of notification */, notificationBuilder1.Build());
            }
            else
            {
                var notificationBuilder = new NotificationCompat.Builder(this, CHANNEL_ID)
               .SetSmallIcon(Resource.Drawable.micon48)
               .SetContentTitle("uWatch Alert")
               .SetContentText(messageBody)
               .SetAutoCancel(true)
               .SetStyle(new NotificationCompat.BigTextStyle().BigText(messageBody))
               .SetLargeIcon(BitmapFactory.DecodeResource(this.Resources, Resource.Drawable.icon))
               .SetSound(soundUri)
               .SetContentIntent(pendingIntent);

                var notificationManager = NotificationManager.FromContext(this);
                Random random = new Random();
                int m = random.Next(9999 - 1000) + 1000;
                //notificationManager.Notify(0 /* ID of notification */, notificationBuilder.Build());
                notificationManager.Notify(m /* ID of notification */, notificationBuilder.Build());
            }
           
        }
        void SendMessages(string messageBody)
        {
            Intent intent;
            //if (MyController.isAppClosed)
            //{
            intent = new Intent(this, typeof(SplashScreen));
            //}
            //else
            //{
            //	intent = new Intent(this, typeof(MainActivity));
            //}
            intent.AddFlags(ActivityFlags.ClearTop);
            var pendingIntent = PendingIntent.GetActivity(this, 0 /* Request code */, intent, PendingIntentFlags.OneShot);

            //string pathToPushSound = "android.resource://" + this.PackageName + "/raw/" + Resource.Drawable.uWatchTone;
            //global::Android.Net.Uri soundUri = global::Android.Net.Uri.Parse(pathToPushSound);

            var notificationBuilder = new NotificationCompat.Builder(this)
                .SetSmallIcon(Resource.Drawable.micon24)
                .SetContentTitle("uWatch Messages")
                .SetContentText(messageBody)
                .SetAutoCancel(true)
                .SetStyle(new NotificationCompat.BigTextStyle().BigText(messageBody))
                .SetLargeIcon(BitmapFactory.DecodeResource(this.Resources, Resource.Drawable.icon))
                //.SetSound(soundUri)
                .SetContentIntent(pendingIntent);
            try
            {
                global::Android.Net.Uri notification = global::Android.Net.Uri.Parse("android.resource://" + this.PackageName + "/" + Resource.Drawable.uWatchTone);
                Ringtone r = RingtoneManager.GetRingtone(global::Android.App.Application.Context, notification);
                r.Play();
            }
            catch (Exception e)
            {

            }
            var notificationManager = NotificationManagerCompat.From(this);
            Random random = new Random();
            int m = random.Next(9999 - 1000) + 1000;
            //notificationManager.Notify(0 /* ID of notification */, notificationBuilder.Build());
            notificationManager.Notify(m /* ID of notification */, notificationBuilder.Build());
        }
    }
}
