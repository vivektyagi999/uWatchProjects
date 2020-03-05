using System;
using UwatchPCL;
using Xamarin.Forms;

namespace uWatch
{
	public class UserGuide:ContentPage
	{
		string htmlsource = "";
		public UserGuide()
		{
			Title = "User Guide";
			var wait = new ActivityIndicator {IsRunning=true, Color=Color.Red };
			Content = wait;
		}

		void InitializeComponent()
		{
			var browser = new CustomWebView();

			browser.BackgroundColor = Xamarin.Forms.Color.White;

#if __ANDROID__
            htmlsource = "<p><strong>To access the User Guide, please log in to your web account</strong></p>";
            //  htmlsource = "<p><strong>To access the User Guide, please log in to your web account</strong></p><p>To create a record, select <strong><em>Assets</em> </strong>from the APP menu on your phone, select your device and follow the instructions</p><ul><li>Click on Camera (date and time stamped) for taking pictures</li></ul><p>and if asked, <strong><em>ALLOW</em></strong> uWatch to take pictures</p><ul><li>Full details of your Assets can be added to the picture&nbsp;<strong>on the APP or the Web page.</strong></li><li>These details should include any&nbsp;<strong>serial number&nbsp;</strong>and distinguishing marks.</li><li>Up to 8 records can be added&nbsp;</li></ul><p><strong>Powering up your Cube.</strong></p><p>Install the 3 batteries. The two outside batteries have the +ve terminal in one direction and the middle one is reversed. the batteries used in the Cube MUST BE&nbsp;</p><p><strong>&ldquo;</strong><strong>Disposable Lithium AA batteries&rdquo;.</strong></p><ul><li>On inserting the last battery, the Cube will display <strong>&ldquo;Initialising&rdquo;</strong> on the LCD.</li><li>After a few seconds this will disappear and the Cube will be Switched off and <strong>Ready to be switched on and used</strong></li><li>Hold <strong>the Switch </strong>(under the LC Display)<strong> down for 4 seconds</strong> to switch the Cube on (or off)</li><li>The first time a Cube is powered up it may take some time to identify the <strong>strongest local signal</strong> and Carrier (EE, O2, Vodafone etc). The <strong>LCD display</strong> will indicate its progress.</li><li>The first thing to be displayed is &ldquo;logging on to GSM&rdquo; and then <strong>&ldquo;Signal Strength&rdquo;</strong></li><li>This confirms there is a mobile phone signal available so your Cube will work.</li><li>Several other initialisation processes will take place and a <strong>Switch on alert sent.</strong></li></ul>";
#endif
#if __IOS__
            htmlsource="<font size='6'><p><strong>To access the User Guide, please log in to your web account</strong></p></font>";
			 //htmlsource = "<font size='6'><p><strong>Creating an Insurance Record (uARM Asset List)</strong></p><p>To create a record, select <strong><em>Assets</em> </strong>from the APP menu on your phone, select your device and follow the instructions</p><ul><li>Click on Camera (date and time stamped) for taking pictures</li></ul><p>and if asked, <strong><em>ALLOW</em></strong> uWatch to take pictures</p><ul><li>Full details of your Assets can be added to the picture&nbsp;<strong>on the APP or the Web page.</strong></li><li>These details should include any&nbsp;<strong>serial number&nbsp;</strong>and distinguishing marks.</li><li>Up to 8 records can be added&nbsp;</li></ul><p><strong>Powering up your Cube.</strong></p><p>Install the 3 batteries. The two outside batteries have the +ve terminal in one direction and the middle one is reversed. the batteries used in the Cube MUST BE&nbsp;</p><p><strong>&ldquo;</strong><strong>Disposable Lithium AA batteries&rdquo;.</strong></p><ul><li>On inserting the last battery, the Cube will display <strong>&ldquo;Initialising&rdquo;</strong> on the LCD.</li><li>After a few seconds this will disappear and the Cube will be Switched off and <strong>Ready to be switched on and used</strong></li><li>Hold <strong>the Switch </strong>(under the LC Display)<strong> down for 4 seconds</strong> to switch the Cube on (or off)</li><li>The first time a Cube is powered up it may take some time to identify the <strong>strongest local signal</strong> and Carrier (EE, O2, Vodafone etc). The <strong>LCD display</strong> will indicate its progress.</li><li>The first thing to be displayed is &ldquo;logging on to GSM&rdquo; and then <strong>&ldquo;Signal Strength&rdquo;</strong></li><li>This confirms there is a mobile phone signal available so your Cube will work.</li><li>Several other initialisation processes will take place and a <strong>Switch on alert sent.</strong></li></ul></font>";
#endif
            var htmlSource = new HtmlWebViewSource();

			htmlSource.Html = @htmlsource;

			if (Device.OS != TargetPlatform.iOS)
			{
				htmlSource.BaseUrl = DependencyService.Get<IBaseUrl>().Get();
			}

			browser.Source = htmlSource;

			Grid grid = new Grid
			{
				VerticalOptions = LayoutOptions.FillAndExpand,
				RowDefinitions = {
					new RowDefinition { Height = new GridLength (40, GridUnitType.Absolute) },
					new RowDefinition { Height = new GridLength (1, GridUnitType.Star) }
				},
				ColumnDefinitions = {
					new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },
				}
			};

			grid.Children.Add(browser, 0, 3, 0, 3);

			Content = grid;
		}
		protected override void OnAppearing()
		{
			base.OnAppearing();

			InitializeComponent();
		}
	}
}
