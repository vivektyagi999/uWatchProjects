using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using Newtonsoft.Json;
using uWatch.ViewModels;
using UwatchPCL;
using Xamarin.Forms;

namespace uWatch
{
    public class InventoryViewModel:BaseViewModel,INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this,
                    new PropertyChangedEventArgs(propertyName));
            }
        }
        private Command loadmoreCommand;
        public const string LoadMorePropertyName = "LoadMoreCommand";
        public Command LoadMoreCommand
        {
            get
            {
                return loadmoreCommand ?? (loadmoreCommand = new Command(async () => await LoadMoreInventoryCommand()));
            }
        }

        //public ICommand AddInventoryCommand
        //{
        //    get;
        //    set;
        //}

        public bool ismessage;
        public bool isMessage
        {
            set
            {
                if (ismessage != value)
                {
                    ismessage = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this,
                            new PropertyChangedEventArgs("isMessage"));
                    }
                }
            }
            get
            {
                return ismessage;
            }
        }
        public bool isinventoryavilable;
        public bool isInventoryavilable
        {
            set
            {
                if (isinventoryavilable != value)
                {
                    isinventoryavilable = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this,
                                        new PropertyChangedEventArgs("isInventoryavilable"));
                    }
                }
            }
            get
            {
                return isinventoryavilable;
            }
        }
        public bool isloading;
        public bool isLoading
        {
            set
            {
                if (isloading != value)
                {
                    isloading = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this,
                            new PropertyChangedEventArgs("isLoading"));
                    }
                }
            }
            get
            {
                return isloading;
            }
        }
        public string messagetext;
        public string MessageText
        {
            set
            {
                if (messagetext != value)
                {
                    messagetext = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this,
                             new PropertyChangedEventArgs("MessageText"));
                    }
                }
            }
            get
            {
                return messagetext;
            }
        }
        public string roomName;
        public string RoomName
        {
            set
            {
                if (roomName != value)
                {
                    roomName = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this,
                                        new PropertyChangedEventArgs("RoomName"));
                    }
                }
            }
            get
            {
                return roomName;
            }
        }
        public ObservableCollection<Room> inventorylist
        {
            get;
            set;
        }
        public ObservableCollection<Room> InventoryList
        {
            set
            {
                if (inventorylist != value)
                {
                    inventorylist = value;

                    if (PropertyChanged != null)
                    {

                        PropertyChanged(this,
                                        new PropertyChangedEventArgs("InventoryList"));

                    }
                }
            }
            get
            {
                return inventorylist;
            }
        }
        int pageindex = 0;
        public InventoryViewModel()
        {
            try
            {
                // AddInventoryCommand = new Command(AddInventoryMethod);
                InventoryList = new ObservableCollection<Room>();

                Task.Run(async () => await LoadInventory());
            }
            catch (Exception ex)
            {
                
            }
        }
        async Task LoadInventory()
        {
            try
            {
                isLoading = true;
                if(InventoryList.Count==0)
                {
                    pageindex = 0;
                }
                else if(InventoryList.Count>0)
                {
                    pageindex = pageindex + 1;
                }
                var result = await ApiService.Instance.GetInventoryList();
                if(result.ErrorCode==0)
                {
                    var lst = Task.Run(() =>JsonConvert.DeserializeObject<List<Room>>(result.data)).Result;
                    foreach (var item in lst)
                    {
                        try
                        {
                            InventoryList.Add(item);
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    isLoading = false;
                    isInventoryavilable = true;
                    isMessage = false;
                }
                else
                {
                    isLoading = false;
                    isInventoryavilable = false;
                    isMessage = true;
                    MessageText = result.ErrorMessage;
                }

            }
            catch (Exception ex)
            {
                isLoading = false;
                isInventoryavilable = false;
                isMessage = true;
                MessageText = "No Inventory Found!";
            }
        }
        private async Task LoadMoreInventoryCommand()
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Loading");
                await LoadMore();
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {

            }
        }
        async Task LoadMore()
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Adds the inventory.
        /// </summary>
        /// <returns>The inventory.</returns>
        //async void AddInventoryMethod()
        //{
        //    try
        //    {
        //        isLoading = true;
        //        var result = await ApiService.Instance.AddInventory(RoomName);
        //        if (result.ErrorCode == 0)
        //        {
        //            var lst = Task.Run(() => JsonConvert.DeserializeObject<List<Room>>(result.data)).Result;
        //            foreach (var item in lst)
        //            {
        //                try
        //                {
        //                    InventoryList.Add(item);
        //                }
        //                catch (Exception ex)
        //                {

        //                }
        //            }
        //            isLoading = false;
        //            isInventoryavilable = true;
        //            isMessage = false;
        //        }
        //        else
        //        {
        //            isLoading = false;
        //            isInventoryavilable = false;
        //            isMessage = true;
        //            MessageText = result.ErrorMessage;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        isLoading = false;
        //        isInventoryavilable = false;
        //        isMessage = true;
        //        MessageText = "No Inventory Found!";
        //    }
        //}
    }
}
