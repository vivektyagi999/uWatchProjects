using System;
using Android.Content;
using Android.Graphics;
using uWatch.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly:ExportRenderer(typeof(Xamarin.Forms.Label),typeof(FontAwesomeRenderer))]
namespace uWatch.Droid
{
	public class FontAwesomeRenderer:LabelRenderer
    {
        public FontAwesomeRenderer(Context context):base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                Control.Typeface = Typeface.CreateFromAsset(Forms.Context.Assets, "fontawesome-webfont.ttf");
            }
        }
    }
}
