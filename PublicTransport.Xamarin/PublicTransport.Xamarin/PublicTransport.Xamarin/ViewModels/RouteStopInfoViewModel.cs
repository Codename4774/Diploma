using GTFS.Entities;
using PublicTransport.Backend.Services.GTFS;
using PublicTransport.Xamarin.Models;
using PublicTransport.Xamarin.Services;
using PublicTransport.Xamarin.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace PublicTransport.Xamarin.ViewModels
{
    public class RouteStopInfoViewModel : BaseViewModel
    {
        private Route _route;

        private Stop _stop;

        private IEnumerable<Trip> _trips;

        private IEnumerable<Calendar> _calendars;

        private IEnumerable<StopTime> _stopTimes;

        private IGTFSProvider _GTFSProvider;

        private string _direction;

        private string _selectedDay;

        private string _routeName;

        public string RouteName
        {
            get
            {
                return _routeName;
            }
            set
            {
                _routeName = value;
                OnPropertyChanged();
            }
        }

        public string Direction
        {
            get
            {
                return _direction;
            }
            set
            {
                _direction = value;
                OnPropertyChanged();
            }
        }

        private List<string> _days;

        public List<string> Days
        {
            get
            {
                return _days;
            }
            set
            {
                _days = value;
                OnPropertyChanged();
            }
        }

        public string SelectedDay
        {
            get
            {
                return _selectedDay;
            }
            set
            {
                _selectedDay = value;
                if (_selectedDay != null)
                {
                    UpdateData(_selectedDay);
                }
            }
        }

        public ObservableCollection<TimeItem> Times { get; } = new ObservableCollection<TimeItem>();


        public RouteStopInfoViewModel()
        {
            _GTFSProvider = ServiceProvider.GTFSProvider;
        }


        public override void Initialize(object navigationData = null)
        {
            base.Initialize(navigationData);

            if (navigationData != null)
            {
                InitData(navigationData);
            }
        }

        private void InitData(object navigationData)
        {
            RouteStopParameter routeStop = (RouteStopParameter)navigationData;

            _route = routeStop.Route;

            _stop = routeStop.Stop;

            _trips = _GTFSProvider.GTFSFeed.Trips.Where(trip => trip.RouteId == _route.Id).ToList();

            _calendars = GetCalendars(_trips);

            _stopTimes = InitStopTimes(_trips, _stop);

            Direction = "Direction: " + routeStop.Direction;

            Days = GetDays();

            RouteName = _route.LongName;
        }

        private DayOfWeek DayOfWeekFromString(string day)
        {
            return (DayOfWeek)(Enum.Parse(typeof(DayOfWeek), day));
        }

        private List<string> GetDays()
        {
            return (new[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" }).ToList();
        }

        private IEnumerable<Calendar> GetCalendars(IEnumerable<Trip> trips)
        {
            List<Calendar> calendars = new List<Calendar>();

            foreach (Trip trip in trips)
            {
                calendars.Add(_GTFSProvider.GTFSFeed.Calendars.Where(calendar => calendar.ServiceId == trip.ServiceId).First());
            }

            return calendars;
        }

        private void UpdateData(string day)
        {
            DayOfWeek dayOfWeek = DayOfWeekFromString(day);

            IEnumerable<Trip> trips = GetTrips(_trips, _calendars, dayOfWeek);

            IEnumerable<StopTime> stopTimes = GetStopTimes(trips, _stopTimes, _stop);

            IEnumerable<IGrouping<int, StopTime>> stopTimesGroupedByHours = stopTimes.GroupBy(stopTime => stopTime.ArrivalTime.Value.Hours).OrderBy(groupedItem => groupedItem.Key);

            Times.Clear();

            foreach (IGrouping<int, StopTime> groupedItems in stopTimesGroupedByHours)
            {
                Times.Add(new TimeItem(groupedItems));
            }
        }

        private IEnumerable<StopTime> GetStopTimes(IEnumerable<Trip> trips, IEnumerable<StopTime> stopTimes, Stop stop)
        {
            IEnumerable<string> tripIDs = trips.Select(trip => trip.Id);

            return stopTimes.Where(stopTime => stopTime.StopId == stop.Id && tripIDs.Contains(stopTime.TripId));
        }

        private IEnumerable<StopTime> InitStopTimes(IEnumerable<Trip> trips, Stop stop)
        {
            IEnumerable<string> tripIDs = trips.Select(trip => trip.Id).ToList();

            List<StopTime> result = new List<StopTime>();

            var selector = _GTFSProvider.GTFSFeed.StopTimes.Where(stopTime => stopTime.StopId == stop.Id && tripIDs.Contains(stopTime.TripId));

            foreach (StopTime stopTime in selector)
            {
                result.Add(stopTime);
            }

            return result;
        }

        private IEnumerable<Trip> GetTrips(IEnumerable<Trip> trips, IEnumerable<Calendar> calendars, DayOfWeek dayOfWeek)
        {
            IEnumerable<Calendar> selectedCalendars = calendars.Where(calendar => calendar[dayOfWeek]);

            IEnumerable<string> serviceIDs = selectedCalendars.Select(calendar => calendar.ServiceId);

            return trips.Where(trip => serviceIDs.Contains(trip.ServiceId));
        }
    }
}
