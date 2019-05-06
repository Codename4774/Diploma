using GTFS.Entities;
using PublicTransport.Backend.Services.Time.TimeEvents;
using System;
using System.Collections.Generic;
using System.Text;

namespace PublicTransport.Backend.Services.Time
{
    public interface IArriveTimeManager : IDisposable
    {
        event EventHandler DayChanged;
        IArriveTimeNotificator GetArriveTimeNotificator(IEnumerable<string> times);
        IArriveTimeNotificator GetArriveTimeNotificator(IEnumerable<TimeOfDay> times);
    }
}
