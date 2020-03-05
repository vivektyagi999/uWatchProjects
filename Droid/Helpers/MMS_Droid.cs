using Android.Telephony;
using Xamarin;
using Android.Content;
using Android.App;
using Xamarin.Forms;
using Java.IO;
using System.Collections.Generic;
using Android.OS;
using uWatch.Droid;
using Android.Net;
using Android.Provider;
using UwatchPCL;
using Android.Content.PM;
using System.Linq;
using Java.Util;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(MMS_Droid))]
namespace uWatch.Droid
{
    public class MMS_Droid : IMMS
    {

        public void SendMMS(string to, string msg, List<string> contents, AlertsEsclatedToAgentViewModel _alertsEsclatedToAgentViewModel)
        {
            Intent sendIntent = new Intent(Intent.ActionSendMultiple);
            sendIntent.SetType("image/*");

            if (to == "Email")
            {

                sendIntent.SetPackage("com.google.android.gm");

                var uris = new List<IParcelable>();
                foreach (var content in contents)
                {
                    var f = new File(content);
                    uris.Add(Uri.FromFile(f));
                }
                sendIntent.PutParcelableArrayListExtra(Intent.ExtraStream, uris);
                sendIntent.PutExtra(Intent.ExtraText, msg);
                sendIntent.PutExtra(Intent.ExtraSubject, "Escalate Alert Details");
                sendIntent.StartNewActivity();


            }
            else if (to == "Sms")
            {
                var data = sendIntent.ResolveActivity(global::Android.App.Application.Context.PackageManager);
                if (data != null)
                {
                    if (data.PackageName == "com.samsung.android.messaging")
                    {
                        sendIntent.SetPackage("com.samsung.android.messaging");
                    }
                    else
                    {
                        sendIntent.SetPackage(Telephony.Sms.GetDefaultSmsPackage(Forms.Context));

                    }
                }

                var uris = new List<IParcelable>();
                foreach (var content in contents)
                {
                    var f = new File(content);
                    uris.Add(Uri.FromFile(f));
                }
                sendIntent.PutParcelableArrayListExtra(Intent.ExtraStream, uris);
                sendIntent.PutExtra(Intent.ExtraText, msg);
                sendIntent.StartNewActivity();

            }

            else if (to == "Image")
            {
                var uris = new List<IParcelable>();
                foreach (var content in contents)
                {
                    var f = new File(content);
                    uris.Add(Uri.FromFile(f));
                }
                sendIntent.PutParcelableArrayListExtra(Intent.ExtraStream, uris);
                Forms.Context.StartActivity(Intent.CreateChooser(sendIntent, "Escalate to Others"));

            }
            else if (to == "Details")
            {


                sendIntent.PutExtra(Intent.ExtraText, msg);
                Forms.Context.StartActivity(Intent.CreateChooser(sendIntent, "Escalate to Others"));

               // sendIntent.StartNewActivity();

            }



        }
    }
}
