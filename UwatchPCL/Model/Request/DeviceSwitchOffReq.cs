using System;
using UwatchPCL.Model;

namespace UwatchPCL
{
	public class DeviceSwitchOffReq:BaseModel
	{
        public int DeviceHours { get; set; }
        public int DeviceId { get; set; }
        public DateTime? cfg_changed { get; set; }

        public DateTime? cfg_delivered { get; set; }

        public DateTime? ServerCurentTime { get; set; }

        public int? PendingTime { get; set; }

        public bool? IsDevicePermanentOff { get; set; }

        public bool IsCancelable { get; set; }

        public string DeviceStatus { get; set; }
        public string strcfg_changed { get; set; }

        public bool isDeviceEnable { get; set; }

        public int AppConfig_idx { get; set; }
    }
}
