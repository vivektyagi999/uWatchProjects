using System;
using Xamarin.Forms;

namespace uWatch
{
	public class GlossaryPage : ContentPage
	{
		public GlossaryPage()
		{
			Title = "Glossary";
			var layoutGSMmain = new StackLayout { };
			var layoutGSM = new StackLayout { BackgroundColor = Color.FromRgb(229,229,229), Padding = new Thickness(0,6,0,6) };
			var lblGSM = new Label { Text = "GSM",FontSize = 13, TextColor = Color.Black, FontAttributes = FontAttributes.Bold };
			var lblGSMValue = new Label { FontSize = 13,Text = "Global System for Mobile Telecommunications \nwas the European technology for mobile phones that was adopted worldwide using early notations such as 2g, 3g and now 4g."};
			layoutGSM.Children.Add(lblGSM);
			layoutGSMmain.Children.Add(layoutGSM);
			layoutGSMmain.Children.Add(lblGSMValue);

			var layoutGPRSmain = new StackLayout { };
			var layoutGPRS = new StackLayout { BackgroundColor = Color.FromRgb(229, 229, 229 ),Padding = new Thickness(0, 6, 0, 6) };
			var lblGPRS = new Label { Text = "GPRS", FontSize = 13,TextColor = Color.Black, FontAttributes = FontAttributes.Bold };
			var lblGPRSValue = new Label { FontSize = 12, Text = "General Packet Radio Service \nIs an upgraded GSM technology which provides for the secure transmission of data via the GSM network.  Generally known as M2M, “machine to machine” this is the technology that the Cube uses to communicate Alerts through the MCS to your phone." };
			layoutGPRS.Children.Add(lblGPRS);
			layoutGPRSmain.Children.Add(layoutGPRS);
			layoutGPRSmain.Children.Add(lblGPRSValue);


			var layoutMCSmain = new StackLayout { };
			                                        var layoutMCS = new StackLayout { BackgroundColor = Color.FromRgb(229, 229, 229),Padding = new Thickness(0, 6, 0, 6) };
			var lblMCS = new Label {FontSize = 13, Text = "MCS",TextColor = Color.Black, FontAttributes = FontAttributes.Bold };
			var lblMCSValue = new Label { FontSize = 12,Text = "Management and Communications System \nThis is the uWatch cloud based system which manages the uWatch Cubes and how they communicate with you and your phone." };
			layoutMCS.Children.Add(lblMCS);
			layoutMCSmain.Children.Add(layoutMCS);
			layoutMCSmain.Children.Add(lblMCSValue);


			var layoutPushNotificationmain = new StackLayout { };
			                                                                  var layoutPushNotification = new StackLayout { BackgroundColor = Color.FromRgb(229, 229, 229),Padding = new Thickness(0, 6, 0, 6) };
			var lblPushNotification = new Label {FontSize = 13, Text = "Push notification",TextColor = Color.Black , FontAttributes = FontAttributes.Bold };
            var lblPushNotificationValue = new Label {FontSize = 12, Text = "This is a message received by, and displayed on, your mobile phone, sent from the MCS, to tell you that your uWatch APP has received and alert or message. \nThe app does not need to be open for your phone to receive this, but you must remain logged in." };
			layoutPushNotification.Children.Add(lblPushNotification);
			layoutPushNotificationmain.Children.Add(layoutPushNotification);
			layoutPushNotificationmain.Children.Add(lblPushNotificationValue);


			var layoutSIMmain = new StackLayout { };
			var layoutSIM = new StackLayout { BackgroundColor = Color.FromRgb(229, 229, 229),Padding = new Thickness(0, 6, 0, 6) };
			var lblSIM = new Label { FontSize = 13,Text = "SIM",TextColor = Color.Black , FontAttributes = FontAttributes.Bold };
            var lblSIMValue = new Label {FontSize = 12, Text = "Subscriber identity module \nEach Cube comes with a preinstalled SIM card which is already registered on the uWatch MSC and is included in the first 12 months."};
			layoutSIM.Children.Add(lblSIM);
			layoutSIMmain.Children.Add(layoutSIM);
			layoutSIMmain.Children.Add(lblSIMValue);


			var layoutmain = new StackLayout { Padding = 8, Spacing = 8 };
			layoutmain.Children.Add(layoutGSMmain);
			layoutmain.Children.Add(layoutGPRSmain);
			layoutmain.Children.Add(layoutMCSmain);
			layoutmain.Children.Add(layoutPushNotificationmain);
			layoutmain.Children.Add(layoutSIMmain);

			var scrlview = new ScrollView { };
			scrlview.Content = layoutmain;


			Content = scrlview;
		}


	}
}

