using System;
using System.Collections.Generic;
using UwatchPCL;
using Xamarin.Forms;

namespace uWatch
{
    public partial class RegistrationUpdatePage : ContentPage
    {
        void Handle_Clicked(object sender, System.EventArgs e)
        {

        }

        RegistrationUpdateViewModel viewModel;
        private AccessCodeModel accessCodeModel;

        public RegistrationUpdatePage()
        {

            InitializeComponent();

        }


        public RegistrationUpdatePage(AccessCodeModel accessCodeModel)
        {
            InitializeComponent();
            entryDate.Focused += OnDateFocused;
            entryMonth.Focused += OnMonthFocused;
            entryYear.Focused += OnYearFocused;

            this.BindingContext = viewModel = new RegistrationUpdateViewModel(Navigation, accessCodeModel);
            this.accessCodeModel = accessCodeModel;
        }

        private void OnDateFocused(object sender, FocusEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                entryDate.Unfocus();
                this.pkrDate.Focus();
            });
        }

        private void OnMonthFocused(object sender, FocusEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                entryMonth.Unfocus();
                this.pkrMonth.Focus();

            });
        }

        private void OnYearFocused(object sender, FocusEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {

                entryYear.Unfocus();
                this.pkrYear.Focus();

            });
        }

        //void OnYearFocused(object sender, System.EventArgs e)
        //{
        //    Device.BeginInvokeOnMainThread(() =>
        //    {
        //        this.pkrYear.Focus();

        //    });
        //}

        //async void OnDateFocused(object sender, System.EventArgs e)
        //{
        //    //Device.BeginInvokeOnMainThread(() =>
        //    //{
        //        //this.pkrDate.Focus();

        //    //});
        //}

        //void OnMonthFocused(object sender, System.EventArgs e)
        //{
        //    Device.BeginInvokeOnMainThread(() =>
        //    {
        //        this.pkrMonth.Focus();

        //    });
        //}


    }
}
