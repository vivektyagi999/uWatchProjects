using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using UwatchPCL;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace uWatch
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomConfirmPage : PopupPage
    {
        string pageTitle;
        int[] alertId;
        AlertsListViewModel ViewModel;
        public CustomConfirmPage(int[] _alertId,AlertsListViewModel viewModel)
        {
            alertId = _alertId;
            ViewModel = viewModel;
            InitializeComponent();
        }
        async void Handle_ClickedAsync(object sender, System.EventArgs e)
        {
            var buttonConfirm = sender as Button;
            if (buttonConfirm.Text == "Yes")
            {
                Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
                {
                    var result = await ApiService.Instance.DeleteAlertLogs(alertId);
                    if (result.ErrorCode == 0)
                    {
                        var deleteItem = ViewModel.AlertList.Where(X => X.alertlog_idx == alertId[0]).FirstOrDefault();
                        ViewModel.AlertList.Remove(deleteItem);
                        await Navigation.PopPopupAsync();
                    }
                    else if (result.ErrorCode == 1)
                    {
                        await Navigation.PopPopupAsync();
                        UserDialogs.Instance.Alert(result.ErrorMessage, " Alert ", "OK");
                    }
                    else
                    {
                        await Navigation.PopPopupAsync();
                        UserDialogs.Instance.Alert(result.ErrorMessage, " Alert ", "OK");
                    }
                });

            }
            else if (buttonConfirm.Text == "No")
            {
                await Navigation.PopPopupAsync();
                //MenuListItemPage.Main_MenuPage.IsPresented = false;
            }
        }

    }

}
