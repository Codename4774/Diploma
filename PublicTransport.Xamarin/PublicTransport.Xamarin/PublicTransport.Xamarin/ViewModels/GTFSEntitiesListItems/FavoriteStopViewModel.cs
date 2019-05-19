using PublicTransport.Backend.Models;
using PublicTransport.Backend.Services.Shedule;
using PublicTransport.Backend.Services.Time;
using PublicTransport.Backend.Services.Time.TimeEvents;
using PublicTransport.Xamarin.Common;
using PublicTransport.Xamarin.Services;
using PublicTransport.Xamarin.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace PublicTransport.Xamarin.ViewModels.GTFSEntitiesListItems
{
    public class FavoriteStopViewModel : BaseViewModel
    {
        private Action<FavoriteStop> _updateListFunc;

        private FavoriteStop _favoriteStop;

        private IArriveTimeNotificator _arriveTimeNotificator;

        private ISheduleManager _sheduleManager;

        public FavoriteStop FavoriteStop
        {
            get
            {
                return _favoriteStop;
            }
        }

        public FavoriteStopViewModel(FavoriteStop favoriteStop, Action<FavoriteStop> updateListFunc, 
            IArriveTimeManager arriveTimeManager, ISheduleManager sheduleManager)
        {
            _favoriteStop = favoriteStop;
            _updateListFunc = updateListFunc;
            _sheduleManager = sheduleManager;
            _arriveTimeNotificator = arriveTimeManager.GetArriveTimeNotificator(_favoriteStop.times[CommonMethods.GetCurrentDay()]);
            _arriveTimeNotificator.MinutesToNextArriveChanged += (sender, e) => MinutesToNextArriveTime = _arriveTimeNotificator.MinutesToNextArrive.ToString() + " min";
            MinutesToNextArriveTime = _arriveTimeNotificator.MinutesToNextArrive.ToString() + " min";
            _arriveTimeNotificator.NextArriveTimeChanged += 
                (sender, e) => NextArriveTime = sheduleManager.FormatTime(_arriveTimeNotificator.NextArriveTime);
        }


        public string Title
        {
            get
            {
                return "Stop: " + _favoriteStop.stop_name + Environment.NewLine +
                    "Route: " + _favoriteStop.route_short_name + Environment.NewLine +
                    "Route type: " + CommonMethods.GetRouteTypeStr(_favoriteStop.route_type);
            }
        }

        private string _nextArriveTime;

        public string NextArriveTime
        {
            get
            {
                return _nextArriveTime;
            }
            set
            {
                _nextArriveTime = value;
                OnPropertyChanged();
            }
        }

        private string _minutesToNextArriveTime;

        public string MinutesToNextArriveTime
        {
            get
            {
                return _minutesToNextArriveTime;
            }
            set
            {
                _minutesToNextArriveTime = value;
                OnPropertyChanged();
            }
        }

        public ImageSource ImagePath
        {
            get
            {
                return CommonMethods.GetIconForRouteType(_favoriteStop.route_type);
            }
        }

        public ImageSource ButtonFavoriteImageSource
        {
            get
            {
                return ServiceProvider.ImageResourceManager.GetImageSourceFromCache(Constants.FAVORITE_BUTTON_REMOVE_ICON_FILE_PATH);
            }
        }

        public ICommand _removeFromFavoriteListCommand;
        public ICommand RemoveFromFavoriteListCommand
        {
            get
            {
                _removeFromFavoriteListCommand = _removeFromFavoriteListCommand ?? new Command(async () =>
                {
                    FavoriteStop removedStop = ServiceProvider.FavoritesListManager
                        .RemoveFromList(_favoriteStop.stop_id, _favoriteStop.route_id, _favoriteStop.direction);
                    _updateListFunc(removedStop);
                });
                return _removeFromFavoriteListCommand;
            }
        }
    }
}
