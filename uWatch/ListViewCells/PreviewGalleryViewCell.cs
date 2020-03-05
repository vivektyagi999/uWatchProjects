using System;
using UwatchPCL;
using Xamarin.Forms;

namespace uWatch
{
    public class PreviewGalleryViewCell:ContentView
    {

        //protected override void OnBindingContextChanged()
        //{
        //    var item = BindingContext as string;
        //    if (item != null)
        //    {

        //        string htmlsource = "<style>img{ height: 100%; padding:0px; margin: 0px; max-width: 100%; width:100%;}\n</style><html>\n<head>\n</head>\n<body style =\"background-color:black\">\n\n<img src = " + item + " />\n</body>\n</html>";
        //        var browser = new CustomWebView();
        //        var htmlSource = new HtmlWebViewSource();
        //        if (Device.OS != TargetPlatform.iOS)
        //        {
        //            htmlSource.BaseUrl = DependencyService.Get<IBaseUrl>().Get();
        //        }
        //        htmlSource.Html = @htmlsource;
        //        browser.Source = htmlSource;
        //        Content = browser;
        //    }
        //}

        public PreviewGalleryViewCell()
        {

            //var image = new Image();
            //image.SetBinding(Image.SourceProperty, "AssetImage");
            //Content = image;

              //  string htmlsource = "<style>img{ height: 100%; padding:0px; margin: 0px; max-width: 100%; width:100%;}\n</style><html>\n<head>\n</head>\n<body style =\"background-color:black\">\n\n<img src = " + item + " />\n</body>\n</html>";
                var browser = new CustomWebView();
                var htmlSource = new HtmlWebViewSource();
                if (Device.OS != TargetPlatform.iOS)
                {
                    htmlSource.BaseUrl = DependencyService.Get<IBaseUrl>().Get();
                }
                htmlSource.SetBinding(HtmlWebViewSource.HtmlProperty, "AssetImage");
               // htmlSource.Html = @htmlsource;
                browser.Source = htmlSource;
                Content = browser;
          
        }
    }
}
