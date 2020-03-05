using System;
using Xamarin.Forms;
using System.IO;
using System.Reflection;
using Acr.UserDialogs;
using UwatchPCL;
using System.Collections.Generic;
using PanCardView;
using UwatchPCL.WebServices;
using FFImageLoading.Forms;

namespace uWatch
{
	public class ZoomPage : ContentPage
	{
        

		string _orderImage;
		string logoutText;
        WebServiceManager webServiceManager;
        public ZoomPage(ImageSource source, string TitleName,string receiptImage)
        {
            Title =TitleName;
            webServiceManager = new WebServiceManager();
           // var a = (UriImageSource)source;
           /// var ImageSource = a.Uri.ToString() + "?v="+ DateTime.Now.TimeOfDay;
            var browser = new CachedImage();
        
  
            browser.BackgroundColor = Xamarin.Forms.Color.Black;
            //convert image from binary to string
         //   _orderImage = ImageSource;
            string htmlsource = "<style>img{ height: 100%; padding:0px; margin: 0px; max-width: 100%; width:100%;}\n</style><html>\n<head>\n</head>\n<body style =\"background-color:black\">\n\n<img src = " + _orderImage + " />\n</body>\n</html>";

            var btnSwipe = new Button()
            {
                Text = "Receipt exists, click to display",
                VerticalOptions = LayoutOptions.CenterAndExpand,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                BackgroundColor = Color.Transparent,
                TextColor = Color.White,
            };
            btnSwipe.Clicked +=  (object sender, EventArgs e) => 
            {
                Navigation.PushAsync(new ZoomPage(receiptImage, "Receipt Image","" ));
            };

			//var htmlSource = new HtmlWebViewSource();

			//htmlSource.Html = @htmlsource;

			//if (Device.OS != TargetPlatform.iOS)
			//{
			//	htmlSource.BaseUrl = DependencyService.Get<IBaseUrl>().Get();
			//}

			browser.Source = source;
            Grid grid = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                RowSpacing = 0,
                ColumnSpacing = 0
            };
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1,GridUnitType.Auto) });
            grid.Children.Add(browser, 0, 0);
            if (Title == "Asset Image" && !(string.IsNullOrEmpty(receiptImage)))
            {
                grid.Children.Add(btnSwipe, 0, 1);
            }
            Content = grid;
			Content.BackgroundColor = Xamarin.Forms.Color.Black;
		}

	}

    public class ImagePreview
    {
        public string AssetImage
        {
            get;
            set;
        }
    }
}