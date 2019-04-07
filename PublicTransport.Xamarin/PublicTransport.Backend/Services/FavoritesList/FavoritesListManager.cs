using System;
using System.Collections.Generic;
using System.Text;
using GTFS.Entities;
using PublicTransport.Backend.Models;

namespace PublicTransport.Backend.Services.FavoritesList
{
    public class FavoritesListManager : IFavoritesListManager
    {
        private readonly string _favoriteListJSONPath;

        private readonly ICollection<FavoriteStop> _favoriteStops;

        public FavoritesListManager()
        {

        }

        #region IFavoritesListManager Implementation
        
        public ICollection<FavoriteStop> FavoriteStops => throw new NotImplementedException();

        public void AddToFavoriteList(Stop stop, Trip trip, StopTime stopTime, string favoriteName)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
