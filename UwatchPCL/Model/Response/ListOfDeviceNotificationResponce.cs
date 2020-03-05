using System;
namespace UwatchPCL
{
	public class ListOfDeviceNotificationResponce
	{
		public int MailID { get; set; }

	
		public string Title { get; set; }


		public string Body { get; set; }


		public DateTime? SendDate { get; set; }


		public string strSendDate { get; set; }


		public DateTime? SentDate { get; set; }


		public short? AppearOn { get; set; }

		public string SendBy { get; set; }

		public int? SendTo { get; set; }


		public string[] UserNameList { get; set; }

		public string From { get; set; }

		public string To { get; set; }

		public DateTime? Date { get; set; }

	}
}
