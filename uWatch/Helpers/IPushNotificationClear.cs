using System;
using System.Threading.Tasks;

namespace uWatch
{
    public interface IPushNotificationClear
    {
        Task<bool> clearNotification();
    }
}
