using System;
namespace UwatchPCL.WebServices
{
    public interface IWebServiceResult
    {
        int ErrorCode
        {
            get;
            set;
        }
        string ErrorMessage
        {
            get;
            set;
        }
        bool IsSuccess
        {
            get;
           
        }
    }
    public interface IWebServiceResult<TData> :IWebServiceResult
    {
        TData Data
        {
            get;
            set;
        }
    }
}
