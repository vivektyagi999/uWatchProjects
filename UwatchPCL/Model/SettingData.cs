using System;
namespace UwatchPCL
{
	public class SettingData
	{
		public int device_idx { get; set; }

		public int config_idx { get; set; }

		public string imei { get; set; }

		public string FriendlyName { get; set; }

		public bool ble_available { get; set; }

		public bool Batch_Images { get; set; }

		
		public bool chkBatchAlertUpload { get; set; }

		public bool chkCamera { get; set; }

		public bool chkMovementSensonPIR { get; set; }

		public bool chkShockSensor { get; set; }

		public bool chkSounder { get; set; }

		public bool chkTemperatureSensor { get; set; }

		public bool chkTemperatureRange { get; set; }

		public bool chkBluetoothLowEnergy { get; set; }

		public bool chkDeviceLCDDisplayMode { get; set; }

		public bool rbLowCameraResolution { get; set; }

		public bool rbHighCameraResolution { get; set; }

		public int numOccurrencesInOneMin { get; set; }

		public int numSensitivityOfSensor { get; set; }

		public int numOccurrencesInOneMinSS { get; set; }

		public bool chkFindmeFlag { get; set; }

		public int numUpperDegree { get; set; }

		public int numLowerDegree { get; set; }

		public int numIntervalBetweenReadings { get; set; }

		public int numFrequencyInHours { get; set; }

		public int numActivePeriodAfterAlert { get; set; }

		public int numIdlePeriodAfterActivation { get; set; }

		public bool rbNormal { get; set; }

		public bool rbIcon { get; set; }

		public bool rbDiagnostic { get; set; }

		public bool chkForcedAlert { get; set; }

		public bool chkGPS { get; set; }

		public DateTime? cfg_changed { get; set; }

		public DateTime? cfg_delivered { get; set; }

		public string message_id { get; set; }

		public string actionStartDate { get; set; }

		public string actionStartTime { get; set; }

		public bool chkActiveTime { get; set; }

		public int User_Idx { get; set; }

		public string ConfigurationName { get; set; }

		public string Description { get; set; }

		public string OwnerActions { get; set; }

		public int? ConfigurationID { get; set; }

		public string configType { get; set; }

		public bool rbBleActivityAlways { get; set; }

		public bool rbBleActivityFrequently { get; set; }

		public int BLEactivity { get; set; }

		public int intBleActivityFreqencyHours { get; set; }

		public string ControlName { get; set; }

		public bool rdoSwitchOnNever { get; set; }

		public bool rdoSwitchOnnow { get; set; }

		public bool rdoSwitchOnfor { get; set; }

		public bool rdoSwitchOnAt { get; set; }

		public string SwitchOnForTime { get; set; }

		public string SwitchOnTime { get; set; }

		public string SwitchOffTime { get; set; }

		public string SwitchOnAtTime { get; set; }

		public bool chkSwitchOnRepeat { get; set; }

		public bool rdoSwitchOffNever { get; set; }

		public bool rdoSwitchOffnow { get; set; }

		public bool rdoSwitchOfffor { get; set; }

		public bool rdoSwitchOffAt { get; set; }

		public short? SwitchOffType { get; set; }

		public short? SwitchOnType { get; set; }

		public string SwitchOffForTime { get; set; }

		public string SwitchOffAtTime { get; set; }

		public bool chkSwitchOffRepeat { get; set; }

		public bool switch_off_enabled { get; set; }

		public bool chkSwitchAlerts { get; set; }

		public bool chkSwitchOnAlerts { get; set; }

		public bool chkSwitchOffAlerts { get; set; }

	}
}
