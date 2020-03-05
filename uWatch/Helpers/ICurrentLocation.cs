using System;
namespace uWatch
{
	public interface ICurrentLocation
	{
		void GetCurrentLocation();

		void StopLocationServices();
	}
}
