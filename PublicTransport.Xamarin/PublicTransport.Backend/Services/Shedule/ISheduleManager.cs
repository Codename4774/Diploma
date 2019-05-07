using GTFS.Entities;
using GTFS.Entities.Enumerations;
using PublicTransport.Backend.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PublicTransport.Backend.Services.Shedule
{
    public interface ISheduleManager
    {
        IEnumerable<TimeOfDay> GetOrderedArriveTimeOfDay(string dayOfWeek, Route route, Stop stop, IEnumerable<Trip> trips = null, IEnumerable<StopTime> stopTimes = null, IEnumerable<Calendar> calendars = null);
        IEnumerable<string> GetOrderedArriveTime(string dayOfWeek, Route route, Stop stop, IEnumerable<Trip> trips = null, IEnumerable<StopTime> stopTimes = null, IEnumerable<Calendar> calendars = null);
        IEnumerable<IEnumerable<string>> GetOrderedArriveTimeForAllDays(Route route, Stop stop, IEnumerable<Trip> trips = null, IEnumerable<StopTime> stopTimes = null, IEnumerable<Calendar> calendars = null);
        IEnumerable<TimeItem> GetOrderedArriveTimeByHours(string dayOfWeek, Route route, Stop stop, IEnumerable<Trip> trips = null, IEnumerable<StopTime> stopTimes = null, IEnumerable<Calendar> calendars = null);
        IEnumerable<IEnumerable<TimeItem>> GetOrderedArriveTimeForAllDaysByHours(Route route, Stop stop, IEnumerable<Trip> trips = null, IEnumerable<StopTime> stopTimes = null, IEnumerable<Calendar> calendars = null);
        IEnumerable<Calendar> InitCalendars(IEnumerable<Trip> trips);
        IEnumerable<StopTime> InitStopTimes(IEnumerable<Trip> trips, Stop stop);
        int GetRouteTypeSimple(RouteTypeExtended routeTypeExtended);
        DayOfWeek DayOfWeekFromString(string day);
        string[] GetDays();
        string FormatTime(TimeOfDay timeOfDay);
        string GetCurrentDay();
    }
}
