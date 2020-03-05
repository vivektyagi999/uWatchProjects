using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Acr.UserDialogs;
using uWatch.ViewModels;
using UwatchPCL;
using UwatchPCL.WebServices;
using Xamarin.Forms;

namespace uWatch
{
    public class AddCubeConfigViewModel : INotifyPropertyChanged
    {

        public Command _confirmCommand, _cancelCommand, _cubeConfigSelectionCommand;
        public WebServiceManager webServiceManager;
        public int Value
        {
            get;
            set;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<CubeConfigure> _listCubeConfig;
        public ObservableCollection<CubeConfigure> ListCubeConfig
        {
            get { return _listCubeConfig; }
            set
            {
                _listCubeConfig = value;

                if (PropertyChanged != null)
                {
                    PropertyChanged(this,
                                    new PropertyChangedEventArgs("ListCubeConfig"));
                }

            }
        }

        public void ExecuteCubeConfigSelectionCommand(CubeConfigure SelectedCube)
        {
            if (SelectedCube != null)
            {
                
                var Cube = ListCubeConfig.Where(x => x.Selected == true).FirstOrDefault();
                if (Cube != null)
                {
                    Cube.Selected = false;
                }
                SelectedCube.Selected = true;
                Value = SelectedCube.Value;
            }
        }

        //private Action<object> ExecuteCubeConfigSelectionCommand()
        //{
        //    throw new NotImplementedException();
        //}

        //public int _selectedCubeConfigIndex = -1;
        //public int SelectedCubeConfigIndex
        //{
        //    get { return _selectedCubeConfigIndex; }
        //    set
        //    {
        //        _selectedCubeConfigIndex = value;
        //        if (_selectedCubeConfigIndex != -1)
        //        {
        //            SelectedCubeConfig = PickerCubeConfig[_selectedCubeConfigIndex].ToString();
        //        }
        //        if (PropertyChanged != null)
        //        {
        //            PropertyChanged(this,
        //                            new PropertyChangedEventArgs("SelectedMonthIndex"));
        //        }

        //    }
        //}

        //public string _selectedCubeConfig="Select Default Configuration";
        //public string SelectedCubeConfig
        //{
        //    get { return _selectedCubeConfig; }
        //    set
        //    {
        //        _selectedCubeConfig = value;
        //        if (PropertyChanged != null)
        //        {
        //            PropertyChanged(this,
        //                            new PropertyChangedEventArgs("SelectedCubeConfig"));
        //        }

        //    }
        //}

        public AddCubeConfigViewModel()
        {
            webServiceManager = new WebServiceManager();
            ListCubeConfig = new ObservableCollection<CubeConfigure>();
            GetCubeConfigAsync();
        }


        private void GetCubeConfigAsync()
        {
            var networkconnection = DependencyService.Get<INetworkConnection>();
            networkconnection.CheckNetworkConnection();
            var networkStatus = networkconnection.IsConnected ? "Connected" : "Not Connected";
            if (networkStatus == "Not Connected")
            {

                UserDialogs.Instance.Alert("Please check your internet connection", "Alert", "Ok");
                return;
            }
            UserDialogs.Instance.ShowLoading();
            Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await webServiceManager.GetDefaultConfigLibAsync();
                if (result.ErrorCode == 0)
                {
                    if (result.Data != null)
                    {
                        foreach (var data in result.Data)
                        {
                            ListCubeConfig.Add(data);
                        }
                    }
                    else
                    {
                        UserDialogs.Instance.Alert("No data Found!", "Alert", "OK");
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

            });
        }
    }
}
