using System;
using UIKit;
using uWatch;
using uWatch.iOS;
using UwatchPCL;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(MyViewCell), typeof(MyViewCellRenderer))]
namespace uWatch.iOS
{
	public class MyViewCellRenderer: ViewCellRenderer
	{
		public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
		{
			var cell = base.GetCell(item, reusableCell, tv);

			cell.SelectionStyle = UITableViewCellSelectionStyle.None;

			return cell;
		}
	}
}
