using System;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using Acr.UserDialogs;
using System.Reflection;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.IO.Compression;
using System.Runtime.InteropServices;
using Plugin.Settings;
using uWatch.ViewModels;
#if __ANDROID__
using FFImageLoading.Forms;
#endif

namespace uWatch
{
    public class FloorPlanPage : ContentPage
    {
        bool flag;
        ActivityIndicator lblConfirmSketchesIndicator;
        StackLayout layoutConfirmIndicator;
        int SizeOficon;
        string _OrderAddress, _orderStatus, stringColorHex, ImageType = "Grid";
        bool flagisVisible = true, flagisvisibleConfirm = false, DismissPopup = false, PopupFlag = false;
        int globleOrderId;
        StackLayout layoutbtn, LayoutOrderData, MainLayoutFloorPlan;
        Frame block;
        ScrollView scrlview;
        Image _imgDelete, imgAddMore;
        Button btnCnfirm, btnSaveLater, btnPopup1, btnPopup2;
        Gallerylayout layout;
        ToolbarItem Grid, Nav, ToolBarlist;
        double screenHeight;
        Label lblTitle;
        int j;
        StackLayout layoutMessage;
      
        static int i;
        double ScreenWidth;
       
//#if __IOS__
        Image _imgOrder;
//#elif __ANDROID__
//        CachedImage _imgOrder;
//#endif
        StackLayout layoutConfirm;


        public FloorPlanPage( double height, double width)
        {
            ScreenWidth = width;
            //setting of toolbar item
            Title = "Floor plans";

            NavigationPage.SetHasNavigationBar(this, true);
            NavigationPage.SetHasBackButton(this, true);
            NavigationPage.SetBackButtonTitle(this, "");
           
            screenHeight = height;
            // set the orderId public
            //globleOrderId = order.OrderId;
            //_orderStatus = order.OrderStatus;
            //_OrderAddress = order.Address;
          
            BindingContext = new GalleryViewModel(globleOrderId);

            //get the result from viewmodel
            MessagingCenter.Subscribe<BaseViewModel, string[]>(this, "DisplayAlert", (sender, values) =>
            {
                try
                {
                    FloorPlan();
                }
                catch
                {
                }
            });
            FloorPlan();
        }

        public static int _orderimgId;
        public static string _orderimg;

        private void FloorPlan()
        {
            NavigationPage.SetHasBackButton(this, true);
            try
            {
                if (Device.Idiom == TargetIdiom.Phone)
                {

                    SizeOficon = 30;
                }
                if (Device.Idiom == TargetIdiom.Tablet)
                {

                    SizeOficon = 36;
                }

                //check the orderstatus 
                if (_orderStatus == "Done")
                {
                    stringColorHex = "c87d7f";
                    flagisVisible = false;
                    flagisvisibleConfirm = true;
                }
                else
                {
                    stringColorHex = "9d2728";
                    flagisVisible = true;
                    flagisvisibleConfirm = false;
                }


                var imgMap = new Image { Source = "map.png", HeightRequest = SizeOficon, WidthRequest = SizeOficon, VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.EndAndExpand };
                var lblOrderAddress = new Label
                {
                    Text = "order.FloorPlanAddress",
                    FontSize = 13,
                    VerticalOptions = LayoutOptions.StartAndExpand,
                    HorizontalOptions = LayoutOptions.StartAndExpand,
                    HorizontalTextAlignment = TextAlignment.Start,

                };

                var tapGestureRecognizerMap = new TapGestureRecognizer();
                imgMap.GestureRecognizers.Add(tapGestureRecognizerMap);

                var layoutMap = new StackLayout { Orientation = StackOrientation.Horizontal, Padding = new Thickness(8, 0, 8, 0), VerticalOptions = LayoutOptions.FillAndExpand };
                layoutMap.Children.Add(lblOrderAddress);
                layoutMap.Children.Add(imgMap);





                var lblFllorplanImg = new Image
                {
                    Source = "iconPlus.png",
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.EndAndExpand,
                    //XAlign = TextAlignment.Center,

                };

                var layoutOrderNotesHeader = new StackLayout { Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.StartAndExpand, Padding = new Thickness(8, 5, 10, 5), HorizontalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Xamarin.Forms.Color.FromHex("e9e8e8"), };
                //layoutOrderNotesHeader.Children.Add(lblOrderNotesHeader);
                layoutOrderNotesHeader.Children.Add(lblFllorplanImg);

            
            
                var lblFllorplanPrefImg = new Image
                {
                    Source = "iconPlus.png",
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.EndAndExpand,


                };
                var layoutOrderPrefHeader = new StackLayout { Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.StartAndExpand, Padding = new Thickness(8, 5, 10, 5), HorizontalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Xamarin.Forms.Color.FromHex("e9e8e8"), };
               // layoutOrderPrefHeader.Children.Add(lblOrderPrefHeader);
                layoutOrderPrefHeader.Children.Add(lblFllorplanPrefImg);


             
                var lblAddressLayout = new StackLayout { Padding = new Thickness(0, 0, 0, 8), HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.StartAndExpand, };
                 lblAddressLayout.Children.Add(layoutMap);
                lblAddressLayout.Children.Add(layoutOrderNotesHeader);
               lblAddressLayout.Children.Add(layoutOrderPrefHeader);
              

     
                //wraplayout for showing the all orderimages in gallery----------
                this.layout = new Gallerylayout
                {
                    Spacing = 5,
                    Orientation = StackOrientation.Horizontal,
                    VerticalOptions = LayoutOptions.StartAndExpand
                };
                var orderimgCollection = GalleryViewModel.OrdersImage;
                this.LayoutOrderData = new StackLayout
                {
                    Padding = new Thickness(0, 0, 0, 0),
                    BackgroundColor = Xamarin.Forms.Color.Black,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.StartAndExpand,
                };
                //if orderimages are null the return messege of no images found
                if(orderimgCollection != null)
                {
                    Getimage(orderimgCollection);

                    // if orderimgcollection count is > 0 then show gallery else show mwssege of no images found

                   
                    LayoutOrderData.Children.Add(scrlview);  
                }
                    
              
                this.btnCnfirm = new Button
                {
                    Text = "Confirm Sketches",
                    IsVisible = flagisVisible,
                    FontSize = 14,
                    BorderColor = Xamarin.Forms.Color.White,
                    BorderRadius = 0,
                    BorderWidth = 1,
                    FontAttributes = FontAttributes.Bold,
                    BackgroundColor = Xamarin.Forms.Color.White,
                    TextColor = Xamarin.Forms.Color.Blue,
                    VerticalOptions = LayoutOptions.EndAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    HeightRequest = 55
                };
                //on confirm of order
              
                var layoutAddMore = new StackLayout { Orientation = StackOrientation.Vertical, VerticalOptions = LayoutOptions.EndAndExpand, Spacing = 0 };
               
                    imgAddMore = new Image
                    {
                        Source = "addbtnUwatch.png",
                        HeightRequest = 72,
                        WidthRequest = 72,
                        HorizontalOptions = LayoutOptions.EndAndExpand
                    };
                    var tapGestureRecognizer = new TapGestureRecognizer();
                    tapGestureRecognizer.Tapped += (object sender, EventArgs e) =>
                    {

                       

                       
                            var viewModel1 = new GalleryViewModel(globleOrderId);
                            viewModel1.ExecuteUserImageSelectionCommand();
                       
                    };
                    imgAddMore.GestureRecognizers.Add(tapGestureRecognizer);
             
               
                this.btnSaveLater = new Button
                {
                    Text = "   Save for later   ",
                    FontSize = 14,
                    BorderRadius = 0,
                    BorderWidth = 1,
                    BorderColor = Xamarin.Forms.Color.White,
                    IsVisible = flagisVisible,
                    FontAttributes = FontAttributes.Bold,
                    BackgroundColor = Xamarin.Forms.Color.White,
                   // TextColor = ColorUtil.ButtonTextColor,
                    VerticalOptions = LayoutOptions.EndAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    HeightRequest = 55
                };
                this.btnSaveLater.Clicked += (object sender, EventArgs e) =>
                {
                    try
                    {
                        this.ToolbarItems.Clear();
                        if (ImageType == "List")
                        {
                            this.ToolbarItems.Add(Grid);
                        }
                        else
                        {
                            this.ToolbarItems.Add(ToolBarlist);
                        }
                        this.ToolbarItems.Add(Nav);


                        //#if __IOS__
                        //                      Navigation.PushModalAsync(new Mainpage());
                        //                      //Navigation.PopAsync();
                        //                      //                  //Navigation.PopModalAsync();
                        //                      //                  Navigation.PushModalAsync (new  Mainpage ());
                        //#elif __ANDROID__
                       // Navigation.PushModalAsync(new Mainpage());
                        //Navigation.PushModalAsync (new NavigationPage (new Mainpage())); 
                        //#endif
                    }
                    catch
                    {
                    }

                };
                this.layoutbtn = new StackLayout
                {
                    IsVisible = flagisVisible,
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.EndAndExpand,
                    Spacing = 0,
                    Children = { btnSaveLater, btnCnfirm }
                };
                var lblConfirmSketches = new Label
                {
                    Text = "Sketches Confirmed!",
                    FontSize = 14,
                    FontAttributes = FontAttributes.Bold,
                    //TextColor = ColorUtil.ButtonTextColor,
                    HeightRequest = 55
                };

                lblConfirmSketchesIndicator = new ActivityIndicator { IsRunning = true, Color = Xamarin.Forms.Color.FromRgb(125, 0, 3), BackgroundColor = Xamarin.Forms.Color.Red};

                layoutConfirm = new StackLayout
                {
                    IsVisible = flagisvisibleConfirm,
                    Orientation = StackOrientation.Horizontal,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    Padding = new Thickness(0, 20, 0, 0),
                    Children = {
                        lblConfirmSketches,

                    }
                };

                var lblUploadingProgress = new Label { Text = "Uploading Sketches...!!", HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };
                //  
                layoutConfirmIndicator = new StackLayout
                {
                    //IsVisible = GobleClass.IsProgessToUploadImage,
                    Orientation = StackOrientation.Horizontal,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    Padding = new Thickness(0, 10, 0, 10),
                    Children = {
                        lblConfirmSketchesIndicator,lblUploadingProgress

                    }
                };
                //layoutbtn,layoutConfirm
                var saperatorbtn = new BoxView { HeightRequest = 1 };
                layoutAddMore.Children.Add(saperatorbtn);
                layoutAddMore.Children.Add(layoutbtn);
                layoutAddMore.Children.Add(layoutConfirm);
                //layoutAddMore.Children.Add(layoutConfirmIndicator);

                var stack = new StackLayout
                {
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    Children = {    lblAddressLayout,
                        LayoutOrderData
                        }
                };

                var scr = new ScrollView();
                scr.Content = stack;

                var mainlayout = new StackLayout
                {
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    //BackgroundColor = ColorUtil.BackgroudColor,
                    Spacing = 0,
                    Padding = new Thickness(0, 0, 0, 0),
                    Children = {
                        scr,
                        layoutAddMore
                    }
                };



                RelativeLayout relativeLayout = new RelativeLayout();
                relativeLayout.Children.Add(mainlayout,
                    Constraint.Constant(0),
                    Constraint.Constant(0),
                    Constraint.RelativeToParent((parent) =>
                    {
                        return this.Width;
                    }),
                    Constraint.RelativeToParent((parent) =>
                    {
                        return this.Height;
                    }));
                relativeLayout.Children.Add(imgAddMore,
                    Constraint.RelativeToParent((parent) =>
                    {
                        if (Device.Idiom == TargetIdiom.Tablet)
                        {
                            return parent.Width - 90;
                        }
                        else
                        {
                            return parent.Width - 81;
                        }
                    }),
                    Constraint.Constant(screenHeight - 130));

                var custom = new CustomPopup();
                var popupLayouts = this.Content as CustomPopup;
                MainLayoutFloorPlan = new StackLayout();
                MainLayoutFloorPlan.BackgroundColor = Xamarin.Forms.Color.Red;
                MainLayoutFloorPlan.Children.Add(new ScrollView { Content = relativeLayout });

                Content = custom;
                custom.Content = MainLayoutFloorPlan;
            }
            catch (Exception ex)
            {
                //Console.WriteLine("", ex.Message, "");
            }
        }

        public void Getimage(ObservableCollection<OrderImage> orderimgCollection)
        {

            try
            {
                UserDialogs.Instance.ShowLoading("Please Wait!");

                foreach (var fileName in orderimgCollection)
                {

                    this._imgDelete = new Image();
                   
                        _imgDelete.Source = "bluetoothicon.png";
                 
                    this.scrlview = new ScrollView();

                    #if __IOS__
                    double ChildWidthConstant;
                    double ChildHeightConstant;
                    if (Device.Idiom == TargetIdiom.Tablet)
                    {
                        if (ImageType == "Grid")
                        {
                            ChildHeightConstant = -7;
                            ChildWidthConstant = 294;
                        }
                        else
                        {
                            ChildHeightConstant = -10;
                            ChildWidthConstant = 660;
                        }
                    }
                    else
                    {

                        if (App.ScreenHeight == 736 && App.ScreenWidth == 414)
                        {
                            if (ImageType == "Grid")
                            {
                                ChildHeightConstant = -2;
                                ChildWidthConstant = 110;
                            }
                            else
                            {
                                ChildHeightConstant = -2;
                                ChildWidthConstant = 283;
                            }

                        }
                        else
                        {

                            if (ImageType == "Grid")
                            {
                                ChildHeightConstant = -2;
                                ChildWidthConstant = 95;
                            }
                            else
                            {
                                ChildHeightConstant = -2;
                                ChildWidthConstant = 241;
                            }
                        }
                    }

#elif __ANDROID__



                    double ChildWidthConstant;
                    double ChildHeightConstant;
                    //-----
                    if (Device.Idiom == TargetIdiom.Phone) {
                        if (ImageType == "Grid") {
                            ChildHeightConstant = -10;
                            ChildWidthConstant = 107;
                        } else {
                            ChildWidthConstant = 245;
                            ChildHeightConstant = 2;

                        }
                    } 
                    //------
                    else {
                        if (ImageType == "Grid") {
                            ChildHeightConstant = -10;
                            ChildWidthConstant = 280;
                        } else {

                            ChildWidthConstant = 650;
                            ChildHeightConstant = -8;
                        }
                    }

#endif
                    //gesture of imgdelete
                    var tapGestureRecognizer = new TapGestureRecognizer();
                    if (flagisVisible == true)
                    {
                        _imgDelete.GestureRecognizers.Add(tapGestureRecognizer);
                    }
                    tapGestureRecognizer.Tapped += (object sender, EventArgs e) =>
                    {
                        this.ToolbarItems.Clear();
                        if (ImageType == "List")
                        {
                            this.ToolbarItems.Add(Grid);
                        }
                        else
                        {
                            this.ToolbarItems.Add(ToolBarlist);
                        }
                        this.ToolbarItems.Add(Nav);
                        _orderimgId = fileName.Id;
                        DeleteOrderImageByOrderImageId(_orderimgId);
                    };
#if __IOS__
                    this._imgOrder = new Image
                    {
                        Aspect = Aspect.AspectFill,
                        Source = ImageSource.FromStream(() => new MemoryStream(fileName.orderImage)),
                        VerticalOptions = LayoutOptions.StartAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                    };
#elif __ANDROID__
                    this._imgOrder = new CachedImage {
                        CacheDuration = TimeSpan.FromDays(50),
                        DownsampleHeight = 300,
                        RetryCount = 10,
                        RetryDelay = 10,
                        TransparencyEnabled = false,
                        //LoadingPlaceholder = "icon.png",
                        Aspect = Aspect.AspectFill,
                        Source = ImageSource.FromStream (() => new MemoryStream (fileName.orderImage)),
                        VerticalOptions = LayoutOptions.StartAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                    };
#endif
                    //tap of imgorder
                    var tapGestureRecognizerimg = new TapGestureRecognizer();
                    _imgOrder.GestureRecognizers.Add(tapGestureRecognizerimg);

                    tapGestureRecognizerimg.Tapped += (object sender, EventArgs e) =>
                    {
                        this.ToolbarItems.Clear();
                        if (ImageType == "List")
                        {
                            this.ToolbarItems.Add(Grid);
                        }
                        else
                        {
                            this.ToolbarItems.Add(ToolBarlist);
                        }
                        this.ToolbarItems.Add(Nav);
                        _orderimg = fileName.FileName;
                        //Navigation.PushModalAsync(new ZoomOrderImgPage(orderData, fileName.orderImage)
                        //{
                        //    BackgroundColor = Xamarin.Forms.Color.Black
                        //});
                    };
                    RelativeLayout layoutRelative = new RelativeLayout();
                    layoutRelative.Padding = new Thickness(
                        top: Device.OnPlatform(iOS: 10, Android: 20, WinPhone: 0),
                        right: 0,
                        bottom: 0,
                        left: Device.OnPlatform(iOS: 0, Android: 0, WinPhone: 0));
                    layoutRelative.Children.Add(_imgOrder,
                        Constraint.Constant(0),
                        Constraint.Constant(0),

                        Constraint.RelativeToParent((parent) =>
                        {
                        
#if __IOS__
                            if (ImageType == "Grid")
                            {
                                return this.Width / 2 - 30;
                            }
                            else
                            {
                                return this.Width - 40;
                            }
#elif __ANDROID__
                            if (ImageType == "Grid") {
                                return this.Width / 2 - 30;
                            } else {
                                return this.Width - 40;
                            }

#endif
                        }),
                        Constraint.RelativeToParent((parent) =>
                        {
#if __IOS__
                            if (ImageType == "Grid")
                            {
                                return this.Width / 2 - 60;
                            }
                            else
                            {
                                return this.Width - 110;
                            }
#elif __ANDROID__
                            if (ImageType == "Grid") {
                                return this.Width / 2 - 30;
                            } else {
                                return this.Width - 100;
                            }
#endif
                        }));

                    layoutRelative.Children.Add(_imgDelete,
#if __IOS__
                        Constraint.Constant(ChildWidthConstant),
#elif __ANDROID__
                        Constraint.RelativeToParent ((parent) => {

#if __ANDROID__
                            //                          if (ImageType == "Grid") {
                            //                              return this.Width / 2 - 30-36;
                            //                          } else {
                            //                              return this.Width - 40-52;
                            //                          }



                            if (Device.Idiom == TargetIdiom.Phone) {
                                if (ImageType == "Grid") {

                                    var width=this.Width / 2 - 20 - this.Width / 6 - 46;
                                    return this.Width / 2 -width-15;
                                } else {

                                    var width=this.Width - 30 - this.Width / 2 - 100;
                                    return this.Width - 40-width-5;
                                }
                            } else {
                                if (ImageType == "Grid") {
                                    var width=this.Width / 2 - 20 - this.Width / 4 - 94;

                                    return this.Width / 2 - 30-width;
                                } else {

                                    var width=this.Width / 2 - 20 - this.Width / 4 - 90;
                                    return this.Width - 40-width;
                                }
                            }





#endif
                        }),

#endif
                        Constraint.Constant(ChildHeightConstant),
                        Constraint.RelativeToParent((parent) =>
                        {
#if __IOS__
                            if (Device.Idiom == TargetIdiom.Phone)
                            {
                                if (ImageType == "Grid")
                                {
                                    return this.Width / 2 - 20 - this.Width / 6 - 50;
                                }
                                else
                                {
                                    return this.Width - 30 - this.Width / 2 - 90;
                                }
                            }
                            else
                            {
                                if (ImageType == "Grid")
                                {
                                    return this.Width / 2 - 20 - this.Width / 6 - 175;
                                }
                                else
                                {
                                    return this.Width - 30 - this.Width / 2 - 285;
                                }
                            }
#elif __ANDROID__
                            if (Device.Idiom == TargetIdiom.Phone) {
                                if (ImageType == "Grid") {
                                    return this.Width / 2 - 20 - this.Width / 6 - 62;
                                } else {
                                    return this.Width - 30 - this.Width / 2 - 86;
                                }
                            } else {
                                if (ImageType == "Grid") {
                                    return this.Width / 2 - 18 - this.Width / 4 - 100;
                                } else {
                                    return this.Width / 2 - 20 - this.Width / 4 - 90;
                                }
                            }
#endif
                        }),
                        Constraint.RelativeToParent((parent) =>
                        {
#if __IOS__
                            
                                if (ImageType == "Grid")
                                {
                                    return this.Width / 2 - 20 - this.Width / 6 - 50;
                                }
                                else
                                {
                                    return this.Width - 30 - this.Width / 2 - 90;
                                }
                           
                            
                                if (ImageType == "Grid")
                                {
                                    return this.Width / 2 - 20 - this.Width / 6 - 160;
                                }
                                else
                                {
                                    return this.Width - 30 - this.Width / 2 - 270;
                                }
                           
#elif __ANDROID__
                            
                                if (ImageType == "Grid") {

                                    return this.Width / 2 - 20 - this.Width / 6 - 46;
                                } else {
                                    return this.Width - 30 - this.Width / 2 - 100;
                                }
                           
                                if (ImageType == "Grid") {
                                    return this.Width / 2 - 20 - this.Width / 4 - 94;
                                } else {
                                    return this.Width / 2 - 20 - this.Width / 4 - 90;
                                }
                           

#endif
                        }));
                    layout.Padding = new Thickness(11, 5, 0, 0);
#if __IOS__
                    {
                        if (ImageType == "Grid")
                        {
                            block = new Frame { BackgroundColor = Xamarin.Forms.Color.White, Padding = new Thickness(7, 7, 7, 0), HasShadow = false };
                        }
                        else
                        {
                            block = new Frame { BackgroundColor = Xamarin.Forms.Color.White, Padding = new Thickness(7, 7, 7, 0), HasShadow = false };
                        }
                    }
#elif __ANDROID__
                    {
                        
                            if (ImageType == "Grid") {
                                block = new Frame{ BackgroundColor = Xamarin.Forms.Color.White, Padding = new Thickness (7, 7, 7, 0), HasShadow = false };
                            } else {
                                block = new Frame{ BackgroundColor = Xamarin.Forms.Color.White, Padding = new Thickness (7, 7, 0, 0), HasShadow = false };
                            }
                        

                            block = new Frame{ BackgroundColor = Xamarin.Forms.Color.White, Padding = new Thickness (7, 7, 7, 0), HasShadow = false };
                        
                    }

#endif

                    block.Content = layoutRelative;
                    layout.Children.Add(block);
                }
                scrlview.HorizontalOptions = LayoutOptions.FillAndExpand;
                scrlview.VerticalOptions = LayoutOptions.StartAndExpand;
                scrlview.Content = layout;
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
               // Console.WriteLine(ex.Message);
            }
        }


        protected async Task DeleteOrderImageByOrderImageId(int id)
        {
            try
            {
                this.ToolbarItems.Clear();
                if (ImageType == "List")
                {
                    this.ToolbarItems.Add(Grid);
                }
                else
                {
                    this.ToolbarItems.Add(ToolBarlist);
                }
                this.ToolbarItems.Add(Nav);
                string address = "Are you sure you want to delete this image?";
                var answer = await DisplayAlert("Delete Image", address, "Yes", "No");
                if (answer == true)
                {
                    var viewModel1 = new GalleryViewModel(globleOrderId);
                    viewModel1.DeletePhotoByOrderImageId(id);


                }
            }
            catch
            {
            }
        }

    }
}

