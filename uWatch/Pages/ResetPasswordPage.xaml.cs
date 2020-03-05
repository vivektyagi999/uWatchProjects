using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace uWatch
{
    public partial class ResetPasswordPage : ContentPage
    {
        ResetPasswordViewModel viewModel;
        private string userName;

        public ResetPasswordPage()
        {
            InitializeComponent();
        }

        public ResetPasswordPage(string userName, string oldPassword)
        {
            InitializeComponent();
            this.BindingContext = viewModel = new ResetPasswordViewModel(Navigation,userName,oldPassword);
            this.userName = userName;
          //  lblCheck.SetBinding(Label.TextColorProperty, (ResetPasswordViewModel p) => p.IsStrong, converter: new BooleanToColorConverter(Color.Green, Color.Transparent));

            entryNewPwd.TextChanged += OnTextChanged;
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            viewModel.IsStrong = false;
            int passChar=0;
            if ((Regex.Match(e.NewTextValue, @"[a-z]|[A-Z]").Success))
            {
                passChar += 1;
            }
            if((Regex.Match(e.NewTextValue, @"[0-9]").Success))
            {
                passChar += 1;
            }
            if((Regex.Match(e.NewTextValue, @"[!@#$%^&*?_~]").Success))
            {
                passChar += 1;
            }
            if(passChar>=2)
            {
                viewModel.IsStrong = true;
            }
        }
    }
}
