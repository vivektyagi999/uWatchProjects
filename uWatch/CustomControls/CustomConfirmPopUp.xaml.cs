using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace uWatch
{
    public partial class CustomConfirmPopUp : ContentView
    {
        public CustomConfirmPopUp()
        {
            InitializeComponent();

        }
        public static readonly BindableProperty TitleProperty =
            BindableProperty.Create("Title", typeof(string), typeof(CustomConfirmPopUp), null, propertyChanged: TitlePropertyChanged);

        public static readonly BindableProperty BodyTextProperty =
            BindableProperty.Create("BodyText", typeof(string), typeof(CustomConfirmPopUp), null, propertyChanged: BodyTextPropertyChanged);

        public string Title
        {
            get { return (string)this.GetValue(TitleProperty); }
            set { this.SetValue(TitleProperty, value); }
        }

        public string BodyText
        {
            get { return (string)this.GetValue(BodyTextProperty); }
            set { this.SetValue(BodyTextProperty, value); }
        }
        private static void TitlePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as CustomConfirmPopUp;
            view.txtTitle.Text = newValue.ToString();
        }
        private static void BodyTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as CustomConfirmPopUp;
            view.txtBodyText.Text = newValue.ToString();
        }


        public static readonly BindableProperty ConfirmTextProperty =
            BindableProperty.Create("ConfirmText", typeof(string), typeof(CustomConfirmPopUp), null, propertyChanged: ConfirmTextPropertyChanged);

        public static readonly BindableProperty CancelTextProperty =
            BindableProperty.Create("CancelText", typeof(string), typeof(CustomConfirmPopUp), null, propertyChanged: CancelTextPropertyChanged);

        public string ConfirmText
        {
            get { return (string)this.GetValue(ConfirmTextProperty); }
            set { this.SetValue(ConfirmTextProperty, value); }
        }

        public string CancelText
        {
            get { return (string)this.GetValue(CancelTextProperty); }
            set { this.SetValue(CancelTextProperty, value); }
        }
        private static void ConfirmTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as CustomConfirmPopUp;
            view.btnConfirm.Text = newValue.ToString();
        }
        private static void CancelTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as CustomConfirmPopUp;
            view.btnCancel.Text = newValue.ToString();
        }


    }
}
