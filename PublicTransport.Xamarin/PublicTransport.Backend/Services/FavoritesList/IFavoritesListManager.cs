using GTFS.Entities;
using PublicTransport.Backend.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PublicTransport.Backend.Services.FavoritesList
{
    public interface IFavoritesListManager
    {
        void AddToFavoriteList(Stop stop, Trip trip, StopTime stopTime, string favoriteName);
        ICollection<FavoriteStop> FavoriteStops { get; }
    }
}
