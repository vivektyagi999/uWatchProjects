using System;
using CoreLocation;

namespace uWatch.iOS
{
	public class LocationUpdatedEventArgs: EventArgs
	{
		CLLocation location;

		public LocationUpdatedEventArgs(CLLocation location)
		{
			this.location = location;
		}

		public CLLocation Location
		{
			get { return location; }
		}
	}
}
