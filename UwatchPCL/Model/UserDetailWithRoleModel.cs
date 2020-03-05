using System;
using UwatchPCL.Model;

namespace UwatchPCL
{
	public class UserDetailWithRoleModel : BaseModel
	{
		public string UserName { get; set; }
		public string FullName { get; set; }
		public int UDetail_Idx { get; set; }
		public int User_Idx { get; set; }
		public string FName { get; set; }
		public string LName { get; set; }
		public string Salutaion { get; set; }
		public string Sex { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
		public string strDOB { get; set; }
		public string CompanyName { get; set; }
		public string Mobile1 { get; set; }
		public string Mobile2 { get; set; }
		public string LandLine1 { get; set; }
		public string LandLine2 { get; set; }
		public string Email { get; set; }
		public string UserNumber { get; set; }
		public string AccessCode { get; set; }
		public string ProductNo { get; set; }
		public string ProductSlNo { get; set; }
		public string Address { get; set; }
		public string Town { get; set; }
		public string State { get; set; }
		public string ZipCode { get; set; }
		public Nullable<int> CountryId { get; set; }
		public int RoleId { get; set; }
		public string RoleName { get; set; }
		public string Country { get; set; }


	}
}

