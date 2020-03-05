using System;
using System.ComponentModel;

namespace uWatch
{
    public class ParentViewModel: INotifyPropertyChanged
    {
       
        public event PropertyChangedEventHandler PropertyChanged;

        public void PropertyChangedBase(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ParentViewModel()
        {
        }
    }
}
