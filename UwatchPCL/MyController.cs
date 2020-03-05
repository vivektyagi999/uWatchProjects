using System; using System.Collections.Generic; using System.Threading.Tasks; using Acr.UserDialogs;  namespace UwatchPCL {
    public static class MyController
    {
        public static bool isAppClosed { get; set; }

        public static bool isMessage { get; set; }
        public static int user_id { get; set; }
        public static int Roll_id { get; set; }
        public static string Device_OS { get; set; }


        public static int ScreenWidth { get; set; }
        public static int ScreenHeight { get; set; }

        public static int VirtualWidth = 384;
        public static int VirtualHeight = 592;

        public static bool isDashbaord = true;
        public static bool isRememberMe = true;
        public static bool fromAssetsToGallery = false;

        public static string FullName { get; set; }

        public static string AlertId = "";
        public static int multiple = 0;
        public static List<string> lstNs = new List<string>();





        public static int ConvertHeight(int val)
        {
            float h = VirtualHeight;
            float per = (val * 100) / h;
            return Convert.ToInt32((ScreenHeight * per) / 100);
        }

        public static int ConvertWidth(int val)
        {
            float w = VirtualWidth;
            float per = (val * 100) / w;
            return Convert.ToInt32((ScreenWidth * per) / 100);
        }


        public static async Task<string> BatteryLevelImage(int? BatteryLevel)
        {
            BatteryLevel = BatteryLevel ?? 0;

            if (BatteryLevel <= 10)
            {
                return "battery0.png";
            }
            else if (BatteryLevel > 10 && BatteryLevel <= 40)
            {
                return "battery1.png";
            }

            else if (BatteryLevel > 40 && BatteryLevel <= 70)
            {
                return "battery2.png";
            }
            else
            {
                return "battery3.png";
            }
        }


        public static async Task<string> SignalLevelImage(int? SignalLevel)
        {
            SignalLevel = SignalLevel ?? 0;

            if (SignalLevel <= 10)
            {
                return "signal0.png";
            }
            else if (SignalLevel > 10 && SignalLevel <= 40)
            {
                return "signal1.png";
            }

            else if (SignalLevel > 40 && SignalLevel <= 70)
            {
                return "signal2.png";
            }
            else
            {
                return "signal3.png";
            }
        }

        public static string GetAlertTypelImage(string AlertType)
        {
            if (AlertType.ToLower() == "HeartBeat".ToLower())
            {
                return "heartbeat.png";
            }
            else if (AlertType.ToLower() == "PIR".ToLower())
            {
                return "pir.png";
            }

            else if (AlertType.ToLower() == "Shock".ToLower())
            {
                return "shock.png";
            }
            else
            {
                return "heartbeat.png";
            }
        }


        public enum SENSOR_TYPE
        {
            HEARTBEAT = 0,
            PIR,
            SHOCK,
            TEMPERATURE,
            LOWBATTERY,
            FORCED,
            SWITCHON,
            SWITCHOFF,
            BLUETOOTH,
            BATCH,
            LowBatterry_30 = 12,
            LowBatterry_10 = 13,
            PowerSource =14


        };         public enum AppNames         {             uWatch = 1,             BeeWatch,             DodgyGear,             RomaxCube         };
        public static void ErrorManagement(string message)
        {

        }


        public static async Task<string> GetAlertTypelImage(int? AlertType_id)
        {
            AlertType_id = AlertType_id ?? 0;



            switch (AlertType_id)
            {
                case (int)SENSOR_TYPE.PIR:
                    return "pir.png";

                case (int)SENSOR_TYPE.SHOCK:
                    return "shock.png";

                case (int)SENSOR_TYPE.TEMPERATURE:
                    return "tem.png";

                case (int)SENSOR_TYPE.SWITCHON:
                    return "power.png";

                case (int)SENSOR_TYPE.SWITCHOFF:
                    return "poweroff.png";

                case (int)SENSOR_TYPE.HEARTBEAT:
                    return "heartbeat.png";

                case (int)SENSOR_TYPE.FORCED:
                    return "force.png";

                case (int)SENSOR_TYPE.BLUETOOTH:
                    return "blue.png";

                case (int)SENSOR_TYPE.BATCH:
                    return "picture.png";

                case (int)SENSOR_TYPE.LowBatterry_30:
                    return "lowbattery30.png";

                case (int)SENSOR_TYPE.LowBatterry_10:
                    return "Battery_Empty.png";

                case (int)SENSOR_TYPE.PowerSource:
                    return "NoPower.png";

                default:
                    return "pir.png";
            }
        }



        public static string GetAlertTypeName(int? AlertType_id)
        {
            AlertType_id = AlertType_id ?? 0;

            switch (AlertType_id)
            {
                case (int)SENSOR_TYPE.PIR:
                    return "PIR";

                case (int)SENSOR_TYPE.SHOCK:
                    return "Shock";

                case (int)SENSOR_TYPE.TEMPERATURE:
                    return "Temperature";

                case (int)SENSOR_TYPE.BLUETOOTH:
                    return "Bluetooth";

                case (int)SENSOR_TYPE.SWITCHON:
                    return "Switch on";

                case (int)SENSOR_TYPE.SWITCHOFF:
                    return "Switch off";

                case (int)SENSOR_TYPE.HEARTBEAT:
                    return "Heartbeat";

                case (int)SENSOR_TYPE.FORCED:
                    return "Forced";

                case (int)SENSOR_TYPE.LowBatterry_30:
                    return "Low battery 30%";

                case (int)SENSOR_TYPE.LowBatterry_10:
                    return "Low battery 10%";

                case (int)SENSOR_TYPE.PowerSource:
                    return "Power Off";

                default:
                    return "Unknown";
            }
        }

        public static async Task<string> TempratureLevelImage(int? TempLevel)
        {
            TempLevel = TempLevel ?? 0;

            if (TempLevel <= 10)
            {
                return "temperature0.png";
            }
            else if (TempLevel > 10 && TempLevel <= 40)
            {
                return "temperature1.png";
            }

            else if (TempLevel > 40 && TempLevel <= 70)
            {
                return "temperature2.png";
            }
            else
            {
                return "temperature3.png";
            }
        }


    } }   