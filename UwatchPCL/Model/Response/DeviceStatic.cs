using System;
using UwatchPCL.Model;

namespace UwatchPCL
{
	public class DeviceStatic : BaseModel
	{
		public int device_idx { get; set; }
		public string imei { get; set; }
		public Nullable<System.DateTime> init_date { get; set; }
		public string strinit_date { get; set; }
		public string model_no { get; set; }
		public string serial_no { get; set; }
		public string batch_no { get; set; }
		public string FriendlyName { get; set; }
		public Nullable<System.DateTime> SimExpireDate { get; set; }
		public string strSimExpireDate { get; set; }
		public string Device_build_No { get; set; }
		public string fw_version { get; set; }
		public string iccid { get; set; }
		public string DistributerId { get; set; }
		public Nullable<System.DateTime> LastUpdated { get; set; }
		public string strLastUpdated { get; set; }
		public string DistributerName { get; set; }
		public int OwnerUserID { get; set; }
		public string OwnerUserName { get; set; }
		public string OwnerFullName { get; set; }
		public int state_code { get; set; }
		public string deviceStatus { get; set; }
		public string fwStatus { get; set; }
		public int? Signal { get; set; }
		public int? Battery { get; set; }
		public int? DegC { get; set; }
		public DateTime? LastAlert { get; set; }

		public string DeviceSwitchStatus { get; set; }

		public string LastLocation { get; set; }

		
		public decimal? lat { get; set; }

		
		public decimal? lang { get; set; }

		public decimal Radius { get; set; }

		public string lSource { get; set; }


	}
}

