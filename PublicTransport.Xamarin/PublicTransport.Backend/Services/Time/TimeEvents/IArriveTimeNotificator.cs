using GTFS.Entities;
using PublicTransport.Backend.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PublicTransport.Backend.Services.Time.TimeEvents
{
    public interface IArriveTimeNotificator
    {
        void UpdateState(DateTime dateTime);
        TimeOfDay NextArriveTime { get; }
        int MinutesToNextArrive { get; }
        event EventHandler NoArrivesToday;
        event EventHandler NextArriveTimeChanged;
        event EventHandler MinutesToNextArriveChanged;
    }
}
