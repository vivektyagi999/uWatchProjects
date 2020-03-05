using System;
using System.Net;

using Plugin.Messaging;
using uWatch.Droid;
using UwatchPCL;
using Xamarin.Forms;
[assembly: Dependency(typeof(SendMessageImplimenation))]
namespace uWatch.Droid
{
	public class SendMessageImplimenation : ISendMessage
	{

		public void SendMessages(string number)
		{
			CrossMessaging.Current.SmsMessenger.SendSampleSms(number);
		}

		public void SendMail(string emailId)
		{
			CrossMessaging.Current.EmailMessenger.SendSampleEmail(emailId);
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


		public static void SendSampleSms(this ISmsTask smsTask, string number)
		{
			if (smsTask.CanSendSms)
			{

				//smsTask.
				smsTask.SendSms(number, "");
			}


		}


		public static void SendSampleEmail(this IEmailTask emailTask, string emailId)
		{
			if (emailTask.CanSendEmailAttachments)
			{
				//emailTask.SendEmail("to.plugins@xamarin.com", "Xamarin Messaging Plugin", "Well hello there from Xam.Messaging.Plugin");

				// Alternatively use EmailBuilder fluent interface to construct more complex e-mail with multiple recipients, bcc, attachments etc. 
				var email = new EmailMessageBuilder()
				  .To(emailId)
				  .Cc("")
				  .Subject("")
				  .Body("")
					//.WithAttachment("http://www.rishabhsoft.com/wp-content/uploads/2016/04/Rapid-Mobile-App-Development-with-Xamarin-Plugins.png","application/json")
				  .Build();

				emailTask.SendEmail(email);
				
				//emailTask.SendEmail("", _alertsEsclatedToAgentViewModel.FullName + "," + "http://www.myvisitinghours.org/pics/96.jpg");
			}


		}

		#endregion
	}
}

