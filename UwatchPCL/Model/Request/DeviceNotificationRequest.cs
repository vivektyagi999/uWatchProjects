using System;
namespace UwatchPCL
{
	public class DeviceNotificationRequest
	{
		public int UserID { get; set;}
		public MessageType type { get; set;}
		public int PageIndex { get; set;}
		public int RecordPerPage { get; set; }
	}
	public enum MessageType
	{
		INBOX,
		SENT
	};
}
