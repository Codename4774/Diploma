﻿using PublicTransport.Backend.Services.GTFS;
using PublicTransport.Xamarin.Services;
using PublicTransport.Xamarin.Services.MapManager;
using PublicTransport.Xamarin.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace PublicTransport.Xamarin.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private IMapManager _mapManager;
        private IGTFSProvider _GTFSProvider;

        public bool ShowDetailsPage { get; set; }

        public MainViewModel(Map map)
        {
            _GTFSProvider = ServiceProvider.GTFSProvider;
            _mapManager = new MapManager(map);
            if (_GTFSProvider.IsInited)
            {
                _mapManager.AddStopsToMap(_GTFSProvider.GTFSFeed.Stops);
            }
            else
            {
                _GTFSProvider.InitCompleted += (sender, e) =>
                {
                    _mapManager.AddStopsToMap(_GTFSProvider.GTFSFeed.Stops);
                };
            }
        }

        public ICommand _findButtonCommand;
        public ICommand FindButtonCommand
        {
            get
            {
                _findButtonCommand = _findButtonCommand ?? new Command(async () =>
                {
                });
                return _findButtonCommand;
            }
        }

        private ICommand _openDetailsPage;
        public ICommand OpenDetailsPageCommand
        {
            get
            {
                _openDetailsPage = _openDetailsPage ?? new Command(async () =>
                {
                    ShowDetailsPage = true;
                });
                return _openDetailsPage;
            }
        }
    }
}
