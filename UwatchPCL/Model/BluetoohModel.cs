using System;
using UwatchPCL.Model;

namespace UwatchPCL
{
	public class BluetoohModel:BaseModel
	{
		
		public string DeviceName { get; set; }
		public string DeviceId { get; set; }
		public string DeviceMACAddress { get; set; }


		public string DeviceRSSI { get; set; }
	}
}
