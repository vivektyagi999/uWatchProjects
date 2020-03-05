using System;
namespace UwatchPCL
{
    public class AppDetails
    {
        public string CurrentVersion { get; set; }

        public string AppName { get; set; }

        public string Platform { get; set; }

        public bool UpdateRequired { get; set; }

        public bool UpdateAvailable { get; set; }
    }
}
