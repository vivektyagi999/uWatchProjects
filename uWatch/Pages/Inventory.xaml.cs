using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Acr.UserDialogs;
using UwatchPCL;
using Xamarin.Forms;

namespace uWatch
{
    public partial class Inventory : ContentPage
    {
        bool isPageCalled = false;
        public Inventory()
        {
            try
            {
                isPageCalled = true;
            }
            catch (Exception ex)
            {

            }
        }

        protected override void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                if (isPageCalled)
                {
                    InitializeComponent();
                    NavigationPage.SetBackButtonTitle(this, "");
                    BindingContext = new InventoryViewModel();
                    isPageCalled = false;
                }
            }
            catch (Exception ex)
            {

            }

        }
        async void intentorylist_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Loading...");
                await Task.Delay(500);
                var itme = e.Item as Room;
                InventoryDetails.RoomId = itme.RoomID;
                var result = await ApiService.Instance.GetRoomDetailsList(itme.RoomID.ToString()).ConfigureAwait(true);
                var resultimagelist = await ApiService.Instance.RoomImagesList(itme.RoomID.ToString()).ConfigureAwait(true);
                await Navigation.PushAsync(new InventoryDetails(result, resultimagelist));
                await Task.Delay(500);
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        void imgedit_Tapped(object sender, System.EventArgs e)
        {
            try
            {
                var senderlabel = sender as Button;
                var txt = senderlabel.Text;
                var strroomname = txt.Split('#')[0];
                var strroomid = txt.Split('#')[1];
                Navigation.PushAsync(new InventoryEdit(false, strroomname, strroomid));
            }
            catch (Exception ex)
            {

            }
        }
        //public static void NavigatetoEdit(bool foradd,string name)
        //{
        //    try
        //    {
        //        Navigation.PushAsync(new InventoryEdit(false));
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
        void Add_Clicked(object sender, System.EventArgs e)
        {
            try
            {
                Navigation.PushAsync(new InventoryEdit(true));
            }
            catch (Exception ex)
            {

            }
        }
    }
}
