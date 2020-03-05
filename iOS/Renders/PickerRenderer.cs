using System;
using uWatch;
using uWatch.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomPicker), typeof(CustomPickerRenderer))]
namespace uWatch.iOS
{
    public class CustomPickerRenderer:PickerRenderer
    {
        public CustomPickerRenderer()
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);   
            if(Control!=null)
            {
               // this.Control.fo
            }
        }
    }
}
