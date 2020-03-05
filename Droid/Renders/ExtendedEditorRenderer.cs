using System;
using Android.Graphics.Drawables;
using Android.Text.Method;
using Android.Views;
using uWatch.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(ExtendedEditor), typeof(ExtendedEditorRenderer))]

namespace XLabs.Forms.Controls
{
	/// <summary>
	/// Class ExtendedEditorRenderer.
	/// </summary>
	public class ExtendedEditorRenderer : EditorRenderer
	{
		/// <summary>
		/// The mi n_ distance
		/// </summary>
		private const int MIN_DISTANCE = 10;
		/// <summary>
		/// The _down x
		/// </summary>
		private float _downX, _downY, _upX, _upY;

		/// <summary>
		/// Called when [element changed].
		/// </summary>
		/// <param name="e">The e.</param>
		protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
		{
			base.OnElementChanged(e);

			var view = (ExtendedEditor)Element;
			SetBorder(view);


			if (e.NewElement == null)
			{
				this.Touch -= HandleTouch;
			}

			if (e.OldElement == null)
			{
				this.Touch += HandleTouch;
			}
            var nativeEditText = (global::Android.Widget.EditText)Control;

			nativeEditText.OverScrollMode = OverScrollMode.Always;
			nativeEditText.ScrollBarStyle = ScrollbarStyles.InsideInset;
			nativeEditText.SetOnTouchListener(new DroidTouchListener());

			//For Scrolling in Editor innner area
			Control.VerticalScrollBarEnabled = true;
			Control.MovementMethod = ScrollingMovementMethod.Instance;
			Control.ScrollBarStyle = Android.Views.ScrollbarStyles.InsideInset;
           // Control.SetPadding(0, 0, 0, 5);
			//Force scrollbars to be displayed
			Android.Content.Res.TypedArray a = Control.Context.Theme.ObtainStyledAttributes(new int[0]);
			InitializeScrollbars(a);
			a.Recycle();
		}

		/// <summary>
		/// Sets the border.
		/// </summary>
		/// <param name="view">The view.</param>
		private void SetBorder(ExtendedEditor view)
		{
			GradientDrawable gd = new GradientDrawable();
			gd.SetColor(Color.White.ToAndroid());
			gd.SetCornerRadius(10);
			gd.SetStroke(2, Color.Gray.ToAndroid());
			this.Control.SetBackgroundDrawable(gd);
		}


		/// <summary>
		/// Handles the touch.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="Android.Views.View.TouchEventArgs"/> instance containing the event data.</param>
		void HandleTouch(object sender, Android.Views.View.TouchEventArgs e)
		{
			var element = this.Element as ExtendedEditor;
			switch (e.Event.Action)
			{
				case MotionEventActions.Down:
					_downX = e.Event.GetX();
					_downY = e.Event.GetY();
					return;
				case MotionEventActions.Up:
				case MotionEventActions.Cancel:
				case MotionEventActions.Move:
					_upX = e.Event.GetX();
					_upY = e.Event.GetY();

					float deltaX = _downX - _upX;
					float deltaY = _downY - _upY;

					// swipe horizontal?
					if (Math.Abs(deltaX) > Math.Abs(deltaY))
					{
						if (Math.Abs(deltaX) > MIN_DISTANCE)
						{
							// left or right
							if (deltaX < 0) { element.OnRightSwipe(this, EventArgs.Empty); return; }
							if (deltaX > 0) { element.OnLeftSwipe(this, EventArgs.Empty); return; }
						}
						else
						{
							Android.Util.Log.Info("ExtendedEntry", "Horizontal Swipe was only " + Math.Abs(deltaX) + " long, need at least " + MIN_DISTANCE);
							return; // We don't consume the event
						}
					}
				
					return;
			}
		}

	}
    public class DroidTouchListener : Java.Lang.Object, global::Android.Views.View.IOnTouchListener
	{
		

        public bool OnTouch(Android.Views.View v, MotionEvent e)
        {
			v.Parent?.RequestDisallowInterceptTouchEvent(true);
			//if ((e.Action & MotionEventActions.Up) != 0 && (e.ActionMasked & MotionEventActions.Up) != 0)
			//{
			//	v.Parent?.RequestDisallowInterceptTouchEvent(false);
			//}
			return false;
        }
    }
}
