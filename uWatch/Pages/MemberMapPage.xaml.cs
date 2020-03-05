using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using UwatchPCL;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace uWatch
{
    public partial class MemberMapPage : ContentPage
    {

        public MemberMapPage(List<LatLong> items)
        {

            getPin(items);



        }
        public async void getPin(List<LatLong> items)
        {

            InitializeComponent();
            if (items.Count != 0)
            {
                Map MyMap = new Map();
                // MyMap.IsShowingUser = true;
                MyMap.MapType = MapType.Satellite;

                Content = MyMap;
                foreach (var pinPos in items)
                {

                    var pos = new Position(Convert.ToDouble(pinPos.lat), Convert.ToDouble(pinPos.lng));
                    var pin = new Pin
                    {
                        Type = PinType.Place,
                        Position = pos,
                    };
                    pin.Label = pos.Latitude.ToString() + "," + pos.Longitude.ToString();
                    MyMap.Pins.Add(pin);
                    //MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(pos, Distance.FromMiles(1.0)));

                }
                double lowestLat = (double)items.Min(x => x.lat);
                double highestLat = (double)items.Max(x => x.lat);
                double lowestLong = (double)items.Min(x => x.lng);
                double highestLong = (double)items.Max(x => x.lng);
                double finalLat = (lowestLat + highestLat) / 2;
                double finalLong = (lowestLong + highestLong) / 2;
                double distance = DistanceCalculation.GeoCodeCalc.CalcDistance(lowestLat, lowestLong, highestLat, highestLong, DistanceCalculation.GeoCodeCalcMeasurement.Kilometers);
                MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(finalLat, finalLong), Distance.FromKilometers(distance)));
            }
        }
    }
    public class LatLong
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }
}

