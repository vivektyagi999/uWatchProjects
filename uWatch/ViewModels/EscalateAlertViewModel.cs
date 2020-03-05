using System;
using uWatch.ViewModels;
using UwatchPCL;

namespace uWatch
{
	public class EscalateAlertViewModel : BaseViewModel
	{
		public string  AgentUserName { get; set; }
		public int AlertLogId { get; set; }
		public UserDetailWithRoleModel Agent { get; set; }

		public EscalateAlertViewModel()
		{
			
		}

		public async void GetAgent()
		{
			try
			{
				if (!String.IsNullOrEmpty(AgentUserName))
				{
					Agent = await ApiService.Instance.GetAgentDetails(AgentUserName);
				}
			}
			catch { }
		}

	}	
}

