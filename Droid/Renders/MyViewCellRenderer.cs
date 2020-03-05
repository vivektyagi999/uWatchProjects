using System;
using uWatch;
using uWatch.Droid;
using UwatchPCL;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Views;

[assembly: ExportRenderer(typeof(MyViewCell), typeof(MyViewCellRenderer))]
namespace uWatch.Droid
{
	public class MyViewCellRenderer: ViewCellRenderer
	{
		protected override global::Android.Views.View GetCellCore(Cell item, global::Android.Views.View convertView, global::Android.Views.ViewGroup parent, global::Android.Content.Context context)
		{
			var cell = base.GetCellCore(item, convertView, parent, context);

			cell.SetBackgroundResource(Resource.Drawable.ViewCellBackground);

			return cell;
		}
	}

}

