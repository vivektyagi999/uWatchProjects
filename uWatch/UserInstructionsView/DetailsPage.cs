using System;
using Xamarin.Forms;
using System.IO;
using System.Reflection;
using Acr.UserDialogs;
using UwatchPCL;

namespace uWatch
{
	public interface IBaseUrl
	{
		string Get();

	}

	public class BaseUrlWebView : WebView
	{

	}
	public class DetailsPage: ContentPage
	{
		string _image ,_title= "";
		public DetailsPage(string image,string title)
		{
			
			
		    NavigationPage.SetHasNavigationBar(this, true);
			NavigationPage.SetBackButtonTitle(this, "");
			NavigationPage.SetHasBackButton(this, true);
			_image = image;
			_title = title;
			var wait = new ActivityIndicator { IsRunning = true, Color = Color.Red };
			Content = wait;
		}
		protected override void OnAppearing()
		{
			base.OnAppearing();
			InitializeComponent();
		}
		void InitializeComponent( )
		{
			try
			{
				Title = _title;

				var browser = new CustomWebView();

				browser.BackgroundColor = Xamarin.Forms.Color.Black;
				//convert image from binary to string

				//create a string having the dynamic image source
				//String.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(ViewModel.AlertImage.Image))
				//string htmlsource = "<style>img{display: inline; height: auto; margin: 0 auto; max-width: 100%; width:80%;}\n</style><html>\n<head>\n</head>\n<body style =\"background-color:black\">\n< div> </div>\n<img src = " + _image + " />\n</body>\n</html>";
			//	string htmlsource = < img src = " + _image + " />;
				string htmlsource = "<style>img{display: inline; height: auto; margin: 0 auto; max-width: 100%;}img-container {position: relative;top: 2%; width:100%; height:300px; overflow:hidden; text-align:center;}img{ max-width:100%;width:100%;vertical-align: middle;}\n</style><html>\n<head>\n</head>\n<body style =\"background-color:black\">\n< div class=\"img-container\"> </div>\n<img src = " + _image + " />\n</body>\n</html>";
				//string htmlsource = "<html>\n<head>\n<style>\nbody, html {\n    height: 100%;\n    margin: 0;\n}\n\n.bg {\n \n    background-image: url("+_image+");\n\n\n    height: 100%; \n\n  \n    background-position: center;\n    background-repeat: no-repeat;\n    background-size: cover;\n}\n</style>\n</head>\n<body>\n\n<div class=\"bg\"></div>\n\n\n</body>\n</html>";
				
				var htmlSource = new HtmlWebViewSource();

				htmlSource.Html = @htmlsource;

				if (Device.OS != TargetPlatform.iOS)
				{
					htmlSource.BaseUrl = DependencyService.Get<IBaseUrl>().Get();
				}

				browser.Source = htmlSource;



				//var lbl = new Label { Text = "Functionality in Progress", VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.CenterAndExpand, FontSize = 18 };
				Content = browser;
				Content.BackgroundColor = Xamarin.Forms.Color.Black;
			}
			catch (System.Exception ex)
			{
			}
		}
	}
}
