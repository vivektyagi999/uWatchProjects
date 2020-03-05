using System;
using static UwatchPCL.MyController;

namespace UwatchPCL.Model.Request
{
	public class AccountModelReq
	{
        public AppNames? AppName { get; set; }
		public string Username {get;set;}
		public string Password {get;set;}
		public string CurrentVersion { get; set; }
		public string Platform { get; set; }
	}
}

