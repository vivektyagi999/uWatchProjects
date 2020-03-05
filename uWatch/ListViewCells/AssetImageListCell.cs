using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Acr.UserDialogs;
using FFImageLoading.Forms;
using UwatchPCL;
using UwatchPCL.Model;
using Xamarin.Forms;

namespace uWatch
{

    public class AssetImageListCell : ViewCell
    {

        protected override void OnBindingContextChanged()
        {
            try
            {
                var items = BindingContext as ObservableCollection<DeviceAssetsModel>;
                if (items == null)
                    return;

                int row = 0;
                var grid = new Grid
                {
                    Padding = new Thickness(7, 7, 7, 0),
                    BackgroundColor = Color.Transparent,
                    ColumnDefinitions = new ColumnDefinitionCollection {
                    new ColumnDefinition {  },
                    new ColumnDefinition {  },
                 }
                };
                if (items.Count == 1)
                {
                    items.Add(new DeviceAssetsModel { Deviceimage_thumbnail = null });
                }
                foreach (var item in items)
                {
                    var cellLayout = new Grid
                    {
                        ColumnSpacing = 1,
                        BackgroundColor = Color.Transparent,
                        RowDefinitions = new RowDefinitionCollection() 
                        {
                             new RowDefinition() {  },
                        }

                    };
                    var box = new BoxView
                    {
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions= LayoutOptions.FillAndExpand,
                        Color = Color.FromHex("#c0c0c0")
                    };
                    var image = new CachedImage
                    {

                    };
                    if (item.Deviceasset_idx != 0)
                    {
                        //image.Source = ImageSource.FromStream(() => new MemoryStream(item.Deviceimage_thumbnail));
                        image.Source = ApiService.ImageUrl + "uwatch/common/RenderImage?id=deviceassetsthumbnail_" + item.Deviceasset_idx;

                    }
                    cellLayout.Children.Add(box, 0, 0);

                    cellLayout.Children.Add(image, 0, 0);

                    grid.Children.Add(cellLayout, row++, 0);
                    var gestrure = new TapGestureRecognizer();
                    cellLayout.GestureRecognizers.Add(gestrure);
                    gestrure.Tapped += async (s, e) =>
                    {
                        if (item.Deviceasset_idx == 0)
                            return;
                        var networkConnection = DependencyService.Get<INetworkConnection>();
                        networkConnection.CheckNetworkConnection();
                        var networkStatus = networkConnection.IsConnected ? "Connected" : "Not Connected";
                        if (networkStatus != "Connected")
                        {
                            UserDialogs.Instance.Alert("Please check your internet connection.", "Alert", "OK");
                            return;
                        }
                        UserDialogs.Instance.ShowLoading("Loading...");
                        await Task.Delay(100);
                        var AssetDetailsViewModel = new AssetDetailsViewModel();
                        await AssetDetailsViewModel.GetActualAssetImage(item);
                        if (AssetDetailsViewModel.DeviceAsset != null)
                        {
                            (App.Current.MainPage as MasterDetailPage).Detail.Navigation.PushAsync(new NewAssetsDetailPage(item.Position_no, item, AssetDetailsViewModel), true);
                        }
                        UserDialogs.Instance.HideLoading();
                    };
                }
                View = grid;
            }
            catch (Exception ex)
            {

            }

        }
    }
}
