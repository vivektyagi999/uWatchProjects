using System;
namespace UwatchPCL.WebServices
{
    public class WebServiceResult:IWebServiceResult
    {
        public int ErrorCode
        {
            get;
            set;
        }
        public string ErrorMessage
        {
            get;
            set;
        }

        public bool IsSuccess
        {
            get
            {
                return ErrorCode == 0 ? true : false;    
            }
           
        }

        public WebServiceResult()
        {
            
        }

    }
    public class WebServiceResult<TData> : WebServiceResult, IWebServiceResult<TData>
    {
        public TData Data
        {
            get;
            set;
        }
    }
}
