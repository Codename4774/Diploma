using GTFS.Entities;
using PublicTransport.Xamarin.Common;
using PublicTransport.Xamarin.Models;
using PublicTransport.Xamarin.Services.ImageResourceManager;
using PublicTransport.Xamarin.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace PublicTransport.Xamarin.Services.MapManager
{
    public class MapManager : BaseViewModel, IMapManager
    {
        private Dictionary<String, BitmapDescriptor> _bitmaps;

        private static IMapManager _mainMap;

        private Map _map;

        private Dictionary<int, StopItem> _stopItems;

        private IImageResourceManager _imageResourceManager;

        public IMapManager MainMap
        {
            get => MainMapInstanse;
            set => MainMapInstanse = value;
        }

        public static IMapManager MainMapInstanse
        {
            get => _mainMap;
            set => _mainMap = value;
        }

        public MapManager(Map map)
        {
            _map = map;
            _imageResourceManager = ServiceProvider.ImageResourceManager;
            _bitmaps = LoadBitmapDescriptors();
            _stopItems = new Dictionary<int, StopItem>();      
        }


        private Dictionary<string, BitmapDescriptor> LoadBitmapDescriptors()
        {
            Dictionary<string, BitmapDescriptor> descriptors = new Dictionary<string, BitmapDescriptor>();

            descriptors.Add(Constants.STOP_ICON_FILE_PATH,
                BitmapDescriptorFactory.FromStream(_imageResourceManager.GetImageFromCache(Constants.STOP_ICON_FILE_PATH))
            );

            return descriptors;
        }

        #region IMapManager implementation

        public StopItem this[int hashCode] => _stopItems[hashCode];

        public void AddStopToMap(Stop stop, bool focus)
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
            if (focus)
            {
                _map.SelectedPin = pin;
            }
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
                AddStopToMap(stop, false);
            }
        }

        public void AddStopToMapWithFocus(Stop stop)
        {
            AddStopToMap(stop, true);
        }

        public void AddStopToMap(Stop stop)
        {
            AddStopToMap(stop, false);
        }

        public void SetVisibilityOfStops(bool visibility)
        {
            foreach (Pin pin in _map.Pins)
            {
                pin.IsVisible = visibility;
            }
        }

        public void SetFocusToStop(Stop stop)
        {
            Pin pin = _stopItems.Where(stopItem => stopItem.Value.Stop.Id == stop.Id).First().Value.Pin;

            _map.MoveCamera(CameraUpdateFactory.NewPositionZoom(
                pin.Position, 16d));

            pin.IsVisible = true;

            _map.SelectedPin = pin;       
        }

        #endregion
    }
}
