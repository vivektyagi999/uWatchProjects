using System;
namespace uWatch
{
	public interface INetworkConnection
	{
		bool IsConnected { get; }

		void CheckNetworkConnection();
	}
}

