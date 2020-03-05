using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Acr.UserDialogs;
using uWatch.ViewModels;
using UwatchPCL;
using UwatchPCL.Helpers;
using UwatchPCL.WebServices;
using Xamarin.Forms;

namespace uWatch
{
    public class RegistrationUpdateViewModel : BaseViewModel, INotifyPropertyChanged
    {

        #region GlobalVariables

        public Command _nextCommand, _cancelRegistrationCommand, _confirmMessageCommand, _confirmCommand, _cancelCommand, _confirmMacCommand, _closeMacCommand;
        private INavigation navigation;
        WebServiceManager webServiceManager;
        SignUpModel userDetail;
        bool isDateSelected = false;
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

        public ObservableCollection<string> _pickerDate;
        public ObservableCollection<string> PickerDate
        {
            get { return _pickerDate; }
            set
            {
                _pickerDate = value;

                if (PropertyChanged != null)
                {
                    PropertyChanged(this,
                                    new PropertyChangedEventArgs("PickerDate"));
                }

            }
        }

        public ObservableCollection<string> _pickerMonth;
        public ObservableCollection<string> PickerMonth
        {
            get { return _pickerMonth; }
            set
            {
                _pickerMonth = value;

                if (PropertyChanged != null)
                {
                    PropertyChanged(this,
                                    new PropertyChangedEventArgs("PickerMonth"));
                }

            }
        }


        public ObservableCollection<int> _pickerYear;
        public ObservableCollection<int> PickerYear
        {
            get { return _pickerYear; }
            set
            {
                _pickerYear = value;

                if (PropertyChanged != null)
                {
                    PropertyChanged(this,
                                    new PropertyChangedEventArgs("PickerYear"));
                }

            }
        }



        public int _selectedDateIndex = -1;
        public int SelectedDateIndex
        {
            get { return _selectedDateIndex; }
            set
            {
                _selectedDateIndex = value;
                if (_selectedDateIndex != -1)
                {
                    SelectedDate = PickerDate[_selectedDateIndex].ToString();
                }
                if (PropertyChanged != null)
                {
                    PropertyChanged(this,
                                    new PropertyChangedEventArgs("SelectedDateIndex"));
                }

            }
        }

        public int _selectedYearIndex = -1;
        public int SelectedYearIndex
        {
            get { return _selectedYearIndex; }
            set
            {
                _selectedYearIndex = value;
                if (_selectedYearIndex != -1)
                {
                    SelectedYear = PickerYear[_selectedYearIndex].ToString();
                }
                if (PropertyChanged != null)
                {
                    PropertyChanged(this,
                                    new PropertyChangedEventArgs("SelectedYearIndex"));
                }

            }
        }

        public int _selectedMonthIndex = -1;
        public int SelectedMonthIndex
        {
            get { return _selectedMonthIndex; }
            set
            {
                _selectedMonthIndex = value;
                if (_selectedMonthIndex != -1)
                {
                    SelectedMonth = PickerMonth[_selectedMonthIndex].ToString();
                }
                if (PropertyChanged != null)
                {
                    PropertyChanged(this,
                                    new PropertyChangedEventArgs("SelectedMonthIndex"));
                }

            }
        }

        public string _selectedDate;
        public string SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                _selectedDate = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this,
                                    new PropertyChangedEventArgs("SelectedDate"));
                }

            }
        }

        public string _selectedMonth;
        public string SelectedMonth
        {
            get { return _selectedMonth; }
            set
            {
                _selectedMonth = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this,
                                    new PropertyChangedEventArgs("SelectedMonth"));
                }

            }
        }

        public string _selectedYear;
        public string SelectedYear
        {
            get { return _selectedYear; }
            set
            {
                _selectedYear = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this,
                                    new PropertyChangedEventArgs("SelectedYear"));
                }

            }
        }

        public string _fullName;
        public string FullName
        {
            get { return _fullName; }
            set
            {
                _fullName = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this,
                                    new PropertyChangedEventArgs("FullName"));
                }

            }
        }



        public string _emailAddress;
        public string EmailAddress
        {
            get { return _emailAddress; }
            set
            {
                _emailAddress = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this,
                                    new PropertyChangedEventArgs("EmailAddress"));
                }

            }
        }

        public string _confirmEmailAddress;
        public string ConfirmEmailAddress
        {
            get { return _confirmEmailAddress; }
            set
            {
                _confirmEmailAddress = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this,
                                    new PropertyChangedEventArgs("ConfirmEmailAddress"));
                }

            }
        }

        public string _stdob;
        public string StDOB
        {
            get { return _stdob; }
            set
            {
                if (value != null)
                {
                    _stdob = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this,
                                        new PropertyChangedEventArgs("StDOB"));
                    }
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

        public bool _isMacAddress = false;
        public bool IsMacAddress
        {
            get { return _isMacAddress; }
            set
            {
                if (value != null)
                {
                    _isMacAddress = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this,
                                        new PropertyChangedEventArgs("IsMacAddress"));
                    }
                }
            }
        }



        public string _bodyText;
        public string BodyText
        {
            get { return _bodyText; }
            set
            {
                if (value != null)
                {
                    _bodyText = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this,
                                        new PropertyChangedEventArgs("BodyText"));
                    }
                }
            }
        }

        //public DateTime _dob;
        //public DateTime DOB
        //{
        //    get { return _dob; }
        //    set
        //    {
        //        if (value != null)
        //        {
        //            _dob = value;
        //            StDOB = _dob.ToString("dd-MMM-yyy");
        //            if (PropertyChanged != null)
        //            {
        //                PropertyChanged(this,
        //                                new PropertyChangedEventArgs("DOB"));
        //            }
        //        }
        //    }
        //}

        public string _dob;
        public string DOB
        {
            get { return _dob; }
            set
            {
                if (value != null)
                {
                    _dob = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this,
                                        new PropertyChangedEventArgs("DOB"));
                    }
                }
            }
        }

        public string _macAddress;
        public string MacAddress
        {
            get { return _macAddress; }
            set
            {
                if (value != null)
                {
                    if(value.Length<=12)
                    {
                        _macAddress = value;

                    }
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this,
                                        new PropertyChangedEventArgs("MacAddress"));
                    }
                   
                }
            }
        }

        public bool _isConfirmPopUp = false;
        public bool IsConfirmPopUp
        {
            get { return _isConfirmPopUp; }
            set
            {
                if (value != null)
                {
                    _isConfirmPopUp = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this,
                                        new PropertyChangedEventArgs("IsConfirmPopUp"));
                    }
                }
            }
        }

        #endregion

        #region Commands

        public Command NextCommand
        {
            get { return _nextCommand ?? (_nextCommand = new Command(() => { ExecuteNextCommandAsync(); })); }
        }

        public Command CancelRegistrationCommand
        {
            get { return _cancelRegistrationCommand ?? (_cancelRegistrationCommand = new Command(() => { ExecuteCancelRegistrationCommandAsync(); })); }
        }

        public Command CancelCommand
        {
            get { return _cancelCommand ?? (_cancelCommand = new Command(() => { ExecuteCancelCommandAsync(); })); }
        }

        public Command ConfirmCommand
        {
            get { return _confirmCommand ?? (_confirmCommand = new Command(() => { ExecuteConfirmCommandAsync(); })); }
        }

        public Command ConfirmMessageCommand
        {
            get { return _confirmMessageCommand ?? (_confirmMessageCommand = new Command(() => { ExecuteConfirmMessageCommandAsync(); })); }
        }

        public Command ConfirmMacCommand
        {
            get { return _confirmMacCommand ?? (_confirmMacCommand = new Command(() => { ExecuteConfirmMacCommandAsync(); })); }
        }

        public Command CloseMacCommand
        {
            get { return _closeMacCommand ?? (_closeMacCommand = new Command(() => { ExecuteCloseMacCommandAsync(); })); }
        }      

        #endregion

        #region Methods

        private void ExecuteConfirmMacCommandAsync()
        {
            if(string.IsNullOrEmpty(MacAddress))
            {
                UserDialogs.Instance.Alert("please enter mac address", "Mac Address", "Ok");
                return;
            }
            else
            {
              //  userDetail.MacAddress = MacAddress;
            }
        }

        private void ExecuteCloseMacCommandAsync()
        {
            IsMacAddress = false;
        }

        private void ExecuteCancelCommandAsync()
        {
            IsConfirmPopUp = false;
            SignUpCompleteAsync();
        }

        private void ExecuteConfirmCommandAsync()
        {
            IsConfirmPopUp = false;
            IsMacAddress = true;
        }

        private void ExecuteConfirmMessageCommandAsync()
        {
            Application.Current.MainPage = new Login();
        }

        private void ExecuteCancelRegistrationCommandAsync()
        {
            navigation.PopModalAsync();
        }


        private async void ExecuteNextCommandAsync()
        {
            try
            {

                var networkconnection = DependencyService.Get<INetworkConnection>();
                networkconnection.CheckNetworkConnection();
                var networkStatus = networkconnection.IsConnected ? "Connected" : "Not Connected";
                if (networkStatus == "Not Connected")
                {

                    UserDialogs.Instance.Alert("Please check your internet connection", "Alert", "Ok");
                    return;
                }
                if (string.IsNullOrEmpty(EmailAddress) || string.IsNullOrEmpty(SelectedDate) || string.IsNullOrEmpty(SelectedMonth) || string.IsNullOrEmpty(SelectedYear) || string.IsNullOrEmpty(FullName))
                {
                    UserDialogs.Instance.Alert("please fill all details", "Alert", "Ok");
                    return;
                }
                if (!(Regex.Match(EmailAddress, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").Success))
                {
                    UserDialogs.Instance.Alert("Incorrect Email", "Alert", "Ok");
                    return;
                }
                if (!EmailAddress.Equals(ConfirmEmailAddress))
                {
                    UserDialogs.Instance.Alert("Email Address and Confirm Email Address does not match.", "Alert", "Ok");
                    return;
                }

                DOB = SelectedMonth + "-" + SelectedDate + "-" + SelectedYear;
                DateTime a = System.DateTime.Now;
                var checkDate = DateTime.TryParse(DOB, out a);
                if (!checkDate)
                {
                    UserDialogs.Instance.Alert("Incorrect Date of birth", "Alert", "Ok");
                    return;
                }

                userDetail.strDOB = DOB;
                userDetail.Email = EmailAddress;
                userDetail.FullName = FullName;
                //if (string.IsNullOrEmpty(userDetail.AccessCode))
                //{
                //    IsConfirmPopUp = true;
                //}
                //else
                //{
                    SignUpCompleteAsync();
                //}

            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        public void SignUpCompleteAsync()
        {
            try
            {
                UserDialogs.Instance.ShowLoading();
                Device.BeginInvokeOnMainThread(async () =>
                {
                    var result = await webServiceManager.SignUpForAppAsync(userDetail);
                    if (result.ErrorCode == 0)
                    {
                        if (result.IsSuccess)
                        {
                            IsSuccess = true;
                            if (result.Data != null)
                            {
                                Settings.IsRememberMe = true;
                                Settings.UserName = result.Data.UserName;
                                Settings.Password = result.Data.Password;
                            }
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
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        #endregion

        #region Constructor

        public RegistrationUpdateViewModel(INavigation navigation, AccessCodeModel accessCodeModel)
        {
            webServiceManager = new WebServiceManager();
            this.navigation = navigation;
            userDetail = new SignUpModel();
            PickerDate = new ObservableCollection<string>();
            PickerYear = new ObservableCollection<int>();

            PickerMonth = new ObservableCollection<string>() { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            for (int i = 1; i <= 31; i++)
            {
                if (i < 10)
                {
                    PickerDate.Add("0" + i);
                }
                else
                {
                    PickerDate.Add(i.ToString());
                }
            }
            var currentYear = System.DateTime.Now.AddYears(-13).Year;
            for (int i = currentYear; i >= currentYear - 100; i--)
            {
                PickerYear.Add(i);
            }

            if (accessCodeModel != null)
            {
                if (!string.IsNullOrEmpty(accessCodeModel.AccessCode))
                {
                    userDetail.AccessCode = accessCodeModel.AccessCode;
                }
                else
                {
                    userDetail.model_no = accessCodeModel.model_no;
                    userDetail.serial_no = accessCodeModel.serial_no;
                }
            }

            MessagingCenter.Subscribe<string>(this, "DateSelected", (args) =>
            {
                isDateSelected = true;
            });
        }

        #endregion
    }
}
