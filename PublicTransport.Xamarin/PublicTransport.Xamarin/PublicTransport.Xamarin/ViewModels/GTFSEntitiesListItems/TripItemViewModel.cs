using Acr.UserDialogs;
using GTFS.Entities;
using PublicTransport.Backend.Services.FavoritesList;
using PublicTransport.Xamarin.Common;
using PublicTransport.Xamarin.Models;
using PublicTransport.Xamarin.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace PublicTransport.Xamarin.ViewModels.GTFSEntitiesListItems
{
    public class TripItemViewModel : FindedItemViewModel
    {
        private Route _route;

        private Stop _stop;

        protected Trip Trip
        {
            get
            {
                return (Trip)Entity;
            }
            set
            {
                Entity = value;
            }
        }

        public override ImageSource ImagePath
        {
            get
            {
                return ServiceProvider.ImageResourceManager.GetImageSourceFromCache(Constants.STOP_ICON_FILE_PATH);
            }
        }
        public override string Title
        {
            get
            {
                return _route.ShortName;
            }
        }

        public TripItemViewModel(Stop stop, Trip trip, Route route) : base(trip)
        {
            _route = route;
            _stop = stop;
        }

        public ICommand _openDetailsCommand;
        public override ICommand OpenDetailsCommand
        {
            get
            {
                _openDetailsCommand = _openDetailsCommand ?? new Command(async () =>
                {
                    await _navigationService.OpenAsync<RouteStopInfoViewModel>(new RouteStopParameter() { Route = _route, Direction = Trip.Headsign, Stop = _stop });
                });
                return _openDetailsCommand;
            }
        }

        public ICommand _addToFavoriteListCommand;
        public override ICommand AddToFavoriteListCommand
        { 
            get
            {
                _addToFavoriteListCommand = _addToFavoriteListCommand ?? new Command(async () =>
                {
                    string result = ServiceProvider.FavoritesListManager.AddToList(_stop, _route, Trip.Headsign);
                    UserDialogs.Instance.Alert(result == "" ? "Item was added" : result);
                });
                return _addToFavoriteListCommand;
            }
        }
    }
}
