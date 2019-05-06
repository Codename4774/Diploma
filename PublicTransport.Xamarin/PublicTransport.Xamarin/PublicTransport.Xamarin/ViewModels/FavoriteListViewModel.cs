using PublicTransport.Backend.Models;
using PublicTransport.Backend.Services.Configuration;
using PublicTransport.Backend.Services.FavoritesList;
using PublicTransport.Backend.Services.Shedule;
using PublicTransport.Backend.Services.Time;
using PublicTransport.Xamarin.Services;
using PublicTransport.Xamarin.ViewModels.Base;
using PublicTransport.Xamarin.ViewModels.GTFSEntitiesListItems;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace PublicTransport.Xamarin.ViewModels
{
    public class FavoriteListViewModel : BaseViewModel
    {
        private IFavoritesListManager _favoritesListManager;

        private IBackendConfiguration _backendConfiguration;

        private ISheduleManager _sheduleManager;

        private IArriveTimeManager _arriveTimeManager;

        public ObservableCollection<FavoriteStopViewModel> FavoriteList { get; } = new ObservableCollection<FavoriteStopViewModel>();

        public FavoriteListViewModel()
        {
            _backendConfiguration = ServiceProvider.BackendConfiguration;
            _arriveTimeManager = new ArriveTimeManager(_backendConfiguration, false);
            _favoritesListManager = ServiceProvider.FavoritesListManager;
            _sheduleManager = ServiceProvider.SheduleManager;
            InitData();
        }

        private void InitData()
        {
            ICollection<FavoriteStop> favoriteStops = _favoritesListManager.FavoriteStops;

            FavoriteList.Clear();

            foreach (FavoriteStop stop in favoriteStops)
            {
                FavoriteList.Add(new FavoriteStopViewModel(stop, RemoveItem, _arriveTimeManager, _sheduleManager));
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            _arriveTimeManager.Dispose();
        }

        private void RemoveItem(FavoriteStop stop)
        {
            FavoriteStopViewModel itemToRemove = FavoriteList.Where(item => item.FavoriteStop == stop).First();

            FavoriteList.Remove(itemToRemove);
        }
    }
}
