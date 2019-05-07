﻿using Akavache;
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
using PublicTransport.Xamarin.Services.Bluetooth;
using Xamarin.Forms;

namespace PublicTransport.Xamarin.Services
{
    public class ServiceProvider : BackendServiceProvider
    {
        private static INavigationService _navigationService;

        private static IImageResourceManager _imageResourceManager;

        private static IBluetoothService _bluetoothService;

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

        public static IBluetoothService BluetoothService
        {
            get
            {
                return _bluetoothService;
            }
        }

        public static void Initialize()
        {
            InitializeBackend((data) => {
                App.Current.Properties["favorite_list"] = data;
                App.Current.SavePropertiesAsync();
            },
            () => {
                if (App.Current.Properties.ContainsKey("favorite_list"))
                {
                    return App.Current.Properties["favorite_list"].ToString();
                }
                else
                {
                    return "[]";
                }
            });
            _navigationService = new NavigationService();
            _bluetoothService = DependencyService.Get<IBluetoothService>();
            _imageResourceManager = new PublicTransport.Xamarin.Services.ImageResourceManager.ImageResourceManager(
                typeof(ServiceProvider).GetTypeInfo().Assembly);
        }
    }
}