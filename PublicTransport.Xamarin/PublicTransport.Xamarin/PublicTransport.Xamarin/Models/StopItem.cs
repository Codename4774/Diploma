using GTFS.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.GoogleMaps;

namespace PublicTransport.Xamarin.Models
{
    public class StopItem
    {
        public Pin Pin { get; set; }
        public Stop Stop { get; set; }

        public override int GetHashCode()
        {
            return Pin.GetHashCode();
        }
    }
}
