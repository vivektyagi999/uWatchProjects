using System;
using UIKit;
using uWatch;
using uWatch.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(InfiniteListView), typeof(InfiniteListViewRendrer))]
namespace uWatch.iOS
{
    public class InfiniteListViewRendrer:ListViewRenderer
    {
		protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
		{
			base.OnElementChanged(e);

			if (this.Control == null) return;

			this.Control.TableFooterView = new UIView();
		}
    }
}
