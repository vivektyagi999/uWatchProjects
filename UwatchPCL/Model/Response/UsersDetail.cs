using System;

namespace UwatchPCL.Model.Response
{
	public class UsersDetail 
	{
		// for Users Detail
        public string User_Idx { get; set; }

		public string Email { get; set; }

		public string FName { get; set; }

		public string LName { get; set; }

		public  string Mobile1 { get; set; }

		public int Roleid { get; set; }

		public int? CountryId { get; set; }

		public string error {get;set;}
	}
}

