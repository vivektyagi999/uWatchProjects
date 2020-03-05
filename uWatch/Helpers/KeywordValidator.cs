using System;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace uWatch
{
    public class KeywordValidator:Behavior<Editor>
    {
		const string emailRegex = @"^[a-zA-Z0-9-]+$";


		protected override void OnAttachedTo(Editor bindable)
		{
			bindable.TextChanged += HandleTextChanged;
			base.OnAttachedTo(bindable);
			
		}

		void HandleTextChanged(object sender, TextChangedEventArgs e)
		{
			bool IsValid = false;
			IsValid = (Regex.IsMatch(e.NewTextValue, emailRegex, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)));
			((Editor)sender).TextColor = IsValid ? Color.Default : Color.Red;
		}

		protected override void OnDetachingFrom(Editor bindable)
		{
			bindable.TextChanged -= HandleTextChanged;
			base.OnDetachingFrom(bindable);
		}
	}
}
