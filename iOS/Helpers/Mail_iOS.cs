using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
#if __UNIFIED__
using Foundation;
using MessageUI;
using uWatch.iOS;
using UIKit;
#else
using MonoTouch.Foundation;
using MonoTouch.MessageUI;
using MonoTouch.UIKit;
#endif

[assembly: Xamarin.Forms.Dependency(typeof(Mail_iOS))]
namespace uWatch.iOS
{
	public class Mail_iOS : IMail
	{
		private MFMailComposeViewController _mailController;

		async void IMail.SendMail(List<string> listOfRecepients, List<string> listOfRecepientsBCC, List<string> listOfRecepientsCC, string subject, string message, List<string> listOfAttachement)
		{
			try
			{


				_mailController = new MFMailComposeViewController();
				_mailController.SetSubject(subject);
				_mailController.SetMessageBody(message, false);
				_mailController.SetToRecipients(listOfRecepients.ToArray());

				if (listOfRecepientsCC.Count > 0)
					_mailController.SetCcRecipients(listOfRecepientsCC.ToArray());

				if (listOfRecepientsBCC.Count > 0)
					_mailController.SetBccRecipients(listOfRecepientsBCC.ToArray());

				foreach (var attachment in listOfAttachement)
				{
					if (File.Exists(attachment))
					{
						var filename = attachment.Substring(attachment.LastIndexOf('/') + 1);
						_mailController.AddAttachmentData(UIImage.FromFile(attachment).AsJPEG(), "image", filename);
					}
				}

				EventHandler<MFComposeResultEventArgs> handler = null;
				handler = (sender, e) =>
								{
									_mailController.Finished -= handler;

									var uiViewController = sender as UIViewController;
									if (uiViewController == null)
									{
										throw new ArgumentException("sender");
									}

									uiViewController.DismissViewController(true, () => { });
								};

				_mailController.Finished += handler;

				_mailController.PresentUsingRootViewController();

			}
			catch (System.Exception ex)
			{
				//await DisplayAlert("Error", ex.Message, "OK");
			}
		}

	}
}

