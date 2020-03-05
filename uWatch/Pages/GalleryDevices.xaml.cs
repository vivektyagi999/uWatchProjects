using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace uWatch
{
	public partial class GalleryDevices : ContentPage
	{
		public GalleryDevices()
		{
			InitializeComponent();
			var ViewModelDevices = new DashboardViewModel();
			if (ViewModelDevices.DeviceList.Count > 1)
			{
				this.Navigation.PushAsync(new DevicesPage());
			}
			else
			{
				this.Navigation.PushAsync(new AssetsPage());
			}
		}
	}
}

