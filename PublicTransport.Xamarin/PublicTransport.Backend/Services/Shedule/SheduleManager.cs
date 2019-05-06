using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTFS.Entities;
using GTFS.Entities.Enumerations;
using PublicTransport.Backend.Models;
using PublicTransport.Backend.Services.GTFS;

namespace PublicTransport.Backend.Services.Shedule
{
    public class SheduleManager : ISheduleManager
    {
        private readonly IGTFSProvider _GTFSProvider;

        public SheduleManager(IGTFSProvider GTFSProvider)
        {
            _GTFSProvider = GTFSProvider;
        }

        public IEnumerable<string> GetOrderedArriveTime(string dayOfWeek, Route route, Stop stop, IEnumerable<Trip> trips = null, IEnumerable<StopTime> stopTimes = null, IEnumerable<Calendar> calendars = null)
        {
            IEnumerable<StopTime> stopTimesToProcess = GetStopTimesToProcess(dayOfWeek, route, stop, trips, stopTimes, calendars);

            return stopTimesToProcess.Select(stopTime => stopTime.ArrivalTime.HasValue ?
                                                            FormatTime(stopTime.ArrivalTime.Value)
                                                            :
                                                            null)
                .Where(stopTimeStr => stopTimeStr != null)
                .OrderBy(stopTimeStr => stopTimeStr);
        }

        public IEnumerable<IEnumerable<string>> GetOrderedArriveTimeForAllDays(Route route, Stop stop, IEnumerable<Trip> trips = null, IEnumerable<StopTime> stopTimes = null, IEnumerable<Calendar> calendars = null)
        {
            List<IEnumerable<string>> result = new List<IEnumerable<string>>();

            var days = GetDays();

            foreach (var day in days)
            {
                result.Add(GetOrderedArriveTime(day, route, stop, trips, stopTimes, calendars));
            }

            return result;
        }


        public IEnumerable<TimeItem> GetOrderedArriveTimeByHours(string dayOfWeek, Route route, Stop stop, IEnumerable<Trip> trips = null, IEnumerable<StopTime> stopTimes = null, IEnumerable<Calendar> calendars = null)
        {
            IEnumerable<StopTime> stopTimesToProcess = GetStopTimesToProcess(dayOfWeek, route, stop, trips, stopTimes, calendars);

            IEnumerable<IGrouping<int, StopTime>> stopTimesGroupedByHours = stopTimesToProcess.GroupBy(stopTime => stopTime.ArrivalTime.Value.Hours).OrderBy(groupedItem => groupedItem.Key);

            List<TimeItem> result = new List<TimeItem>();

            foreach (IGrouping<int, StopTime> groupedItems in stopTimesGroupedByHours)
            {
                result.Add(new TimeItem(groupedItems));
            }

            return result;
        }

        public IEnumerable<IEnumerable<TimeItem>> GetOrderedArriveTimeForAllDaysByHours(Route route, Stop stop, IEnumerable<Trip> trips = null, IEnumerable<StopTime> stopTimes = null, IEnumerable<Calendar> calendars = null)
        {
            List<IEnumerable<TimeItem>> result = new List<IEnumerable<TimeItem>>();

            var days = GetDays();

            foreach (var day in days)
            {
                result.Add(GetOrderedArriveTimeByHours(day, route, stop, trips, stopTimes, calendars));
            }

            return result;
        }

        public string FormatTime(TimeOfDay timeOfDay)
        {
            return timeOfDay.Hours.ToString("00") + ":" + timeOfDay.Minutes.ToString("00") + ":" + timeOfDay.Seconds.ToString("00");
        }

        public DayOfWeek DayOfWeekFromString(string day)
        {
            return (DayOfWeek)(Enum.Parse(typeof(DayOfWeek), day));
        }

        public List<string> GetDays()
        {
            return (new[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" }).ToList();
        }

        private IEnumerable<StopTime> GetStopTimesToProcess(string dayOfWeek, Route route, Stop stop, IEnumerable<Trip> trips = null, IEnumerable<StopTime> stopTimes = null, IEnumerable<Calendar> calendars = null)
        {
            DayOfWeek dayOfWeekEnum = DayOfWeekFromString(dayOfWeek);

            IEnumerable<Trip> tripsToProcess = GetTrips(trips, calendars, route, dayOfWeekEnum);

            IEnumerable<StopTime> stopTimesToProcess = GetStopTimes(tripsToProcess, stopTimes, stop, route);

            return stopTimesToProcess;
        }

        private IEnumerable<StopTime> GetStopTimes(IEnumerable<Trip> trips, IEnumerable<StopTime> stopTimes, Stop stop, Route route)
        {
            stopTimes = stopTimes ?? InitStopTimes(trips, stop);

            trips = trips ?? _GTFSProvider.GTFSFeed.Trips.Where(trip => trip.RouteId == route.Id).ToList();

            IEnumerable<string> tripIDs = trips.Select(trip => trip.Id);

            IDictionary<string, string> tripIDsDictionary = tripIDs.ToDictionary(tripID => tripID);

            return stopTimes.Where(stopTime => stopTime.StopId == stop.Id && tripIDsDictionary.ContainsKey(stopTime.TripId));
        }

        public IEnumerable<StopTime> InitStopTimes(IEnumerable<Trip> trips, Stop stop)
        {
            IEnumerable<string> tripIDs = trips.Select(trip => trip.Id).ToList();

            IDictionary<string, string> tripIDsDictionary = tripIDs.ToDictionary(tripID => tripID);

            List<StopTime> result = new List<StopTime>();

            var selector = _GTFSProvider.GTFSFeed.StopTimes.Where(stopTime => stopTime.StopId == stop.Id && tripIDsDictionary.ContainsKey(stopTime.TripId));

            foreach (StopTime stopTime in selector)
            {
                result.Add(stopTime);
            }

            return result;
        }

        private IEnumerable<Trip> GetTrips(IEnumerable<Trip> trips, IEnumerable<Calendar> calendars, Route route, DayOfWeek dayOfWeek)
        {
            trips = trips ?? _GTFSProvider.GTFSFeed.Trips.Where(trip => trip.RouteId == route.Id).ToList();

            IEnumerable<Calendar> selectedCalendars = calendars != null ?
                calendars.Where(calendar => calendar[dayOfWeek])
                :
                InitCalendars(trips).Where(calendar => calendar[dayOfWeek]);

            IEnumerable<string> serviceIDs = selectedCalendars.Select(calendar => calendar.ServiceId);

            IDictionary<string, object> serviceIDsDictionary = new Dictionary<string, object>();

            foreach (var serviceID in serviceIDs)
            {
                if (!serviceIDsDictionary.ContainsKey(serviceID))
                {
                    serviceIDsDictionary.Add(serviceID, null);
                }
            }

            return trips.Where(trip => serviceIDsDictionary.ContainsKey(trip.ServiceId));
        }


        public IEnumerable<Calendar> InitCalendars(IEnumerable<Trip> trips)
        {
            List<Calendar> calendars = new List<Calendar>();

            foreach (Trip trip in trips)
            {
                calendars.Add(_GTFSProvider.GTFSFeed.Calendars.Where(calendar => calendar.ServiceId == trip.ServiceId).First());
            }

            return calendars;
        }

        public int GetRouteTypeSimple(RouteTypeExtended routeTypeExtended)
        {
            return (int)((int)routeTypeExtended / 100);
        }
    }
}
