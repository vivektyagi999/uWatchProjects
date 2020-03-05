using System;
using System.Threading.Tasks;
using UwatchPCL.Model.Request;

namespace UwatchPCL
{
	public interface ILoginService
	{
		Task<clsAccessToken> UserLoginAsync (AccountModelReq request);
		bool IsAuthenticated { get; }
		bool IsSuccess { get; set;}
	}
}

