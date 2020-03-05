using System;
using Android.OS;

namespace FindLocally.Droid
{
	public class ServiceConnectedEventArgs: EventArgs
	{
		public IBinder Binder { get; set; }
	}
}

