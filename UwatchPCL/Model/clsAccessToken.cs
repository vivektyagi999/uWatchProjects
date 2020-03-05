using System;
namespace UwatchPCL
{
	public class clsAccessToken
	{
		public string access_token { get; set; }

		public string token_type { get; set; }

		public DateTime issued { get; set; }

		public DateTime expires { get; set; }

		public string error { get; set; }

		public int User_Idx { get; set; }

		public bool IsPasswordChanged { get; set; }

		public string UserName { get; set; }

		public string FullName { get; set; }

		public int Roleid { get; set; }

		public bool UpdateRequired { get; set; }

        public bool UpdateAvailable { get; set; }

        public DateTime? LastLoginTime { get; set; }

	}
}

