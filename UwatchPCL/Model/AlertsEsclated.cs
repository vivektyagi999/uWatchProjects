using System;
using UwatchPCL.Model;
using Xamarin.Forms;

namespace UwatchPCL
{
    public class AlertsEsclatedToAgentViewModel : BaseModel
    {
        public string FriendlyName { get; set; }

        public int alertlog_idx { get; set; }

        public int device_idx { get; set; }

        public int device_alert_id { get; set; }

        public System.DateTime DeviceDate { get; set; }

        public string AlertType { get; set; }

        public int alert_type { get; set; }

        public string data_string { get; set; }

        public int batch_id { get; set; }

        public bool IsBatchUpload { get; set; }

        public string geo_coords { get; set; }

        public bool image_id { get; set; }

        public string LatestImageID { get; set; }

        public short archived { get; set; }

        public int? Signal { get; set; }

        public Nullable<int> Battery { get; set; }

        public int DegC { get; set; }




        public bool IsBattery { get; set; }
        public string Sleep { get; set; }

        public string Device_Action { get; set; }

        public System.DateTime? dd_time { get; set; }

        public string AlertNote { get; set; }
        public Color stackColor;
        public Color StackColor
        {
            get
            {
                if (!(AlertTypeName == "Low battery 10%" || AlertTypeName == "Low battery 30%"))
                {
                    stackColor = Color.Transparent;
                    IsBattery = true;
                }
                else
                {
                    stackColor = Color.FromHex("#ddd");
                    IsBattery = false;
                }
                return stackColor;
            }

            set
            {
                stackColor = value;
                OnPropertyChanged("StackColor");
            }
        }



        public decimal? lat { get; set; }


        public decimal? lang { get; set; }

        public decimal Radius { get; set; }

        public string lSource { get; set; }

        public string FullName { get; set; }

        public string Mobile1 { get; set; }

        public int? EscalateTo { get; set; }

        public int? EscalateToAgentID { get; set; }

        public string EscalateMethod { get; set; }

        public int? WakeTime { get; set; }

        public int? CountDown { get; set; }

        public string GpsImage { get; set; }

        public string strAlertImage { get; set; }

        public string AssetsImage { get; set; }

        public string Mobile2 { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string OwnerFullName { get; set; }

        public byte[] alertImage { get; set; }

        public int OwnerUserID { get; set; }

        public bool? ImageComing { get; set; }

        public string EscalateToAgentName { get; set; }

        public DateTime? EscalateTime { get; set; }

        public int? EscalateBy { get; set; }

        public string EscalateByMethod { get; set; }

        public DateTime? EscalateByTime { get; set; }

        public string CarrierName { get; set; }

        public bool? Camera_Enable { get; set; }

        public bool? Gps_enable { get; set; }

        public string EscalateMethodByAgent { get; set; }

        public int? EscalateByAgent { get; set; }

        public int? EscalateToAreaManager { get; set; }

        public System.DateTime? EscalateToAreaManagerTime { get; set; }

        public System.DateTime? EscalateByAgentTime { get; set; }

        public string SlaveFriendlyName { get; set; }

        public string TotalBatchAlert { get; set; }

        public string batchImageIdentifier { get; set; }

        public string serial_no { get; set; }
    }
}

