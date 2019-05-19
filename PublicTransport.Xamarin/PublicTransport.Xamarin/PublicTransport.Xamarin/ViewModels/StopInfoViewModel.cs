using GTFS.Entities;
using PublicTransport.Backend.Models;
using PublicTransport.Backend.Services.Configuration;
using PublicTransport.Backend.Services.GTFS;
using PublicTransport.Backend.Services.Shedule;
using PublicTransport.Backend.Services.Time;
using PublicTransport.Xamarin.Common;
using PublicTransport.Xamarin.Models;
using PublicTransport.Xamarin.Services;
using PublicTransport.Xamarin.Services.MapManager;
using PublicTransport.Xamarin.ViewModels.Base;
using PublicTransport.Xamarin.ViewModels.GTFSEntitiesListItems;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace PublicTransport.Xamarin.ViewModels
{
    public class StopInfoViewModel : BaseViewModel
    {
        private Stop _stop;

        private IArriveTimeManager _arriveTimeManager;

        private ISheduleManager _sheduleManager;

        public ObservableCollection<TripItemViewModel> Trips { get; } = new ObservableCollection<TripItemViewModel>();

        public ObservableCollection<NearestArriveTimeViewModel> NearestArriveTimes { get; } = new ObservableCollection<NearestArriveTimeViewModel>();

        private ICollection<NearestArriveTimeModel> _nearestArriveTimeModels = new List<NearestArriveTimeModel>();

        private string _stopName;

        public string StopName
        {
            get
            {
                return _stopName;
            }
            set
            {
                _stopName = value;
                OnPropertyChanged();
            }
        }

        private IMapManager _mapManager;

        private IBackendConfiguration _backendConfiguration;

        private IGTFSProvider _GTFSProvider;

        public StopInfoViewModel(Map map)
        {
            _mapManager = new MapManager(map);
            _GTFSProvider = ServiceProvider.GTFSProvider;
            _backendConfiguration = ServiceProvider.BackendConfiguration;
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
            Stop stop = (Stop)navigationData;

            _stop = stop;

            IEnumerable<StopTime> stopTimes = _GTFSProvider.GTFSFeed.StopTimes
                .Where(stopTime => stopTime.StopId == stop.Id)
                .OrderBy(stopTime => stopTime.ArrivalTime.Value.TotalSeconds);

            IEnumerable<StopTimeTripRouteModel> tripsData = stopTimes
                .Join(_GTFSProvider.GTFSFeed.Trips,
                    stopTime => stopTime.TripId,
                    trip => trip.Id,
                    (stopTime, trip) => new { Trip = trip, StopTime = stopTime })
                .Join(_GTFSProvider.GTFSFeed.Routes,
                    tripData => tripData.Trip.RouteId,
                    route => route.Id,
                    (tripData, route) => new StopTimeTripRouteModel() { StopTime = tripData.StopTime,
                        Trip = tripData.Trip,
                        Route = route });

            IEnumerable<Trip> tripsToDisplayShedule = tripsData.Select(item => item.Trip)
                .Distinct(new TripEqualityComparer<Trip>());

            IEnumerable<Route> routes = tripsToDisplayShedule.Join(_GTFSProvider.GTFSFeed.Routes,
                trip => trip.RouteId,
                route => route.Id,
                (trip, route) => route);

            _mapManager.AddStopToMapWithFocus(stop);

            StopName = stop.Name;

            Trips.Clear();

            NearestArriveTimes.Clear();

            foreach (Trip trip in tripsToDisplayShedule)
            {
                Trips.Add(new TripItemViewModel(stop, trip, routes.Where(route => route.Id == trip.RouteId).First()));
            }

            ThreadPool.QueueUserWorkItem((state) =>
            {
                _nearestArriveTimeModels = tripsData.Select(item => new NearestArriveTimeModel(item.StopTime, item.Route)).ToList();

                _arriveTimeManager = new ArriveTimeManager(_backendConfiguration, true);

                var result = _arriveTimeManager.AddNearestArriveTimesToProcessing(_nearestArriveTimeModels)
                    .Select(addedItem => new NearestArriveTimeViewModel(_sheduleManager, addedItem, addedItem.Route));

                foreach (NearestArriveTimeViewModel nearestArriveTimeViewModel in result)
                {
                    NearestArriveTimes.Add(nearestArriveTimeViewModel);                   
                }

                OnPropertyChanged("NearestArriveTimes");

                _arriveTimeManager.OnNearestArriveTimeShow += (sender, e) =>
                {
                    if (NearestArriveTimes.Where(item => item.NearestArriveTimeModel == e).Count() == 0)
                    {
                        NearestArriveTimes.Add(new NearestArriveTimeViewModel(_sheduleManager, e, e.Route));
                        OnPropertyChanged("NearestArriveTimes");
                    }
                };

                _arriveTimeManager.OnNearestArriveTimeHide += (sender, e) =>
                {
                    NearestArriveTimes
                        .Remove(NearestArriveTimes.Where(item => item.NearestArriveTimeModel == e).First());
                    OnPropertyChanged("NearestArriveTimes");
                };

                _arriveTimeManager.OnTick += (sender, e) =>
                {
                    foreach (NearestArriveTimeViewModel nearestArriveTimeViewModel in NearestArriveTimes)
                    {
                        nearestArriveTimeViewModel.UpdateFields();
                    }
                    OnPropertyChanged("NearestArriveTimes");
                };
            }, null);
        }

        public ICommand _showOnMainMapCommand;
        public ICommand ShowOnMainMapCommand
        {
            get
            {
                _showOnMainMapCommand = _showOnMainMapCommand ?? new Command(async () =>
                {
                    MapManager.MainMapInstanse.SetFocusToStop(_stop);
                    await _navigationService.GoToRoot();
                });
                return _showOnMainMapCommand;
            }
        }

        //public ICommand _displayNearestArriveCommand;
        //public ICommand DisplayNearestArriveCommand
        //{
        //    get
        //    {
        //        _displayNearestArriveCommand = _displayNearestArriveCommand ?? new Command(async () =>
        //        {
        //            DisplayRoutes = false;
        //            DisplayNearestArrive = true;
        //        });
        //        return _displayNearestArriveCommand;
        //    }
        //}

        //public ICommand _displayRoutesCommand;
        //public ICommand DisplayRoutesCommand
        //{
        //    get
        //    {
        //        _displayRoutesCommand = _displayRoutesCommand ?? new Command(async () =>
        //        {
        //            DisplayNearestArrive = false;
        //            DisplayRoutes = true;
        //        });
        //        return _displayRoutesCommand;
        //    }
        //}
    }
}
