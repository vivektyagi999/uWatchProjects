using System;

namespace UwatchPCL
{
	public class AlertList
	{
		public String alertid { get; set; }
		public String deviceid { get; set; }
		public String alerttimestamp { get; set; }

		public String friendlyname { get; set; }
		public String sensortype { get; set; }
		public String picturerequested { get; set; }
		public String imageurl { get; set; }
		public DateTime alertTime{ get; set;}
	}
}

