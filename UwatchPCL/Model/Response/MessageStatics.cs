using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UwatchPCL.Model;
using Xamarin.Forms;

namespace UwatchPCL
{
	public class MessageStatics : BaseModel
	{
		public int NotificationID { get; set; }

		public string title { get; set; }
		public string Description { get; set; }


		public string NotificationFrom { get; set; }

		public int? NotificationFor { get; set; }

		public bool? IsRead { get; set; }

		public bool? IsDeleted { get; set; }

		public int? Device_idx { get; set; }

		public string friendly_name { get; set; }

	


		public const string TextColorOfSelectedMsgPropertyName = "TextColorOfSelectedMsg";
		private Color textColorOfSelectedMsg;

		public Color TextColorOfSelectedMsg
		{
			get { return textColorOfSelectedMsg; }


			set
			{
				textColorOfSelectedMsg = value;
				if (PropertyChanged != null)
				{
					OnPropertyChanged("TextColorOfSelectedMsg");
				}
			}

			
		}


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



		public const string FontOfSelectedMsgPropertyName = "TextColorOfSelectedMsg";
		private FontAttributes fontOfSelectedMsg = FontAttributes.Bold;

		public FontAttributes FontOfSelectedMsg
		{
			get { return fontOfSelectedMsg; }
			set { SetProperty(ref fontOfSelectedMsg, value, FontOfSelectedMsgPropertyName); }
		}


		
	}
}

