using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PublicTransport.Backend.Services.Configuration;

namespace PublicTransport.Backend.Services
{
    public class BackendServiceProvider
    {
        private static IBackendConfiguration _backendConfiguration;


        public static IBackendConfiguration BackendConfiguration
        {
            get
            {
                return _backendConfiguration;
            }
        }

        public static void InitializeBackend()
        {
            _backendConfiguration = new BackendConfiguration();
        }
    }
}
