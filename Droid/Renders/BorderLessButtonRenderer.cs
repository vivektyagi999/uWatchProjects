using System;
using uWatch;
using uWatch.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;


[assembly:ExportRenderer(typeof(BorderLessButton),typeof(BorderLessButtonRenderer))]
namespace uWatch.Droid
{
    public class BorderLessButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);
            this.Control.Background = null;
        }
    }
}
