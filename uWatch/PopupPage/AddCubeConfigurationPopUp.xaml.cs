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
using Rg.Plugins.Popup.Services;
using System.Linq;

namespace uWatch
{
	public partial class AddCubeConfigurationPopUp : PopupPage
    {
        AddCubeConfigViewModel viewModel;
        BleTransConfigModel configModel;
        public AddCubeConfigurationPopUp()
        {
            InitializeComponent();
            this.BindingContext = viewModel = new AddCubeConfigViewModel();
            configModel = new BleTransConfigModel();
            listCube.ItemTapped += OnListItemTapped;
        }

        private void OnListItemTapped(object sender, ItemTappedEventArgs e)
        {
            var data = e.Item as CubeConfigure;
            if(data!=null)
            {
                viewModel.ExecuteCubeConfigSelectionCommand(data);
            }
                
        }



        //void Handle_Tapped(object sender, System.EventArgs e)
        //{
        //    Device.BeginInvokeOnMainThread(() =>
        //    {

        //        this.pkrCubeConfig.Focus();

        //    });
        //}

        async void ConfirmClickedAsync(object sender, System.EventArgs e)
        {
            
            if(viewModel.Value==0)
            {
                UserDialogs.Instance.Alert("please select cube configuration!", "Alert", "Ok");
                return;
            }

            var answer = await UserDialogs.Instance.ConfirmAsync("Do you have a tag?", "Tag", "Yes", "No");
            if (answer)
            {
                configModel.CreatedBy = Settings.UserID;
                configModel.Configuration_idx = viewModel.Value;
                configModel.SaveTag = true;
                await Navigation.PopPopupAsync();
                await PopupNavigation.PushAsync(new AddMacAddressPopUp(configModel)
                {
                    CloseWhenBackgroundIsClicked = false
                }, true);

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
                    configModel.Configuration_idx = viewModel.Value;
                    configModel.SaveTag = false;
                    var result = await viewModel.webServiceManager.LoginWithTagAsync(configModel);
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
