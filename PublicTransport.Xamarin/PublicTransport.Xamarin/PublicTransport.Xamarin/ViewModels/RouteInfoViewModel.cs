using GTFS.Entities;
using PublicTransport.Backend.Services.GTFS;
using PublicTransport.Xamarin.Services;
using PublicTransport.Xamarin.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace PublicTransport.Xamarin.ViewModels
{
    public class RouteInfoViewModel : BaseViewModel
    {
        private Route _route;

        private IEnumerable<Trip> _trips;

        public Route Route
        {
            get
            {
                return _route;
            }
        }

        private IGTFSProvider _GTFSProvider;

        private string _selectedDirection;

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

        private string _routeType;

        public string RouteType
        {
            get
            {
                return _routeType;
            }
            set
            {
                _routeType = value;
                OnPropertyChanged();
            }
        }

        private string _routeNumber;

        public string RouteNumber
        {
            get
            {
                return _routeNumber;
            }
            set
            {
                _routeNumber = value;
                OnPropertyChanged();
            }
        }

        private List<string> _directions;

        public List<string> Directions
        {
            get
            {
                return _directions;
            }
            set
            {
                _directions = value;
                OnPropertyChanged();
            }
        }

        public string SelectedDirection
        {
            get
            {
                return _selectedDirection;
            }
            set
            {
                _selectedDirection = value;
                if (_selectedDirection != null)
                {
                    UpdateData(_selectedDirection);
                }
            }
        }

        public ObservableCollection<Stop> Stops { get; } = new ObservableCollection<Stop>();


        public RouteInfoViewModel()
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
            Route route = (Route)navigationData;

            _route = route;

            RouteName = String.Format("Route name: " + route.LongName);

            RouteType = String.Format("Route type: " + Enum.GetName( typeof(GTFS.Entities.Enumerations.RouteTypeExtended), route.Type));

            RouteNumber = String.Format("Route number: " + route.ShortName);

            _trips = _GTFSProvider.GTFSFeed.Trips.Where(trip => trip.RouteId == _route.Id).ToList();

            Directions = _trips.Select(trip => trip.Headsign).Distinct().Take(2).ToList();

            //SelectedDirection = Directions.First();
        }

        private IEnumerable<Stop> GetStops(Route route, string direction)
        {
            Trip firstTrip = _trips.Where(trip => trip.Headsign == direction && trip.RouteId == route.Id).First();

            IEnumerable<StopTime> stopTimes = _GTFSProvider.GTFSFeed.StopTimes
                .Where(stopTime => stopTime.TripId == firstTrip.Id)
                .OrderBy(stopTime => stopTime.StopSequence);

            Int32 count = stopTimes.Count();

            IEnumerable<Stop> stops = stopTimes.Select(stopTime => _GTFSProvider.GTFSFeed.Stops.Get(stopTime.StopId));

            return stops;
        }

        private void UpdateData(string selectedDirection)
        {
            Stops.Clear();

            var stops = GetStops(_route, selectedDirection);

            foreach (Stop stop in stops)
            {
                Stops.Add(stop);
            }
        }

        public ICommand _addToFavoriteListCommand;
        public ICommand AddToFavoriteListCommand
        {
            get
            {
                _addToFavoriteListCommand = _addToFavoriteListCommand ?? new Command(async (obj) =>
                {
                    Stop stop = (Stop)obj;
                    ServiceProvider.FavoritesListManager.AddToList(stop, _route, SelectedDirection);
                });
                return _addToFavoriteListCommand;
            }
        }
    }
}
