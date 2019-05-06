using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PublicTransport.Backend.Services.Configuration;
using PublicTransport.Backend.Services.FavoritesList;
using PublicTransport.Backend.Services.GTFS;
using PublicTransport.Backend.Services.Shedule;

namespace PublicTransport.Backend.Services
{
    public class BackendServiceProvider
    {
        private static IBackendConfiguration _backendConfiguration;

        private static IGTFSProvider _GTFSProvider;

        private static ISheduleManager _sheduleManager;

        private static IFavoritesListManager _favoritesListManager;

        public static IBackendConfiguration BackendConfiguration
        {
            get
            {
                return _backendConfiguration;
            }
        }

        public static IGTFSProvider GTFSProvider
        {
            get
            {
                return _GTFSProvider;
            }
        }

        public static ISheduleManager SheduleManager
        {
            get
            {
                return _sheduleManager;
            }
        }

        public static IFavoritesListManager FavoritesListManager
        {
            get
            {
                return _favoritesListManager;
            }
        }

        public static void InitializeBackend(Action<string> saveListFunc, Func<string> loadListFunc, object GTFSFeedFromProerties = null)
        {
            _backendConfiguration = new BackendConfiguration();
            _GTFSProvider = new GTFSProvider(_backendConfiguration, GTFSFeedFromProerties);
            _sheduleManager = new SheduleManager(_GTFSProvider);
            _favoritesListManager = new FavoritesListManager(_GTFSProvider, _sheduleManager, _backendConfiguration, saveListFunc, loadListFunc);
        }
    }
}
