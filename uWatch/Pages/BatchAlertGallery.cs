using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Acr.UserDialogs;
using FFImageLoading.Forms;
using UwatchPCL;
using Xamarin.Forms;

namespace uWatch
{
	public class BatchAlertGallery:ContentPage
	{
		Grid galleryGrid;ScrollView scrlview;
		StackLayout mainstklayout;StackLayout galleryListStack;
		List<BatchImages> _lstBatchImages;
		static bool isRun ;
		public BatchAlertGallery(List<BatchImages> _BatchImages)
		{
			Title = "Batch Gallery";
			isRun = false;
			_lstBatchImages = new List<BatchImages>();
			_lstBatchImages = _BatchImages;
			scrlview = new ScrollView { };
			mainstklayout = new StackLayout { Spacing = 0, VerticalOptions = LayoutOptions.StartAndExpand };
			galleryGrid = new Grid();
			galleryListStack = new StackLayout { Padding = new Thickness(10, 5, 0, 0) };

			var waitindicator = new ActivityIndicator { Color = Color.Red, IsRunning=true,HeightRequest = 30, WidthRequest = 30,HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };
			Content = waitindicator;
		}
		protected override void OnAppearing()
		{
			base.OnAppearing();
			if (!isRun)
			{
				if (_lstBatchImages.Count > 0)
				{
					InitializeComponet();
				}
				else
				{
					var waitindicator = new Label { Text = "No Images Found!", FontSize = 18, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };
					Content = waitindicator;
			    }
			}
		}
		void InitializeComponet()
		{
			try
			{
				
				galleryGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = App.ScreenWidth / 2 - 10 });
				galleryGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = App.ScreenWidth / 2 - 10 });
				galleryGrid.RowDefinitions.Add(new RowDefinition { Height = App.ScreenWidth / 2 });

				CreateGalleryGrid();
				scrlview.Content = galleryListStack;
				mainstklayout.Children.Add(scrlview);

				Content = mainstklayout;
				isRun = true;
			}
			catch(System.Exception ex)
			{
			}
		}
		async void CreateGalleryGrid()
		{
			try
			{


				for (int i = 0; i < _lstBatchImages.Count; i++)
				{
					var mainFrame = new Frame
					{
						Padding = new Thickness(5),
						HasShadow = false,
						OutlineColor = Color.Silver,
						BackgroundColor = Color.FromHex("#F1F1F1"),
						HorizontalOptions = LayoutOptions.FillAndExpand,
						VerticalOptions = LayoutOptions.FillAndExpand,

					};
					var frameContent = new StackLayout
					{
						Spacing = 0,
						Padding = new Thickness(0),
						HorizontalOptions = LayoutOptions.FillAndExpand,
						VerticalOptions = LayoutOptions.FillAndExpand,
						Orientation = StackOrientation.Vertical,
						HeightRequest = 180
						

					};
					var galleryImage = new CachedImage
					{

						HorizontalOptions = LayoutOptions.FillAndExpand,
						VerticalOptions = LayoutOptions.FillAndExpand,
						HeightRequest = App.ScreenWidth / 2 - 60,
						WidthRequest = App.ScreenWidth / 2 - 30,
						Aspect = Aspect.AspectFill,
						LoadingPlaceholder = "comingSoonImage.png",
						CacheDuration = TimeSpan.FromDays(50),
						DownsampleHeight = 300,
						RetryCount = 30,
						RetryDelay = 5
					};
					var indicator = new ActivityIndicator { Color = Xamarin.Forms.Color.Red, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand,HeightRequest=30,WidthRequest=30 };
					System.Uri uri;
					System.Uri.TryCreate(_lstBatchImages[i].ImageUrl, UriKind.Absolute, out uri);
					Task<ImageSource> result = Task<ImageSource>.Factory.StartNew(() => ImageSource.FromUri(uri));
					galleryImage.Source = await result;

					indicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsLoading");

					var relativelayout = new RelativeLayout { };
					relativelayout.Children.Add(galleryImage,
					 Constraint.RelativeToParent((parent) =>
						{
							return 0;
						}),
						Constraint.RelativeToParent((parent) =>
						{
							return 0;
						}),
						Constraint.RelativeToParent((parent) =>
						{
							return App.ScreenWidth / 2 - 20;
						}),
						Constraint.RelativeToParent((parent) =>
						{
							return App.ScreenWidth / 2 - 25;
						}));

					relativelayout.Children.Add(indicator,
						Constraint.RelativeToParent((parent) =>
						{
							return 70;
						}),
						Constraint.RelativeToParent((parent) =>
						{
							return 60;
						}));


					indicator.BindingContext = galleryImage;
					var lblLostdateText = new Label
					{
							FontSize = 14
					};
					var lblLostdateValue = new Label
					{
					};
					var lblLostId = new Label
					{
					};
					frameContent.Children.Add(relativelayout);
					var tapGestureRecognizer = new TapGestureRecognizer();

					tapGestureRecognizer.Tapped += async (sender, e) =>
									 {
										 UserDialogs.Instance.ShowLoading("Please wait...", MaskType.Gradient);

										 try
										 {

											
											 ContentPage _batchimagepage = new ContentPage();
											 var browser = new CustomWebView();

											 browser.BackgroundColor = Xamarin.Forms.Color.Black;
											 //convert image from binary to string

											 //create a string having the dynamic image source
											
							                 string htmlsource = "<style>img{display: inline; height: auto; margin: 0 auto; max-width: 100%;}img-container {position: relative;top: 20%; width:100%; height:300px; overflow:hidden; text-align:center;}img{ max-width:100%;width:100%;vertical-align: middle;}\n</style><html>\n<head>\n</head>\n<body style =\"background-color:black\">\n< div class=\"img-container\"> </div>\n<img src = " + galleryImage.Source.GetValue(UriImageSource.UriProperty) + " />\n</body>\n</html>";

											 var htmlSource = new HtmlWebViewSource();

											 htmlSource.Html = @htmlsource;

											 if (Xamarin.Forms.Device.OS != TargetPlatform.iOS)
											 {
												 htmlSource.BaseUrl = DependencyService.Get<IBaseUrl>().Get();
											 }

											 browser.Source = htmlSource;

											 Grid grid = new Grid
											 {
												 VerticalOptions = LayoutOptions.FillAndExpand,
												 RowDefinitions = {
											new RowDefinition { Height = new GridLength (40, GridUnitType.Absolute) },
											new RowDefinition { Height = new GridLength (1, GridUnitType.Star) }
										   },
												 ColumnDefinitions = {
											new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },
												}
											 };


											 grid.Children.Add(browser, 0, 3, 0, 3);
											

											 _batchimagepage.Title = "Batch Image";
											 _batchimagepage.Content = grid;

											 await Navigation.PushAsync(_batchimagepage);

										 }
										 catch
										 {
										 }
										 await System.Threading.Tasks.Task.Delay(50);
										 UserDialogs.Instance.HideLoading();
									 };
					frameContent.GestureRecognizers.Add(tapGestureRecognizer);
					mainFrame.Content = frameContent;
					int row = i / 2;
					if (i % 2 == 0)
					{

						galleryGrid.Children.Add(mainFrame, 0, row);
					}
					else
					{
						galleryGrid.Children.Add(mainFrame, 1, row);
					}

				}
				galleryListStack.Children.Add(galleryGrid);
			}

			catch (Exception ex)
			{

			}
		}
	}
}
