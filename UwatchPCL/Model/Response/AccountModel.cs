using System;
using System.Collections.Generic;

namespace UwatchPCL.Model.Response
{
	public class AccountModel
	{
		public int User_Idx { get; set; }

		public string UserName { get; set; }

		public string FullName { get; set; }

		public string EmailID { get; set; }

		public Nullable<System.DateTime> CreateDate { get; set; }

		public string Password { get; set; }

		public Nullable<System.DateTime> PasswordChangeDate { get; set; }

		public int? Roleid{get;set;}

		public string error {get;set;}



	}

	public class AddUserModel : UsersDetail
	{
		
		public string Password { get; set; }


		public string ConfirmPassword { get; set; }

		public string RoleName { get; set; }

		public string FriendlyName { get; set; }

		public int Device_idx { get; set; }

		public int? Roleid { get; set; }

		public string FullName { get; set; }

		public string ZipCode { get; set; }

	}




}

