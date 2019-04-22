using GTFS.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PublicTransport.Xamarin.Models
{
    public class RouteStopParameter
    {
        public Stop Stop { get; set; }
        public Route Route { get; set; }
        public string Direction { get; set; }
    }
}
