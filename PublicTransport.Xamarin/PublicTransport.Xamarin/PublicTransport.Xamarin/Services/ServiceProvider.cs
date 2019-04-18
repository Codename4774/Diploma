using Akavache;
using GTFS;
using PublicTransport.Backend.Services;
using PublicTransport.Xamarin.Common;
using PublicTransport.Xamarin.Services.Navigation;
using System.Reactive.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PublicTransport.Xamarin.Services.ImageResourceManager;
using System.Reflection;

namespace PublicTransport.Xamarin.Services
{
    public class ServiceProvider : BackendServiceProvider
    {
        private static INavigationService _navigationService;

        private static IImageResourceManager _imageResourceManager;

        public static INavigationService NavigationService
        {
            get
            {
                return _navigationService;
            }
        }

        public static IImageResourceManager ImageResourceManager
        {
            get
            {
                return _imageResourceManager;
            }
        }

        public static void Initialize()
        {
            InitializeBackend();
            _navigationService = new NavigationService();
            _imageResourceManager = new PublicTransport.Xamarin.Services.ImageResourceManager.ImageResourceManager(
                typeof(ServiceProvider).GetTypeInfo().Assembly);
        }
    }
}