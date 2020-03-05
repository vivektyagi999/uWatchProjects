using System;
using UwatchPCL;
using Xamarin.Forms;

namespace uWatch
{
    public class AlertImagePage : ContentPage
    {
        public AlertImagePage(AlertImage _AlertImage, string PrevTitle)
        {
            if (Device.RuntimePlatform == Device.iOS)
            {
                NavigationPage.SetHasNavigationBar(this, false);
            }
            else
            {
                var uWatchlogo = new ToolbarItem()
                {
                    Icon = "uwatchlogo.png"
                };
                ToolbarItems.Add(uWatchlogo);
            }

            var browser = new CustomWebView();

            browser.BackgroundColor = Xamarin.Forms.Color.Black;

            string htmlsource = "<style>img{display: inline; height: auto; margin: 0 auto; max-width: 100%;}img-container {position: relative;top: 20%; width:100%; height:300px; overflow:hidden; text-align:center;}img{ max-width:100%;width:100%;vertical-align: middle;}\n</style><html>\n<head>\n</head>\n<body style =\"background-color:black\">\n< div class=\"img-container\"> </div>\n<img src = " + String.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(_AlertImage.Image)) + " />\n</body>\n</html>";

            var htmlSource = new HtmlWebViewSource();

            htmlSource.Html = @htmlsource;

            if (Xamarin.Forms.Device.OS != TargetPlatform.iOS)
            {
                htmlSource.BaseUrl = DependencyService.Get<IBaseUrl>().Get();
            }
            Label lblTitle = new Label()
            {
                Text = "Alert Image",
                TextColor = Color.White,
                FontAttributes = FontAttributes.Bold,
                FontSize = 18,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,

            };
            Label lblPrevTitle = new Label()
            {
                Text = PrevTitle,
                TextColor = Color.White,
                //FontAttributes = FontAttributes.Bold,
                FontSize = 16,
                VerticalOptions = LayoutOptions.CenterAndExpand,

            };
            Image imgLabel = new Image()
            {
                Source = "uwatchlogo.png",
                HorizontalOptions = LayoutOptions.EndAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand

            };
            Image imgBack = new Image()
            {
                Source = "left_arrow.png",
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.CenterAndExpand

            };
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) => {
                Navigation.PopAsync();
            };
            imgBack.GestureRecognizers.Add(tapGestureRecognizer);
            lblPrevTitle.GestureRecognizers.Add(tapGestureRecognizer);

            StackLayout headerlayout = new StackLayout
            {
                Spacing = 3,
                Padding = new Thickness(0, 16, 5, 0),
                BackgroundColor = Color.Red,
                Orientation = StackOrientation.Horizontal
            }; ;

            headerlayout.Children.Add(imgBack);
            headerlayout.Children.Add(lblPrevTitle);
            headerlayout.Children.Add(lblTitle);
            headerlayout.Children.Add(imgLabel);
            browser.Source = htmlSource;


            Grid grid = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                RowDefinitions = {
                                new RowDefinition { Height = 60 },
                                new RowDefinition { Height = new GridLength (40, GridUnitType.Absolute) },
                                new RowDefinition { Height = new GridLength (1, GridUnitType.Star) }
                               },
                ColumnDefinitions = {
                                new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },
                                        }
            };

            if (Device.RuntimePlatform == Device.iOS)
            {
                grid.RowSpacing = 0;
                grid.Children.Add(headerlayout, 0, 3, 0, 1);
                grid.Children.Add(browser, 0, 3, 1, 3);
            }
            else
            {
                grid.Children.Add(browser, 0, 3, 0, 3);
                Title = "Alert Image";
            }


            Content = grid;
        }
    }
}