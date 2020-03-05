using System;
using System.Collections.Generic;
using UwatchPCL;

namespace uWatch
{
    public interface IMail
    {
        void SendMail(List<string> listOfRecepients, List<string> listOfRecepientsBCC, List<string> listOfRecepientsCC, string subject, string message, List<string> listOfAttachement);
    }
}
