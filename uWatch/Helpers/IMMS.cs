using System.Collections.Generic;
using UwatchPCL;

namespace uWatch
{
	public interface IMMS
	{
		void SendMMS(string to, string msg, List<string> content, AlertsEsclatedToAgentViewModel _alertsEsclatedToAgentViewModel);
	}
}
