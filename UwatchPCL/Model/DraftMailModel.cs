using System;
using UwatchPCL.Model;

namespace UwatchPCL
{
	public class DraftMailModel:BaseModel
	{
		public int DraftMailID { get; set; }

		public string Title { get; set; }

		public string Body { get; set; }

		public DateTime? SendDate { get; set; }

		public short? AppearOn { get; set; }

		public int SendBy { get; set; }

		public string SendTo { get; set; }


	}
}
