using System;
using uWatch.ViewModels;
using UwatchPCL.Model;
using Xamarin.Forms;

namespace ImageGallery.Models
{
	public class GalleryImage : BaseViewModel
	{
		public GalleryImage()
		{
			ImageId = Guid.NewGuid();
		}

		public Guid ImageId
		{
			get;
			set;
		}

		public ImageSource Source
		{
			get;
			set;
		}

		public byte[] OrgImage
		{
			get;
			set;
		}

	}
}
