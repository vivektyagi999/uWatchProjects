using System;
using Android.Graphics;
using Android.Graphics.Drawables;
using uWatch;
using uWatch.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Switch = Android.Widget.Switch;


[assembly: ExportRenderer(typeof(ExtendedSwitch), typeof(ExtendedSwitchRenderer))]
namespace uWatch.Droid
{
	public class ExtendedSwitchRenderer: ViewRenderer<ExtendedSwitch, Switch>
	{
		/// <summary>
		/// Called when [element changed].
		/// </summary>
		/// <param name="e">The e.</param>
		protected override void OnElementChanged(ElementChangedEventArgs<ExtendedSwitch> e)
		{
			if (this.Control == null)
			{
				var toggle = new Switch(this.Context);

				toggle.CheckedChange += ControlValueChanged;
				this.SetNativeControl(toggle);
			}

			base.OnElementChanged(e);

			if (e.OldElement != null)
			{
				e.OldElement.Toggled -= ElementToggled;
			}

			if (e.NewElement != null)
			{
				this.Control.Checked = e.NewElement.IsToggled;
			
				this.Element.Toggled += ElementToggled;
			}
		}

		/// <summary>
		/// Handles the <see cref="E:ElementPropertyChanged" /> event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			
		}

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources.
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.Control.CheckedChange -= this.ControlValueChanged;
				this.Element.Toggled -= ElementToggled;
			}

			base.Dispose(disposing);
		}


		/// <summary>
		/// Handles the Toggled event of the Element control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="ToggledEventArgs"/> instance containing the event data.</param>
		private void ElementToggled(object sender, ToggledEventArgs e)
		{
			this.Control.Checked = this.Element.IsToggled;
		}

		/// <summary>
		/// Handles the ValueChanged event of the Control control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void ControlValueChanged(object sender, EventArgs e)
		{
			this.Element.IsToggled = this.Control.Checked;
			if (this.Control.Checked)
			{
				this.Control.ThumbDrawable.SetColorFilter(global::Android.Graphics.Color.Green, PorterDuff.Mode.SrcAtop);
			}
			else
			{
				this.Control.ThumbDrawable.SetColorFilter(global::Android.Graphics.Color.Red, PorterDuff.Mode.SrcAtop);
			}
		}
	}
}
