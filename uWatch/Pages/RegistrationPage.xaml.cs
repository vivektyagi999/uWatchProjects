using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace uWatch
{
    public partial class RegistrationPage : ContentPage
    {
        RegistrationViewModel viewModel;
        public RegistrationPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            this.BindingContext = viewModel = new RegistrationViewModel(Navigation);
            entryProductCode.TextChanged += OnTextChanged;
            entrySerialNumber.TextChanged += OnTextChanged;
            entryAccessKey.TextChanged += OnTextChanged;

        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var ctrl = sender as Entry;
            if (ctrl.Placeholder == "Access key")
            {
                if (e.NewTextValue.Length > 10)
                {

                    ctrl.Text = e.OldTextValue;
                }
            }
            else
            {
                if (e.NewTextValue.Length > 8)
                {

                    ctrl.Text = e.OldTextValue;
                }
            }
        }
    }
}
