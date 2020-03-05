using System;
using Xamarin;
using Xamarin.Forms;
using System.Collections.Generic;
using uWatch.iOS;
using Foundation;
using AssetsLibrary;
using UwatchPCL;
using System.IO;
using System.Threading.Tasks;
//using MessageUI;
//using UIKit;
#if __UNIFIED__
using MessageUI;
using UIKit;
#else
using MonoTouch.MessageUI;
using MonoTouch.UIKit;
#endif

[assembly: Xamarin.Forms.Dependency(typeof(MMS_iOS))]
namespace uWatch.iOS
{

    public class MMS_iOS : IMMS
    {

        public void SendMMS(string to, string msg, List<string> contents, AlertsEsclatedToAgentViewModel _alertsEsclatedToAgentViewModel)
        {
            try
            {
                var items = new System.Collections.Generic.List<NSObject>();
                foreach (var content in contents)
                {
                    if (File.Exists(content))
                    {
                        var filename = content.Substring(content.LastIndexOf('/') + 1);
                        items.Add(UIImage.FromFile(content).AsJPEG());
                    }
                }

                if (to == "Email")
                {


                    MFMailComposeViewController _mailController = new MFMailComposeViewController() ;
                    _mailController.SetMessageBody(msg, false);
                    _mailController.SetSubject("Escalate Alert Details");
                   
                    foreach (var attachment in contents)
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




                else if (to == "Sms")
                {

                    var smsController = new MFMessageComposeViewController { Body = msg };
                    foreach (var attachment in contents)
                    {
                        if (File.Exists(attachment))
                        {
                            var filename = attachment.Substring(attachment.LastIndexOf('/') + 1);
                            smsController.AddAttachment(UIImage.FromFile(attachment).AsJPEG(), "image", filename);
                        }
                    }
                    smsController.Finished += (sender, e) => smsController.DismissViewController(true, null);
                    UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(smsController, true, null);


                }

                else if (to == "Image")
                {

                    var controller = new UIActivityViewController(items.ToArray(), null);
                    controller.SetValueForKey(NSObject.FromObject("Alert Escalate"), new NSString("subject"));
                    UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(controller, true, null);
                }
                else if (to == "Details")
                {
                    items.Add(new NSString(msg ?? string.Empty));
                    var controller = new UIActivityViewController(items.ToArray(), null);
                    controller.SetValueForKey(NSObject.FromObject("Alert Escalate"), new NSString("subject"));
                    UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(controller, true, null);
                }
            }
            catch (Exception ex)
            {

            }


        }
    }

    //public class MMS_iOS : IMMS
    //{
    //    public void SendMMS(string to, string msg, List<string> contents, AlertsEsclatedToAgentViewModel _alertsEsclatedToAgentViewModel)
    //    {
    //        var items = new System.Collections.Generic.List<NSObject>();
    //        if(to=="Image")
    //        {
    //            //if (File.Exists(contents[1]))
    //            //{
    //            //    var filename = contents[1].Substring(contents[1].LastIndexOf('/') + 1);
    //            //    items.Add(UIImage.FromFile(contents[1]).AsJPEG());
    //            //}
    //            foreach (var content in contents)
    //            {
    //                if (File.Exists(content))
    //                {
    //                    var filename = content.Substring(content.LastIndexOf('/') + 1);
    //                    items.Add(UIImage.FromFile(content).AsJPEG());
    //                }
    //            }
    //        }
    //        else if(to=="Details")
    //        {
    //            items.Add(new NSString(msg ?? string.Empty));
    //        }


    //        //var items = new System.Collections.Generic.List<NSObject> { new NSString(msg ?? string.Empty) };

    //        //foreach (var content in contents)
    //        //{
    //        //    if (File.Exists(content))
    //        //    {
    //        //        var filename = content.Substring(content.LastIndexOf('/') + 1);
    //        //        items.Add(UIImage.FromFile(content).AsJPEG());
    //        //    }
    //        //}

    //        var controller = new UIActivityViewController(items.ToArray(), null);
    //        controller.SetValueForKey(NSObject.FromObject("Alert Escalate"), new NSString("subject"));
    //        UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(controller, true, null);

    //        //var messageController = new MFMessageComposeViewController();

    //        //if (MFMessageComposeViewController.CanSendText)
    //        //{
    //        	//messageController.Body = msg;

    //        	////Add attachment as NSData, and set the uti   
    //        	//foreach (var content in contents)
    //        	//{
    //        	//	if (File.Exists(content))
    //        	//	{
    //        	//		var filename = content.Substring(content.LastIndexOf('/') + 1);
    //        	//		var result = messageController.AddAttachment(UIImage.FromFile(content).AsJPEG(), filename, filename);
    //        	//	}
    //        	//}

    //        	//EventHandler<MFMessageComposeResultEventArgs> handler = null;
    //        	//handler = (sender, args) =>
    //        	//{
    //        	//	messageController.Finished -= handler;
    //        	//	var uiViewController = sender as UIViewController;
    //        	//	if (uiViewController == null)
    //        	//	{
    //        	//		throw new ArgumentException("sender");
    //        	//	}
    //        	//	uiViewController.DismissViewController(true, () => { });
    //        	//};
    //        	//messageController.Finished += handler;

    //        	//messageController.PresentUsingRootViewController();

    //        //}
    //    }
    //}
}
