using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UwatchPCL.WebServices
{
    public interface IWebService
    {
        Task<WebServiceResult<T>> PostAsync<T>(string action,string objectdata,bool returntype);

        Task<WebServiceResult<T>> GetAsync<T>(string action, string objectdata,bool returntype);
    }
}
