using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using GTFS.Entities;
using Newtonsoft.Json;
using PublicTransport.Backend.Models;
using PublicTransport.Backend.Services.Configuration;
using PublicTransport.Backend.Services.GTFS;
using PublicTransport.Backend.Services.Shedule;

namespace PublicTransport.Backend.Services.FavoritesList
{
    public class FavoritesListManager : IFavoritesListManager
    {
        private Func<string> _loadListFunc;

        private Action<string> _saveListFunc;

        private readonly IGTFSProvider _GTFSProvider;

        private readonly IBackendConfiguration _backendConfiguration;

        private readonly ISheduleManager _sheduleManager;

        private ICollection<FavoriteStop> _favoriteStops;

        public FavoritesListManager(IGTFSProvider GTFSProvider, ISheduleManager sheduleManager, IBackendConfiguration configurationManager, 
            Action<string> saveListFunc, Func<string> loadListFunc)
        {
            _GTFSProvider = GTFSProvider;
            _sheduleManager = sheduleManager;
            _backendConfiguration = configurationManager;
            _saveListFunc = saveListFunc;
            _loadListFunc = loadListFunc;
            _favoriteStops = new List<FavoriteStop>();
            LoadList();
        }

        #region IFavoritesListManager Implementation
        
        public ICollection<FavoriteStop> FavoriteStops
        {
            get
            {
                return _favoriteStops;
            }
        }

        public string AddToList(Stop stop, Route route, string direction)
        {
            string result = "";

            if (_favoriteStops.Where(item => item.direction == direction 
                                             && item.route_id == route.Id 
                                             && item.stop_id == stop.Id).Count() == 0)
            {

                FavoriteStop favoriteStop = new FavoriteStop()
                {
                    direction = direction,
                    stop_id = stop.Id,
                    stop_name = stop.Name,
                    route_id = route.Id,
                    route_long_name = route.LongName,
                    route_short_name = route.ShortName,
                    route_type = _sheduleManager.GetRouteTypeSimple(route.Type),
                    times = _sheduleManager.GetOrderedArriveTimeForAllDays(route, stop)
                        .Select(item => item.ToArray()).ToArray()
                };

                _favoriteStops.Add(favoriteStop);

                ThreadPool.QueueUserWorkItem((state) => {
                    lock (_favoriteStops)
                    {
                        SaveListState();
                    }
                });

                return result;
            }
            else
            {
                result = "Item already was added.";

                return result;
            }
        }

        public void RemoveFromList(FavoriteStop stop)
        {
            _favoriteStops.Remove(stop);
            ThreadPool.QueueUserWorkItem((state) => {
                lock (_favoriteStops)
                {
                    SaveListState();
                }
            });
        }

        public FavoriteStop RemoveFromList(Stop stop, Route route, string direction)
        {
            FavoriteStop stopToRemove = _favoriteStops.Where(item => item.direction == direction
                                             && item.route_id == route.Id
                                             && item.stop_id == route.Id).FirstOrDefault();

            if (stopToRemove != default(FavoriteStop))
            {
                RemoveFromList(stopToRemove);
            }

            return stopToRemove;
        }

        public void SaveListState()
        {
            string result = JsonConvert.SerializeObject(_favoriteStops);

            _saveListFunc(result);
        }

        private void LoadList()
        {
            string listStr = _loadListFunc();

            _favoriteStops = JsonConvert.DeserializeObject<List<FavoriteStop>>(listStr);
        }

        public bool IsItemContained(Stop stop, Route route, string direction)
        {
            return _favoriteStops.Where(item => item.direction == direction
                                             && item.route_id == route.Id
                                             && item.stop_id == stop.Id).Count() != 0;
        }

        public FavoriteStop RemoveFromList(string stopID, string routeID, string direction)
        {
            FavoriteStop stopToRemove = _favoriteStops.Where(item => item.direction == direction
                                             && item.route_id == routeID
                                             && item.stop_id == stopID).FirstOrDefault();

            if (stopToRemove != default(FavoriteStop))
            {
                RemoveFromList(stopToRemove);
            }

            return stopToRemove;
        }

        public bool IsItemContained(string stopID, string routeID, string direction)
        {
            return _favoriteStops.Where(item => item.direction == direction
                                             && item.route_id == routeID
                                             && item.stop_id == stopID).Count() != 0;
        }

        public string GetSerializedList()
        {
            string result = JsonConvert.SerializeObject(_favoriteStops);

            return result;
        }

        #endregion
    }
}
