using System;
using Xamarin.Forms;
using UwatchPCL;
using FFImageLoading.Forms;

namespace uWatch
{
    public class MessegesListSentBoxCell: MyViewCell
	{
		Label MessageLabel, Notification_idxLabel, Date;
		protected override void OnBindingContextChanged()
		{

			base.OnBindingContextChanged();
			var account = BindingContext as InMailModel;
			if (account != null)
			{
				
				if (account.Date != null)
				{
					Date.Text = "Send Date:" + DateFormat.GetDateTime(account.Date, TimeType.OnlyDate);
				}
			}
		}
		public MessegesListSentBoxCell()
		{
			try
			{
				MessageLabel = new Label()
				{
					FontSize = 15,
					TextColor = Color.Black,
					VerticalOptions = LayoutOptions.StartAndExpand,
					HorizontalOptions = LayoutOptions.FillAndExpand,
				};
				MessageLabel.SetBinding(Label.TextProperty, new Binding("Title", BindingMode.Default, stringFormat: "#{0}"));



				Date = new Label()
				{
					FontSize = 13,
					VerticalOptions = LayoutOptions.StartAndExpand,
					HorizontalOptions = LayoutOptions.FillAndExpand,
				};


				var friendlyLabel = new Label()
				{
					
					FontSize = 12,
					TextColor = Color.FromHex("#666"),
					HorizontalTextAlignment = TextAlignment.Start,
					HorizontalOptions = LayoutOptions.StartAndExpand,
					VerticalOptions = LayoutOptions.Center
				};
				friendlyLabel.SetBinding(Label.TextProperty, new Binding("Body", BindingMode.TwoWay, stringFormat: "Friendly Name: {0}"));


				var CreatedDateLabel = new Label()
				{
					
					FontSize = 12,
					TextColor = Color.FromHex("#666"),
					HorizontalTextAlignment = TextAlignment.Start,
					HorizontalOptions = LayoutOptions.StartAndExpand,
					VerticalOptions = LayoutOptions.End
				};
				CreatedDateLabel.SetBinding(Label.TextProperty, new Binding("CreatedDate", BindingMode.TwoWay));

				var layoutSerialAndModel = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.StartAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };
				layoutSerialAndModel.Children.Add(MessageLabel);
				layoutSerialAndModel.Children.Add(CreatedDateLabel);
				Notification_idxLabel = new Label()
				{
					IsVisible = false,
					FontSize = 12,
					TextColor = Color.FromHex("#666"),
					HorizontalTextAlignment = TextAlignment.Start,
					HorizontalOptions = LayoutOptions.StartAndExpand,
					VerticalOptions = LayoutOptions.End
				};
				Notification_idxLabel.SetBinding(Label.TextProperty, new Binding("NotificationID", BindingMode.Default));
				var starImage = new CachedImage()
				{
					Source = "right_arrow.png",
					HeightRequest = 50,
					WidthRequest = 50,
					LoadingPlaceholder = ImageSource.FromFile("placeholder.png"),
				};

				var Stack1 = new StackLayout()
				{
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.Fill,
					Padding = new Thickness(2),
					Children =
				{
					layoutSerialAndModel,Date

				}
				};
				var Stack3 = new StackLayout()
				{
					HorizontalOptions = LayoutOptions.CenterAndExpand,
					VerticalOptions = LayoutOptions.CenterAndExpand,
					Padding = new Thickness(0, 0, 10, 0),
					Children =
				{
					starImage,
				}
				};

				var MainStackLayout = new StackLayout()
				{
					Orientation = StackOrientation.Horizontal,
					HorizontalOptions = LayoutOptions.Fill,
					VerticalOptions = LayoutOptions.Fill,
					Children =
				{
					Stack1,
					
				}
				};

				var frmMain = new Frame { BackgroundColor = Color.FromRgb(246, 241, 238), HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, HasShadow = false };
				frmMain.Content = MainStackLayout;

				var frmLayout = new StackLayout { Padding = 6 };
				frmLayout.Children.Add(frmMain);

				View = frmLayout;
			}
			catch(System.Exception ex)
			{
				

			}
			}
		}
	}

