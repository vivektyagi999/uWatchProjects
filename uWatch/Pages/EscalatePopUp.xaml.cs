using System;
using System.Collections.Generic;
using Acr.UserDialogs;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using UwatchPCL;
using Xamarin.Forms;

namespace uWatch
{
    public partial class EscalatePopUp : PopupPage
    {
        AlertsEsclatedToAgentViewModel sendToActionPage;
        List<string> listOfContent;
        public EscalatePopUp(List<string> _listOfContent,AlertsEsclatedToAgentViewModel _sendToActionPage)
        {
            InitializeComponent();
            if(_sendToActionPage!=null)
            {
                sendToActionPage = _sendToActionPage;
            }
            if(_listOfContent!=null)
            {
                listOfContent = _listOfContent;
            }
        }

        async void Handle_ClickedAsync(object sender, System.EventArgs e)
        {
            await Navigation.PopPopupAsync();
        }

        async void SmsClickedAsync(object sender, System.EventArgs e)
        {
            await Navigation.PopPopupAsync();

            var day = sendToActionPage.DeviceDate.ToString("ddd");
            var date = DateFormat.GetDateTime(sendToActionPage.DeviceDate, TimeType.DateAndTime);
            DependencyService.Get<IMMS>().SendMMS("Sms", "From: " + sendToActionPage.OwnerFullName + "," + sendToActionPage.AddressLine1 + " " + sendToActionPage.AddressLine2 + "\n" + sendToActionPage.Mobile1 + " " + sendToActionPage.Mobile2 + "\n" + "Alert Device: " + sendToActionPage.FriendlyName + "/ sn " + sendToActionPage.serial_no + "\n" + "Type: " + MyController.GetAlertTypeName(sendToActionPage.alert_type) + ", " + day + " " + date, listOfContent, sendToActionPage);

        }
        async void EmailClicked(object sender, System.EventArgs e)
        {
            await Navigation.PopPopupAsync();

            var day = sendToActionPage.DeviceDate.ToString("ddd");
            var date = DateFormat.GetDateTime(sendToActionPage.DeviceDate, TimeType.DateAndTime);
            DependencyService.Get<IMMS>().SendMMS("Email", "From: " + sendToActionPage.OwnerFullName + "," + sendToActionPage.AddressLine1 + " " + sendToActionPage.AddressLine2 + "\n" + sendToActionPage.Mobile1 + " " + sendToActionPage.Mobile2 + "\n" + "Alert Device: " + sendToActionPage.FriendlyName + "/ sn " + sendToActionPage.serial_no + "\n" + "Type: " + MyController.GetAlertTypeName(sendToActionPage.alert_type) + ", " + day + " " + date, listOfContent, sendToActionPage);


        }
        async void OtherClicked(object sender, System.EventArgs e)
        {
            var day = sendToActionPage.DeviceDate.ToString("ddd");
            var date = DateFormat.GetDateTime(sendToActionPage.DeviceDate, TimeType.DateAndTime);
            string type = "";
            await Navigation.PopPopupAsync();
            var result = await UserDialogs.Instance.ActionSheetAsync("Select to escalate", "Cancel", null, new string[] { "Escalate Alert Image", "Escalate Alert Details" });
            if (result == "Escalate Alert Image")
            {
                type = "Image";
            }
            else if (result == "Escalate Alert Details")
            {
                type = "Details";
            }
            else
            {
                UserDialogs.Instance.HideLoading();
                return;
            }
            DependencyService.Get<IMMS>().SendMMS(type, "From: " + sendToActionPage.OwnerFullName + "," + sendToActionPage.AddressLine1 + " " + sendToActionPage.AddressLine2 + "\n" + sendToActionPage.Mobile1 + " " + sendToActionPage.Mobile2 + "\n" + "Alert Device: " + sendToActionPage.FriendlyName +  "/ sn " +  sendToActionPage.serial_no + "\n" + "Type: " + MyController.GetAlertTypeName(sendToActionPage.alert_type) + ", " + day + " " + date, listOfContent, sendToActionPage);


        }


    }
}
