using GTFS.Entities;
using PublicTransport.Backend.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PublicTransport.Backend.Services.FavoritesList
{
    public interface IFavoritesListManager
    {
        string AddToList(Stop stop, Route route, string direction);
        void RemoveFromList(FavoriteStop stop);
        FavoriteStop RemoveFromList(Stop stop, Route route, string direction);
        FavoriteStop RemoveFromList(string stopID, string routeID, string direction);
        bool IsItemContained(Stop stop, Route route, string direction);
        bool IsItemContained(string stopID, string routeID, string direction);
        void SaveListState();
        ICollection<FavoriteStop> FavoriteStops { get; }
    }
}
