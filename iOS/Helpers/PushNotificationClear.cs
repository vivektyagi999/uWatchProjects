using System;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(uWatch.iOS.PushNotificationClear))]
namespace uWatch.iOS
{
    public class PushNotificationClear : IPushNotificationClear
    {

        async Task<bool> IPushNotificationClear.clearNotification()
        {
            AppDelegate.Instance.PushNotificationClear();
            return true;
        }
    }
}
