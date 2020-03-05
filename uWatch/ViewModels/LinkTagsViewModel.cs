using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using uWatch.ViewModels;
using UwatchPCL;
using Xamarin.Forms;
using Rg.Plugins.Popup.Extensions;
using UwatchPCL.WebServices;
using Acr.UserDialogs;

namespace uWatch
{
    public class LinkTagsViewModel : ParentViewModel
    {
        public WebServiceManager webServiceManager;
        public Command _listCelTappedCommand, _closeCommand, _linkTagCommand;
        public List<BLETransTag> SelectedLinkTags;
        INavigation navigation;
        int DeviceId;

        public BLETagLinks _bleTagsLink;
        public BLETagLinks BleTagsLink
        {
            get { return _bleTagsLink; }
            set
            {
                if (value != null)
                {
                    _bleTagsLink = value;
                    PropertyChangedBase("BleTagsLink");
                }
            }
        }



        public ObservableCollection<BLETransTag> _linkTags;
        public ObservableCollection<BLETransTag> LinkTags
        {
            get { return _linkTags; }
            set
            {
                if (value != null)
                {
                    _linkTags = value;
                    PropertyChangedBase("LinkTags");
                }
            }
        }
        public bool _isFound = false;
        public bool IsFound
        {
            get { return _isFound; }
            set
            {
                if (value != _isFound)
                {
                    _isFound = value;
                    PropertyChangedBase("IsFound");
                }
            }
        }


        public string _title = "Link Tags";
        public string Title
        {
            get { return _title; }
            set
            {
                if (value != null)
                {
                    _title = value;
                    PropertyChangedBase("Title");
                }
            }
        }

        public string _subTitle = "Select BLE Tags to link to this Cube";
        public string SubTitle
        {
            get { return _subTitle; }
            set
            {
                if (value != null)
                {
                    _subTitle = value;
                    PropertyChangedBase("SubTitle");
                }
            }
        }

        public Command ListCelTappedCommand
        {
            get { return _listCelTappedCommand ?? (_listCelTappedCommand = new Command(ExecuteListCelTappedCommandAsync)); }
        }

        public Command CloseCommand
        {
            get
            {
                return _closeCommand ?? (_closeCommand = new Command(() => { ExecuteCloseCommand(); }));
            }
        }

        public Command LinkTagCommand
        {
            get
            {
                return _linkTagCommand ?? (_linkTagCommand = new Command(() => { ExecuteLinkTagCommandAsync(); }));
            }
        }

        private async void ExecuteLinkTagCommandAsync()
        {
            if (BleTagsLink.PendingSetting.ConfigIdx > 0)
            {
                await UserDialogs.Instance.AlertAsync("Pending changes cannot be amended, delete Pending first and then make all changes.", "", "Ok");
                await navigation.PopPopupAsync(true);
            }
            else
            {
                try
                {
                    Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
                    {
                        if (SelectedLinkTags.Count > 0)
                        {
                            UserDialogs.Instance.ShowLoading("Loading...");
                            var result = await webServiceManager.SaveTagsToCubefromAppAsync(SelectedLinkTags, DeviceId);
                            if (result.ErrorCode == 0)
                            {
                                UserDialogs.Instance.HideLoading();
                                if (result.IsSuccess)
                                {
                                    await UserDialogs.Instance.AlertAsync("BLE Tags successfully linked", "", "Ok");
                                    await navigation.PopPopupAsync(true);
                                }
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
                        }
                        else
                        {
                            UserDialogs.Instance.Alert("Please select tag links", "Alert", "Ok");
                        }
                    });
                }
                catch (Exception ex)
                {
                    UserDialogs.Instance.ShowLoading("Loading...");

                }

            }
        }

        private void ExecuteCloseCommand()
        {
            navigation.PopPopupAsync(true);
        }

        private void ExecuteListCelTappedCommandAsync(object obj)
        {

            try
            {
                if (obj != null)
                {
                    var selectedData = obj as BLETransTag;

                    if (selectedData.TagSelected.Value)
                    {
                        selectedData.TagSelected = false;
                        if (SelectedLinkTags.Count > 0)
                        {
                            SelectedLinkTags.Remove(selectedData);
                        }
                    }
                    else
                    {
                        selectedData.TagSelected = true;
                        SelectedLinkTags.Add(selectedData);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public LinkTagsViewModel(INavigation _navigation, BLETagLinks data, int deviceId)
        {

            _navigation = navigation;
            DeviceId = deviceId;
            SelectedLinkTags = new List<BLETransTag>();
            webServiceManager = new WebServiceManager();
            BleTagsLink = new BLETagLinks();
            LinkTags = new ObservableCollection<BLETransTag>();
            if (data != null)
            {
                BleTagsLink = data;
                if (data.CurrentSetting.BlueTransTags.Count > 0)
                {
                    IsFound = true;
                    foreach (var bleData in data.CurrentSetting.BlueTransTags)
                    {
                        LinkTags.Add(bleData);
                    }
                }
                else
                {
                    IsFound = false;
                }
            }
        }
    }
}
