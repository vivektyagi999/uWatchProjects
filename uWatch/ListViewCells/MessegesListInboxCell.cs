	using System;

	using Xamarin.Forms;
	using UwatchPCL;
	using FFImageLoading.Forms;

namespace uWatch
{
	public class MessegesListInboxCell : ViewCell
	{
		Label MessageLabel, Notification_idxLabel,Date;
		StackLayout frmLayout;
		protected override void OnBindingContextChanged()
		{

			base.OnBindingContextChanged();
			var account = BindingContext as InMailModel;
			if (account != null)
			{
				if (account.Date != null)
				{
					Date.Text = "Receive Date:"+DateFormat.GetDateTime(account.Date,TimeType.DateAndTime);
				}

			}
		}
		public MessegesListInboxCell()
		{
			try
			{
				MessageLabel = new Label()
				{
					FontSize = 15,
					TextColor = Color.Red,
					VerticalOptions = LayoutOptions.StartAndExpand,
					HorizontalOptions = LayoutOptions.FillAndExpand,
				};

				MessageLabel.SetBinding(Label.TextProperty, new Binding("Title", BindingMode.Default, stringFormat: "#{0}"));
				MessageLabel.SetBinding(Label.TextColorProperty, new Binding("TextColorOfSelectedMsg", BindingMode.TwoWay));
				var dtUnread = new DataTrigger(typeof(Label));
				dtUnread.Binding = new Binding("IsRead", BindingMode.TwoWay);
				dtUnread.Value = false;
				dtUnread.Setters.Add(new Setter { Property = Label.FontAttributesProperty, Value = FontAttributes.Bold });
				MessageLabel.Triggers.Add(dtUnread);
				var dtread = new DataTrigger(typeof(Label));
				dtread.Binding = new Binding("IsRead", BindingMode.TwoWay);
				dtread.Value = true;
				dtread.Setters.Add(new Setter { Property = Label.FontAttributesProperty, Value = FontAttributes.None });
				MessageLabel.Triggers.Add(dtread);


				Date = new Label()
				{
					FontSize = 13,
					VerticalOptions = LayoutOptions.StartAndExpand,
					HorizontalOptions = LayoutOptions.FillAndExpand,
				};



				var friendlyLabel = new Label()
				{
					FontAttributes = FontAttributes.Bold,
					FontSize = 12,
					TextColor = Color.FromHex("#666"),
					HorizontalTextAlignment = TextAlignment.Start,
					HorizontalOptions = LayoutOptions.StartAndExpand,
					VerticalOptions = LayoutOptions.Center
				};
				friendlyLabel.SetBinding(Label.TextProperty, new Binding("Body", BindingMode.TwoWay, stringFormat: "Friendly Name: {0}"));


				var CreatedDateLabel = new Label()
				{
					FontAttributes = FontAttributes.Bold,
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
					FontAttributes = FontAttributes.Bold,
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
                //frmMain.SetBinding(Frame.BackgroundColorProperty,"IsRead")
                frmMain.SetBinding(Frame.BackgroundColorProperty, (InMailModel p) => p.IsRead, converter: new BooleanToColorConverter(Color.FromHex("#ddd"), Color.White));

				frmMain.Content = MainStackLayout;


				 frmLayout = new StackLayout { Padding = 6 };

				frmLayout.Children.Add(frmMain);

				
				View = frmLayout;


			}
			catch (System.Exception ex)
			{
			}
		}

	}
}







