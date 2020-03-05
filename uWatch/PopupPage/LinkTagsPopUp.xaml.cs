using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace uWatch
{
    public partial class LinkTagsPopUp : PopupPage
    {
        LinkTagsViewModel viewModel;

        public LinkTagsPopUp(UwatchPCL.WebServices.BLETagLinks data, int deviceId)
        {
            InitializeComponent();
            this.BindingContext = viewModel = new LinkTagsViewModel(Navigation, data,deviceId);
        }
      

        private async System.Threading.Tasks.Task PopuPageAsync()
        {
            await Navigation.PopPopupAsync();
        }
    }
}
