using GTFS.Entities;
using PublicTransport.Xamarin.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PublicTransport.Xamarin.Services.MapManager
{
    public interface IMapManager
    {
        IMapManager MainMap { get; set; }
        StopItem this[int hashCode] { get; }
        void SetFocusToStop(Stop stop);
        void AddStopToMap(Stop stop);
        void AddStopsToMap(IEnumerable<Stop> stops);
        void RemoveStopFromMap(int stopHashCode);
        void ClearMap();
        void AddStopToMapWithFocus(Stop stop);
        void SetVisibilityOfStop(int stopHashCode, bool visibility);
        void SetVisibilityOfStops(bool visibility);
    }
}
