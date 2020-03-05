using System;
using Xamarin.Forms;
using System.Threading.Tasks;


namespace uWatch
{

	// custom popup for both android and ios
	public class CustomPopup : RelativeLayout
	{
		private View _content;
		private View _popup;
		private RelativeLayout _backdrop;

		public View Content {
			get { return _content; }

			set {
				if (_content != null) {
					Children.Remove (this._content);
				}

				_content = value;
				Children.Add (this._content, () => this.Bounds);
			}
		}

		public bool IsPopupActive {
			get { return _popup != null; }
		}

		public async Task<bool> ShowPopup (View popupView)
		{
			this.DismissPopup ();
			this._popup = popupView;

			this._content.InputTransparent = true;
			var backdrop = new RelativeLayout {
				VerticalOptions= LayoutOptions.FillAndExpand,
				BackgroundColor = Color.FromRgba (0, 0, 0, 0.4),
				Opacity = 1,

			};
			

			backdrop.Children.Add (_popup,
				Constraint.RelativeToParent (p => (this.Width / 2) - (this._popup.WidthRequest / 2)),
				Constraint.RelativeToParent (p => (this.Height / 2) - (this._popup.HeightRequest / 2)),
				Constraint.RelativeToParent (p => this._popup.WidthRequest),
				Constraint.RelativeToParent (p => this._popup.HeightRequest));

			this._backdrop = backdrop;

			Children.Add (backdrop,
				Constraint.Constant (0),
				Constraint.Constant (0),
				Constraint.RelativeToParent (p => p.Width),
				Constraint.RelativeToParent (p => p.Height)
			);

			
			this.UpdateChildrenLayout ();

			return await _backdrop.FadeTo (1);
		}

		public async Task DismissPopup ()
		{
			if (this._popup != null) {
				await Task.WhenAll (_popup.FadeTo (0), _backdrop.FadeTo (0));

				_backdrop.Children.Remove (_popup);
				this.Children.Remove (_backdrop);
				this._popup = null;
				
			}

			this._content.InputTransparent = false;
		}
	}
}
