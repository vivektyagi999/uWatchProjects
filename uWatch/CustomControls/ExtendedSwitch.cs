using System;
using Xamarin.Forms;

namespace uWatch
{
	public class ExtendedSwitch: Switch
	{
		/// <summary>
		///     Identifies the Switch tint color bindable property.
		/// </summary>
		public static readonly BindableProperty TintColorProperty =
			BindableProperty.Create<ExtendedSwitch, Color>(
				p => p.TintColor, Color.Green);

		/// <summary>
		///     Gets or sets the color of the tint.
		/// </summary>
		/// <value>The color of the tint.</value>
		public Color TintColor
		{
			get { return (Color)GetValue(TintColorProperty); }

			set { SetValue(TintColorProperty, value); }
		}
	}
}
