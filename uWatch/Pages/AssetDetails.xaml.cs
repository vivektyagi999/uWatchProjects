using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Acr.UserDialogs;
#if __ANDROID__
using Android.App;
using Android.Widget;
using uWatch.Droid;
using Android.Media;
using Android.Graphics;
#endif
using FFImageLoading.Forms;
using uWatch.Controls;
using UwatchPCL;
using UwatchPCL.Model;
using Xamarin.Forms;
using System.IO;
#if __IOS__
using UIKit;
using CoreGraphics;
using System.Drawing;
using Foundation;
using System.Text;
#endif

namespace uWatch
{
	public partial class AssetDetails : ContentPage
	{

		public DeviceAssetsModel DeviceAsset;
		public DeviceAssetsModel TempDeviceAsset;

		public Xamarin.Forms.Image Asset { get; set; }

		Xamarin.Forms.RelativeLayout relativeLayout;
		Xamarin.Forms.ScrollView scrollview;
		int currentimg = 0;
		int Position = 0;

		Xamarin.Forms.Image imgAsset;

		double w = MyController.VirtualWidth;
		double h = MyController.VirtualHeight;

		public Xamarin.Forms.DatePicker EntPurDate, EntExpDate;
		public ExtendedEditor EntDescription;

		public Entry txtKeyword;

		public AssetDetailsViewModel ViewModel { get; set; }

		public AssetDetails(int position, DeviceAssetsModel selectedImageModel, AssetDetailsViewModel viewmodel)
		{
			try
			{
				DeviceAsset = selectedImageModel;
				TempDeviceAsset = selectedImageModel;

				this.ViewModel = viewmodel;
				this.Position = position;
				InitializeComponent();

				BindingContext = ViewModel.DeviceAsset;
				SetLayout();

				NavigationPage.SetHasNavigationBar(this, true);
			}
			catch (Exception ex)
			{
				MyController.ErrorManagement(ex.Message);
			}

		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
		}

		private async void SetLayout()
		{
			try
			{
				Title = "Asset Details";
				relativeLayout = new Xamarin.Forms.RelativeLayout();
				relativeLayout.Padding = new Thickness(0, 0, 0, 20);
				AddLayout();
				scrollview = new Xamarin.Forms.ScrollView();
				scrollview.Content = relativeLayout;
				Content = scrollview;
			}
			catch { }
		}


		private async void AddLayout()
		{
			try
			{
				double position = 0;
				double newx20 = MyUiUtils.getPercentual(w, 20);
				double newx40 = MyUiUtils.getPercentual(w, 40);
				double newx60 = MyUiUtils.getPercentual(w, 60);
				double newx80 = MyUiUtils.getPercentual(w, 80);


				var imgAsset = new CachedImage();
				imgAsset.Aspect = Aspect.AspectFill;
				imgAsset.TransparencyEnabled = false;
				imgAsset.ErrorPlaceholder = "comingSoon.png";
				imgAsset.HeightRequest = 500;
				imgAsset.WidthRequest = w;
				imgAsset.HorizontalOptions = LayoutOptions.Center;

				System.Uri uri;
				System.Uri.TryCreate(ViewModel.DeviceAsset.UploadedFilePath, UriKind.Absolute, out uri);
				Task<ImageSource> result = Task<ImageSource>.Factory.StartNew(() => ImageSource.FromUri(uri));
				imgAsset.Source = await result;
			
				var indicator = new ActivityIndicator { Color = Xamarin.Forms.Color.Red };

				indicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsLoading");
				
				indicator.BindingContext = imgAsset;

				TapGestureRecognizer bigt = new TapGestureRecognizer();
				bigt.Tapped += async (object sender, EventArgs e) =>
				{
					if (!indicator.IsRunning)
					{
						try
						{
							var networkConnection = DependencyService.Get<INetworkConnection>();
							networkConnection.CheckNetworkConnection();
							var networkStatus = networkConnection.IsConnected ? "Connected" : "Not Connected";
							if (networkStatus != "Connected")
							{
								UserDialogs.Instance.Alert("Please check your internet connection.", "Alert", "OK");
								return;
							}

							UserDialogs.Instance.ShowLoading("Loading...");


							var browser = new WebView
							{
								Source = "http://xamarin.com"
							};



							ContentPage p = new ContentPage();
							StackLayout st = new StackLayout { Padding = 10, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
							Xamarin.Forms.RelativeLayout r = new Xamarin.Forms.RelativeLayout();


							var i = new CachedImage();
							i.Aspect = Aspect.AspectFill;

							i.CacheDuration = TimeSpan.FromDays(50);
							i.DownsampleHeight = 300;
							i.RetryCount = 20;
							i.RetryDelay = 5;
							i.TransparencyEnabled = false;
						
							i.HorizontalOptions = LayoutOptions.FillAndExpand;
							i.VerticalOptions = LayoutOptions.FillAndExpand;

							
							i.Source = imgAsset.Source;


							i.Aspect = Aspect.AspectFit;
							st.Children.Add(i);

							p.Title = "Asset Image";


							p.Content = st;
							await Navigation.PushAsync(p);

							await Task.Delay(500);

							UserDialogs.Instance.HideLoading();
					}
					catch
					{
					}
					}
				};
				indicator.GestureRecognizers.Add(bigt);
				imgAsset.GestureRecognizers.Add(bigt);
				if (ViewModel != null && ViewModel.DeviceAsset != null)
					ViewModel.DeviceAsset.IsEditable = false;

				var imgLleft = MyUILibrary.AddImage(relativeLayout, "rotate_left.png", 0, position + 300, 40, 40, Aspect.AspectFit);
				imgLleft.BackgroundColor = Xamarin.Forms.Color.White;
				TapGestureRecognizer Tapleft = new TapGestureRecognizer();
				Tapleft.Tapped += (object sender, EventArgs e) =>
			   {
				  
				   imgAsset.Rotation -= 90;
				   ViewModel.DeviceAsset.RotationAngle -= 90;
			   };
				imgLleft.GestureRecognizers.Add(Tapleft);

				var imgRight = MyUILibrary.AddImage(relativeLayout, "rotate_right.png", w - 40, position + 300, 40, 40, Aspect.AspectFit);
				imgRight.BackgroundColor = Xamarin.Forms.Color.White;
				TapGestureRecognizer TapRight = new TapGestureRecognizer();
				TapRight.Tapped += (object sender, EventArgs e) =>
				   {
					   imgAsset.Rotation += 90;
					   ViewModel.DeviceAsset.RotationAngle += 90;
				   };
				imgRight.GestureRecognizers.Add(TapRight);

				//position += 400 + 10;
				position += 10 + 10;
				MyUILibrary.AddLabel(relativeLayout, "Purchase Date", 20, position, newx80, 40, Xamarin.Forms.Color.Black, 15);
				EntPurDate = MyUILibrary.AddDatePicker(relativeLayout, 20, position + 30, newx80, 40);
				EntPurDate.WidthRequest = 110;
				EntPurDate.HorizontalOptions = LayoutOptions.StartAndExpand;
				EntPurDate.SetBinding(Xamarin.Forms.DatePicker.DateProperty, new Binding("PurchaseDate", BindingMode.TwoWay,stringFormat:"dd-MMM-yy"));
			
				var lblExpDate = MyUILibrary.AddLabel(relativeLayout, "Expiry Date", 50, position, newx80, 40, Xamarin.Forms.Color.Black, 15);
				lblExpDate.HorizontalOptions = LayoutOptions.EndAndExpand;
				EntExpDate = MyUILibrary.AddDatePicker(relativeLayout, 20 + 30, position + 30, newx80, 40);
				EntExpDate.WidthRequest = 110;
				EntExpDate.HorizontalOptions = LayoutOptions.EndAndExpand;
				EntExpDate.SetBinding(Xamarin.Forms.DatePicker.DateProperty, new Binding("ExpireDate", BindingMode.TwoWay, stringFormat: "dd-MMM-yy"));

				position += 90;
				MyUILibrary.AddLabel(relativeLayout, "Keyword", 20, position, newx80, 40, Xamarin.Forms.Color.Black, 15);
				txtKeyword = MyUILibrary.AddEntry(relativeLayout, "", "Keyword", (w - newx80) / 2, position + 20, newx80, 45, Xamarin.Forms.Color.Black, Keyboard.Default, TextAlignment.Start, fontsize: 15);
				

				position += 50 + 40+10;
				MyUILibrary.AddLabel(relativeLayout, "Description", 20, position, newx80, 40, Xamarin.Forms.Color.Black, 15);
				EntDescription = MyUILibrary.AddExtendedEditor(relativeLayout, "", "Description", 20, position + 25, w - 50, 100, Xamarin.Forms.Color.Black, Keyboard.Default, TextAlignment.Start, false, 15);
				EntDescription.SetBinding(ExtendedEditor.TextProperty, new Binding("description", BindingMode.TwoWay));

				position += 100+10;
				imgAsset = MyUILibrary.AddCachedImage(relativeLayout, imgAsset, 50, position + 50, w - 100, 200, Aspect.AspectFit);
				indicator = MyUILibrary.AddActivityIndicator(relativeLayout, indicator, 150, position + 110, 100, 100, Aspect.AspectFill);

				position += 200 + 20+10;
				var btnEdit = MyUILibrary.AddButton(relativeLayout, "Save", 20, position + 50 + 15, 150, 50, Xamarin.Forms.Color.Red, Xamarin.Forms.Color.Gray, Xamarin.Forms.Color.White, 15);
				btnEdit.Clicked += async (object sender, EventArgs e) =>
				{
					var networkConnection = DependencyService.Get<INetworkConnection>();
					networkConnection.CheckNetworkConnection();
					var networkStatus = networkConnection.IsConnected ? "Connected" : "Not Connected";
					if (networkStatus != "Connected")
					{
						UserDialogs.Instance.Alert("Please check your internet connection.", "Alert", "OK");
						return;
					}

					string address = "Are you sure you want to update this Asset?";
					var answer = await DisplayAlert("Update Asset", address, "Yes", "No");
					if (answer == true)
					{
						
						EntPurDate.IsEnabled = true;
						EntExpDate.IsEnabled = true;
						EntDescription.IsEnabled = true;
					
						UserDialogs.Instance.ShowLoading("Updating Asset...");

						ViewModel.DeviceAsset.RotationAngle = ViewModel.DeviceAsset.RotationAngle / 90;
						ViewModel.DeviceAsset.Device_id = ViewModel.DeviceAsset.Device_id;
						ViewModel.DeviceAsset.Deviceimage = ViewModel.DeviceAsset.Deviceimage;
						ViewModel.DeviceAsset.Deviceasset_idx = ViewModel.DeviceAsset.Deviceasset_idx;
						ViewModel.DeviceAsset.geo_coords = ViewModel.DeviceAsset.geo_coords;
						ViewModel.DeviceAsset.friendly_name = ViewModel.DeviceAsset.friendly_name;
						ViewModel.DeviceAsset.description = EntDescription.Text;
						ViewModel.DeviceAsset.strExpireDate = EntExpDate.Date.ToString("dd-MMM-yyyy");
						ViewModel.DeviceAsset.strCreatedDate = System.DateTime.Now.ToString("dd-MMM-yyyy");
						ViewModel.DeviceAsset.strModifyDate = System.DateTime.Now.ToString("dd-MMM-yyyy");
						ViewModel.DeviceAsset.strPurchaseDate = EntPurDate.Date.ToString("dd-MMM-yyyy");

						var saveimg = await ApiService.Instance.SaveAsset(ViewModel.DeviceAsset);

						UserDialogs.Instance.HideLoading();
						await UserDialogs.Instance.AlertAsync("Asset Updated Successfully", "Information", "OK");
						System.GC.Collect();
						MyController.fromAssetsToGallery = true;
						await Navigation.PopAsync(true);
					}
				};
				var btnDelete = MyUILibrary.AddButton(relativeLayout, "Delete", 20 + 150 + 40, position + 50 + 15, 150, 50, Xamarin.Forms.Color.Red, Xamarin.Forms.Color.Gray, Xamarin.Forms.Color.White, 15);
				btnDelete.Clicked += async (object sender, EventArgs e) =>
				{

					var networkConnection = DependencyService.Get<INetworkConnection>();
					networkConnection.CheckNetworkConnection();
					var networkStatus = networkConnection.IsConnected ? "Connected" : "Not Connected";
					if (networkStatus != "Connected")
					{
						UserDialogs.Instance.Alert("Please check your internet connection.", "Alert", "OK");
						return;
					}

					string address = "Are you sure you want to delete this Asset?";
					var answer = await DisplayAlert("Delete Asset", address, "Yes", "No");
					if (answer == true)
					{
						UserDialogs.Instance.ShowLoading("Deleting Asset...");
					
						var resu = await ApiService.Instance.DeleteAsset(ViewModel.DeviceAsset);
						UserDialogs.Instance.HideLoading();

						await UserDialogs.Instance.AlertAsync("Asset Deleted Successfully", "Information", "OK");
						MyController.fromAssetsToGallery = true;
						await Navigation.PopAsync(true);
				}
			};
			}
			catch{}
		}

#if __IOS__


		public static UIImage ScaleImage(UIImage image, int maxSize)
		{
		     
			UIImage res;


			using (CGImage imageRef = image.CGImage)
			{
				CGImageAlphaInfo alphaInfo = imageRef.AlphaInfo;
				CGColorSpace colorSpaceInfo = CGColorSpace.CreateDeviceRGB();
				if (alphaInfo == CGImageAlphaInfo.None)
				{
					alphaInfo = CGImageAlphaInfo.NoneSkipLast;
				}

				int width, height;

				width = (int)imageRef.Width;
				height = (int)imageRef.Height;


				if (height >= width)
				{
					width = (int)Math.Floor((double)width * ((double)maxSize / (double)height));
					height = maxSize;
				}
				else
				{
					height = (int)Math.Floor((double)height * ((double)maxSize / (double)width));
					width = maxSize;
				}


				CGBitmapContext bitmap = null;

				try
				{

				
					if (image.Orientation == UIImageOrientation.Up || image.Orientation == UIImageOrientation.Down)
					{
						bitmap = new CGBitmapContext(IntPtr.Zero, width, height, imageRef.BitsPerComponent, imageRef.BytesPerRow, colorSpaceInfo, alphaInfo);
					}
					else
					{
						bitmap = new CGBitmapContext(IntPtr.Zero, height, width, imageRef.BitsPerComponent, imageRef.BytesPerRow, colorSpaceInfo, alphaInfo);
					}

				}
				catch (Exception ex)
				{

				}

					switch (image.Orientation)
					{
						case UIImageOrientation.Left:
							bitmap.RotateCTM((float)Math.PI / 2);
							bitmap.TranslateCTM(0, -height);
							break;
						case UIImageOrientation.Right:
							bitmap.RotateCTM(-((float)Math.PI / 2));
							bitmap.TranslateCTM(-width, 0);
							break;
						case UIImageOrientation.Up:
							break;
						case UIImageOrientation.Down:
							bitmap.TranslateCTM(width, height);
							bitmap.RotateCTM(-(float)Math.PI);
							break;
					}

					bitmap.DrawImage(new RectangleF(0, 0, width, height), imageRef);

				

				res = UIImage.FromImage(bitmap.ToImage());
				bitmap = null;

			}


			return res;
		}


		UIImage ScaleAndRotateImage(UIImage imageIn, UIImageOrientation orIn)
		{
			int kMaxResolution = 1024;

			CGImage imgRef = imageIn.CGImage;
			float width = imgRef.Width;
			float height = imgRef.Height;
			CGAffineTransform transform = CGAffineTransform.MakeIdentity();
			RectangleF bounds = new RectangleF(0, 0, width, height);

			if (width > kMaxResolution || height > kMaxResolution)
			{
				float ratio = width / height;

				if (ratio > 1)
				{
					bounds.Width = kMaxResolution;
					bounds.Height = bounds.Width / ratio;
				}
				else
				{
					bounds.Height = kMaxResolution;
					bounds.Width = bounds.Height * ratio;
				}
			}

			float scaleRatio = bounds.Width / width;
			SizeF imageSize = new SizeF(width, height);
			UIImageOrientation orient = orIn;
			float boundHeight;

			switch (orient)
			{
				case UIImageOrientation.Up:                                        //EXIF = 1
					transform = CGAffineTransform.MakeIdentity();
					break;

				case UIImageOrientation.UpMirrored:                                //EXIF = 2
					transform = CGAffineTransform.MakeTranslation(imageSize.Width, 0f);
					transform = CGAffineTransform.MakeScale(-1.0f, 1.0f);
					break;

				case UIImageOrientation.Down:                                      //EXIF = 3
					transform = CGAffineTransform.MakeTranslation(imageSize.Width, imageSize.Height);
					transform = CGAffineTransform.Rotate(transform, (float)Math.PI);
					break;

				case UIImageOrientation.DownMirrored:                              //EXIF = 4
					transform = CGAffineTransform.MakeTranslation(0f, imageSize.Height);
					transform = CGAffineTransform.MakeScale(1.0f, -1.0f);
					break;

				case UIImageOrientation.LeftMirrored:                              //EXIF = 5
					boundHeight = bounds.Height;
					bounds.Height = bounds.Width;
					bounds.Width = boundHeight;
					transform = CGAffineTransform.MakeTranslation(imageSize.Height, imageSize.Width);
					transform = CGAffineTransform.MakeScale(-1.0f, 1.0f);
					transform = CGAffineTransform.Rotate(transform, 3.0f * (float)Math.PI / 2.0f);
					break;

				case UIImageOrientation.Left:                                      //EXIF = 6
					boundHeight = bounds.Height;
					bounds.Height = bounds.Width;
					bounds.Width = boundHeight;
					transform = CGAffineTransform.MakeTranslation(0.0f, imageSize.Width);
					transform = CGAffineTransform.Rotate(transform, 3.0f * (float)Math.PI / 2.0f);
					break;

				case UIImageOrientation.RightMirrored:                             //EXIF = 7
					boundHeight = bounds.Height;
					bounds.Height = bounds.Width;
					bounds.Width = boundHeight;
					transform = CGAffineTransform.MakeScale(-1.0f, 1.0f);
					transform = CGAffineTransform.Rotate(transform, (float)Math.PI / 2.0f);
					break;

				case UIImageOrientation.Right:                                     //EXIF = 8
					boundHeight = bounds.Height;
					bounds.Height = bounds.Width;
					bounds.Width = boundHeight;
					transform = CGAffineTransform.MakeTranslation(imageSize.Height, 0.0f);
					transform = CGAffineTransform.Rotate(transform, (float)Math.PI / 2.0f);
					break;

				default:
					throw new Exception("Invalid image orientation");
					break;
			}

			UIGraphics.BeginImageContext(bounds.Size);


			CGContext context = UIGraphics.GetCurrentContext();

			if (orient == UIImageOrientation.Right || orient == UIImageOrientation.Left)
			{
				context.ScaleCTM(-scaleRatio, scaleRatio);
				context.TranslateCTM(-height, 0);
			}
			else
			{
				context.ScaleCTM(scaleRatio, -scaleRatio);
				context.TranslateCTM(0, -height);
			}

			context.ConcatCTM(transform);
			context.DrawImage(new RectangleF(0, 0, width, height), imgRef);

			UIImage imageCopy = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();

			return imageCopy;
		}
#endif

		protected override bool OnBackButtonPressed()
		{
			try
			{
				System.GC.Collect();
				MyController.fromAssetsToGallery = true;

			}
			catch { }
			return base.OnBackButtonPressed();
		}



	}


}

