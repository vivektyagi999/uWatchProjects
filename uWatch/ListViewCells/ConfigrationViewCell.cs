using System;
using Xamarin.Forms;

namespace uWatch
{
    public class ConfigrationViewCell:ViewCell
    {
        public ConfigrationViewCell()
        {
            var lblconfigrationname = new Label();
            lblconfigrationname.FontSize = 16;
            lblconfigrationname.TextColor = Color.Black;
            lblconfigrationname.VerticalOptions = LayoutOptions.CenterAndExpand;
            lblconfigrationname.FontAttributes = FontAttributes.Bold;
            lblconfigrationname.SetBinding(Label.TextProperty, new Binding("ConfigurationName", BindingMode.TwoWay));
            var mainlayout = new StackLayout();
            mainlayout.Padding = new Thickness(5, 5, 0, 5);
            mainlayout.Children.Add(lblconfigrationname);
            View = mainlayout;

        }
    }
}
