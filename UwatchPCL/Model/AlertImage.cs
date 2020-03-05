using System;
using UwatchPCL.Model;

namespace UwatchPCL
{
	public class AlertImage : BaseModel
	{
		public int Alertlog_id { get; set; }

		public byte[] Image { get; set; }
	}
}

