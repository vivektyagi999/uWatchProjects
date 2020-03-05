using System;
using Android.Content;
using Android.App;

namespace uWatch.Droid
{
	internal static class MessagingExtensions
	{
		#region Methods

		public static void StartNewActivity(this Intent intent)
		{
			if (intent == null)
				throw new ArgumentNullException(nameof(intent));

			intent.SetFlags(ActivityFlags.ClearTop);
			intent.SetFlags(ActivityFlags.NewTask);

			Application.Context.StartActivity(intent);
		}

		#endregion
	}

}
