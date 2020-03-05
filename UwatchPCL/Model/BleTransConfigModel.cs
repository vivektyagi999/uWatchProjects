using System;
using UwatchPCL.Model;

namespace UwatchPCL
{
    public class BleTransConfigModel : BaseModel
    {
        public bool isChanged;

        public BleTransConfigModel()
        {
            //rbAdvFrequencyHigh = true;
            SlaveSleepTime = 1;
            //AdvFrequency = (int)AdvertiseFrequency.High;
        }

        public bool IsNew { get; set; }

        public int Configuration_idx
        {
            get;
            set;
        }

        public bool SaveTag
        {
            get;
            set;
        }

        public int BleTransConfigSetting_idx { get; set; }

        public int BleTransConfig_idx { get; set; }

        public int? DeviceID { get; set; }

        public int? OrderNo { get; set; }

        public bool rbAdvFrequencyHigh { get; set; }

        public bool rbAdvFrequencyMedium { get; set; }

        public bool rbAdvFrequencyLow { get; set; }

        public int AdvFrequency { get; set; }

        public int? SlaveSleepTime { get; set; }

        public DateTime? cfg_change { get; set; }

        public DateTime? cfg_delivered { get; set; }

        public string MacAddress { get; set; }

        //[Required(ErrorMessage = "please enter friendly name")]
        //[DisplayName("Friendly Name")]
        public string FriendlyName { get; set; }

        //public SlaveType ConfigType { get; set; }

        public bool IsDeleted { get; set; }

        public int intBleActivityFreqencyHours { get; set; }

    }
}
