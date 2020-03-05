using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using uWatch.ViewModels;
using UwatchPCL;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace uWatch
{
	public class AlertDetailsViewModel : BaseViewModel
	{

		long countdown;

		public event PropertyChangedEventHandler PropertyChanged;
		public bool image_id { get; set;}
		public Byte[] Image { get; set; }
		public string TempImage { get; set; }
		public string BatteryImage { get; set; }
		public string SignalImage { get; set; }
		public string DeviceName { get; set; }
		public string AddressLine1 { get; set; }
		public string AddressLine2 { get; set; }
		public string PostalCode { get; set; }
		public string AlertDateTime { get; set; }
		public bool Camera_Enable { get; set;}
		public bool Gps_enable { get; set; }
		public int alert_Type { get; set;}

		public AlertImage AlertImage { get; set; }

		public Position AlertPosition { get; set; }

		public long CountDown
		{
			set
			{
				if (countdown != value)
				{
					countdown = value;

					if (PropertyChanged != null)
					{
						
						PropertyChanged(this,
							new PropertyChangedEventArgs("CountDown"));
						
					}
				}
			}
			get
			{
				return countdown;
			}
		}


		public AlertsEsclatedToAgentViewModel device { get; set; }

		public AlertDetailsViewModel()
		{
			
		}

		public AlertDetailsViewModel(AlertsEsclatedToAgentViewModel obj)
		{
			try
			{
				
				GetDetails(obj);
				

			}
			catch { }
		}

		public async Task Countdownstart()
		{
			this.CountDown = (long)(device.WakeTime * 60);

			while(this.CountDown>0)
			{
				this.CountDown = this.CountDown - 1;
				await Task.Delay(1000);
			};
		}

		private async Task GetDetails(AlertsEsclatedToAgentViewModel Alert)
		{
			try
			{
				device = Alert;

				AlertPosition = new Position(Convert.ToDouble(device.lat),Convert.ToDouble(device.lang));

				var Atemp = Alert.DegC == null ? 0 : Convert.ToInt32(Alert.DegC);
				TempImage = await MyController.TempratureLevelImage(Atemp);
				Alert.TemperatureImage = TempImage;

				var Abattery = Alert.Battery == null ? 0 : Convert.ToInt32(Alert.Battery);
				BatteryImage =  await MyController.BatteryLevelImage(Abattery);
				Alert.BatteryImage = BatteryImage;

              	var Asignal = Alert.Signal == null ? 0 : Convert.ToInt32(Alert.Signal);
				SignalImage = await MyController.SignalLevelImage(Asignal);
				Alert.SignalImage = SignalImage;

				AlertImage = new AlertImage();
				AlertImage.Alertlog_id = device.alertlog_idx;
				var bytes = ApiService.Instance.GetAlertImage(AlertImage).Result;
				if(bytes!=null)
				AlertImage.Image = bytes;

                Camera_Enable = Convert.ToBoolean(Alert.Camera_Enable);
                alert_Type = Alert.alert_type;
                Gps_enable = Convert.ToBoolean(Alert.Gps_enable);
                image_id = Alert.image_id;
				
			}
			catch (Exception ex)
			{
				MyController.ErrorManagement(ex.Message);
			}
		}

	}
}

