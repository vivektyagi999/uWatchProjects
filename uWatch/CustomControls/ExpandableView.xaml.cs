using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace uWatch
{
	public partial class ExpandableView : ContentView
	{
		private TapGestureRecognizer _tapRecogniser;
		private StackLayout _summary;
		private StackLayout _details;
		private StackLayout _viewmore;
		Label moredetail;

		public ExpandableView()
		{
			InitializeComponent();
			_viewmore = new StackLayout();
			DetailsRegion.IsVisible = false;
			 moredetail = new Label { Text="more details...",TextColor=Color.Blue,FontSize=10,VerticalOptions=LayoutOptions.End};
			_viewmore.Children.Add(moredetail);
			InitializeGuestureRecognizer();
			SubscribeToGuestureHandler();
		}

		private void InitializeGuestureRecognizer()
		{
			_tapRecogniser = new TapGestureRecognizer();
			//SummaryRegion.GestureRecognizers.Add(_tapRecogniser);
			_viewmore.GestureRecognizers.Add(_tapRecogniser);
		}

		private void SubscribeToGuestureHandler()
		{
			_tapRecogniser.Tapped += TapRecogniser_Tapped;
		}

		public virtual StackLayout Summary
		{
			get { return _summary; }
			set
			{
				_summary = value;
				SummaryRegion.Children.Add(_summary);
				SummaryRegion.Children.Add(_viewmore);
				OnPropertyChanged();
			}
		}

		public virtual StackLayout Details
		{
			get { return _details; }
			set
			{
				_details = value;
				DetailsRegion.Children.Add(_details);
				OnPropertyChanged();
			}
		}

		private void TapRecogniser_Tapped(object sender, EventArgs e)
		{
			if (DetailsRegion.IsVisible)
			{
				
				moredetail.Text = "more details...";
				DetailsRegion.IsVisible = false;

			}
			else
			{
				moredetail.Text = "see less...";
				DetailsRegion.IsVisible = true;

			}
		}
	}
}
