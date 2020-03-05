using System;
namespace UwatchPCL
{
    public class ResetPasswordModel
    {
		public string UserName { get; set; }
       
        public string NewPassword { get; set; }
       
		public string ConfirmPassword { get; set; }

		public string OldPassword { get; set; }

    }
}
