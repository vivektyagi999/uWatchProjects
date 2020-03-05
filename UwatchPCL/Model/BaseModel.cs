using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace UwatchPCL.Model
{
	public class BaseModel : Paged, INotifyPropertyChanged
	{
		public event PropertyChangingEventHandler PropertyChanging;

		public Nullable<System.DateTime> CreatedDate { get; set; }

		public string strCreatedDate { get; set; }

		public Nullable<int> CreatedBy { get; set; }

		public Nullable<System.DateTime> ModifyDate { get; set; }

		public string strModifyDate { get; set; }

		public Nullable<int> ModifyBy { get; set; }

		public bool IsActive { get; set; }

        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

		public String BatteryImage { get; set; }
		public String SignalImage { get; set; }
		public String TemperatureImage { get; set; }

		public String AlertTypeImage { get; set; }
		private string escimage;

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

		public String EscalateImage 
		{ 
			get { return escimage; }

			set
			{
				escimage = value;
				if (PropertyChanged != null)
				{
					OnPropertyChanged("EscalateImage");
				}
			} 
		}

		



		public String AlertImageIcon { get; set; }
		public String AlertDateTime { get; set; }
		public String AlertTypeName { get; set; }

        public string rightArrow;
        public string RightArrow
        {
            get
            {
                if (!(AlertTypeName == "Low battery 10%" || AlertTypeName == "Low battery 30%"))
                {
                    rightArrow = "right_arrow.png";
                }
                else
                {
                    rightArrow = "";
                }
                return rightArrow;
            }

            set
            {
                rightArrow = value;
                OnPropertyChanged("RightArrow");
            }
        }
        protected void SetProperty<U>(
				ref U backingStore, U value,
				string propertyName,
				Action onChanged = null,
				Action<U> onChanging = null)
			{
				if (EqualityComparer<U>.Default.Equals(backingStore, value))
					return;

				if (onChanging != null)
					onChanging(value);

				OnPropertyChanging(propertyName);

				backingStore = value;

				if (onChanged != null)
					onChanged();

				OnPropertyChanged(propertyName);



	}
		protected void OnPropertyChanging(string propertyName)
		{
			if (PropertyChanging == null)
				return;

			PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
		}

	}
}

