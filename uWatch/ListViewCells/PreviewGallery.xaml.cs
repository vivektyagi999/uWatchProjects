using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace uWatch
{
    public partial class PreviewGallery : ContentView
    {
        double currentScale = 1;
        double startScale = 1;
        double xOffset = 0;
        double yOffset = 0;
        string baseImage;

        public PreviewGallery()
        {
            InitializeComponent();
            //imgPreview.HeightRequest = App.ScreenHeight;
        }

        protected override void OnBindingContextChanged()
        {
            var item = BindingContext as ImagePreview;
            if(item!=null)
            {
                string htmlsource = "<style>img{ height: 100%; padding:0px; margin: 0px; max-width: 100%; width:100%;}\n</style><html>\n<head>\n</head>\n<body style =\"background-color:black\">\n\n<img src = " + item.AssetImage + " />\n</body>\n</html>";
                var htmlSource = new HtmlWebViewSource();
                htmlSource.Html = @htmlsource;
                imgWebView.Source = htmlSource;
            }
        }

        void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        {
            if (e.Status == GestureStatus.Started)
            {
                // Store the current scale factor applied to the wrapped user interface element,
                // and zero the components for the center point of the translate transform.
                startScale = Content.Scale;
                Content.AnchorX = 0;
                Content.AnchorY = 0;
            }
            if (e.Status == GestureStatus.Running)
            {
                // Calculate the scale factor to be applied.
                currentScale += (e.Scale - 1) * startScale;
                currentScale = Math.Max(1, currentScale);

                // The ScaleOrigin is in relative coordinates to the wrapped user interface element,
                // so get the X pixel coordinate.
                double renderedX = Content.X + xOffset;
                double deltaX = renderedX / Width;
                double deltaWidth = Width / (Content.Width * startScale);
                double originX = (e.ScaleOrigin.X - deltaX) * deltaWidth;

                // The ScaleOrigin is in relative coordinates to the wrapped user interface element,
                // so get the Y pixel coordinate.
                double renderedY = Content.Y + yOffset;
                double deltaY = renderedY / Height;
                double deltaHeight = Height / (Content.Height * startScale);
                double originY = (e.ScaleOrigin.Y - deltaY) * deltaHeight;

                // Calculate the transformed element pixel coordinates.
                double targetX = xOffset - (originX * Content.Width) * (currentScale - startScale);
                double targetY = yOffset - (originY * Content.Height) * (currentScale - startScale);

                // Apply translation based on the change in origin.  
                //Content.TranslationX = targetX.Clamp(-Content.Width * (currentScale - 1), 0);
                //Content.TranslationY = targetY.Clamp(-Content.Height * (currentScale - 1), 0);

                // Apply scale factor  
                Content.Scale = currentScale;
            }
            if (e.Status == GestureStatus.Completed)
            {
                // Store the translation delta's of the wrapped user interface element.  
                xOffset = Content.TranslationX;
                yOffset = Content.TranslationY;
            }
        }
    }
}
