using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace uWatch
{
    public partial class DatePickerWithIcon : ContentView
    {
        public DatePickerWithIcon()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty ValueProperty =
  BindableProperty.Create<DatePickerWithIcon, DateTime>(w => w.Value, default(DateTime));

        public DateTime Value
        {
            get { return (DateTime)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        void ChevronDown_Click(Object Sender, EventArgs e)
        {
            this.dtPicker.Focus(); 
        }

        public CustomDatePicker DtPicker
        {
            get
            {
                return this.dtPicker;
            }
            set
            {
                this.dtPicker = value;
            }
        }
    }
}
