using System; using System.Collections.Generic; using Plugin.BLE; using Xamarin.Forms; using Acr.UserDialogs; using System.Linq; using System.Threading.Tasks;
using System.Collections.ObjectModel;
using UwatchPCL;

namespace uWatch { 	public class Bluetoothlist: ContentPage 	{ 		CustomPopup popupLayouts;double pddingH, paddingW;int PaddingMain; 	public static	bool turnonbluetooth = false; 		public BluetoothlistViewModel _BluetoothlistViewModel { get; set; } 		ListView lbl; 		ActivityIndicator activity; 		ToolbarItem Stop, Start; 		Label txt;  		public bool OnAppearFlag = true; 		public Bluetoothlist(BluetoothlistViewModel bluetoohviewmoedls) 		{ 			try 			{ 				Title = "Bluetooth Devices"; 				NavigationPage.SetHasNavigationBar(this, true); 				NavigationPage.SetBackButtonTitle(this, ""); 				NavigationPage.SetHasBackButton(this, true); 				_BluetoothlistViewModel = bluetoohviewmoedls; 				BindingContext = _BluetoothlistViewModel; 			  				 Start = new ToolbarItem 					{ 					 					Icon="reload.png" 						 					} ; 		 					this.ToolbarItems.Add(Start); 				Start.Clicked += async (sender, e) => 				  { 					UserDialogs.Instance.ShowLoading("Searching for uWatch Bluetooth Devices!");
					  lbl.ItemsSource = null;  					OnAppearFlag = true; 				  					  						var ble = CrossBluetoothLE.Current; 						 if (!ble.IsOn) 						 { 							  UserDialogs.Instance.HideLoading(); 							 txt.Text = "Please turn On Bluetooth!";
						     							 OnAppearFlag = false;  						 }

					  if (OnAppearFlag)
					  {
						  try
						  {
							  var bluetoohviewmoedl = new BluetoothlistViewModel();
							  await bluetoohviewmoedl.GetDeviceList();
							  _BluetoothlistViewModel = bluetoohviewmoedl;
							  if (bluetoohviewmoedl.Bluetootdevicelist.Count == 0)
							  {  turnonbluetooth = false;
								  txt.Text = "No BLE device found"; 								  lbl.BindingContext = _BluetoothlistViewModel; 								  BindData();
								  UserDialogs.Instance.HideLoading();
							  }
							  else
							  {
								  txt.Text = "";
								  lbl.BindingContext = _BluetoothlistViewModel;
								  BindData();
								  UserDialogs.Instance.HideLoading();
							  }

						  }

						  catch (System.Exception ex)
						  {
						  }
					  }  				  };  		 				InitializeComponent(); 			     				UserDialogs.Instance.HideLoading(); 			} 			catch (System.Exception ex) 			{ 				UserDialogs.Instance.HideLoading(); 			} 		} 		public void BindData() 		{ 			 			lbl.BindingContext = _BluetoothlistViewModel; 			lbl.SetBinding(ListView.ItemsSourceProperty, new Binding("Bluetootdevicelist")); 			 		} 		public void InitializeComponent() 		{ 			 			 activity = new ActivityIndicator { Color = Color.Red,HeightRequest = 25, WidthRequest = 25, IsRunning = true  }; 			txt = new Label { TextColor = Color.Red, HorizontalOptions = LayoutOptions.CenterAndExpand,FontSize=16 }; 			var txtLayout = new StackLayout { Spacing = 1, Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.StartAndExpand, Padding = new Thickness(0, 10, 0, 10) }; 			txtLayout.Children.Add(txt);   			if (_BluetoothlistViewModel.Bluetootdevicelist.Count == 0) 			{ 				activity.IsRunning = false; 				txt.Text = "No BLE device found"; 			}  			lbl = new ListView {  }; 			lbl.BindingContext = _BluetoothlistViewModel; 			lbl.ItemTemplate = new DataTemplate(typeof(BluetoothViewCell)); 			lbl.RowHeight = 80;  			var mainLayout = new StackLayout { VerticalOptions = LayoutOptions.StartAndExpand }; 			mainLayout.Children.Add(txtLayout); 				BindData();  				var custom = new CustomPopup(); 				popupLayouts = this.Content as CustomPopup;   				mainLayout.Children.Add(lbl);  				custom.Content = mainLayout; 				Content = custom; 		}       		protected override void OnDisappearing() 		{ 			base.OnDisappearing(); 			OnAppearFlag = false; 		}  		protected override void OnAppearing() 		{ 			base.OnAppearing();   			OnAppearFlag = true; 		}  	} 	public class BluetoothViewCell : ViewCell 	{ 		public BluetoothViewCell() 		{ 			var cellmainstk = new StackLayout {Orientation=StackOrientation.Vertical, VerticalOptions=LayoutOptions.Start,HorizontalOptions=LayoutOptions.Start,Padding=new Thickness(10,10,10,0)}; 			var addressstack = new StackLayout { Orientation=StackOrientation.Horizontal}; 			var rssistack = new StackLayout { Orientation = StackOrientation.Horizontal };   			var name = new Label(); 			name.FontSize = 22; 			name.FontAttributes = FontAttributes.Bold; 			name.TextColor = Color.FromHex("#666"); 			name.VerticalOptions = LayoutOptions.CenterAndExpand; 			name.SetBinding(Label.TextProperty, new Binding("DeviceName"));  			var box = new BoxView { HeightRequest = 4 };  			var rssitext = new Label(); 			rssitext.FontSize = 18; 			rssitext.TextColor = Color.FromHex("#666"); 			rssitext.Text = "Signal Strength(RSSI):"; 			var rssivalue = new Label(); 			rssivalue.FontSize = 18; 			rssivalue.TextColor = Color.FromHex("#666"); 			rssivalue.SetBinding(Label.TextProperty, new Binding("DeviceRSSI"));  			rssistack.Children.Add(rssitext); 			rssistack.Children.Add(rssivalue);  			cellmainstk.Children.Add(name); 			cellmainstk.Children.Add(box); 			cellmainstk.Children.Add(rssistack);  			View = cellmainstk;    		} 		protected override void OnBindingContextChanged() 		{ 			base.OnBindingContextChanged(); 			var text = BindingContext as BluetoothlistViewModel;  		}     	} }  