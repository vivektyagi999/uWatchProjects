using System;
namespace UwatchPCL
{
	public enum TimeType
	{
		OnlyDate = 1,
		OnlyTime,
		DateAndTime
	}
	public class DateFormat
	{
		public DateFormat()
		{
		}



		public static string GetDateTime(DateTime? dt, TimeType TimeType)
		{
			if (dt == null)
			{
				return "";
			}
			else if (TimeType == TimeType.OnlyTime)
			{
				return Convert.ToDateTime(dt).ToString("HH:mm");
			}
			else if (TimeType == TimeType.OnlyDate)
			{
				return Convert.ToDateTime(dt).ToString("dd-MMM-yy");
			}
			else if (TimeType == TimeType.DateAndTime)
			{
				return Convert.ToDateTime(dt).ToString("dd-MMM-yy HH:mm");
			}
			else
			{
				return "";
			}
		}
	}
}
