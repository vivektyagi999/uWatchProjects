using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace UwatchPCL
{
    public class CubeConfigure:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void PropertyChangedBase(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        public bool Disabled
        {
            get;
            set;
        }

        public string Group
        {
            get;
            set;
        }

        public bool _selected;
        public bool Selected
        {
            get {return _selected; }
            set
            {
                if(value!=null)
                {
                    _selected = value;
                    PropertyChangedBase("Selected");
                }
            }
        }

        public string Text
        {
            get;
            set;
        }

        public int Value
        {
            get;
            set;
        }

        public CubeConfigure()
        {
        }

       
    }
}
