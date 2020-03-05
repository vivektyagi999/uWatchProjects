using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using Acr.UserDialogs;
using FFImageLoading.Forms;
using UwatchPCL;
using UwatchPCL.Helpers;
using Xamarin.Forms;

namespace uWatch
{
    public class AlertListCell : ViewCell
    {
        CachedImage AlertImageIcon, AlertImageBatchIcon;
        CachedImage GpsIconImage, SetetileIconImage, EscalateImage;
        Label Fullname, contact, AlertDateTime, BatchAlertCount;
        double font;
        StringCaseConverter _StringCaseConverter = new StringCaseConverter();
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext != null)
            {
                var item = (AlertsEsclatedToAgentViewModel)BindingContext;
                try
                {
                    if (item != null)
                    {
                        if (item.geo_coords != "")
                        {
                            if (item.lSource != null)
                            {

                                if (item.lSource.ToLower() == "gps")
                                {
                                    GpsIconImage.Source = "stalite.png";

                                }
                                else
                                {
                                    GpsIconImage.Source = "tower.png";

                                }

                            }

                        }
                        if (Settings.RoleID == 3)
                        {
                            if (item.FullName != null)
                            {
                                if (item.FullName != string.Empty)
                                {
                                    Fullname.Text = item.FullName;
                                }
                                else
                                {
                                    Fullname.Text = "";
                                }
                            }
                            else
                            {
                                Fullname.Text = "";
                            }
                            if (item.Mobile1 != null)
                            {
                                if (item.Mobile1 != string.Empty)
                                {
                                    contact.Text = item.Mobile1;
                                }
                                else
                                {
                                    contact.Text = "";
                                }
                            }
                            else
                            {
                                contact.Text = "";
                            }

                        }
                        if (item.IsBatchUpload)
                        {
                            AlertImageBatchIcon.Source = "batch.png";
                            BatchAlertCount.Text = "X" + item.TotalBatchAlert;

                        }
                    }
                }
                catch (System.Exception ex)
                {

                }
            }

        }
        private void OnViewBindingContext(object sender, EventArgs e)
        {
            try
            {
                GpsIconImage.Source = null;
                AlertImageBatchIcon.Source = null;
                BatchAlertCount.Text = "";
                Fullname.Text = "";
                if (!(sender is StackLayout grid)) { return; }

                if (grid.BindingContext is AlertsEsclatedToAgentViewModel item)
                {
                    if (item.geo_coords != "")
                    {
                        if (item.lSource != null)
                        {

                            if (item.lSource.ToLower() == "gps")
                            {
                                GpsIconImage.Source = "stalite.png";

                            }
                            else
                            {
                                GpsIconImage.Source = "tower.png";

                            }

                        }

                    }
                    if (Settings.RoleID == 3)
                    {
                        if (item.FullName != null)
                        {
                            if (item.FullName != string.Empty)
                            {
                                Fullname.Text = item.FullName;
                            }
                            else
                            {
                                Fullname.Text = "";
                            }
                        }
                        else
                        {
                            Fullname.Text = "";
                        }
                        if (item.Mobile1 != null)
                        {
                            if (item.Mobile1 != string.Empty)
                            {
                                contact.Text = item.Mobile1;
                            }
                            else
                            {
                                contact.Text = "";
                            }
                        }
                        else
                        {
                            contact.Text = "";
                        }

                    }
                    if (item.IsBatchUpload)
                    {
                        AlertImageBatchIcon.Source = "batch.png";
                        BatchAlertCount.Text = "X" + item.TotalBatchAlert;

                    }
                }
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
        public AlertListCell()
        {

#if __IOS__
            var deleteAction = new Xamarin.Forms.MenuItem ();
            deleteAction.IsDestructive = true;
            deleteAction.Text = "                ";
            deleteAction.Clicked += async (sender, e) => 
            {
                try
                {
                    var data = ((Xamarin.Forms.MenuItem)sender).BindingContext as AlertsEsclatedToAgentViewModel;
                    var networkconnection = DependencyService.Get<INetworkConnection>();
                    networkconnection.CheckNetworkConnection();
                    var networkStatus = networkconnection.IsConnected ? "Connected" : "Not Connected";
                    if (networkStatus == "Not Connected")
                    {
                        UserDialogs.Instance.Alert("Please check your internet connection.", "", "OK");
                        return;
                    }
                    var alertarr = new int[] { data.alertlog_idx };
                   
                    var currentPage = (App.Current.MainPage as MasterDetailPage).Detail.Navigation.NavigationStack[0];
                    (currentPage as AlertListPage).SwipeDeleteItem(alertarr);
                }
                catch (Exception ex)
                {

                }
            };
            if (Settings.RoleID == 6)
            {
                ContextActions.Add(deleteAction);
            }

#endif
            if (Device.Idiom == TargetIdiom.Tablet)
            {
                font = 16;
            }
            else
            {
                font = 14;
            }

            try
            {
                Fullname = new Label
                {
                    FontAttributes = FontAttributes.Bold,
                    FontSize = font,
                    TextColor = Color.FromHex("#666"),
                    HorizontalTextAlignment = TextAlignment.Start,
                    VerticalOptions = LayoutOptions.Start
                };
                contact = new Label
                {
                    FontAttributes = FontAttributes.Bold,
                    FontSize = font,
                    TextColor = Color.FromHex("#666"),
                    HorizontalTextAlignment = TextAlignment.Start,
                    VerticalOptions = LayoutOptions.Start
                };

                var nameLabel = new Label()
                {
                    FontSize = font + 2,
                    TextColor = Color.Black,
                    FontAttributes = FontAttributes.Bold,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.StartAndExpand,
                };
                nameLabel.SetBinding(Label.TextProperty, new Binding("FriendlyName", BindingMode.TwoWay, stringFormat: "{0}"));

                var AlertTypeImage = new CachedImage()
                {
                    WidthRequest = 18,
                    HeightRequest = 18,
                    CacheDuration = TimeSpan.FromDays(30),
                    DownsampleToViewSize = true,
                    RetryCount = 3,
                    RetryDelay = 250,
                    TransparencyEnabled = false,
                    Aspect = Aspect.AspectFit,
                    LoadingPlaceholder = ImageSource.FromFile("blankcard.jpg"),
                    VerticalOptions = LayoutOptions.End,
                    HorizontalOptions = LayoutOptions.Center,
                };
                AlertTypeImage.SetBinding(CachedImage.SourceProperty, new Binding("AlertTypeImage", BindingMode.TwoWay));

                AlertImageIcon = new CachedImage()
                {
                    WidthRequest = 18,
                    HeightRequest = 18,
                    VerticalOptions = LayoutOptions.End,
                    HorizontalOptions = LayoutOptions.Center,
                };
                AlertImageIcon.SetBinding(CachedImage.SourceProperty, new Binding("AlertImageIcon", BindingMode.TwoWay));


                AlertImageBatchIcon = new CachedImage()
                {
                    WidthRequest = 18,
                    HeightRequest = 18,
                    VerticalOptions = LayoutOptions.End,
                    HorizontalOptions = LayoutOptions.Center,
                };
                BatchAlertCount = new Label()
                {
                    FontSize = 10
                };

                AlertDateTime = new Label()
                {
                    FontAttributes = FontAttributes.Bold,
                    FontSize = font - 2,
                    TextColor = Color.FromHex("#666"),
                    HorizontalTextAlignment = TextAlignment.Start,
                    VerticalOptions = LayoutOptions.Start
                };
                AlertDateTime.SetBinding(Label.TextProperty, new Binding("AlertDateTime", BindingMode.TwoWay));//, stringFormat: day + " " + date));

                var TempratureImage = new CachedImage()
                {
                    WidthRequest = 18,
                    HeightRequest = 18,
                    CacheDuration = TimeSpan.FromDays(30),
                    DownsampleToViewSize = true,
                    RetryCount = 3,
                    RetryDelay = 250,
                    TransparencyEnabled = false,
                    Aspect = Aspect.AspectFit,
                    LoadingPlaceholder = ImageSource.FromFile("blankcard.jpg"),
                    //ErrorPlaceholder = ImageSource.FromFile("noimage.gif"),
                    VerticalOptions = LayoutOptions.Start,
                    HorizontalOptions = LayoutOptions.Start,
                };
                TempratureImage.SetBinding(CachedImage.SourceProperty, new Binding("TemperatureImage", BindingMode.TwoWay));

                var TempratureLabel = new Label()
                {
                    FontAttributes = FontAttributes.Bold,
                    FontSize = font - 2,
                    TextColor = Color.FromHex("#666"),
                    HorizontalTextAlignment = TextAlignment.Start,
                    VerticalOptions = LayoutOptions.Center
                };
                TempratureLabel.SetBinding(Label.TextProperty, new Binding("DegC", BindingMode.TwoWay, stringFormat: "{0}.C"));


                var signalImage = new CachedImage()
                {
                    WidthRequest = 18,
                    HeightRequest = 18,
                    CacheDuration = TimeSpan.FromDays(30),
                    DownsampleToViewSize = true,
                    RetryCount = 3,
                    RetryDelay = 250,
                    TransparencyEnabled = false,
                    Aspect = Aspect.AspectFit,
                    LoadingPlaceholder = ImageSource.FromFile("blankcard.jpg"),
                    //ErrorPlaceholder = ImageSource.FromFile("noimage.gif"),
                    VerticalOptions = LayoutOptions.Start,
                    HorizontalOptions = LayoutOptions.Start
                };
                signalImage.SetBinding(CachedImage.SourceProperty, new Binding("SignalImage", BindingMode.TwoWay));
                var SignalLabel = new Label()
                {
                    FontAttributes = FontAttributes.Bold,
                    FontSize = font - 2,
                    TextColor = Color.FromHex("#666"),
                    HorizontalTextAlignment = TextAlignment.Start,
                    HorizontalOptions = LayoutOptions.Start,
                    VerticalOptions = LayoutOptions.Center
                };
                SignalLabel.SetBinding(Label.TextProperty, new Binding("Signal", BindingMode.TwoWay, stringFormat: "{0}%"));


                var BatteryImage = new CachedImage()
                {
                    WidthRequest = 18,
                    HeightRequest = 18,
                    CacheDuration = TimeSpan.FromDays(30),
                    DownsampleToViewSize = true,
                    RetryCount = 3,
                    RetryDelay = 250,
                    TransparencyEnabled = false,
                    Aspect = Aspect.AspectFit,
                    LoadingPlaceholder = ImageSource.FromFile("blankcard.jpg"),
                    VerticalOptions = LayoutOptions.Start,
                    HorizontalOptions = LayoutOptions.Start,
                };
                BatteryImage.SetBinding(CachedImage.SourceProperty, new Binding("BatteryImage", BindingMode.TwoWay));

                var BatteryLabel = new Label()
                {
                    FontAttributes = FontAttributes.Bold,
                    FontSize = font - 2,
                    TextColor = Color.FromHex("#666"),
                    HorizontalTextAlignment = TextAlignment.Start,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Start,
                };
                BatteryLabel.SetBinding(Label.TextProperty, new Binding("Battery", BindingMode.TwoWay, stringFormat: "{0}%"));


                EscalateImage = new CachedImage()
                {
                    HeightRequest = 18,
                    WidthRequest = 18,
                    CacheDuration = TimeSpan.FromDays(30),
                    DownsampleToViewSize = true,
                    RetryCount = 3,
                    RetryDelay = 250,
                    TransparencyEnabled = false,
                    Aspect = Aspect.AspectFit,
                    LoadingPlaceholder = ImageSource.FromFile("blankcard.jpg"),
                    HorizontalOptions = LayoutOptions.Start,
                    VerticalOptions = LayoutOptions.Start
                };
                EscalateImage.SetBinding(CachedImage.SourceProperty, new Binding("EscalateImage", BindingMode.TwoWay));


                GpsIconImage = new CachedImage()
                {

                    HeightRequest = 18,
                    WidthRequest = 18,
                    CacheDuration = TimeSpan.FromDays(30),
                    DownsampleToViewSize = true,
                    RetryCount = 3,
                    RetryDelay = 250,
                    TransparencyEnabled = false,
                    Aspect = Aspect.AspectFit,
                    LoadingPlaceholder = ImageSource.FromFile("blankcard.jpg"),
                    ErrorPlaceholder = ImageSource.FromFile("noimage.gif"),
                    HorizontalOptions = LayoutOptions.Start,
                    VerticalOptions = LayoutOptions.Start
                };

                var starImage = new CachedImage()
                {
                    Source = "right_arrow.png",
                    HeightRequest = 18,
                    WidthRequest = 18,
                    CacheDuration = TimeSpan.FromDays(30),
                    DownsampleToViewSize = true,
                    RetryCount = 3,
                    RetryDelay = 250,
                    TransparencyEnabled = false,
                    Aspect = Aspect.AspectFit,
                    LoadingPlaceholder = ImageSource.FromFile("blankcard.jpg"),
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand
                };
                starImage.SetBinding(CachedImage.SourceProperty, new Binding("RightArrow", BindingMode.TwoWay));
                var StartSubLayout1 = new StackLayout()
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    Children =
                {
                    AlertTypeImage,
                    AlertImageIcon,
                    AlertImageBatchIcon,
                        BatchAlertCount

                }
                };

                var lblStartSubLayout2Text = new Label
                {
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand
                };
                lblStartSubLayout2Text.SetBinding(Label.TextProperty, new Binding("AlertTypeName", BindingMode.TwoWay));

                var StartSubLayout2 = new StackLayout()
                {
                    Orientation = StackOrientation.Vertical,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.EndAndExpand,
                    Children =
                {
                    lblStartSubLayout2Text
                }
                };



                var TopContentLayout = new StackLayout()
                {


                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Orientation = StackOrientation.Horizontal,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    Padding = new Thickness(5, 0, 0, 2),

                    Children =
                {
                    nameLabel,
                }
                };

                var SubLayoutStak1 = new StackLayout()
                {

                    Orientation = StackOrientation.Vertical,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,

                    Children =
                {
                    StartSubLayout1,
                    StartSubLayout2
                }
                };



                var CenterSubLayout = new StackLayout()
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.Center,
                    Children =
                {
                    Fullname,contact
                }
                };

                var CenterSubLayout1 = new StackLayout()
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.Center,
                    Children =
                {
                    AlertDateTime
                }
                };

                var CenterSubLayout2 = new StackLayout()
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.Center,
                    Children =
                {
                    TempratureImage,
                    TempratureLabel,
                    BatteryImage,
                    BatteryLabel,
                    signalImage,
                    SignalLabel,
                    EscalateImage,
                    GpsIconImage
                }
                };

                CenterSubLayout2.SetBinding(VisualElement.IsVisibleProperty, new Binding("IsBattery", BindingMode.TwoWay));

                var SubLayoutStak2 = new StackLayout()
                {
                    Orientation = StackOrientation.Vertical,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,

                };
                if (Settings.RoleID == 3)
                {
                    SubLayoutStak2.Children.Add(CenterSubLayout);
                }
                SubLayoutStak2.Children.Add(CenterSubLayout1);
                SubLayoutStak2.Children.Add(CenterSubLayout2);


                var SubLayoutStak3 = new StackLayout()
                {
                    HorizontalOptions = LayoutOptions.EndAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    Padding = new Thickness(0, 0, 8, 0),
                    Children =
                {
                    starImage,
                }
                };

                var BottomContentLayout = new StackLayout()
                {
                    Padding = new Thickness(2, 2, 2, 5),
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    Children =
                {
                    SubLayoutStak1,
                    SubLayoutStak2,
                    SubLayoutStak3
                }
                };
                var box = new BoxView { Color = Color.Black,HeightRequest = 1, Margin =0, HorizontalOptions =LayoutOptions.FillAndExpand };
                var MainStackLayout = new StackLayout()
                {
                    Padding = 0,
                    Spacing = 0,
                    Orientation = StackOrientation.Vertical,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    Children =
                {
                    TopContentLayout,
                    BottomContentLayout,
                    box
                }
                };
               // MainStackLayout.SetBinding(VisualElement.BackgroundColorProperty, new Binding("StackColor",BindingMode.TwoWay));
                var buttonList = new Button
                {
                    BackgroundColor = Color.Transparent,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand
                };
                var grid = new Grid ();
                grid.SetBinding(VisualElement.BackgroundColorProperty, new Binding("StackColor", BindingMode.TwoWay));
                grid.Children.Add(MainStackLayout, 0, 0);
                grid.Children.Add(buttonList, 0, 0);
                buttonList.Clicked += async (sender, e) =>
                 {

                     try
                     {
                         buttonList.BackgroundColor = Color.FromHex("#1A000000");
                         await Task.Delay(100);
                         buttonList.BackgroundColor = Color.Transparent;
#if __IOS__

                        

                         if ((sender as Button).BindingContext is AlertsEsclatedToAgentViewModel data)
                         {
                           
                             if (!(data.AlertTypeName == "Low battery 10%" || data.AlertTypeName == "Low battery 30%"))
                             {
                                 UserDialogs.Instance.ShowLoading("Loading...");
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


                                 var alert = data;

                                 var ExistingAlertId = alert.alertlog_idx;
                                 AlertsEsclatedToAgentViewModel req = new AlertsEsclatedToAgentViewModel
                                 {
                                     alertlog_idx = ExistingAlertId
                                 };
                                 var EscaltingAlert = await ApiService.Instance.GetAlert(req);

                                 //await Task.Delay(250);
                                 if (Settings.RoleID == 3)
                                 {
                                     await (Application.Current.MainPage as MasterDetailPage).Detail.Navigation.PushAsync(new AlertDetailsForAgent(EscaltingAlert));
                                 }
                                 else
                                 {
                                     await (Application.Current.MainPage as MasterDetailPage).Detail.Navigation.PushAsync(new AlertDetail(EscaltingAlert));
                                 }
                                 await Task.Delay(750);

                                 UserDialogs.Instance.HideLoading();
                             }
                             else
                             {
                                 
                                 if (Device.RuntimePlatform == Device.Android)
                                 {
                                     UserDialogs.Instance.Toast(new ToastConfig(new ToastEvent { }, "", "Details not available."));

                                 }
                                 else
                                 {
                                   await  UserDialogs.Instance.AlertAsync("Details not available.","Alert","Ok");

                                 }




                             }
                         }
#endif
                     }
                     catch
                     {
                         UserDialogs.Instance.HideLoading();
                     }

                 };



                View = grid;
#if __ANDROID__
                try
                {
                    this.View.BindingContextChanged += OnViewBindingContext;

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
#endif
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex);
            }

        }

    }
}

