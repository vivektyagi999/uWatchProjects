using System;
namespace uWatch
{
    public interface ICustomContextActionsManager
    {
        void RestoreContextActionsViews();
        void SetCustomBackgroundColor(Xamarin.Forms.Color color, bool isForDestructive = false);
        void SetCustomView(Xamarin.Forms.View color, bool isForDestructive = false);
    }
}
