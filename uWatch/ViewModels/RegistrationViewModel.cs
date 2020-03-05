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
    public class RegistrationViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public Command _nextCommand, _cancelCommand;
        private INavigation navigation;
        WebServiceManager webServiceManager;
        AccessCodeModel accessCodeModel;
        public event PropertyChangedEventHandler PropertyChanged;

        public Command NextCommand
        {
            get { return _nextCommand ?? (_nextCommand = new Command(() => { ExecuteNextCommandAsync(); })); }
        }

        public Command CancelCommand
        {
            get { return _cancelCommand ?? (_cancelCommand = new Command(() => { ExecuteCancelCommandAsync(); })); }
        }

        private void ExecuteCancelCommandAsync()
        {
            navigation.PopModalAsync();
        }


        public string _accessCode;
        public string AccessCode
        {
            get { return _accessCode; }
            set
            {
                _accessCode = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this,
                                    new PropertyChangedEventArgs("AccessCode"));
                }
                if (string.IsNullOrEmpty(AccessCode))
                {
                    IsAccessKey = true;
                    IsProductKey = true;
                    WarningText = "";
                }
                else
                {
                    WarningText = "If you want to fill product code and serial number then remove access key.";
                    IsProductKey = false;
                }
            }
        }

        public string _productCode;
        public string ProductCode
        {
            get { return _productCode; }
            set
            {
                _productCode = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this,
                                    new PropertyChangedEventArgs("ProductCode"));
                }
                if ((string.IsNullOrEmpty(ProductCode) && string.IsNullOrEmpty(SerialNumber)))
                {
                    IsAccessKey = true;
                    IsProductKey = true;
                    WarningText = "";
                }
                else
                {
                    WarningText = "If you want to fill access key then remove product code and serial number.";
                    IsAccessKey = false;
                }
            }
        }

        public string _serialNumber;
        public string SerialNumber
        {
            get { return _serialNumber; }
            set
            {
                _serialNumber = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this,
                                    new PropertyChangedEventArgs("SerialNumber"));
                }
                if ((string.IsNullOrEmpty(SerialNumber) && string.IsNullOrEmpty(ProductCode)))
                {
                    IsAccessKey = true;
                    IsProductKey = true;
                    WarningText = "";
                }
                else
                {
                    WarningText = "If you want to fill access key then remove product code and serial number.";
                    IsAccessKey = false;
                }
            }
        }

        public string _warningText = "";
        public string WarningText
        {
            get
            {
                if (string.IsNullOrEmpty(_warningText))
                {
                    return _warningText;
                }
                else
                {
                    return ("* " + _warningText);
                }

            }
            set
            {
                _warningText = value;
                if (PropertyChanged != null)
                {

                    PropertyChanged(this,
                                    new PropertyChangedEventArgs("WarningText"));
                }
            }
        }


        public bool _isAccessKey = true;
        public bool IsAccessKey
        {
            get { return _isAccessKey; }
            set
            {
                _isAccessKey = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this,
                                    new PropertyChangedEventArgs("IsAccessKey"));
                }
            }
        }

        public bool _isProductKey = true;
        public bool IsProductKey
        {
            get { return _isProductKey; }
            set
            {
                _isProductKey = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this,
                                    new PropertyChangedEventArgs("IsProductKey"));
                }
            }
        }

        private async void ExecuteNextCommandAsync()
        {
            try
            {
                if ((string.IsNullOrEmpty(ProductCode) && string.IsNullOrEmpty(SerialNumber)) && string.IsNullOrEmpty(AccessCode))
                {
                    UserDialogs.Instance.Alert("Please fill access key or product code and serial number", "Alert", "Ok");
                    return;
                }
                var networkconnection = DependencyService.Get<INetworkConnection>();
                networkconnection.CheckNetworkConnection();
                var networkStatus = networkconnection.IsConnected ? "Connected" : "Not Connected";
                if (networkStatus == "Not Connected")
                {

                    UserDialogs.Instance.Alert("Please check your internet connection", "Alert", "Ok");
                    return;
                }

                UserDialogs.Instance.ShowLoading();

                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (string.IsNullOrEmpty(AccessCode))
                    {
                        var result = await webServiceManager.GetUserDetailsByProductCodeAsync(SerialNumber, ProductCode);
                        if (result.ErrorCode == 0)
                        {
                            if (result.Data != -1)
                            {
                                if (result.Data == (int)STATE_CODE.LIVE)
                                {
                                    UserDialogs.Instance.Alert("Product already registered, please Log in.", "Alert", "OK");
                                }
                                else if (result.Data == (int)STATE_CODE.STOCK || result.Data == (int)STATE_CODE.SOLD)
                                {
                                    accessCodeModel.model_no = ProductCode;
                                    accessCodeModel.serial_no = SerialNumber;
                                    navigation.PushModalAsync(new RegistrationUpdatePage(accessCodeModel), true);
                                }
                                else
                                {
                                    UserDialogs.Instance.Alert("Incorrect product code or serial number.", "Alert", "OK");

                                }
                            }
                            else
                            {
                                UserDialogs.Instance.Alert("Incorrect product code or serial number.", "Alert", "OK");
                            }
                            UserDialogs.Instance.HideLoading();
                        }
                        else if (result.ErrorCode > 0)
                        {
                            UserDialogs.Instance.Alert(result.ErrorMessage, "Alert", "OK");
                            UserDialogs.Instance.HideLoading();
                        }
                        else if (result.ErrorCode < 0)
                        {
                            UserDialogs.Instance.Alert(result.ErrorMessage, "Alert", "OK");
                            UserDialogs.Instance.HideLoading();
                        }
                        else
                        {
                            UserDialogs.Instance.Alert(result.ErrorMessage, "Alert", "OK");
                            UserDialogs.Instance.HideLoading();
                        }
                    }
                    else
                    {
                        var result = await webServiceManager.GetUserDetailsByAccessCodeAsync(AccessCode);
                        if (result.ErrorCode == 0)
                        {
                            if (result.Data != null)
                            {
                                if (result.Data.RoleId == (int)UserRole.uARM)
                                {
                                    accessCodeModel = result.Data;
                                    navigation.PushModalAsync(new RegistrationUpdatePage(accessCodeModel), true);
                                }
                                else
                                {
                                    UserDialogs.Instance.Alert("Incorrect Access Key", "Alert", "OK");
                                }
                            }
                            else
                            {
                                UserDialogs.Instance.Alert("Incorrect Access Key", "Alert", "OK");
                            }
                            UserDialogs.Instance.HideLoading();
                        }
                        else if (result.ErrorCode > 0)
                        {
                            UserDialogs.Instance.Alert(result.ErrorMessage, "Alert", "OK");
                            UserDialogs.Instance.HideLoading();
                        }
                        else if (result.ErrorCode < 0)
                        {
                            UserDialogs.Instance.Alert(result.ErrorMessage, "Alert", "OK");
                            UserDialogs.Instance.HideLoading();
                        }
                        else
                        {
                            UserDialogs.Instance.Alert(result.ErrorMessage, "Alert", "OK");
                            UserDialogs.Instance.HideLoading();
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        public RegistrationViewModel(INavigation navigation)
        {
            try
            {
                webServiceManager = new WebServiceManager();
                this.navigation = navigation;
                accessCodeModel = new AccessCodeModel();
            }
            catch (Exception ex)
            {

            }

        }
    }
}
