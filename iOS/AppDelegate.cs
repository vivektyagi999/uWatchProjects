using System;
using System.Collections.Generic;
using System.Linq;
using UwatchPCL;
using Foundation;
using UIKit;
using UwatchPCL.Helpers;
using AudioToolbox;
using AVFoundation;
using MessageUI;
using Acr.UserDialogs;
using Xamarin.Forms;
using Plugin.BLE;
using System.Runtime.InteropServices;

namespace uWatch.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        private string str = "uWatchTone.mp3";
        NSUrl url = null;
        string Alertid;
        SystemSound systemSound;
        private AVAudioPlayer backgroundMusic;
        private string backgroundSong = "";
        public bool MusicOn { get; set; } = true;
        public float MusicVolume { get; set; } = 5.5f;

        public bool EffectsOn { get; set; } = true;
        public float EffectsVolume { get; set; } = 1.0f;
        public static AppDelegate Instance { get; private set; }


        //private const string NotificationSoundPath = @"/System/Library/Audio/UISounds/New/Fanfare.caf";


        public void PlayBackgroundMusic(string filename)
        {
            NSUrl songURL;

            // Music enabled?
            if (!MusicOn) return;

            // Any existing background music?
            if (backgroundMusic != null)
            {
                //Stop and dispose of any background music
                backgroundMusic.Stop();
                backgroundMusic.Dispose();
            }

            // Initialize background music
            songURL = url;
            NSError err;
            backgroundMusic = new AVAudioPlayer(songURL, "wav", out err);
            backgroundMusic.Volume = MusicVolume;
            backgroundMusic.FinishedPlaying += delegate
            {
                backgroundMusic = null;
            };
            backgroundMusic.NumberOfLoops = 0;
            backgroundMusic.Play();
            backgroundSong = filename;

        }


        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            Xamarin.FormsMaps.Init();
            FFImageLoading.Forms.Touch.CachedImageRenderer.Init();
            ZXing.Net.Mobile.Forms.iOS.Platform.Init();
            Rg.Plugins.Popup.Popup.Init();
            url = NSUrl.FromFilename(str);
            var ble = CrossBluetoothLE.Current;

            UINavigationBar.Appearance.TintColor = UIColor.White;
            UINavigationBar.Appearance.BarTintColor = UIColor.Red;
            UINavigationBar.Appearance.SetTitleTextAttributes(new UITextAttributes()
            {
                TextColor = UIColor.White,
            });

            Instance = this;
            App.ScreenWidth = (int)UIScreen.MainScreen.Bounds.Width;
            App.ScreenHeight = (int)UIScreen.MainScreen.Bounds.Height;

           

            //Notification
            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                var pushSettings = UIUserNotificationSettings.GetSettingsForTypes
                (
                    UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
                    new NSSet()
                );

                UIApplication.SharedApplication.RegisterUserNotificationSettings(pushSettings);
                UIApplication.SharedApplication.RegisterForRemoteNotifications();
            }
            else
            {
                UIRemoteNotificationType notificationTypes = UIRemoteNotificationType.Alert | UIRemoteNotificationType.Badge | UIRemoteNotificationType.Sound;
                UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(notificationTypes);
            }
            //processNotification(app, options, true);

            MyController.ScreenWidth = (int)UIScreen.MainScreen.Bounds.Width;
            MyController.ScreenHeight = (int)UIScreen.MainScreen.Bounds.Height;
            DependencyService.Register<IAdapter, Adapter>();
            LoadApplication(new App());
            return base.FinishedLaunching(app, options);
        }

        void HandleAction1(UIAlertAction obj)
        {

        }

        void HandleAction2(UIAlertAction obj)
        {


            LoadingScreenPage.GetAlertPage();
            UIApplication.SharedApplication.ApplicationIconBadgeNumber -= 1;

        }

        void processNotification(UIApplication application, NSDictionary options, bool fromFinishedLaunching)
        {
            //Check to see if the dictionary has the aps key.  This is the notification payload you would have sent
            if (null != options && options.ContainsKey(new NSString("aps")))
            {
                //Get the aps dictionary
                NSDictionary aps = options.ObjectForKey(new NSString("aps")) as NSDictionary;

                string alert = string.Empty;
                string sound = string.Empty;
                int badge = 0;

                //Extract the alert text
                //NOTE: If you're using the simple alert by just specifying "  aps:{alert:"alert msg here"}  "
                //      this will work fine.  But if you're using a complex alert with Localization keys, etc., your "alert" object from the aps dictionary
                //      will be another NSDictionary... Basically the json gets dumped right into a NSDictionary, so keep that in mind
                if (aps.ContainsKey(new NSString("mailid")))
                {
                    MyController.isMessage = true;
                    if (aps.ContainsKey(new NSString("mailid")))
                        alert = (aps[new NSString("mailid")] as NSString).ToString();

                    //Extract the sound string
                    if (aps.ContainsKey(new NSString("sound")))
                        sound = (aps[new NSString("sound")] as NSString).ToString();


                    //Extract the badge

                    string badgeStr = (aps[new NSString("badge")] as NSObject).ToString();
                    int.TryParse(badgeStr, out badge);
                }
                else
                {
                    MyController.isMessage = false;
                    if (aps.ContainsKey(new NSString("alert")))
                        alert = (aps[new NSString("alert")] as NSString).ToString();

                    //Extract the sound string
                    if (aps.ContainsKey(new NSString("sound")))
                        sound = (aps[new NSString("sound")] as NSString).ToString();

                    string badgeStr = (aps[new NSString("badge")] as NSObject).ToString();
                    int.TryParse(badgeStr, out badge);

                }
                //If this came from the ReceivedRemoteNotification while the app was running,
                // we of course need to manually process things like the sound, badge, and alert.
                if (!fromFinishedLaunching)
                {
                    //Manually set the badge in case this came from a remote notification sent while the app was open
                    if (badge >= 0)
                        UIApplication.SharedApplication.ApplicationIconBadgeNumber += 1;


                    //Manually play the sound
                    if (!string.IsNullOrEmpty(alert))
                    {
                        //PlayBackgroundMusic("announcement.mp3");
                        systemSound = new SystemSound(url);
                        if (systemSound != null)
                        {
                            systemSound.PlayAlertSound();

                        }
                    }
                    string str = "";
                    if (application.ApplicationState == UIApplicationState.Active)
                    {
                        if (!string.IsNullOrEmpty(alert))
                        {
                            UIViewController topController = (application.KeyWindow.RootViewController);

                            while ((topController.PresentedViewController) != null)
                            {
                                topController = topController.PresentedViewController;
                            }



                            //Create Alert
                            string title, action = "";
                            if (MyController.isMessage)
                            {
                                title = "New uWatch Message";
                                action = "View Message";
                            }
                            else
                            {
                                title = "New uWatch Alert";
                                action = "View Alert";
                            }
                            var okCancelAlertController = UIAlertController.Create(title, "Please choose a action", UIAlertControllerStyle.Alert);

                            //Add Actions
                            okCancelAlertController.AddAction(UIAlertAction.Create(action, UIAlertActionStyle.Default, HandleAction2));
                            okCancelAlertController.AddAction(UIAlertAction.Create("Archive", UIAlertActionStyle.Cancel, HandleAction1));

                            topController.PresentViewController(okCancelAlertController, animated: true, completionHandler: null);
                        }
                    }
                    else if (application.ApplicationState == UIApplicationState.Inactive)
                    {

                        UIApplication.SharedApplication.ApplicationIconBadgeNumber += 1;



                    }
                    else if (application.ApplicationState == UIApplicationState.Background)
                    {
                        str = "app is in background, if content-available key of your notification is set to 1, poll to your backend to retrieve data and update your interface here";
                        UIAlertView avAlert = new UIAlertView("Notification", str, null, "OK", null);
                        avAlert.Show();
                    }
                }
            }

            //You can also get the custom key/value pairs you may have sent in your aps (outside of the aps payload in the json)
            // This could be something like the ID of a new message that a user has seen, so you'd find the ID here and then skip displaying
            // the usual screen that shows up when the app is started, and go right to viewing the message, or something like that.
            //if (null != options && options.ContainsKey(new NSString("customKeyHere")))
            //{
            //	launchWithCustomKeyValue = (options[new NSString("customKeyHere")] as NSString).ToString();

            //	//You could do something with your customData that was passed in here
            //}
        }


        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            //// Get current device token
            byte[] result = new byte[deviceToken.Length];
            Marshal.Copy(deviceToken.Bytes, result, 0, (int)deviceToken.Length);
            var DeviceToken = BitConverter.ToString(result).Replace("-", "");

            //var DeviceToken = deviceToken.Description;
            //string token = DeviceToken.ToString();
            //string NewToken = token.ToString().TrimStart('<').TrimEnd('>').Replace(" ", "");
            //	DeviceToken = NewToken;
            Settings.DeviceToken = DeviceToken;
          
            // Get previous device token
            var oldDeviceToken = NSUserDefaults.StandardUserDefaults.StringForKey("PushDeviceToken");

            // Has the token changed?
            if (string.IsNullOrEmpty(oldDeviceToken) || !oldDeviceToken.Equals(DeviceToken))
            {
                //TODO: Put your own logic here to notify your server that the device token has changed/been created!
            }

            // Save new device token 
            NSUserDefaults.StandardUserDefaults.SetString(DeviceToken, "PushDeviceToken");
        }

        public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        {

        }

        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            processNotification(application, userInfo, false);

            if (userInfo != null && userInfo.ContainsKey(new NSString("aps")))
            {
                NSDictionary alertMsg = userInfo;
                var f = alertMsg.Values;
                if (!MyController.isMessage)
                {
                    Alertid = (f[0]).ValueForKey(new NSString("AlertID")).ToString();
                    MyController.isMessage = false;
                }
                else
                {
                    Alertid = (f[0]).ValueForKey(new NSString("mailid")).ToString();
                    MyController.isMessage = true;
                }
                if (Alertid != null)
                    MyController.AlertId = Alertid.ToString();
            }

            if (application.ApplicationState == UIApplicationState.Inactive)
            {

                LoadingScreenPage.GetAlertPage();
                UIApplication.SharedApplication.ApplicationIconBadgeNumber -= 1;
            }

        }

        public override void ReceivedLocalNotification(UIApplication application, UILocalNotification notification)
        {

        }



        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            new UIAlertView("Error registering push notifications", error.LocalizedDescription, null, "OK", null).Show();
        }



        public override void OnActivated(UIApplication application)
        {
            try
            {
                // We need to properly handle activation of the application with regards to SSO
                //  (e.g., returning from iOS 6.0 authorization dialog or from fast app switching).
                //    FBSession.ActiveSession.HandleDidBecomeActive();  
                UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
            }
            catch
            {
            }
        }

        public void PushNotificationClear()
        {
            UIApplication.SharedApplication.ApplicationIconBadgeNumber = -1;
            UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
            UIApplication.SharedApplication.CancelAllLocalNotifications();
        }
    }
}

