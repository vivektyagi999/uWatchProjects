using System;
using UwatchPCL.Model;

namespace UwatchPCL
{
	public class AppTokenModel :BaseModel
	{
        public int DeviceToken_idx { get; set; }

        public string DeviceToken { get; set; }

        public string DeviceOS { get; set; }

        public int UserID { get; set; }

        public string SerialNo { get; set; }

        public DateTime LastLoginTime { get; set; }

        public string AppVersion { get; set; }

        public bool? IsOnProduction { get; set; }

        public string AppName { get; set; }

        public string PhoneModel { get; set; }

        public string Manufacturer { get; set; }

        public string PhoneName { get; set; }

        public string VersionString { get; set; }

        public string Idiom { get; set; }
    }
}

