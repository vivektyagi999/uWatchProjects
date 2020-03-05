using System;
using System.Collections.Generic;
using Acr.UserDialogs;
using Rg.Plugins.Popup.Pages;
using UwatchPCL;
using UwatchPCL.WebServices;
using Xamarin.Forms;
using UwatchPCL.Helpers;
using System.Text.RegularExpressions;
using Rg.Plugins.Popup.Extensions;

namespace uWatch
{
    
    public partial class AddMacAddressPopUp : PopupPage
    {
        WebServiceManager webServiceManager;
        BleTransConfigModel configModel;
        int div = 2;
        public AddMacAddressPopUp(BleTransConfigModel configModel)
        {
            InitializeComponent();
            webServiceManager = new WebServiceManager();
            this.configModel = configModel;
            entryMacAddress.TextChanged += OnMacAddressTextChanged;
            entrySleepTime.Text = "1";
           // entrySleepTime.TextChanged += OnSleepTimeTextChanged;

        }

        private void OnSleepTimeTextChanged(object sender, TextChangedEventArgs e)
        {
            if(!string.IsNullOrEmpty(e.NewTextValue))
            {
                if ((Convert.ToInt32(e.NewTextValue) <= 240) && (Convert.ToInt32(e.NewTextValue) > 0))
                {
                    entrySleepTime.Text = e.NewTextValue;
                }
                else
                {
                    entrySleepTime.Text = e.OldTextValue;
                }
            }
            else
            {
                entrySleepTime.Text = e.OldTextValue;
            }
        }

        private void OnMacAddressTextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue.Length > 17)
            {
                entryMacAddress.Text = e.OldTextValue;
            }
            else
            {
                if (e.NewTextValue.Length != 17)
                {

                    if (e.NewTextValue.Length > 1)
                    {
                        var index = e.NewTextValue.LastIndexOf(":");
                        if (index == -1)
                        {
                            div = 2;
                        }
                        else
                        {
                            div = index + 3;
                        }

                        if (!(e.NewTextValue.Length == e.OldTextValue.LastIndexOf(":")))
                        {
                            if (e.NewTextValue.Length == div)
                            {
                                entryMacAddress.Text = e.NewTextValue + ":";
                            }
                        }
                        if (e.NewTextValue.Length - index == 4)
                        {
                            //if (!(e.NewTextValue[e.NewTextValue.Length - 1] == ':'))
                            //{
                                entryMacAddress.Text = e.OldTextValue + ":" + e.NewTextValue[e.NewTextValue.Length - 1];
                            //}
                        }
                    }
                }

            }
        }

        async void CancelClickedAsync(object sender, System.EventArgs e)
        {
            await Navigation.PopPopupAsync();
        }
        void SaveClicked(object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(entryMacAddress.Text))
            {
                UserDialogs.Instance.Alert("please enter mac address", "Mac Address", "Ok");
                return;
            }
            if (!(Regex.Match(entryMacAddress.Text, @"^[0-9a-fA-F]{2}(((:[0-9a-fA-F]{2}){5})|((:[0-9a-fA-F]{2}){5}))$").Success))
            {
                UserDialogs.Instance.Alert("Invalid mac address", "Mac Address", "Ok");
            }
            else if (string.IsNullOrEmpty(entrySleepTime.Text))
            {
                UserDialogs.Instance.Alert("Please provide sleep time between 1-240 ", "Slave sleep time", "Ok");
            }
            else if(Convert.ToInt32(entrySleepTime.Text)>240)
            {
                UserDialogs.Instance.Alert("Please provide sleep time between 1-240 ", "Slave sleep time", "Ok");
            }
            else
            {

                var networkconnection = DependencyService.Get<INetworkConnection>();
                networkconnection.CheckNetworkConnection();
                var networkStatus = networkconnection.IsConnected ? "Connected" : "Not Connected";
                if (networkStatus == "Not Connected")
                {

                    UserDialogs.Instance.Alert("Please check your internet connection", "Alert", "Ok");
                    return;
                }
                UserDialogs.Instance.ShowLoading();
                Device.BeginInvokeOnMainThread(async () =>
                {
                    configModel.CreatedBy = Settings.UserID;
                    configModel.FriendlyName = entryFriendlyName.Text;
                    configModel.MacAddress = entryMacAddress.Text;
                    configModel.SlaveSleepTime = Convert.ToInt32(entrySleepTime.Text);
                    var result = await webServiceManager.LoginWithTagAsync(configModel);
                    if (result.ErrorCode == 0)
                    {
                        if (result.IsSuccess)
                        {
                            if (result.Data)
                            {
                                await Navigation.PopPopupAsync();
                            }
                        }
                        UserDialogs.Instance.HideLoading();
                    }
                    else if (result.ErrorCode > 0)
                    {
                        UserDialogs.Instance.Alert(result.ErrorMessage, "Alert", "Ok");
                        UserDialogs.Instance.HideLoading();
                    }
                    else if (result.ErrorCode < 0)
                    {
                        UserDialogs.Instance.Alert(result.ErrorMessage, "Alert", "Ok");
                        UserDialogs.Instance.HideLoading();
                    }
                    else
                    {
                        UserDialogs.Instance.Alert(result.ErrorMessage, "Alert", "Ok");
                        UserDialogs.Instance.HideLoading();
                    }
                });
            }
        }
    }
}
