using GTFS.Entities;
using PublicTransport.Backend.Services.GTFS;
using PublicTransport.Xamarin.Common;
using PublicTransport.Xamarin.Services;
using PublicTransport.Xamarin.Services.MapManager;
using PublicTransport.Xamarin.ViewModels.Base;
using PublicTransport.Xamarin.ViewModels.GTFSEntitiesListItems;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace PublicTransport.Xamarin.ViewModels
{
    public class StopInfoViewModel : BaseViewModel
    {
        public ObservableCollection<TripItemViewModel> Trips { get; } = new ObservableCollection<TripItemViewModel>();

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

        private IGTFSProvider _GTFSProvider;

        public StopInfoViewModel(Map map)
        {
            _mapManager = new MapManager(map);
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
            Stop stop = (Stop)navigationData;

            IEnumerable<Trip> trips = _GTFSProvider.GTFSFeed.StopTimes.Where(stopTime => stopTime.StopId == stop.Id)
                .Join(_GTFSProvider.GTFSFeed.Trips,
                    stopTime => stopTime.TripId,
                    trip => trip.Id,
                    (stopTime, trip) => trip)
                .Distinct(new TripEqualityComparer<Trip>());

            _mapManager.AddStopToMapWithFocus(stop);

            StopName = stop.Name;

            Trips.Clear();

            foreach (Trip trip in trips)
            {
                Trips.Add(new TripItemViewModel(stop, trip, _GTFSProvider.GTFSFeed.Routes.Get(trip.RouteId)));
            }
        }

        public ICommand _showOnMainMapCommand;
        public ICommand ShowOnMainMapCommand
        {
            get
            {
                _showOnMainMapCommand = _showOnMainMapCommand ?? new Command(async () =>
                {
                    //open details page for Stops
                });
                return _showOnMainMapCommand;
            }
        }
    }
}
