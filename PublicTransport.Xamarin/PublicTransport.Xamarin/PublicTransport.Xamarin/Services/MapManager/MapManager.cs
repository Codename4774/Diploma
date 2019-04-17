﻿using GTFS.Entities;
using PublicTransport.Xamarin.Common;
using PublicTransport.Xamarin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Xamarin.Forms.GoogleMaps;

namespace PublicTransport.Xamarin.Services.MapManager
{
    public class MapManager : IMapManager
    {
        private Dictionary<String, BitmapDescriptor> _bitmaps;

        private Map _map;

        private Dictionary<int, StopItem> _stopItems;

        public MapManager(Map map)
        {
            _map = map;
            _bitmaps = LoadBitmapDescriptors();
            _stopItems = new Dictionary<int, StopItem>();
        }


        private Dictionary<string, BitmapDescriptor> LoadBitmapDescriptors()
        {
            Assembly assembly = typeof(MapManager).GetTypeInfo().Assembly;
            Dictionary<string, BitmapDescriptor> descriptors = new Dictionary<string, BitmapDescriptor>();

            descriptors.Add(Constants.STOP_ICON_FILE_PATH, 
                BitmapDescriptorFactory.FromStream(GetResourceStream(Constants.STOP_ICON_FILE_PATH, assembly), Constants.STOP_ICON_FILE_PATH)
            );

            return descriptors;
        }

        private static System.IO.Stream GetResourceStream(string fileName, Assembly assembly)
        {
            return assembly.GetManifestResourceStream($"PublicTransport.Xamarin.Images.{fileName}") ?? assembly.GetManifestResourceStream($"PublicTransport.Xamarin.local.{fileName}");
        }

        #region IMapManager implementation

        public StopItem this[int hashCode] => _stopItems[hashCode];

        public void AddStopToMap(Stop stop)
        {
            Pin pin = new Pin()
            {
                Position = new Position(stop.Latitude, stop.Longitude),
                Label = stop.Name,
                Icon = _bitmaps[Constants.STOP_ICON_FILE_PATH]
            };

            StopItem stopItem = new StopItem() { Stop = stop, Pin = pin };

            _stopItems.Add(stopItem.GetHashCode(), stopItem);
            _map.Pins.Add(stopItem.Pin);
        }

        public void ClearMap()
        {
            _stopItems.Clear();
            _map.Pins.Clear();
        }

        public void RemoveStopFromMap(int stopHashCode)
        {
            StopItem removedStopItem = _stopItems[stopHashCode];

            _stopItems.Remove(stopHashCode);

            _map.Pins.Remove(removedStopItem.Pin);
        }

        public void SetVisibilityOfStop(int stopHashCode, bool visibility)
        {
            _stopItems[stopHashCode].Pin.IsVisible = visibility;
        }

        public void AddStopsToMap(IEnumerable<Stop> stops)
        {
            foreach (Stop stop in stops)
            {
                AddStopToMap(stop);
            }
        }

        #endregion
    }
}
