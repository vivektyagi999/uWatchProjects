using System;
using UwatchPCL;
using Xamarin.Forms;

namespace uWatch
{
	public class ConfigrationPage: ContentPage
	{
		string htmlsource = "";
		public ConfigrationPage()
		{
			Title = "Configuration";
			var wait = new ActivityIndicator { IsRunning = true, Color = Color.Red };
			Content = wait;
		}
		void InitializeComponent()
		{
			
			var browser = new CustomWebView();

			browser.BackgroundColor = Xamarin.Forms.Color.White;
          
#if __ANDROID__
            htmlsource = "<p>To have full access to cube configuration changes, please visit your user web page</p><p>Configuration changes happen on the next alert or heartbeat.Therefore, after making changes and saving them, the configuration will be “pending” and then after an alert has happened it will be “completed” and will encompass the updated settings.</p><p>From the app, you can check your current cube configuration but also apply “pre - set” configurations e.g. Tracking Mode.</p><p>You can create your own “pre-set” configurations from your user web page.</p><p>Visit the user guide or contact uWatch for more details.</p>";
            //htmlsource = "<h3><strong>Configuring your Cube. </strong><strong>How it works:&nbsp;</strong></h3><p>When the Cube is configured via the web site or APP and the new configuration saved, the changes will be &ldquo;<strong>Pending</strong>&rdquo;. When the Cube wakes up during an <strong>Alert</strong> or <strong>Heartbeat </strong>it&rsquo;s configuration will be changed to the new settings, and the changes will be marked on the Web Page as &ldquo;<strong>Completed</strong>&rdquo;.</p><p>The less features invoked the better the battery life will be.</p><h4><strong>The Configuration Store.</strong></h4><p>On the <strong>Devices/configuration</strong> <strong>Web Page</strong> an Owner can edit and store predefined Configurations for future use. Configurations can also be downloaded from the <strong>Configuration Library,</strong> these can be modified and saved in the <strong>owner&rsquo;s Configuration Store.</strong> These can also be <strong>activated from the APP.</strong></p><p>Downloaded Configurations also include instructions on how they should be used.</p><p><strong>Configuration settings: </strong>see the <strong>HELP</strong> on the Web <em>Configuration</em> screen for more details.</p><ul><li><strong>Heartbeat&nbsp;</strong></li></ul><p>A <strong>Heartbeat</strong> will be sent periodically to confirm that the Cube is operational.</p><ul><li>The time of day, the frequency and the start date for the heartbeat can be set.</li><li><strong>Inputs</strong></li></ul><p>An input is where a process or sensor wakes the Cube up. These include</p><ul><li><strong>Temperature range.</strong></li><li>The Cube will send an alert if the temperature changes to the either the maximum or minimum setting.</li><li>In <strong>Forced alert mode</strong> pressing the on/off button sends an alert.</li><li><strong>Outputs&nbsp;</strong></li><li>The LCD and Cube Sounder can be switched off</li><li><strong>Switching</strong></li><li>ON/OFF times</li><li>Times can be pre-set when the Cube will be permanently in sleep mode.</li><li>Switch off</li><li>The ability to switch the device off by the on/off button can be disable</li><li><strong>Times</strong></li><li>Cube Sensor <strong>Sleep time</strong></li><li>Is the period after an alert during which the cube will be inactive.</li><li>Cube <strong>Awake time</strong></li><li>The period after an alert before it goes to sleep.</li><li>Advanced</li><li>Batch Images is an Add On where multiple alerts can be stored and sent as one batch.&nbsp;</li><li>Bluetooth is an Add On where up to 10 &ldquo;shock&rdquo; Tags can be monitored by one Cube. Each tag having its own friendly name to identify it in case of an alert.</li></ul>";
#endif
#if __IOS__
            htmlsource="<font size='6'><p>To have full access to cube configuration changes, please visit your user web page</p><p>Configuration changes happen on the next alert or heartbeat.Therefore, after making changes and saving them, the configuration will be “pending” and then after an alert has happened it will be “completed” and will encompass the updated settings.</p><p>From the app, you can check your current cube configuration but also apply “pre - set” configurations e.g. Tracking Mode.</p><p>You can create your own “pre-set” configurations from your user web page.</p><p>Visit the user guide or contact uWatch for more details.</p><font>";

			 //htmlsource = "<font size='6'><h3><strong>Configuring your Cube. </strong><strong>How it works:&nbsp;</strong></h3><p>When the Cube is configured via the web site or APP and the new configuration saved, the changes will be &ldquo;<strong>Pending</strong>&rdquo;. When the Cube wakes up during an <strong>Alert</strong> or <strong>Heartbeat </strong>it&rsquo;s configuration will be changed to the new settings, and the changes will be marked on the Web Page as &ldquo;<strong>Completed</strong>&rdquo;.</p><p>The less features invoked the better the battery life will be.</p><h4><strong>The Configuration Store.</strong></h4><p>On the <strong>Devices/configuration</strong> <strong>Web Page</strong> an Owner can edit and store predefined Configurations for future use. Configurations can also be downloaded from the <strong>Configuration Library,</strong> these can be modified and saved in the <strong>owner&rsquo;s Configuration Store.</strong> These can also be <strong>activated from the APP.</strong></p><p>Downloaded Configurations also include instructions on how they should be used.</p><p><strong>Configuration settings: </strong>see the <strong>HELP</strong> on the Web <em>Configuration</em> screen for more details.</p><ul><li><strong>Heartbeat&nbsp;</strong></li></ul><p>A <strong>Heartbeat</strong> will be sent periodically to confirm that the Cube is operational.</p><ul><li>The time of day, the frequency and the start date for the heartbeat can be set.</li><li><strong>Inputs</strong></li></ul><p>An input is where a process or sensor wakes the Cube up. These include</p><ul><li><strong>Temperature range.</strong></li><li>The Cube will send an alert if the temperature changes to the either the maximum or minimum setting.</li><li>In <strong>Forced alert mode</strong> pressing the on/off button sends an alert.</li><li><strong>Outputs&nbsp;</strong></li><li>The LCD and Cube Sounder can be switched off</li><li><strong>Switching</strong></li><li>ON/OFF times</li><li>Times can be pre-set when the Cube will be permanently in sleep mode.</li><li>Switch off</li><li>The ability to switch the device off by the on/off button can be disable</li><li><strong>Times</strong></li><li>Cube Sensor <strong>Sleep time</strong></li><li>Is the period after an alert during which the cube will be inactive.</li><li>Cube <strong>Awake time</strong></li><li>The period after an alert before it goes to sleep.</li><li>Advanced</li><li>Batch Images is an Add On where multiple alerts can be stored and sent as one batch.&nbsp;</li><li>Bluetooth is an Add On where up to 10 &ldquo;shock&rdquo; Tags can be monitored by one Cube. Each tag having its own friendly name to identify it in case of an alert.</li></ul></font>";

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
