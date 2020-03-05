using System;
using UwatchPCL.Model;

namespace UwatchPCL
{
	public class DeviceAgentAlerts : BaseModel
	{
		public DeviceAgentAlerts()
		{
			AutoEscalate = false;
		}

		public int AgentAlert_idx { get; set; }

		public int Device_idx { get; set; }



		public string SendTo { get; set; }


		public Nullable<bool> AutoEscalate { get; set; }


		public string AgentUserName { get; set; }


		public int Agent_idx { get; set; }
	}

}

