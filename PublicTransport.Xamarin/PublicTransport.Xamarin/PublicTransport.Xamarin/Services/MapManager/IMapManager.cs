using GTFS.Entities;
using PublicTransport.Xamarin.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PublicTransport.Xamarin.Services.MapManager
{
    public interface IMapManager
    {
        StopItem this[int hashCode] { get; }
        void AddStopToMap(Stop stop);
        void AddStopsToMap(IEnumerable<Stop> stops);
        void RemoveStopFromMap(int stopHashCode);
        void ClearMap();
        void SetVisibilityOfStop(int stopHashCode, bool visibility);
    }
}
