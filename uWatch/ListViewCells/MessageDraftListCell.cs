using System;
using FFImageLoading.Forms;
using UwatchPCL;
using Xamarin.Forms;

namespace uWatch
{
	public class DraftListCell: MyViewCell
	{

		Label MessageLabel, Notification_idxLabel, Date;
		StackLayout frmLayout;
		protected override void OnBindingContextChanged()
		{

			base.OnBindingContextChanged();
			var account = BindingContext as DraftMailModel;
			if (account != null)
			{
				if (account.SendDate != null)
				{
					Date.Text = "Send Date: " + DateFormat.GetDateTime(account.SendDate, TimeType.OnlyDate);
				}

			}
		}
		public DraftListCell()
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






				var Stack = new StackLayout()
				{
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.Fill,
					Padding = new Thickness(2),
					Children =
					{
						MessageLabel,Date

					}
				};


				var MainStackLayout = new StackLayout()
				{
					Orientation = StackOrientation.Horizontal,
					HorizontalOptions = LayoutOptions.Fill,
					VerticalOptions = LayoutOptions.Fill,
					Children =
					{
						Stack
						
					}
				};

				var frmMain = new Frame { BackgroundColor = Color.FromRgb(246, 241, 238), HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, HasShadow = false };
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
