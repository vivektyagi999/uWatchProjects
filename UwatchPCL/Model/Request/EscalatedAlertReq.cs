using System;
namespace UwatchPCL
{
	public class EscalatedAlertReq
	{
		public int AgentID { get; set; }

		public int UserID { get; set; }

		public int DeviceID { get; set; }

		public int PageIndex { get; set; }

		public int PageSize { get; set; }

	}
}
