using System;
using System.ComponentModel;
using Acr.UserDialogs;
using uWatch.ViewModels;
using UwatchPCL;
using UwatchPCL.Helpers;
using UwatchPCL.WebServices;
using Xamarin.Forms;

namespace uWatch
{
    public class ResetPasswordViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public Command _submitCommand, _cancelCommand, _confirmMessageCommand,_showPasswordCommand;
        private INavigation navigation;
        WebServiceManager webServiceManager;
        public event PropertyChangedEventHandler PropertyChanged;
        ResetPasswordModel resetModel;

        public Command SubmitCommand
        {
            get { return _submitCommand ?? (_submitCommand = new Command(() => { ExecuteSubmitCommandAsync(); })); }
        }

        public Command CancelCommand
        {
            get { return _cancelCommand ?? (_cancelCommand = new Command(() => { ExecuteCancelCommandAsync(); })); }
        }

        public Command ConfirmMessageCommand
        {
            get { return _confirmMessageCommand ?? (_confirmMessageCommand = new Command(() => { ExecuteConfirmMessageCommandAsync(); })); }
        }

        public Command ShowPasswordCommand
        {
            get { return _showPasswordCommand ?? (_showPasswordCommand = new Command(() => { ExecuteShowPasswordCommandAsync(); })); }
        }



        public string _newPassword;
        public string NewPassword
        {
            get { return _newPassword; }
            set
            {
                _newPassword = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this,
                                    new PropertyChangedEventArgs("NewPassword"));
                }

            }
        }

        public string _oldPassword;
        public string OldPassword
        {
            get { return _oldPassword; }
            set
            {
                _oldPassword = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this,
                                    new PropertyChangedEventArgs("OldPassword"));
                }

            }
        }

        public bool _isSuccess = false;
        public bool IsSuccess
        {
            get { return _isSuccess; }
            set
            {
                if (value != null)
                {
                    _isSuccess = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this,
                                        new PropertyChangedEventArgs("IsSuccess"));
                    }
                }
            }
        }

        public bool _isStrong = false;
        public bool IsStrong
        {
            get { return _isStrong; }
            set
            {
                if (value != null)
                {
                    _isStrong = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this,
                                        new PropertyChangedEventArgs("IsStrong"));
                    }
                }
            }
        }

        public bool _isPassword = true;
        public bool IsPassword
        {
            get { return _isPassword; }
            set
            {
                if (value != null)
                {
                    _isPassword = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this,
                                        new PropertyChangedEventArgs("IsPassword"));
                    }
                }
            }
        }



        public string _confirmPassword;
        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set
            {
                _confirmPassword = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this,
                                    new PropertyChangedEventArgs("ConfirmPassword"));
                }

            }
        }

        private void ExecuteShowPasswordCommandAsync()
        {
            if (IsPassword)
                IsPassword = false;
            else
                IsPassword = true;
        }

        private void ExecuteConfirmMessageCommandAsync()
        {
            Application.Current.MainPage = new Login();
        }

        private void ExecuteCancelCommandAsync()
        {
            navigation.PopModalAsync();
        }

        private void ExecuteSubmitCommandAsync()
        {
            var networkconnection = DependencyService.Get<INetworkConnection>();
            networkconnection.CheckNetworkConnection();
            var networkStatus = networkconnection.IsConnected ? "Connected" : "Not Connected";
            if (networkStatus == "Not Connected")
            {

                UserDialogs.Instance.Alert("Please check your internet connection", "Alert", "Ok");
                return;
            }
            string message;
            if (!ValidateRegister(out message))
            {
                if(message=="Please provide a strong password")
                {
                    UserDialogs.Instance.Alert(message, "Weak password", "OK");
                }
                else
                {
                    UserDialogs.Instance.Alert(message, "Alert", "OK");
                }
                return;
            }
            else
            {
                resetModel.NewPassword = NewPassword;
                resetModel.ConfirmPassword = ConfirmPassword;
                resetModel.OldPassword = OldPassword;
            }
            UserDialogs.Instance.ShowLoading();
            Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await webServiceManager.ResetPasswordAsync(resetModel);
                if (result.ErrorCode == 0)
                {
                    if (result.Data)
                    {
                        IsSuccess = true;
                        Settings.UserName =resetModel.UserName;
                        Settings.Password = resetModel.ConfirmPassword;
                    }
                    UserDialogs.Instance.HideLoading();
                }
                else if (result.ErrorCode > 0)
                {
                    UserDialogs.Instance.Alert(result.ErrorMessage, "Alert", "Ok");
                    UserDialogs.Instance.HideLoading();
                }
                else if (result.ErrorCode < 0)
                {
                    UserDialogs.Instance.Alert(result.ErrorMessage, "Alert", "Ok");
                    UserDialogs.Instance.HideLoading();
                }
                else
                {
                    UserDialogs.Instance.Alert(result.ErrorMessage, "Alert", "Ok");
                    UserDialogs.Instance.HideLoading();
                }
            });
        }

        public ResetPasswordViewModel(INavigation navigation, string userName, string oldPassword)
        {
            this.navigation = navigation;
            webServiceManager = new WebServiceManager();
            resetModel = new ResetPasswordModel();
            resetModel.UserName = userName;
            OldPassword = oldPassword;
        }

        private bool ValidateRegister(out string message)
        {
            message = null;
            if (string.IsNullOrEmpty(OldPassword))
                message = "Old Password field is required";
            else if (string.IsNullOrEmpty(NewPassword))
                message = "New Password field is required";
            else if (string.IsNullOrEmpty(ConfirmPassword))
                message = "Repeat Password field is required";
            else if (!IsStrong)
                message = "Please provide a strong password";
            else if (NewPassword.Length < 6)
                message = "Please provide minimum six digit new password";
            else if (!NewPassword.Equals(ConfirmPassword))
                message = "New Password and Confirm Password does not match.";
            return string.IsNullOrEmpty(message);
        }

    }
}
      