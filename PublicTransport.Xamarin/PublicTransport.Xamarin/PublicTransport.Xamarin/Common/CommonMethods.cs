using GTFS.Entities;
using GTFS.Entities.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace PublicTransport.Xamarin.Common
{
    public static class CommonMethods
    {
        public static int GetCurrentDay()
        {
            DateTime currentDateTime = DateTime.Now;

            return (int)currentDateTime.DayOfWeek;
        }

        public static int MinutesDiffWithCurrent(TimeOfDay timeOfDay)
        {
            DateTime current = DateTime.Now;

            return (current.Hour * 60 + current.Minute - timeOfDay.Hours * 60 - timeOfDay.Minutes);
        }

        public static string GetRouteTypeStr(int routeType)
        {
            switch (routeType)
            {
                case 0:
                    {
                        return "Tram";
                    }
                    break;
                case 1:
                    {
                        return "Metro";
                    }
                    break;
                case 2:
                    {
                        return "Rail";
                    }
                    break;
                case 3:
                    {
                        return "Bus";
                    }
                    break;
                case 4:
                    {
                        return "Ferry";
                    }
                    break;
                case 5:
                    {
                        return "Cable tram";
                    }
                    break;
                case 6:
                    {
                        return "Aerial lift";
                    }
                    break;
                case 7:
                    {
                        return "Funicular";
                    }
                    break;
                default:
                    {
                        return "Unknown";
                    }
    
                }
            }
        }
}
