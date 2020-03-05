using System;
using ObjCRuntime;
using UIKit;
using uWatch.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;


[assembly: ExportRenderer(typeof(NavigationPage), typeof(NavigationPageRenderer))]
namespace uWatch.iOS
{
    public class NavigationPageRenderer : NavigationRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
            if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
            {
                OverrideUserInterfaceStyle = UIUserInterfaceStyle.Light;
            }
        }
        public override void WillMoveToParentViewController(UIViewController parent)
        {
            try
            {
                if (parent != null)
                {
                    if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
                    {
                        // Obviously in a real application this would be more sophisticated or concrete here, but for
                        // the purposes of this demo we're just doing a quick n dirty check to illustrate things
                        parent.ModalPresentationStyle = (UIKit.UIModalPresentationStyle)(App.UseIos13FullScreenModal ? UIModalPresentationStyle.Automatic : UIModalPresentationStyle.FullScreen);
                    }
                }
                                         
                base.WillMoveToParentViewController(parent);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }
    }


    [Unavailable(PlatformName.WatchOS, PlatformArchitecture.All, null)]
    [Native]
    public enum UIModalPresentationStyle : long
    {
        None = -1L,
        [Introduced(PlatformName.iOS, 13, 0, PlatformArchitecture.All, null)]
        Automatic = -2L,
        FullScreen = 0L,
        [Unavailable(PlatformName.TvOS, PlatformArchitecture.All, null)]
        PageSheet = 1L,
        [Unavailable(PlatformName.TvOS, PlatformArchitecture.All, null)]
        FormSheet = 2L,
        CurrentContext = 3L,
        Custom = 4L,
        OverFullScreen = 5L,
        OverCurrentContext = 6L,
        [Unavailable(PlatformName.TvOS, PlatformArchitecture.All, null)]
        Popover = 7L,
        BlurOverFullScreen = 8L
    }
}
