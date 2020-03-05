using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace UwatchPCL
{
	public class ListOfUsersResponce
	{
		private string image;

		public int SNo { get; set; }

        public string Text { get; set; }

        public string Value { get; set; } 

		public string Name { get; set; }

		public string UserId { get; set; }

		public Boolean FlagValue { get; set; }


		public String Image
		{
			get { return image; }

			set
			{
				image = value;
				if (PropertyChanged != null)
				{
					OnPropertyChanged("Image");
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
	}
}
