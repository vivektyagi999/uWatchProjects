using Xamarin.Forms;
using UwatchPCL;
using Acr.UserDialogs;
using UwatchPCL.Helpers;
using System.Threading.Tasks;
using UwatchPCL.Model;
using Rg.Plugins.Popup.Services;

namespace uWatch
{
    public partial class MainPage : MasterDetailPage
    {
        public NavigationPage nav;
        public MainPage(clsAccessToken un)
        {

            try
            {

                Title = "MainPage";
                var menuPage = new MenuPages();
                menuPage.OnMenuTap = (page) =>
                {
                    IsPresented = false;
                    Detail = new NavigationPage(page);
                };

                Master = menuPage;
                var startpage = Task.Run(async () => await GetStartPage());
                Detail = new NavigationPage(startpage.Result);
                CheckTagAsync(un);

            }
            catch (System.Exception)
            {
            }

        }

        public MainPage()
        {

            try
            {

                Title = "MainPage";

                var menuPage = new MenuPages
                {
                    OnMenuTap = (page) =>
                    {
                        IsPresented = false;
                        Detail = new NavigationPage(page);
                    }
                };

                Master = menuPage;
                var startpage = Task.Run(async () => await GetStartPage());
                Detail = new NavigationPage(startpage.Result);

            }
            catch (System.Exception)
            {

            }

        }

        private async void CheckTagAsync(clsAccessToken un)
        {
            //un.LastLoginTime = null;
            if (un.LastLoginTime == null && Settings.RoleID == (int)UserRole.Owner)
            {
                UserDialogs.Instance.HideLoading();

                await PopupNavigation.Instance.PushAsync(new AddCubeConfigurationPopUp()
                {
                    CloseWhenBackgroundIsClicked = false
                }, true);

            }
        }

        private async Task<Page> GetStartPage()
         {

            Page p = null;
            try
            {

                if (Settings.RoleID == 8)
                {
                    var assetsViewModel = new DashboardViewModel(this.Navigation);
                    await assetsViewModel.LoadDevices();
                    if (assetsViewModel.DeviceList.Count >= 1)
                    {
                        IsPresented = true;
                        p = new AssetsPage(assetsViewModel);

                    }

                    else if (assetsViewModel.DeviceList.Count == 0)
                    {

                        assetsViewModel.Device = new DeviceStatic() { device_idx = 0 };
                        var ViewModel = new DeviceDetailsViewModel();
                        if (ViewModel.AssetsList == null)
                            ViewModel.AssetsList = new System.Collections.ObjectModel.ObservableCollection<DeviceAssetsModel>();
                        ViewModel.AssetsList = await ViewModel.GetDeviceAssets(assetsViewModel.Device.device_idx);
                        ViewModel.device = assetsViewModel.Device;
                        p = new AssetsPage(ViewModel);
                    }

                  
                }
                else
                {
                    IsPresented = true;
                    var alertListViewModel = new AlertsListViewModel(this.Navigation, Settings.UserID);
                    if (Settings.RoleID == 3)
                    {
                        await alertListViewModel.LoadAlertListOfAgents();

                    }
                    else
                    {
                        await alertListViewModel.LoadAlertLists();
                    }

                    p = new AlertListPage(0, alertListViewModel);
                }

            }

            catch (System.Exception ex)
            {

            }

            return p;
        }

        

     
    }
}

