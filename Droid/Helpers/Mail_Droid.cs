using uWatch.Droid;
using Android.Telephony;
using Xamarin;
using Android.Content;
using Android.App;
using Xamarin.Forms;
using Java.IO;
using Android.OS;
using System.Collections.Generic;
using Plugin.Messaging;
using System.Linq;
using Android.Text;
using Android.Net;

[assembly: Xamarin.Forms.Dependency(typeof(Mail_Droid))]
namespace uWatch.Droid
{
	public class Mail_Droid : IMail
	{
		void IMail.SendMail(List<string> listOfRecepients, List<string> listOfRecepientsBCC, List<string> listOfRecepientsCC, string subject, string message, List<string> listOfAttachement)
		{
            Intent share_intent = new Intent();
            share_intent.SetAction(Intent.ActionSend);
            share_intent.SetType("image/png");
            //share_intent.PutExtra(Intent.ExtraStream,
            //Uri.fromFile(new File(url.toString())));
            //share_intent.setFlags(Intent.);
            share_intent.PutExtra(Intent.ExtraSubject, subject);
            share_intent.PutExtra(Intent.ExtraText, message);

			//string intentAction = Intent.ActionSend;
			//if (listOfAttachement.Count > 1)
			//	intentAction = Intent.ActionSendMultiple;

			//Intent mailIntent = new Intent(intentAction);
			//mailIntent.SetType("message/rfc822");

			if (listOfRecepients.Count > 0)
                share_intent.PutExtra(Intent.ExtraEmail, listOfRecepients.ToArray());

			if (listOfRecepientsCC.Count > 0)
                share_intent.PutExtra(Intent.ExtraCc, listOfRecepientsCC.ToArray());

			if (listOfRecepientsBCC.Count > 0)
                share_intent.PutExtra(Intent.ExtraBcc, listOfRecepientsBCC.ToArray());

			//mailIntent.PutExtra(Intent.ExtraSubject, subject);

			//mailIntent.PutExtra(Intent.ExtraText, message);

			if (listOfAttachement.Count > 0)
			{
				var uris = new List<IParcelable>();
				foreach (var attachment in listOfAttachement)
				{
					//var uri = Android.Net.Uri.Parse("file://" + attachment);
					var uri = Uri.Parse(attachment);
					uris.Add(uri);
				}

				if (uris.Count > 1)
                    share_intent.PutParcelableArrayListExtra(Intent.ExtraStream, uris);
				else
                    share_intent.PutExtra(Intent.ExtraStream, uris[0]);
			}

            share_intent.StartNewActivity();
		}

	}
}
