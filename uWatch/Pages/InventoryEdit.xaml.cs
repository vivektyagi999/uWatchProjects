using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Acr.UserDialogs;
using UwatchPCL;
using Xamarin.Forms;

namespace uWatch
{
    public partial class InventoryEdit : ContentPage
    {
        public static bool isUpdate;
        string RID = "";
        public InventoryEdit(bool isSave,string strlabel="",string RoomId="")
        {
            
            InitializeComponent();
            //BindingContext = new InventoryViewModel();
            NavigationPage.SetBackButtonTitle(this, "");
            if (isSave)
            {
                Title = "Add Inventory";
                btnUpdate.Text = "Save";
            }
            else
            {
                Title = "Update Inventory";
                btnUpdate.Text = "Update";
                RID = RoomId;
                nameEntry.Text = strlabel;
            }
            //if (isSave)
            //{
            //    btnUpdate.Command = new Command(AddInventory());
            //}


        }

        async void btnUpdate_Clicked(object sender, System.EventArgs e)
        {
            //async Task btnUpdate_ClickedAsync()
            //{
            try
            {
                if (btnUpdate.Text == "Save")
                {
                    if (!string.IsNullOrEmpty(nameEntry.Text))
                    {
                        UserDialogs.Instance.ShowLoading("Saving!");
                        var result = await ApiService.Instance.AddInventory(nameEntry.Text);
                        if (result.ErrorCode==0)
                        {
                            //UserDialogs.Instance.HideLoading();
                            isUpdate = true;
                            var mainPage = new MainPage();
                            mainPage.nav = new NavigationPage(new Inventory());
                            mainPage.Detail = mainPage.nav;
                            mainPage.IsPresented = false;

                            Xamarin.Forms.Application.Current.MainPage = mainPage;
                            await Task.Delay(500);
                            UserDialogs.Instance.HideLoading();
                            UserDialogs.Instance.Alert("Saved Sucessfully");
                        }
                        if (result.ErrorCode > 0)
                        {
                            UserDialogs.Instance.HideLoading();
                            UserDialogs.Instance.Alert(result.ErrorMessage);

                        }
                        if (result.ErrorCode < 0)
                        {
                            UserDialogs.Instance.HideLoading();
                            UserDialogs.Instance.Alert(result.ErrorMessage);

                        }
                    }
                    else
                    {
                        DisplayAlert("Information", "Kindly insert a valid name", "Ok");
                    }
                }
                if (btnUpdate.Text == "Update")
                {
                    if (!string.IsNullOrEmpty(nameEntry.Text))
                    {
                        UserDialogs.Instance.ShowLoading("Updating!");
                        var result = await ApiService.Instance.AddInventory(nameEntry.Text,Convert.ToInt32(RID));
                        if (result.ErrorCode==0)
                        {
                            
                            //UserDialogs.Instance.HideLoading();
                            var mainPage = new MainPage();
                            mainPage.nav = new NavigationPage(new Inventory());
                            mainPage.Detail = mainPage.nav;
                            mainPage.IsPresented = false;
                            Xamarin.Forms.Application.Current.MainPage = mainPage;
                            await Task.Delay(500);
                            UserDialogs.Instance.HideLoading();
                            UserDialogs.Instance.Alert("Updated Sucessfully");
                        }
                        if (result.ErrorCode > 0)
                        {
                            UserDialogs.Instance.HideLoading();
                            UserDialogs.Instance.Alert(result.ErrorMessage);

                        }
                        if (result.ErrorCode < 0)
                        {
                            UserDialogs.Instance.HideLoading();
                            UserDialogs.Instance.Alert(result.ErrorMessage);

                        }
                    }
                    else
                    {
                        DisplayAlert("Information", "Kindly insert a valid name", "Ok");
                    }
                }
            }
            catch (Exception ex)
            {

            }
            //}
        }

        /// <summary>
        /// To call add api for rooms, Date : 11.12.2017
        /// </summary>
        async Task btnUpdate_ClickedAsync(object sender, System.EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(nameEntry.Text))
                {
                    var result = await ApiService.Instance.AddInventory(nameEntry.Text);
                }
                else
                {
                    DisplayAlert("Information", "Kindly insert a valid name", "Ok");
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
