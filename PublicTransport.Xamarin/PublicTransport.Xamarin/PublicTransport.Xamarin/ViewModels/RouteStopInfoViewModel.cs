using GTFS.Entities;
using PublicTransport.Backend.Models;
using PublicTransport.Backend.Services.GTFS;
using PublicTransport.Backend.Services.Shedule;
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

        private ISheduleManager _sheduleManager;

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
            _sheduleManager = ServiceProvider.SheduleManager;
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

            _calendars = _sheduleManager.InitCalendars(_trips);

            _stopTimes = _sheduleManager.InitStopTimes(_trips, _stop);

            Direction = "Direction: " + routeStop.Direction;

            Days = _sheduleManager.GetDays();

            RouteName = _route.LongName;
        }

        private void UpdateData(string day)
        {
            IEnumerable<TimeItem> times = _sheduleManager.GetOrderedArriveTimeByHours(day, _route, _stop, _trips, _stopTimes, _calendars);

            Times.Clear();

            foreach (TimeItem item in times)
            {
                Times.Add(item);
            }
        }
    }
}
