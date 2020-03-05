using System;
using System.Collections.Generic;

namespace UwatchPCL.Model
{
	public class PushJson
	{
		public List<string> registration_ids { get; set; }
		public Data data { get; set; }
	}
	public class Data
	{
		public string tickerText { get; set; }
		public string contentTitle { get; set; }
		public string message { get; set; }
	}
}
