using System;
using UwatchPCL;

namespace uWatch
{
	public interface ISendMessage
	{
		
		void SendMessages(string mobileNumber);
		void SendMail(string Email);
		void Call(string mobileNumber);
	}
}

