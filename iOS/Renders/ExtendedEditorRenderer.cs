using System;
using Foundation;
using UIKit;
using uWatch.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
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
		/// The _left swipe gesture recognizer
		/// </summary>
		private UISwipeGestureRecognizer _leftSwipeGestureRecognizer;
		/// <summary>
		/// The _right swipe gesture recognizer
		/// </summary>
		private UISwipeGestureRecognizer _rightSwipeGestureRecognizer;

		/// <summary>
		/// Initializes a new instance of the <see cref="ExtendedEditorRenderer"/> class.
		/// </summary>
		public ExtendedEditorRenderer()
		{
		}

		/// <summary>
		/// Called when [element changed].
		/// </summary>
		/// <param name="e">The e.</param>
		/// 
		/// 
		/// 
		/// 
		protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
		{
			base.OnElementChanged(e);

			var view = (ExtendedEditor)Element;
			if (view != null)
			{
				SetFont(view);
				SetTextAlignment(view);
				SetBorder(view);
				//SetPlaceholderTextColor(view);
				//SetMaxLength(view);

				//ResizeHeight();
			}

			if (e.OldElement == null)
			{
				_leftSwipeGestureRecognizer = new UISwipeGestureRecognizer(() => view.OnLeftSwipe(this, EventArgs.Empty))
				{
					Direction = UISwipeGestureRecognizerDirection.Left
				};

				_rightSwipeGestureRecognizer = new UISwipeGestureRecognizer(() => view.OnRightSwipe(this, EventArgs.Empty))
				{
					Direction = UISwipeGestureRecognizerDirection.Right
				};

				Control.AddGestureRecognizer(_leftSwipeGestureRecognizer);
				Control.AddGestureRecognizer(_rightSwipeGestureRecognizer);
			}

			if (e.NewElement == null)
			{
				Control.RemoveGestureRecognizer(_leftSwipeGestureRecognizer);
				Control.RemoveGestureRecognizer(_rightSwipeGestureRecognizer);
			}
		}

		/// <summary>
		/// Sets the text alignment.
		/// </summary>
		/// <param name="view">The view.</param>
		private void SetTextAlignment(ExtendedEditor view)
		{
			//switch (view.XAlign)
			//{
			//	case TextAlignment.Center:
			//		Control.TextAlignment = UITextAlignment.Center;
			//		break;
			//	case TextAlignment.End:
			//		Control.TextAlignment = UITextAlignment.Right;
			//		break;
			//	case TextAlignment.Start:
			Control.TextAlignment = UITextAlignment.Left;
			//Control.VerticalAlignment = UIControlContentVerticalAlignment.Fill;
			//Control.HorizontalAlignment = UIControlContentHorizontalAlignment.Fill;
			//break;
			//}
		}

		/// <summary>
		/// Sets the font.
		/// </summary>
		/// <param name="view">The view.</param>
		private void SetFont(ExtendedEditor view)
		{
			UIFont uiFont;
			if (view.Font != Font.Default && (uiFont = view.Font.ToUIFont()) != null)
				Control.Font = uiFont;
			else if (view.Font == Font.Default)
				Control.Font = UIFont.SystemFontOfSize(17f);
		}

		/// <summary>
		/// Sets the border.
		/// </summary>
		/// <param name="view">The view.</param>
		private void SetBorder(ExtendedEditor view)
		{
			Control.Layer.CornerRadius = 10f;
			Control.Layer.BorderWidth = 0.8f;
			Control.Layer.BorderColor = UIColor.LightGray.CGColor;

			//Control.Layer.bo = view.HasBorder ? UITextBorderStyle.RoundedRect : UITextBorderStyle.None;
		}

		///// <summary>
		///// Sets the maxLength.
		///// </summary>
		///// <param name="view">The view.</param>
		//private void SetMaxLength(ExtendedEditor view)
		//{
		//	Control.ShouldChangeCharacters = (textField, range, replacementString) =>
		//	{
		//		var newLength = textField.Text.Length + replacementString.Length - range.Length;
		//		return newLength <= view.MaxLength;
		//	};
		//}
	}
}
