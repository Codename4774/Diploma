using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PublicTransport.Backend.Services.Configuration;
using PublicTransport.Backend.Services.GTFS;

namespace PublicTransport.Backend.Services
{
    public class BackendServiceProvider
    {
        private static IBackendConfiguration _backendConfiguration;

        private static IGTFSProvider _GTFSProvider;

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

        public static void InitializeBackend(object GTFSFeedFromProerties = null)
        {
            _backendConfiguration = new BackendConfiguration();
            _GTFSProvider = new GTFSProvider(_backendConfiguration, GTFSFeedFromProerties);
        }
    }
}
