using GTFS.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PublicTransport.Xamarin.Common
{
    public class TripEqualityComparer<T> : IEqualityComparer<T> where T : Trip
    {
        public bool Equals(T x, T y)
        {
            return x.RouteId.GetHashCode() == y.RouteId.GetHashCode();
        }

        public int GetHashCode(T obj)
        {
            return obj.RouteId.GetHashCode();
        }
    }
}
