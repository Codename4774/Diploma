using GTFS.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PublicTransport.Backend.Models
{
    public class NearestArriveTimeModel
    {
        public NearestArriveTimeModel(StopTime stopTime, Route route)
        {
            StopTime = stopTime;
            Route = route;
            ArriveTime = StopTime.ArrivalTime.HasValue ? StopTime.ArrivalTime.Value : default(TimeOfDay);
        }

        public StopTime StopTime { get; set; }
        public Route Route { get; set; }
        public TimeOfDay ArriveTime { get; set; }
        public int MinutesToArrive { get; set; }
    }
}
