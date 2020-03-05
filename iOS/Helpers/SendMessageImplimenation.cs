using System;
using System.Collections.Generic;
using System.Net;
using CoreFoundation;
using MessageUI;
using Plugin.Messaging;
using SystemConfiguration;
using UIKit;
using uWatch.iOS;
using UwatchPCL;
using Xamarin.Forms;
[assembly: Dependency(typeof(SendMessageImplimenation))]
namespace uWatch.iOS
{
	public class SendMessageImplimenation : ISendMessage
	{
		private MFMailComposeViewController _mailController;

		public void SendMessages(string mobileNumber)
		{
			List<string> listOfRecepients = new List<string>();
			listOfRecepients.Add(mobileNumber);
			var messageController = new MFMessageComposeViewController();

			if (MFMessageComposeViewController.CanSendText)
			{
				messageController.Recipients = listOfRecepients.ToArray();

				EventHandler<MFMessageComposeResultEventArgs> handler = null;
				handler = (sender, args) =>
				{
					messageController.Finished -= handler;
					var uiViewController = sender as UIViewController;
					if (uiViewController == null)
					{
						throw new ArgumentException("sender");
					}
					uiViewController.DismissViewController(true, () => { });
				};
				messageController.Finished += handler;

				messageController.PresentUsingRootViewController();

			}	}

		public void SendMail(string emailid)
		{
			List<string> listOfRecepients = new List<string>();
			listOfRecepients.Add(emailid);
				try
				{


					_mailController = new MFMailComposeViewController();
					_mailController.SetToRecipients(listOfRecepients.ToArray());


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

		public void Call(string mobileNumber)
		{
			var phoneCallTask = MessagingPlugin.PhoneDialer;
			if (phoneCallTask.CanMakePhoneCall)
				phoneCallTask.MakePhoneCall(mobileNumber);
			

		}
	}
		
	public static class SamplesExtensions
	{
		#region Methods


		public static void SendSampleSms(this ISmsTask smsTask,string number)
		{
			if (smsTask.CanSendSms)
			{
				smsTask.SendSms(number, "");
			}


		}


		public static void SendSampleEmail(this IEmailTask emailTask, AlertsEsclatedToAgentViewModel _alertsEsclatedToAgentViewModel)
		{
			try
			{
				if (emailTask.CanSendEmailAttachments)
				{
				//	var uri = new Uri("mailto:" + "kshamamishra@virtualemployee.com");
				//	UIKit.UIApplication.SharedApplication.OpenUrl(new Foundation.NSUrl(uri.ToString()));
				//	Device.OpenUri(new Uri("mailto:kshamamishra@virtualemployee.com?subject=MySubject&body=MyFile&attachment=" + "http://www.rishabhsoft.com/wp-content/uploads/2016/04/Rapid-Mobile-App-Development-with-Xamarin-Plugins.png "));
					//var emails = new EmailMessageBuilder()

					//emailTask.SendEmail("to.plugins@xamarin.com", "Xamarin Messaging Plugin", "Well hello there from Xam.Messaging.Plugin");

					// Alternatively use EmailBuilder fluent interface to construct more complex e-mail with multiple recipients, bcc, attachments etc. 
					var email = new EmailMessageBuilder()
					  .To("")
					  .Cc("")
					  .Subject("")
					  .Body("")
						//.WithAttachment("http://www.rishabhsoft.com/wp-content/uploads/2016/04/Rapid-Mobile-App-Development-with-Xamarin-Plugins.png")
					  .Build();

					emailTask.SendEmail(email);

					//emailTask.SendEmail("", _alertsEsclatedToAgentViewModel.FullName + "," + "http://www.myvisitinghours.org/pics/96.jpg");
				}
			}
			catch (Exception ex)
			{
				
			}


		}


		#endregion
	}
}

