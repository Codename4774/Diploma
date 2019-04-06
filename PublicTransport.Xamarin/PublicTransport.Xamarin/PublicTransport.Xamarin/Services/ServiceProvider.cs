using PublicTransport.Backend.Services;
using PublicTransport.Xamarin.Services.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicTransport.Xamarin.Services
{
    public class ServiceProvider : BackendServiceProvider
    {
        private static INavigationService _navigationService;


        public static INavigationService NavigationService
        {
            get
            {
                return _navigationService;
            }
        }

        public static void Initialize()
        {
            InitializeBackend();
            _navigationService = new NavigationService();
        }
    }
}
