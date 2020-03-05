using System;
using uWatch;

namespace UwatchPCL
{
    public class BLETransTag : ParentViewModel
    {
        public int ID { get; set; }
        public string FriendlyName { get; set; }
        public string MacAddress { get; set; }
        public int SlaveSleepTime { get; set; }
        public int UserID { get; set; }
        public bool Relay1 { get; set; }
        public string Relay1Type { get; set; }
        public bool Relay2 { get; set; }
        public string Relay2Type { get; set; }
        public string MacAddress1 { get; set; }
        public string MacAddress2 { get; set; }
        public string MacAddress3 { get; set; }
        public string MacAddress4 { get; set; }
        public string MacAddress5 { get; set; }
        public string MacAddress6 { get; set; }
        // public SlaveType ConfigType { get; set; }
        public string SlaveType { get; set; }
        public bool? _tagSelected;
        public bool? TagSelected
        {
            get { return _tagSelected; }
            set
            {
                if (value != _tagSelected)
                {
                    _tagSelected = value;
                    PropertyChangedBase("TagSelected");
                }
            }
        }
        public BLETransTag()
        {

        }
    }
  
}
