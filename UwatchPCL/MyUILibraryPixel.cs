using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FFImageLoading.Forms;
using Syncfusion.ListView.XForms;
using uWatch;
using uWatch.Controls;



using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamd.ImageCarousel.Forms.Plugin.Abstractions;

namespace UwatchPCL
{
    public static class MyUILibrary
    {



        public static DatePicker AddDatePicker(RelativeLayout relativeLayout, double x, double y, double w, double h)
        {
            x = MyController.ConvertWidth(Convert.ToInt32(x));
            y = MyController.ConvertHeight(Convert.ToInt32(y));
            w = MyController.ConvertWidth(Convert.ToInt32(w));
            h = MyController.ConvertHeight(Convert.ToInt32(h));

            var lastTemp = new DatePicker
            {

                Format = "dd-MMM-yy"
            };
            relativeLayout.Children.Add(lastTemp,
                Constraint.Constant(x),
                Constraint.Constant(y),
                Constraint.Constant(w),
                Constraint.Constant(h));
            return lastTemp;
        }


        public static BoxView AddCircle(RelativeLayout relativeLayout, BoxView shape, double x, double y, double w, double h, string label, View val)
        {
            x = MyController.ConvertWidth(Convert.ToInt32(x));
            y = MyController.ConvertHeight(Convert.ToInt32(y));
            w = MyController.ConvertWidth(Convert.ToInt32(w));
            h = MyController.ConvertHeight(Convert.ToInt32(h));

            Label LabelHead = new Label();
            LabelHead.Text = label;
            LabelHead.FontSize = 20;
            LabelHead.FontAttributes = FontAttributes.Bold;
            LabelHead.BackgroundColor = Color.Transparent;
            LabelHead.TextColor = Color.Black;
            LabelHead.HorizontalOptions = LayoutOptions.Center;
            LabelHead.VerticalOptions = LayoutOptions.Center;


            relativeLayout.Children.Add(shape,
                Constraint.Constant(x),
                Constraint.Constant(y),
                Constraint.Constant(100),
                Constraint.Constant(100));

            relativeLayout.Children.Add(LabelHead,
                Constraint.RelativeToView(shape, (parent, view) =>
                {
                    return view.X - 5;
                }),
                Constraint.RelativeToView(shape, (parent, view) =>
                {
                    return view.Y + 13;
                }),
                Constraint.Constant(100),
                Constraint.Constant(40));

            relativeLayout.Children.Add(val,
                Constraint.RelativeToView(shape, (parent, view) =>
                {
                    return view.X - 4;
                }),
                Constraint.RelativeToView(LabelHead, (parent, view) =>
                {
                    return view.Y + 20;
                }),
                Constraint.Constant(100),
                Constraint.Constant(40));

            return shape;
        }


        public static ImageCarousel AddImageCarousel(RelativeLayout relativeLayout, ImageCarousel lastTemp, double x, double y, double w, double h)
        {
            x = MyController.ConvertWidth(Convert.ToInt32(x));
            y = MyController.ConvertHeight(Convert.ToInt32(y));
            w = MyController.ConvertWidth(Convert.ToInt32(w));
            h = MyController.ConvertHeight(Convert.ToInt32(h));


            relativeLayout.Children.Add(lastTemp,
                Constraint.Constant(x),
                Constraint.Constant(y),
                Constraint.Constant(w),
                Constraint.Constant(h));

            return lastTemp;
        }

        public static Map AddMap(RelativeLayout relativeLayout, Map lastTemp, double x, double y, double w, double h)
        {
            x = MyController.ConvertWidth(Convert.ToInt32(x));
            y = MyController.ConvertHeight(Convert.ToInt32(y));
            w = MyController.ConvertWidth(Convert.ToInt32(w));
            h = MyController.ConvertHeight(Convert.ToInt32(h));


            relativeLayout.Children.Add(lastTemp,
                Constraint.Constant(x),
                Constraint.Constant(y),
                Constraint.Constant(w),
                Constraint.Constant(h));

            return lastTemp;
        }

        public static Label AddLabel(RelativeLayout relativeLayout, string testo, double x, double y, double w, double h, Color colore, double fontSize)
        {
            x = MyController.ConvertWidth(Convert.ToInt32(x));
            y = MyController.ConvertHeight(Convert.ToInt32(y));
            w = MyController.ConvertWidth(Convert.ToInt32(w));
            h = MyController.ConvertHeight(Convert.ToInt32(h));

            var lastTemp = new Label
            {
                Text = testo,
                TextColor = colore,
                FontSize = fontSize,
                XAlign = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.StartAndExpand,

            };
            relativeLayout.Children.Add(lastTemp,
                Constraint.Constant(x),
                Constraint.Constant(y),
                Constraint.Constant(w),
                Constraint.Constant(h));
            return lastTemp;
        }


        public static void AddBoxView(RelativeLayout relativeLayout, double x, double y, double w, double h, Color color)
        {

            x = MyController.ConvertWidth(Convert.ToInt32(x));
            y = MyController.ConvertHeight(Convert.ToInt32(y));
            w = MyController.ConvertWidth(Convert.ToInt32(w));
            h = MyController.ConvertHeight(Convert.ToInt32(h));


            var b1 = new BoxView { BackgroundColor = color };
            relativeLayout.Children.Add(b1,
                Constraint.Constant(x),
                Constraint.Constant(y),
                Constraint.Constant(w),
                Constraint.Constant(h)
            );
        }



        public static Entry AddEntry(RelativeLayout relativeLayout, string text, string pholder, double x, double y, double w, double h, Color colore, Keyboard keyb, TextAlignment talign = TextAlignment.Start, bool ispassword = false, int fontsize = 14)
        {
            x = MyController.ConvertWidth(Convert.ToInt32(x));
            y = MyController.ConvertHeight(Convert.ToInt32(y));
            w = MyController.ConvertWidth(Convert.ToInt32(w));
            h = MyController.ConvertHeight(Convert.ToInt32(h));

            var lastTemp = new Entry
            {
                Text = text,
                TextColor = colore,
                Placeholder = pholder,
                PlaceholderColor = Color.Silver,
                Keyboard = keyb,
                IsPassword = ispassword,
                FontSize = fontsize,

            };

            relativeLayout.Children.Add(lastTemp,
                Constraint.Constant(x),
                Constraint.Constant(y),
                Constraint.Constant(w),
                Constraint.Constant(h));
            return lastTemp;
        }

        public static Editor AddEditor(RelativeLayout relativeLayout, string text, string pholder, double x, double y, double w, double h, Color colore, Keyboard keyb, TextAlignment talign = TextAlignment.Start, bool ispassword = false, int fontsize = 14)
        {
            x = MyController.ConvertWidth(Convert.ToInt32(x));
            y = MyController.ConvertHeight(Convert.ToInt32(y));
            w = MyController.ConvertWidth(Convert.ToInt32(w));
            h = MyController.ConvertHeight(Convert.ToInt32(h));

            var lastTemp = new Editor
            {
                Text = text,
                TextColor = colore,
                Keyboard = keyb,
                FontSize = fontsize,
            };

            relativeLayout.Children.Add(lastTemp,
                Constraint.Constant(x),
                Constraint.Constant(y),
                Constraint.Constant(w),
                Constraint.Constant(h));
            return lastTemp;
        }

        public static ExtendedEditor AddExtendedEditor(RelativeLayout relativeLayout, string text, string pholder, double x, double y, double w, double h, Color colore, Keyboard keyb, TextAlignment talign = TextAlignment.Start, bool ispassword = false, int fontsize = 14)
        {
            x = MyController.ConvertWidth(Convert.ToInt32(x));
            y = MyController.ConvertHeight(Convert.ToInt32(y));
            w = MyController.ConvertWidth(Convert.ToInt32(w));
            h = MyController.ConvertHeight(Convert.ToInt32(h));

            var lastTemp = new ExtendedEditor
            {
                Text = text,
                TextColor = colore,
                Keyboard = keyb,
                FontSize = fontsize,
                XAlign = talign,
                HasBorder = true,
            };

            relativeLayout.Children.Add(lastTemp,
                Constraint.Constant(x),
                Constraint.Constant(y),
                Constraint.Constant(w),
                Constraint.Constant(h));
            return lastTemp;
        }


        public static ListView AddListView(RelativeLayout relativeLayout, ListView lst, double x, double y, double w, double h, Color colore, int rowheight=100)
		{
			x = MyController.ConvertWidth (Convert.ToInt32(x));
			y = MyController.ConvertHeight (Convert.ToInt32(y));
			w = MyController.ConvertWidth (Convert.ToInt32(w));
			h = MyController.ConvertHeight (Convert.ToInt32(h));

			lst.SeparatorColor = colore;
			

			relativeLayout.Children.Add(lst,
				Constraint.Constant(x),
				Constraint.Constant(y),
				Constraint.Constant(w),
				Constraint.Constant(h));
			return lst;
		}


        public static SfListView AddSFListView(RelativeLayout relativeLayout, SfListView lst, double x, double y, double w, double h, Color colore, int rowheight = 100)
        {
            x = MyController.ConvertWidth(Convert.ToInt32(x));
            y = MyController.ConvertHeight(Convert.ToInt32(y));
            w = MyController.ConvertWidth(Convert.ToInt32(w));
            h = MyController.ConvertHeight(Convert.ToInt32(h));

           // lst.SeparatorColor = colore;


            relativeLayout.Children.Add(lst,
                Constraint.Constant(x),
                Constraint.Constant(y),
                Constraint.Constant(w),
                Constraint.Constant(h));
            return lst;
        }

		public static Button AddButton(string testo, Color bgcolor, Color bordercolor, TextAlignment textAlign = TextAlignment.Start)
		{
			return AddButton(testo, Color.White, bgcolor, bordercolor, textAlign);
		}

		public static Button AddButton(string testo, Color txtcolor, Color bgcolor, Color bordercolor, TextAlignment textAlign = TextAlignment.Start)
		{
			return new Button
			{
				BackgroundColor = bgcolor,
				TextColor = txtcolor,
				BorderColor = bordercolor,
				Text = testo,
				FontSize = Device.OnPlatform(
					Device.GetNamedSize(NamedSize.Small, typeof(Button)),
					Device.GetNamedSize(NamedSize.Micro, typeof(Button)),
					Device.GetNamedSize(NamedSize.Micro, typeof(Button))
				),

				HeightRequest = 40,
				WidthRequest = MyUiUtils.GetBtnWidth(testo, Device.OnPlatform(NamedSize.Small, NamedSize.Micro, NamedSize.Micro), 3),
			};
		}

		public static Image AddImage(RelativeLayout relativeLayout, string img, double x, double y, double w, double h, Aspect asp = Aspect.AspectFill)
		{
			x = MyController.ConvertWidth (Convert.ToInt32(x));
			y = MyController.ConvertHeight (Convert.ToInt32(y));
			w = MyController.ConvertWidth (Convert.ToInt32(w));
			h = MyController.ConvertHeight (Convert.ToInt32(h));

			var image = new Image
			{
				Aspect = asp,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.StartAndExpand,
				Source = ImageSource.FromFile(img)
			};

			relativeLayout.Children.Add(image,
				Constraint.Constant(x),
				Constraint.Constant(y),
				Constraint.Constant(w),
				Constraint.Constant(h));
			return image;
		}


		public static CachedImage AddCachedImage(RelativeLayout relativeLayout, CachedImage image, double x, double y, double w, double h, Aspect asp = Aspect.AspectFill)
		{
			x = MyController.ConvertWidth(Convert.ToInt32(x));
			y = MyController.ConvertHeight(Convert.ToInt32(y));
			w = MyController.ConvertWidth(Convert.ToInt32(w));
			h = MyController.ConvertHeight(Convert.ToInt32(h));

			relativeLayout.Children.Add(image,
				Constraint.Constant(x),
				Constraint.Constant(y),
				Constraint.Constant(w),
				Constraint.Constant(h));
			return image;
		}
		public static Frame AddnewFrame(RelativeLayout relativeLayout, Frame frm, double x, double y, double w, double h)
		{
			x = MyController.ConvertWidth(Convert.ToInt32(x));
			y = MyController.ConvertHeight(Convert.ToInt32(y));
			w = MyController.ConvertWidth(Convert.ToInt32(w));
			h = MyController.ConvertHeight(Convert.ToInt32(h));

			relativeLayout.Children.Add(frm,
				Constraint.Constant(x),
				Constraint.Constant(y),
				Constraint.Constant(w),
				Constraint.Constant(h));
			return frm;
		}
		public static ActivityIndicator AddActivityIndicator(RelativeLayout relativeLayout, ActivityIndicator ActivityIndicators, double x, double y, double w, double h, Aspect asp = Aspect.AspectFill)
		{
			x = MyController.ConvertWidth(Convert.ToInt32(x));
			y = MyController.ConvertHeight(Convert.ToInt32(y));
			w = MyController.ConvertWidth(Convert.ToInt32(w));
			h = MyController.ConvertHeight(Convert.ToInt32(h));

			relativeLayout.Children.Add(ActivityIndicators,
				Constraint.Constant(x),
				Constraint.Constant(y),
				Constraint.Constant(w),
				Constraint.Constant(h));
			return ActivityIndicators;
		}
		public static Button AddButton(RelativeLayout relativeLayout, string text, double x, double y, double w, double h, Color bgcolor, Color bordercolor, Color txtcolor, int fontsize=11)
		{
			x = MyController.ConvertWidth (Convert.ToInt32(x));
			y = MyController.ConvertHeight (Convert.ToInt32(y));
			w = MyController.ConvertWidth (Convert.ToInt32(w));
			h = MyController.ConvertHeight (Convert.ToInt32(h));

			var btn = new Button
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = text,
				BackgroundColor = bgcolor,
				TextColor = txtcolor,
				FontSize = fontsize,
				BorderColor = bordercolor,
				BorderWidth = 1,

			};


			relativeLayout.Children.Add(btn,
				Constraint.Constant(x),
				Constraint.Constant(y),
				Constraint.Constant(w),
				Constraint.Constant(h));
			return btn;
		}

		public static Frame AddFrame(RelativeLayout relativeLayout, int x, int y, Color color)
		{
			var frame = new Frame
			{
				BackgroundColor = color
			};

			relativeLayout.Children.Add(frame,
				Constraint.RelativeToParent(p => MyUiUtils.GetInWPercent(p, x)),
				Constraint.RelativeToParent(p => MyUiUtils.GetInHPercent(p, y)),
				Constraint.RelativeToParent(p => MyUiUtils.GetInWPercent(p, 100 - x * 2)),
				Constraint.RelativeToParent(p => MyUiUtils.GetInHPercent(p, 100 - y * 2)));

			return frame;
		}

		public static View AddView(RelativeLayout relativeLayout,View view,  double x, double y, double w, double h)
		{

			x = MyController.ConvertWidth(Convert.ToInt32(x));
			y = MyController.ConvertHeight(Convert.ToInt32(y));
			w = MyController.ConvertWidth(Convert.ToInt32(w));
			h = MyController.ConvertHeight(Convert.ToInt32(h));


			relativeLayout.Children.Add(view,
				Constraint.Constant(x),
				Constraint.Constant(y),
				Constraint.Constant(w),
				Constraint.Constant(h));

			return view;
		}

		public static View AddLayout(RelativeLayout relativeLayout, Layout<View> view, double x, double y, double w, double h)
		{

			x = MyController.ConvertWidth(Convert.ToInt32(x));
			y = MyController.ConvertHeight(Convert.ToInt32(y));
			w = MyController.ConvertWidth(Convert.ToInt32(w));
			h = MyController.ConvertHeight(Convert.ToInt32(h));


			relativeLayout.Children.Add(view,
				Constraint.Constant(x),
				Constraint.Constant(y),
				Constraint.Constant(w),
				Constraint.Constant(h));

			return view;
		}

	}
        
}