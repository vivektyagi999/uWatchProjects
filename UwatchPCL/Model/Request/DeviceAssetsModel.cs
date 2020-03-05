using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace UwatchPCL.Model
{
	public class DeviceAssetsModel : BaseModel
	{
		public int Deviceasset_idx { get; set; }

		public string geo_coords { get; set; }

		public string friendly_name { get; set; }

		public string description { get; set; }

		public byte[] Deviceimage { get; set; }

		public Nullable<int> ImageOrder { get; set; }

		public Nullable<int> Device_id { get; set; }

		public DateTime? PurchaseDate { get; set; }

		public string strPurchaseDate { get; set; }

		public DateTime? ExpireDate { get; set; }

		public string strExpireDate { get; set; }

        public byte[] Receiptimage { get; set; }

        public string SerialNumber
        {
            get;
            set;
        }

        public string BarCodeFormat { get; set; }

        public Nullable<bool> IsActive { get; set; }

		public bool IsEditable { get; set; }

		public int RotationAngle { get; set; }

		public int? OwnerUserID { get; set;}

		public int Position_no { get; set; }

		public byte[] Deviceimage_thumbnail { get; set; }

		public string UploadedFilePath { get; set; }

		public string Keyword { get; set; }

		public Boolean? IsNew { get; set; }

        public int? TotalAssets { get; set; }

		private bool isBusyLoading;
		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged(
			[CallerMemberName] string propertyName = null)
		{
			var handler = PropertyChanged;
			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		public bool IsBusyLoading
		{
			get { return isBusyLoading; }

			set
			{
				isBusyLoading = value;
				if (PropertyChanged != null)
				{
					OnPropertyChanged("IsBusyLoading");
				}
			}
		}

		public string Longitude { get; set; }
		public string Latitude { get; set; }

		public int? ViewRotation { get; set; } 

	}
}

