using System;
using UwatchPCL.Model;

namespace UwatchPCL
{
	public class InMailModel : BaseModel
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

		public string FullName { get; set; }

		public string[] UserNameList { get; set; }

		public string From { get; set; }

		public string To { get; set; }

		public DateTime? Date { get; set; }

		bool isRead;

		public bool IsRead
		{
			get
			{
				return isRead;
			}

			set
			{
				isRead = value;
				OnPropertyChanged("IsRead");
			}
		}
		bool isvisible = true;

		public bool isVisible
		{
			get
			{
				return isvisible;
			}

			set
			{
				isvisible = value;
				OnPropertyChanged("isVisible");
			}
		}


		public short? ContactMethod { get; set; }

		public int[] DeviceIdList { get; set; }

		public string[] MessageIdList { get; set; }

		public int? LostID { get; set; }

		public bool? IsPushnotficationRequired { get; set; }

		public int? DraftMailID { get; set; }

	}
}