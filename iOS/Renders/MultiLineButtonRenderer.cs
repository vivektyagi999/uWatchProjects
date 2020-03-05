using System;
using UIKit;
using uWatch;
using uWatch.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(MultiLineButton), typeof(MultiLineButtonRenderer))]
namespace uWatch.iOS
{
    public class MultiLineButtonRenderer:ButtonRenderer
    {
        public MultiLineButtonRenderer()
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);   
            if(Control!=null)
            {
                Control.TitleEdgeInsets = new UIEdgeInsets(4, 4, 4, 4);
                Control.TitleLabel.LineBreakMode = UILineBreakMode.WordWrap;
                Control.TitleLabel.TextAlignment = UITextAlignment.Center;
            }
        }
    }
}
