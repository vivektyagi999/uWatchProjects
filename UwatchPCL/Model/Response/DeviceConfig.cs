using System;
using UwatchPCL.Model;

namespace UwatchPCL
{
	public class DeviceConfig : BaseModel
	{
		public int config_idx { get; set; }

		public int device_idx { get; set; }

		public string message_id { get; set; }

		public int heartbeat_int { get; set; }

		public bool batch_enabled { get; set; }

		public bool GPS_enabled { get; set; }

		public int? batch_int { get; set; }

		public int? alert_wake_time { get; set; }

		public bool switch_off_enabled { get; set; }

		public int sensor_idle_time { get; set; }

		public int display_mode { get; set; }

		public bool pir_enabled { get; set; }

		public int pir_sensitivity { get; set; }

		public bool camera_enabled { get; set; }

		public int camera_res { get; set; }

		public bool buzz_enabled { get; set; }

		public bool shock_enabled { get; set; }

		public int shock_sensitivity { get; set; }

		public int? shock_events { get; set; }

		public bool temperature_enabled { get; set; }

		public int temperature_upper { get; set; }

		public int temperature_lower { get; set; }

		public int temperature_int { get; set; }

		public bool findme_enabled { get; set; }

		public bool config_received { get; set; }

		public string display_message { get; set; }

		public string server_url { get; set; }

		public bool ble_enabled { get; set; }

		public int? ack_code { get; set; }

		public DateTime? cfg_changed { get; set; }

		public DateTime? cfg_delivered { get; set; }

		public DateTime? heartbeat_startdate { get; set; }

		public bool forced_alert_enable { get; set; }

		public bool deviceLcdDisplayMode_enabled { get; set; }

		public bool temperature_range { get; set; }

		public int User_Idx { get; set; }

		public string ConfigurationName { get; set; }

		public int? ConfigurationID { get; set; }

		public int DeviceCustomConfig_Idx { get; set; }

		public string strheartbeat_startdate { get; set; }

		public string strcfg_changed { get; set; }

		public string configType { get; set; }

		

	}
}

