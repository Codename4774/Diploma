using GTFS.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PublicTransport.Xamarin.Models
{
    public class StopTimeTripRouteModel
    {
        public StopTime StopTime { get; set; }
        public Trip Trip { get; set; }
        public Route Route { get; set; }
    }
}
