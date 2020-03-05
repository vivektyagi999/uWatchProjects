using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace UwatchPCL
{
	public class IObservalCollection : INotifyPropertyChanged
	{
		public IObservalCollection ()
		{
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			var handler = PropertyChanged;
			if (handler != null) 
			{
				handler(this, new PropertyChangedEventArgs (propertyName));
			}
		}
	}
}

