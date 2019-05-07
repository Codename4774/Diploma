using GTFS.Entities;
using PublicTransport.Backend.Models;
using PublicTransport.Backend.Services.Time.TimeEvents;
using System;
using System.Collections.Generic;
using System.Text;

namespace PublicTransport.Backend.Services.Time
{
    public interface IArriveTimeManager : IDisposable
    {
        event EventHandler DayChanged;
        event EventHandler OnTick;
        event EventHandler<NearestArriveTimeModel> OnNearestArriveTimeShow;
        event EventHandler<NearestArriveTimeModel> OnNearestArriveTimeHide;
        IArriveTimeNotificator GetArriveTimeNotificator(IEnumerable<string> times);
        IArriveTimeNotificator GetArriveTimeNotificator(IEnumerable<TimeOfDay> times);
        void RemoveArriveTimeNotificator(IArriveTimeNotificator item);
        IEnumerable<NearestArriveTimeModel> AddNearestArriveTimesToProcessing(IEnumerable<NearestArriveTimeModel> nearestArriveTimeModels);
    }
}
