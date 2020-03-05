using System;
using System.Threading.Tasks;
using uWatch.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(PushNotificationClear))]
namespace uWatch.Droid
{
    public class PushNotificationClear:IPushNotificationClear
    {
        async Task<bool> IPushNotificationClear.clearNotification()
        {
            MainActivity.Instance.PushNotificationClear();
            return true;
        }
    }
}
