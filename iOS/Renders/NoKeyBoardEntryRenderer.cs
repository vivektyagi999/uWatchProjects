using System;
using uWatch;
using uWatch.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(NoKeyBoardEntry), typeof(NoKeyBoardEntryRenderer))]
namespace uWatch.iOS
{
    public class NoKeyBoardEntryRenderer : EntryRenderer
    {
        public NoKeyBoardEntryRenderer()
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                //self.theTextField.inputView = [[UIView alloc] initWithFrame: CGRectZero];
                var uiView = new UIKit.UIView();
                uiView.Frame = CoreGraphics.CGRect.Empty;
                this.Control.InputView = uiView;
            }
        }
    }
}
