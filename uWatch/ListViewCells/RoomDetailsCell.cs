using System;
using Xamarin.Forms;

namespace uWatch
{
    public class RoomDetailsCell:MyViewCell
    {
        public RoomDetailsCell()
        {
            try
            {
                var mainstasck = new StackLayout();

                var itemstk = new StackLayout();
                itemstk.Orientation = StackOrientation.Horizontal;
                var lblitem = new Label();
                lblitem.FontSize = 18;
                lblitem.FontAttributes = FontAttributes.Bold;
                lblitem.TextColor = Color.Black;
                lblitem.SetBinding(Label.TextProperty, new Binding("Item"));
                var lblitemText = new Label{Text="Item :",FontAttributes=FontAttributes.Bold,FontSize=18,TextColor=Color.Black};
                itemstk.Children.Add(lblitemText);
                itemstk.Children.Add(lblitem);

                var descstk = new StackLayout();
                descstk.Orientation = StackOrientation.Vertical;
                var lbldesc = new HtmlLabel();
                lbldesc.FontSize = 18;
               // lbldesc.FontAttributes = FontAttributes.Bold;
                lbldesc.TextColor = Color.Black;
                lbldesc.SetBinding(Label.TextProperty, new Binding("Description"));
                var lbldescText = new Label { Text = "Description :", FontAttributes = FontAttributes.Bold, FontSize = 18, TextColor = Color.Black };
                descstk.Children.Add(lbldescText);
                descstk.Children.Add(lbldesc);

                var colorstk = new StackLayout();
                colorstk.Orientation = StackOrientation.Horizontal;
                var lblcolor = new Label();
                lblcolor.FontSize = 18;
                lblcolor.FontAttributes = FontAttributes.Bold;
                lblcolor.TextColor = Color.Black;
                lblcolor.SetBinding(Label.TextProperty, new Binding("Color"));
                var lblcolorText = new Label { Text = "Color :", FontAttributes = FontAttributes.Bold, FontSize = 18, TextColor = Color.Black };
                colorstk.Children.Add(lblcolorText);
                colorstk.Children.Add(lblcolor);

                mainstasck.Children.Add(itemstk);
                mainstasck.Children.Add(colorstk);
                mainstasck.Children.Add(descstk);

                View = mainstasck;
                //var
            }
            catch (Exception ex)
            {

            }


        }
    }
}
