using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace uWatch
{
    public partial class InventoryViewCell : MyViewCell
    {
        public InventoryViewCell()
        {
            InitializeComponent();
        }

        void imgedit_Tapped(object sender, System.EventArgs e)
        {
            var name = lblroomname.Text;

        }

    }
}
