using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Acr.UserDialogs;
using UwatchPCL;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using UwatchPCL.Helpers;
using System.Windows.Input;
using Syncfusion.ListView.XForms;
using Rg.Plugins.Popup.Services;
using Syncfusion.SfPullToRefresh.XForms;
#if __ANDROID__
using uWatch.Droid;
using Android.App;
using Android.Widget;

#endif

namespace uWatch
{
    public partial class AlertListPage : ContentPage
    {
        bool IsMap;
        Xamarin.Forms.RelativeLayout relativelayout;
        Xamarin.Forms.ScrollView scrollview;
        SfPullToRefresh pullToRefresh;
        private int DeviceId;
        double w = MyController.VirtualWidth;
        double h = MyController.VirtualHeight;
        int itemIndex = -1;
        public static int ExistingAlertId { get; set; }
#if __IOS__
        private InfiniteListView listView;

#endif

#if __ANDROID__
        private SfListView listView;

#endif
        // private DataTemplate Cell { get; set; }
        private AlertsListViewModel ViewModel { get; set; }
        Geocoder geoCoder;

        public AlertListPage()
        {
            try
            {
                InitializeComponent();
            }
            catch { }
        }

        public AlertListPage(int deviceid, AlertsListViewModel viewmodel)
        {
            try
            {
                this.DeviceId = deviceid;
                ViewModel = viewmodel;

                InitializeComponent();

                BindingContext = ViewModel;


            }
            catch (Exception ex)
            {
                MyController.ErrorManagement(ex.Message);
            }
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                SetLayout();
#if __Android__
                listView.ItemsSource = ViewModel.AlertList;

#endif
                if (MyController.fromAssetsToGallery)
                {
                    await ViewModel.LoadAlertInExistingList(ExistingAlertId);

                }
            }
            catch (Exception ex)
            {
                MyController.ErrorManagement(ex.Message);
            }
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            GC.Collect();
        }
        public void SetLayout()
        {
            try
            {
                if (Settings.RoleID == 3)
                {
                    Title = "Escalated Alerts";
                }
                else
                {
                    Title = "Alerts";
                }
                relativelayout = new Xamarin.Forms.RelativeLayout();
                scrollview = new Xamarin.Forms.ScrollView();
                AddLayout();
                pullToRefresh.PullableContent = relativelayout;
                scrollview.Content = pullToRefresh;
                Content = pullToRefresh;
            }
            catch { }
        }

        public void SwipeDeleteItem(int[] alertId)
        {
            Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
            {
                await PopupNavigation.Instance.PushAsync(new CustomConfirmPage(alertId, ViewModel), true);

            });
            //Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
            //{
            //    var result = await ApiService.Instance.DeleteAlertLogs(alertId);
            //    if (result.ErrorCode==0)
            //    {
            //        var deleteItem = ViewModel.AlertList.Where(X => X.alertlog_idx == alertId[0]).FirstOrDefault();
            //        ViewModel.AlertList.Remove(deleteItem);
            //    }
            //    else if(result.ErrorCode==1)
            //    {
            //        UserDialogs.Instance.Alert(result.ErrorMessage, " Alert ", "OK");
            //    }
            //    else
            //    {
            //        UserDialogs.Instance.Alert(result.ErrorMessage, " Alert ", "OK");
            //    }
            //});
        }

        private async void AddLayout()
        {
            try
            {
                double newx40 = MyUiUtils.getPercentual(w, 40);
                double newy30 = MyUiUtils.getPercentual(h, 30);

                var RightSwipeTemplate = new DataTemplate(() =>
                {
                    var baseGrid = new Grid();
                    var gridLayout = new Grid();
                    gridLayout.BackgroundColor = Color.Red;
                    var image = new Image()
                    {
                        Source = "delete.png",
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        HeightRequest = 30,
                        WidthRequest = 30,
                        HorizontalOptions = LayoutOptions.CenterAndExpand
                    };
                    var tapGestureRecognizer = new TapGestureRecognizer();
                    tapGestureRecognizer.Tapped += (s, e) =>
                    {
                        try
                        {
                            if (itemIndex >= 0)
                            {
                                var data = ViewModel.AlertList[itemIndex];

                                var networkconnection = DependencyService.Get<INetworkConnection>();
                                networkconnection.CheckNetworkConnection();
                                var networkStatus = networkconnection.IsConnected ? "Connected" : "Not Connected";
                                if (networkStatus == "Not Connected")
                                {
                                    UserDialogs.Instance.Alert("Please check your internet connection.", "", "OK");
                                    return;
                                }
                                var alertarr = new int[] { data.alertlog_idx };
                                SwipeDeleteItem(alertarr);


                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    };
                    image.GestureRecognizers.Add(tapGestureRecognizer);

                    gridLayout.Children.Add(image);
                    baseGrid.Children.Add(gridLayout);
                    return new ViewCell { View = baseGrid };
                });


                var loadMoreTemplate = new DataTemplate(() =>
                {
                    var stackLayout = new StackLayout();
                    stackLayout.HeightRequest = 0;
                    return new ViewCell { View = stackLayout };
                });

#if __IOS__
                listView = new InfiniteListView();
                listView.HasUnevenRows = true;
                listView.LoadMoreCommand = ViewModel.LoadCharactersCommand;
                listView.BindingContext = ViewModel;
                listView.SetBinding(Xamarin.Forms.ListView.ItemsSourceProperty, new Binding("AlertList", BindingMode.TwoWay));
                var Cell = new DataTemplate(typeof(AlertListCell));
                listView.IsPullToRefreshEnabled = true;
                listView.Refreshing += OnPullRefreshAsync;
                listView.ItemTemplate = Cell;
                listView.BackgroundColor = Color.FromRgb(247, 247, 247);
              
                //listView.ItemTapped += async (sender, e) =>
                // {
                //     try
                //     {
                //         UserDialogs.Instance.ShowLoading("Loading...");
                //         await Task.Delay(750);
                //         if (e.Item != null)
                //         {
                //             var networkConnection = DependencyService.Get<INetworkConnection>();
                //             networkConnection.CheckNetworkConnection();
                //             var networkStatus = networkConnection.IsConnected ? "Connected" : "Not Connected";
                //             if (networkStatus != "Connected")
                //             {
                //                 UserDialogs.Instance.HideLoading();
                //                 UserDialogs.Instance.Alert("Please check your internet connection.", "Alert", "OK");
                //                 return;
                //             }


                //             MyController.fromAssetsToGallery = false;


                //             var alert = e.Item as AlertsEsclatedToAgentViewModel;

                //             ExistingAlertId = alert.alertlog_idx;
                //             AlertsEsclatedToAgentViewModel req = new AlertsEsclatedToAgentViewModel();
                //             req.alertlog_idx = ExistingAlertId;
                //             var EscaltingAlert = await ApiService.Instance.GetAlert(req);



                //             await Task.Delay(250);
                //             if (Settings.RoleID == 3)
                //             {
                //                 await Navigation.PushAsync(new AlertDetailsForAgent(EscaltingAlert));
                //             }
                //             else
                //             {
                //                 await Navigation.PushAsync(new AlertDetail(EscaltingAlert));
                //             }
                //             await Task.Delay(750);

                //             UserDialogs.Instance.HideLoading();
                //         }
                //     }
                //     catch
                //     {
                //         UserDialogs.Instance.HideLoading();
                //     }
                // };

#endif
                pullToRefresh = new SfPullToRefresh();
                pullToRefresh.TransitionMode = TransitionType.SlideOnTop;
                pullToRefresh.ProgressBackgroundColor = Color.White;
                pullToRefresh.ProgressStrokeColor = Color.Blue;
                pullToRefresh.PullingThreshold = 150;
                pullToRefresh.ProgressStrokeWidth = 14;
                pullToRefresh.RefreshContentThreshold = 50;
                pullToRefresh.RefreshContentHeight = 50;
                pullToRefresh.RefreshContentWidth = 50;
                pullToRefresh.IsRefreshing = false;
                pullToRefresh.Refreshing += OnPullRefreshAsync;

#if __ANDROID__
                listView = new SfListView
                {
                    ItemSpacing = 2f,
                    AutoFitMode = AutoFitMode.Height,
                    LoadMoreOption = LoadMoreOption.Auto
                };
                if (Settings.RoleID == 6)
                {
                    listView.AllowSwiping = true;
                }
                listView.RightSwipeTemplate = RightSwipeTemplate;
                listView.LoadMoreTemplate = loadMoreTemplate;
                listView.LoadMoreCommand = ViewModel.LoadCharactersCommand;
                listView.BindingContext = ViewModel;
                listView.SetBinding(SfListView.ItemsSourceProperty, new Binding("AlertList", BindingMode.TwoWay));
                var Cell = new DataTemplate(typeof(AlertListCell));
                listView.ItemTemplate = Cell;
                listView.SelectionBackgroundColor = Color.Transparent;
               
                listView.SwipeStarted += (sender, e) =>
                {
                    itemIndex = -1;
                };
                listView.SwipeEnded += (sender, e) =>
                {
                    itemIndex = e.ItemIndex;
                };


                //public async void OnDeleteAsync(object sender, EventArgs e)
                //{

                //    try
                //    {
                //        if (itemIndex >= 0)
                //        {
                //            var data = viewModel.SwarmAlertLists[itemIndex];

                //            var networkconnection = DependencyService.Get<INetworkConnection>();
                //            networkconnection.CheckNetworkConnection();
                //            var networkStatus = networkconnection.IsConnected ? "Connected" : "Not Connected";
                //            if (networkStatus == "Not Connected")
                //            {
                //                UserDialogs.Instance.Alert("Please check your internet connection.", "", "OK");
                //                return;
                //            }
                //            SwipeDeleteItem(data.SwarmID);
                //            //viewModel.MessageList.RemoveAt(itemIndex);
                //            //var result = await messageService.DeleteMessage("1" + data.MailID.ToString());
                //            //if (!result)
                //            //{
                //            //    networkconnection = DependencyService.Get<INetworkConnection>();
                //            //    networkconnection.CheckNetworkConnection();
                //            //    networkStatus = networkconnection.IsConnected ? "Connected" : "Not Connected";
                //            //    if (networkStatus == "Not Connected")
                //            //    {
                //            //        UserDialogs.Instance.Alert("Please check your internet connection.", "", "OK");
                //            //        //return;
                //            //    }
                //            //    MenuListItemPage.Main_MenuPage.Detail = new NavigationPage(new MessagePage());
                //            //}

                //        }
                //    }
                //    catch (Exception ex)
                //    {

                //    }


                //}

                listView.ItemTapped += async (sender, e) =>
                {
                    try
                    {
                        var alert = e.ItemData as AlertsEsclatedToAgentViewModel;
                        if (!(alert.AlertTypeName == "Low battery 10%" || alert.AlertTypeName == "Low battery 30%"))
                        {
                            UserDialogs.Instance.ShowLoading("Loading...");
                            await Task.Delay(750);
                            if (e.ItemData != null)
                            {
                                var networkConnection = DependencyService.Get<INetworkConnection>();
                                networkConnection.CheckNetworkConnection();
                                var networkStatus = networkConnection.IsConnected ? "Connected" : "Not Connected";
                                if (networkStatus != "Connected")
                                {
                                    UserDialogs.Instance.HideLoading();
                                    UserDialogs.Instance.Alert("Please check your internet connection.", "Alert", "OK");
                                    return;
                                }
                                MyController.fromAssetsToGallery = false;
                                ExistingAlertId = alert.alertlog_idx;
                                AlertsEsclatedToAgentViewModel req = new AlertsEsclatedToAgentViewModel();
                                req.alertlog_idx = ExistingAlertId;
                                var EscaltingAlert = await ApiService.Instance.GetAlert(req);

                                await Task.Delay(250);
                                if (Settings.RoleID == 3)
                                {
                                    await Navigation.PushAsync(new AlertDetailsForAgent(EscaltingAlert));
                                }
                                else
                                {
                                    await Navigation.PushAsync(new AlertDetail(EscaltingAlert));
                                }
                                await Task.Delay(750);

                                UserDialogs.Instance.HideLoading();
                            }

                        }
                        else
                        {

                            UserDialogs.Instance.Toast(new ToastConfig(new ToastEvent { },"","Details not available."));
                        }
                    }
                    catch
                    {
                        UserDialogs.Instance.HideLoading();
                    }
                };

#endif


                var customMap = new Map
                {

                    MapType = MapType.Satellite,
                    WidthRequest = w - 40,
                    HeightRequest = 500
                };
                if (Settings.RoleID == 3)
                {
                    foreach (var alert in ViewModel.AlertList)
                    {
                        if ((Convert.ToDouble(alert.lat) > 0) && Convert.ToDouble(alert.lang) > 0)
                        {
                            var pos = new Position(Convert.ToDouble(alert.lat), Convert.ToDouble(alert.lang));
                            var pin = new Pin
                            {
                                Type = PinType.Place,
                                Position = pos,
                            };
                            pin.Label = pos.Latitude.ToString() + "," + pos.Longitude.ToString();
                            customMap.Pins.Add(pin);

                            customMap.MoveToRegion(MapSpan.FromCenterAndRadius(pos, Distance.FromMiles(1.0)));
                            IsMap = true;
                        }
                        else
                        {
                            IsMap = false;
                        }
                    }
                }
                else
                {
                    foreach (var alert in ViewModel.AlertLastLocStatic)
                    {
                        if ((Convert.ToDouble(alert.lat) > 0) && Convert.ToDouble(alert.lang) > 0)
                        {
                            var pos = new Position(Convert.ToDouble(alert.lat), Convert.ToDouble(alert.lang));
                            var pin = new Pin
                            {
                                Type = PinType.Place,
                                Position = pos,
                            };

                            pin.Label = pos.Latitude.ToString() + "," + pos.Longitude.ToString();
                            customMap.Pins.Add(pin);
                            customMap.MoveToRegion(MapSpan.FromCenterAndRadius(pos, Distance.FromMiles(1.0)));
                            IsMap = true;
                        }
                        else
                        {
                            IsMap = false;
                        }
                    }
                }

                if (ViewModel.AlertList.Count > 0)
                {

                    if (customMap.Pins.Count > 0)
                    {
#if __ANDROID__
                        MyUILibrary.AddSFListView(relativelayout, listView, 0, 0, w, h / 2 + 70, Color.Black, 100);

#endif

#if __IOS__
                        MyUILibrary.AddListView(relativelayout, listView, 0, 0, w, h / 2 + 70, listView.SeparatorColor, 100);

#endif
                        if (Xamarin.Forms.Device.Idiom == TargetIdiom.Phone)
                        {
                            MyUILibrary.AddMap(relativelayout, customMap, 0, (h / 2) + 70, w, (h / 2 + 20) - 80);
                        }
                        else if (Xamarin.Forms.Device.Idiom == TargetIdiom.Tablet)
                        {
                            MyUILibrary.AddMap(relativelayout, customMap, 0, (h / 2 - 30) + 10, w, (h / 2 + 20));
                        }
                    }
                    else
                    {

#if __ANDROID__
                        MyUILibrary.AddSFListView(relativelayout, listView, 0, 0, w, h, Color.Black, 100);

#endif

#if __IOS__
                        MyUILibrary.AddListView(relativelayout, listView, 0, 0, w, h, listView.SeparatorColor, 100);

#endif

                    }
                }
                else
                {

#if __ANDROID__
                    var lblerror = MyUILibrary.AddLabel(relativelayout, "No Alerts to display", (w - newx40) / 2, newy30, newx40, 50, Color.Black, 18);

#endif

#if __IOS__
                    var lblerror = MyUILibrary.AddLabel(relativelayout, "No Alerts to display", (w - newx40) / 2, newy30, newx40, 50, listView.SeparatorColor, 18);

#endif
                }

            }
            catch (System.Exception ex)
            {
                await DisplayAlert("Error", ex.StackTrace, "Cancel");
            }
        }
        public int play { get; set; } = 0;

        private async void OnPullRefreshAsync(object sender, EventArgs e)
        {

            if (play == 0)
            {
                //Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
                //{
                play = 1;


                ViewModel.s = -1;
                await ViewModel.LoadAlertList();

#if __ANDROID__
                    //pullToRefresh.IsRefreshing = true;
                    listView.ItemsSource = ViewModel.AlertList;
                    pullToRefresh.IsRefreshing = false;

#endif

#if __IOS__
                listView.IsRefreshing = true;
                listView.ItemsSource = ViewModel.AlertList;
                listView.IsRefreshing = false;

#endif
                play = 0;

                //});

            }

        }

    }

}


